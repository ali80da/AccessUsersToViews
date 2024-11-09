using AccessToAuth.Data.Entities.Employees;
using AccessToAuth.Data.Entities.MainRoleCookie;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AccessToAuth.Data.Context
{
    public class DatabaseContext : IdentityDbContext
    {

        #region CDB

        public DatabaseContext(DbContextOptions context) : base(context) { }

        #endregion

        #region Employee

        public DbSet<Employee> Employees { get; set; }

        #endregion

        #region Main Role

        public DbSet<MainRole> MainRoles { get; set; }

        #endregion


        protected override void OnModelCreating(ModelBuilder builder)
        {
            // call the base if you are using Identity service.
            // Important
            base.OnModelCreating(builder);

            // Code here ...
        }
    }
}