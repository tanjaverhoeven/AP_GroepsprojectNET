using System;
using System.Drawing.Text;
using System.Threading.Tasks;
using System.Web.Http;
using DotnetAcademy.DAL;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(DotnetAcademy.API.Startup))]

namespace DotnetAcademy.API {
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            HttpConfiguration config = new HttpConfiguration();

            ConfigureAuth(app);

            WebApiConfig.Register(config);
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(config);

            this.CreateRoles();
        }

        private void CreateRoles() {
            MainDbContext dbContext = new MainDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(dbContext));

            string adminRoleName = "administrator";
            string customerRoleName = "klant";

            if (!roleManager.RoleExists(adminRoleName)) {
                IdentityRole role = new IdentityRole() {Name = "administrator"};
                roleManager.Create(role);
            }

            if (!roleManager.RoleExists(customerRoleName)) {
                IdentityRole role = new IdentityRole() {Name = "klant"};
                roleManager.Create(role);
            }
        }
    }
}