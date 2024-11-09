namespace AccessTo.Web.Main.DataMo.MainUser
{

    public class MainUserDataMo
    {

        public required string ID { get; set; }
        public required string UserName { get; set; }
        public required string Email { get; set; }

    }


    public class AddUserToRoleDataMo
    {

        #region Constructor

        public AddUserToRoleDataMo()
        {
            UserRoles = new List<UserRolesDataMo>();
        }

        public AddUserToRoleDataMo(string userID, IList<UserRolesDataMo> userRoles)
        {
            UserID = userID;
            UserRoles = userRoles;
        }


        #endregion

        public string UserID { get; set; }

        public IList<UserRolesDataMo> UserRoles { get; set; }
    }

    public class UserRolesDataMo
    {

        #region Constructor

        public UserRolesDataMo()
        {
        }

        public UserRolesDataMo(string roleName)
        {
            RoleName = roleName;
        }


        #endregion

        public string RoleName { get; set; }
        public bool IsSelected { get; set; }
    }




}