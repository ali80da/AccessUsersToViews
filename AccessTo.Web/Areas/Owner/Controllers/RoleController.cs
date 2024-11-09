using AccessTo.Web.Main.DataMo.MainRole;
using AccessTo.Web.Main.Services.AccessRole;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;

namespace AccessTo.Web.Areas.Owner.Controllers
{
    public class RoleController : OwnerMainController
    {

        #region Constructor

        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IRoleService _roleService;
        private readonly IMemoryCache _memoryCache;
        public RoleController(RoleManager<IdentityRole> roleManager, IRoleService roleService, IMemoryCache memoryCache)
        {
            _roleManager = roleManager;
            _roleService = roleService;
            _memoryCache = memoryCache;
        }

        #endregion

        
        [HttpGet("owner-access-to-all-roles")]
        public IActionResult MainAllRoles()
        {
            // GET All Roles
            var roles = _roleManager.Roles.ToList();

            var mo = new List<GetRolesInfoDataMo>();

            foreach (var role in roles)
            {
                mo.Add(new GetRolesInfoDataMo()
                {
                    RoleID = role.Id,
                    RoleName = role?.Name
                });
            }

            return View(mo);
        }




        [HttpGet]
        public IActionResult CreateRole()
        {
            var allMvcNames =
                _memoryCache.GetOrCreate("AreaAndControllerAndActionNamesList", p =>
                {
                    p.AbsoluteExpiration = DateTimeOffset.MaxValue;
                    return _roleService.AreaAndControllerAndActionNamesList();
                });
            var model = new CreateRoleDataMo()
            {
                AreaAndControllerAndActionNames = allMvcNames
            };

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRole(CreateRoleDataMo model)
        {
            if (ModelState.IsValid)
            {
                var role = new IdentityRole(model.RoleName);
                var result = await _roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    var requestRoles =
                        model.AreaAndControllerAndActionNames.Where(c => c.IsSelected).ToList();
                    foreach (var requestRole in requestRoles)
                    {
                        var areaName = (string.IsNullOrEmpty(requestRole.AreaName)) ?
                            "NoArea" : requestRole.AreaName;

                        await _roleManager.AddClaimAsync(role,
                            new Claim($"{areaName}|{requestRole.ControllerName}|{requestRole.ActionName}".ToUpper(),
                                true.ToString()));
                    }


                    return RedirectToAction("MainAllRoles");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }









    }
}