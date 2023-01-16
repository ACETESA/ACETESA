using Acetesa.TomaPedidos.AdminMvc.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Acetesa.TomaPedidos.AdminMvc.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ApplicationDbContext context)
        {
            const string roleName = "admin";
            using (var roleStore = new RoleStore<IdentityRole>(context))
            {
                using (var roleManager = new RoleManager<IdentityRole>(roleStore))
                {
                    var existsRole = roleManager.RoleExists(roleName);
                    if (!existsRole)
                    {
                        var role = new IdentityRole(roleName);
                        roleManager.Create(role);
                    }
                }
            }

            using (var userStore = new UserStore<ApplicationUser>(context))
            {
                using (var userManager = new UserManager<ApplicationUser>(userStore))
                {
                    const string email = "rony.benites.avalos@hotmail.com";
                    const string password = "R@b331tes15";
                    var user = userManager.Find(email, password);
                    if (user == null)
                    {
                        user = new ApplicationUser
                        {
                            Email = email,
                            UserName = email
                        };
                        var resultUser = userManager.Create(user, password);
                        if (resultUser.Succeeded)
                        {
                            userManager.AddToRole(user.Id, roleName);
                        }
                    }
                    const string email2 = "scotrina@acetesa.com";
                    const string password2 = "S@UlC01r#n@@4s@";
                    user = userManager.Find(email2, password2);
                    if (user == null)
                    {
                        user = new ApplicationUser
                        {
                            Email = email2,
                            UserName = email2
                        };
                        var resultUser = userManager.Create(user, password2);
                        if (resultUser.Succeeded)
                        {
                            userManager.AddToRole(user.Id, roleName);
                        }
                    }
                    const string email3 = "richard@acetesa.com";
                    const string password3 = "R1cC4rDCe113@";
                    user = userManager.Find(email3, password3);
                    if (user == null)
                    {
                        user = new ApplicationUser
                        {
                            Email = email3,
                            UserName = email3
                        };
                        userManager.Create(user, password3);
                        
                    }
                }
            }
        }
    }
}
