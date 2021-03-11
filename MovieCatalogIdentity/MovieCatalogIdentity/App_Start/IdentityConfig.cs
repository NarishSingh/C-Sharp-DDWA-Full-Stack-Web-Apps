using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using MovieCatalogIdentity.Models.Identity;
using Owin;

namespace MovieCatalogIdentity.App_Start
{
    /// <summary>
    /// Setup for user authentication via cookies
    /// </summary>
    public class IdentityConfig
    {
        public void Configuration(IAppBuilder app)
        {
            app.CreatePerOwinContext(() => new MovieCatalogDbContext());

            app.CreatePerOwinContext<UserManager<AppUser>>(
                (options, context) => new UserManager<AppUser>(
                    new UserStore<AppUser>(context.Get<MovieCatalogDbContext>())
                )
            );

            app.CreatePerOwinContext<RoleManager<AppRole>>(
                (options, context) => new RoleManager<AppRole>(
                    new RoleStore<AppRole>(context.Get<MovieCatalogDbContext>()
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