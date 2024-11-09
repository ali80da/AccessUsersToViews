using AccessTo.Web.Main.DataMo.MainRole;
using AccessToAuth.Data.Context;
using AccessToAuth.Data.Entities.MainRoleCookie;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace AccessTo.Web.Areas.Owner.Controllers
{
    public class MainRoleController : OwnerMainController
    {

        #region Constructor

        private readonly DatabaseContext _context;
        private readonly IMemoryCache _memoryCache;

        public MainRoleController(DatabaseContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }


        #endregion


        [HttpGet]
        public IActionResult MainRoles()
        {
            var mo = _context.MainRoles.ToList();
            return View(mo);
        }


        [HttpGet]
        public IActionResult RoleValidationGuid()
        {
            // GET Roles
            var roleValidationGuidMainRole =
                _context.MainRoles.FirstOrDefault(t => t.Key == "RoleValidationGuid");

            // GET => Add Value to Key
            var mo = new RoleValidationGuidDataMo()
            {
                Value = roleValidationGuidMainRole.Value,
                LastValidCookie = roleValidationGuidMainRole?.LastValidCookie
            };

            return View(mo);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult RoleValidationGuid(RoleValidationGuidDataMo mo)
        {
            var roleValidationGuidMainRole =
                _context.MainRoles.FirstOrDefault(t => t.Key == "RoleValidationGuid");

            if (roleValidationGuidMainRole == null)
            {
                _context.MainRoles.Add(new MainRole()
                {
                    Key = "RoleValidationGuid",
                    Value = Guid.NewGuid().ToString(),
                    LastValidCookie = DateTime.Now
                });
            }
            else
            {
                roleValidationGuidMainRole.Value = Guid.NewGuid().ToString();
                roleValidationGuidMainRole.LastValidCookie = DateTime.Now;
                _context.Update(roleValidationGuidMainRole);
            }
            _context.SaveChanges();
            _memoryCache.Remove("RoleValidationGuid");

            return RedirectToAction("MainRoles");
        }








    }
}