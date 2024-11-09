using AccessTo.Web.Main.Services.AccessRole;
using AccessToAuth.Data.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;
using System.Security.Cryptography;

namespace AccessTo.Web.Main.Security.MainRole
{
    public class MainRoleHandler : AuthorizationHandler<MainRoleRequirement>
    {

        #region Constructor

        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IRoleService _roleService;
        private readonly IMemoryCache _memoryCache;
        private readonly IDataProtector _protectorToken;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly DatabaseContext _databaseContext;

        public MainRoleHandler(
            IHttpContextAccessor contextAccessor,
            IRoleService roleService,
            IMemoryCache memoryCache,
            IDataProtectionProvider dataProtectionProvider,
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            DatabaseContext databaseContext
            )
        {
            _contextAccessor = contextAccessor;
            _roleService = roleService;
            _memoryCache = memoryCache;
            _protectorToken = dataProtectionProvider.CreateProtector("RvgGuid");
            _signInManager = signInManager;
            _userManager = userManager;
            _databaseContext = databaseContext;
        }

        #endregion


        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, MainRoleRequirement requirement)
        {
            var httpContext = _contextAccessor.HttpContext;
            var userID = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userID)) return;

            // GET Guid in Database, in Cache, KEY = RoleValidationGuid
            var dbRoleValidationGuid = _memoryCache.GetOrCreate("RoleValidationGuid", p =>
            {
                p.AbsoluteExpiration = DateTimeOffset.MaxValue;
                return _roleService.DatabaseRoleValidationGuid();
            });

            // GET Address in URL > Change to ClaimType ( {areaName}|{requestRole.ControllerName}|{requestRole.ActionName} )
            SplitUserRequestedUrl(httpContext, out var areaAndControllerAndActionName);

            // خروچ کوکی از حالت رمز نگاری
            UnprotectRvgCookieData(httpContext, out var unprotectedRvgCookie);

            // Check User Cookie is Valid or Not
            if (unprotectedRvgCookie != null && dbRoleValidationGuid != null)
            {
                if (!IsRvgCookieDataValid(unprotectedRvgCookie, userID, dbRoleValidationGuid))
                {
                    var user = await _userManager.FindByIdAsync(userID);
                    if (user == null) return;

                    AddOrUpdateRvgCookie(httpContext, dbRoleValidationGuid, userID);
                    // Update User Cookie
                    await _signInManager.RefreshSignInAsync(user);

                    //var userRolesId = _dbContext.UserRoles.AsNoTracking()
                    //    .Where(r => r.UserId == userId)
                    //    .Select(r => r.RoleId)
                    //    .ToList();
                    //if (!userRolesId.Any()) return;
                    //var userHasClaims = _dbContext.RoleClaims.AsNoTracking().Any(rc =>
                    //    userRolesId.Contains(rc.RoleId) && rc.ClaimType == areaAndActionAndControllerName);
                    //if (userHasClaims) context.Succeed(requirement);
                }
                else if (httpContext.User.HasClaim(areaAndControllerAndActionName, true.ToString()))
                    context.Succeed(requirement);
            }

            return;
        }


        #region Methods

        private static void SplitUserRequestedUrl(HttpContext httpContext, out string areaAndControllerAndActionName)
        {
            var areaName = httpContext.Request.RouteValues["area"]?.ToString() ?? "NoArea";
            var controllerName = $"{httpContext.Request.RouteValues["controller"]}".ToString() + "Controller";
            var actionName = $"{httpContext.Request.RouteValues["action"]}".ToString();

            areaAndControllerAndActionName = $"{areaName}|{controllerName}|{actionName}".ToUpper();
        }

        private void UnprotectRvgCookieData(HttpContext httpContext, out string? unprotectedRvgCookie)
        {
            //var protectedRvgCookie = httpContext.Request.Cookies
            //    .FirstOrDefault(t => t.Key == "RVG").Value;
            var protectedRvgCookie = httpContext.Request.Cookies["RVG"];
            unprotectedRvgCookie = null;
            if (!string.IsNullOrEmpty(protectedRvgCookie))
            {
                try
                {
                    unprotectedRvgCookie = _protectorToken.Unprotect(protectedRvgCookie);
                }
                catch (CryptographicException)
                {

                }
            }
        }

        private static bool IsRvgCookieDataValid(string rvgCookieData, string validUserID, string validRvg)
            => !string.IsNullOrEmpty(rvgCookieData) &&
               SplitUserIdFromRvgCookie(rvgCookieData) == validUserID &&
               SplitRvgFromRvgCookie(rvgCookieData) == validRvg;

        // 
        private static string SplitUserIdFromRvgCookie(string rvgCookieData)
            => rvgCookieData.Split("|||")[1];

        // 
        private static string SplitRvgFromRvgCookie(string rvgCookieData)
            => rvgCookieData.Split("|||")[0];

        // 
        private static string CombineRvgWithUserID(string rvg, string validUserID)
            => rvg + "|||" + validUserID;

        private void AddOrUpdateRvgCookie(HttpContext httpContext, string validRvg, string validUserID)
        {
            var rvgWithUserID = CombineRvgWithUserID(validRvg, validUserID);
            // Protected Cookie Data
            var protectedRvgWithUserID = _protectorToken.Protect(rvgWithUserID);
            // Add or Update Cookie
            httpContext.Response.Cookies.Append("RVG", protectedRvgWithUserID,
                new CookieOptions
                {
                    // Cookie Expire Time
                    MaxAge = TimeSpan.FromDays(90),
                    // No Access with JS
                    HttpOnly = true,
                    // GET Data, Just SSL
                    Secure = true,
                    SameSite = SameSiteMode.Lax
                });
        }


        #endregion





    }
}