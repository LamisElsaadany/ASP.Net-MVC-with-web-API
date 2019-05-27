using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Moslah.Models;
using Owin;

[assembly: OwinStartupAttribute(typeof(MoslahConsumer.Startup))]
namespace MoslahConsumer
{
    public partial class Startup
    {
        ApplicationDbContext DB = new ApplicationDbContext();

        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateAdmin();

        }

        public void CreateAdmin()
        {

            var Rolemanager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(DB));
            IdentityRole Role = new IdentityRole();
            if (!Rolemanager.RoleExists("admin"))
            {

                Role.Name = "admin";
                Rolemanager.Create(Role);
            }

            var Usermanager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(DB));
            var User = new ApplicationUser();
            User.UserName = "admin@gmail.com";
            User.Email = "admin@gmail.com";

            var success = Usermanager.Create(User, "P@sw0rd");
            if (success.Succeeded)
            {
                Usermanager.AddToRole(User.Id, "admin");
            }
        }
    }
}
