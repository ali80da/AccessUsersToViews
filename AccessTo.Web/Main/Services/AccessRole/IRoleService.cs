using AccessTo.Web.Main.DataMo.MainRole;
using AccessToAuth.Data.Context;
using AccessToAuth.Data.Entities.MainRoleCookie;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace AccessTo.Web.Main.Services.AccessRole
{
    public interface IRoleService
    {

        public IList<AreaAndControllerAndActionNameDataMo> AreaAndControllerAndActionNamesList();
        public IList<string> GetAllAreasNames();
        public string DatabaseRoleValidationGuid();

    }


    public class RoleService : IRoleService
    {

        #region Constructor

        private readonly DatabaseContext _context;

        public RoleService(DatabaseContext context)
        {
            _context = context;
        }

        #endregion


        public IList<AreaAndControllerAndActionNameDataMo> AreaAndControllerAndActionNamesList()
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            var contradistinction = asm.GetTypes()
                .Where(type => typeof(Controller).IsAssignableFrom(type))
                .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                .Select(x => new
                {
                    Controller = x.DeclaringType?.Name,
                    Action = x.Name,
                    Area = x.DeclaringType?.CustomAttributes.Where(c => c.AttributeType == typeof(AreaAttribute))
                });

            var list = new List<AreaAndControllerAndActionNameDataMo>();

            foreach (var item in contradistinction)
            {
                if (item.Area.Count() != 0)
                {
                    list.Add(new AreaAndControllerAndActionNameDataMo()
                    {
                        ControllerName = item.Controller,
                        ActionName = item.Action,
                        AreaName = item.Area.Select(v => v.ConstructorArguments[0].Value.ToString()).FirstOrDefault()
                    });
                }
                else
                {
                    list.Add(new AreaAndControllerAndActionNameDataMo()
                    {
                        ControllerName = item.Controller,
                        ActionName = item.Action,
                        AreaName = null,
                    });
                }
            }

            return list.Distinct().ToList();
        }

        public IList<string> GetAllAreasNames()
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            var contradistinction = asm.GetTypes()
                .Where(type => typeof(Controller).IsAssignableFrom(type))
                .SelectMany(type =>
                    type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                .Select(x => new
                {
                    Area = x.DeclaringType?.CustomAttributes.Where(c => c.AttributeType == typeof(AreaAttribute))

                });

            var list = new List<string>();

            foreach (var item in contradistinction)
            {
                list.Add(item.Area.Select(v => v.ConstructorArguments[0].Value.ToString()).FirstOrDefault());
            }

            if (list.All(string.IsNullOrEmpty))
            {
                return new List<string>();
            }

            list.RemoveAll(x => x == null);

            return list.Distinct().ToList();
        }
        
        public string DatabaseRoleValidationGuid()
        {
            var roleValidationGuid =
                _context.MainRoles.SingleOrDefault(s => s.Key == "RoleValidationGuid")?.Value;

            while (roleValidationGuid == null)
            {
                _context.MainRoles.Add(new MainRole()
                {
                    Key = "RoleValidationGuid",
                    Value = Guid.NewGuid().ToString(),
                    LastValidCookie = DateTime.Now
                });

                _context.SaveChanges();
                roleValidationGuid =
                    _context.MainRoles.SingleOrDefault(s => s.Key == "RoleValidationGuid")?.Value;
            }

            return roleValidationGuid;
        }


    }


}