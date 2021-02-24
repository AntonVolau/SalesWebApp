using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using SalesUpdater.Web.Data.Models;

[assembly: OwinStartupAttribute(typeof(SalesUpdater.Web.Startup))]
namespace SalesUpdater.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateRolesAndUsers();
        }

        private void CreateRolesAndUsers()
        {
            var context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            if (!roleManager.RoleExists("Admin"))
            {
                var role = new IdentityRole { Name = "Admin" };
                roleManager.Create(role);

                var user = new ApplicationUser { UserName = "Tohaman", Email = "volovanton@mail.ru" };

                var userPassword = "Toha19950911_";

                var chkUser = userManager.Create(user, userPassword);

                if (chkUser.Succeeded)
                {
                    var result = userManager.AddToRole(user.Id, "Admin");
                }
            }

            if (roleManager.RoleExists("User")) return;
            {
                var role = new IdentityRole { Name = "User" };
                roleManager.Create(role);
            }
        }
    }
}
