using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using ShackUp.UI.Models.Identity;

namespace ShackUp.UI
{
    public class IdentityConfig
    {
        public void Configuration(IAppBuilder app)
        {
            app.CreatePerOwinContext(() => new ShackUpDbContext());

            app.CreatePerOwinContext<UserManager<AppUser>>(
                (options, context) => new UserManager<AppUser>(
                    new UserStore<AppUser>(context.Get<ShackUpDbContext>())
                )
            );

            app.CreatePerOwinContext<RoleManager<AppRole>>(
                (options, context) => new RoleManager<AppRole>(
                    new RoleStore<AppRole>(context.Get<ShackUpDbContext>()
                    )
                )
            );

            app.UseCookieAuthentication(new CookieAuthenticationOptions
                {
                    AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                    LoginPath = new PathString("/Home/Login")
                }
            );
        }
    }
}