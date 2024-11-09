

using AccessTo.Web.Main.Security.MainRole;
using AccessTo.Web.Main.Services.AccessRole;
using AccessToAuth.Data.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
{

    #region Database Configuration
    /* ( Database Configuration ) */

    builder.Services.AddDbContextPool<DatabaseContext>(op =>
    {
        var connString = builder.Configuration.GetConnectionString("DatabaseConnection");
        op.UseSqlServer(connString);
    });

    #endregion

    #region Authentication Configuration
    /* ( Auth Configuration ) */

    builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
        {
            // Password
            options.Password.RequiredUniqueChars = 1;
            // User
            options.User.RequireUniqueEmail = true;
            options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789.";

            // Lock Acc
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
        })
        .AddEntityFrameworkStores<DatabaseContext>()
        .AddDefaultTokenProviders();

    builder.Services.ConfigureApplicationCookie(op =>
    {

        op.LoginPath = "/sign-in";
        op.AccessDeniedPath = "/sign-in";
        op.LogoutPath = "/sign-out";
        op.Cookie.Name = "UserAccountCookie";
        
        op.ReturnUrlParameter = "returnTo";

    });
    builder.Services.Configure<SecurityStampValidatorOptions>(op =>
    {
        op.ValidationInterval = TimeSpan.FromSeconds(5);
    });

    builder.Services.AddAuthorization(option =>
    {


        option.AddPolicy("MainRole", policy =>
                    policy.Requirements.Add(new MainRoleRequirement()));
    });

    #endregion

    #region Add Sevices Configuration





    #region Inject

    builder.Services.AddMemoryCache();
    builder.Services.AddHttpContextAccessor();

    builder.Services.AddTransient<IRoleService, RoleService>();
    builder.Services.AddScoped<IAuthorizationHandler, MainRoleHandler>();



    #endregion

    #endregion

    builder.Services.AddControllersWithViews()
        .AddRazorRuntimeCompilation();

}
var app = builder.Build();
{
    // Configure The HTTP Request Pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/error-development");
    }
    else
    {
        app.UseExceptionHandler("/ErrorHandler/500");
        app.UseStatusCodePagesWithReExecute("/ErrorHandler/{0}");
    }
    #region Middleware


    app.UseHsts();

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{Id?}");

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{Id?}");

    #endregion
}
/* HELLO FROM -FOUNDATION- ER-A-SE PLATFORM, THIS PROGRAM CREATED BY (Author) AD, FOR ALL PEOPLE IN THE WORLD, NO EXEPTION */
app.Run();