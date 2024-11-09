using System.ComponentModel.DataAnnotations;

namespace AccessTo.Web.Main.DataMo.MainRole
{
    public class RoleValidationGuidDataMo
    {
        public string Value { get; set; }
        public DateTime? LastValidCookie { get; set; }

    }



    public class CreateRoleDataMo
    {
        public CreateRoleDataMo()
        {
            AreaAndControllerAndActionNames = new List<AreaAndControllerAndActionNameDataMo>();
        }

        [Required()]
        [Display(Name = "نام مقام")]
        public string RoleName { get; set; }
        public IList<AreaAndControllerAndActionNameDataMo> AreaAndControllerAndActionNames { get; set; }
    }

    public class AreaAndControllerAndActionNameDataMo
    {
        public string? AreaName { get; set; }
        public string? ControllerName { get; set; }
        public string? ActionName { get; set; }
        public bool IsSelected { get; set; }
    }


    // GET ROLES FOR CONTROLLER -> RoleController
    public class GetRolesInfoDataMo
    {
        public string RoleID { get; set; }
        public string RoleName { get; set; }
    }








}