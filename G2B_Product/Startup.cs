using G2B_Product.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(G2B_Product.Startup))]
namespace G2B_Product
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            // Execute to initialize the roles and admin user
            CreateRolesandUsers();
        }

        // Create default User roles and Admin user for login   
        private void CreateRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            // Create Admin role and 1st Admin user
            if (!roleManager.RoleExists("Admin"))
            {
                // Create Admin role
                var roleAdmin = new IdentityRole
                {
                    Id = "0ded18bc-cead-489a-b107-0fadb1cdf962",
                    Name = "Admin"
                };
                roleManager.Create(roleAdmin);

                // Define 1st Admin super user who will maintain the website 
                var user = new ApplicationUser
                {
                    UserName = "nhat.phamkhac@gmail.com",
                    Email = "nhat.phamkhac@gmail.com"
                };
                var userPWD = "Pwd!23";

                // Create 1st Admin user
                var chkUser = userManager.Create(user, userPWD);

                // Add user to role Admin
                if (chkUser.Succeeded)
                {
                    var result = userManager.AddToRole(user.Id, "Admin");
                }
            }

            // Create ContentContributor role and 1st Admin user
            if (!roleManager.RoleExists("ContentContributor"))
            {
                var role = new IdentityRole
                {
                    Id = "dd9bd5de-674d-4f55-b526-d945d9456da7",
                    Name = "ContentContributor"
                };
                roleManager.Create(role);

                // Define 1st Content Contributor super user who will maintain the website 
                var user = new ApplicationUser
                {
                    UserName = "content@contributor.com",
                    Email = "content@contributor.com"
                };
                var userPWD = "Pwd!23";

                // Create 1st Admin user
                var chkUser = userManager.Create(user, userPWD);

                // Add user to role Admin
                if (chkUser.Succeeded)
                {
                    var result = userManager.AddToRole(user.Id, "ContentContributor");
                }
            }
        }
    }
}
