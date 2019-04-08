using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using HProject.Infrastructure;

namespace HProject.App_Start
{
    public class IdentityConfig
    {

        public void Configuration(IAppBuilder app)
        {

            app.CreatePerOwinContext<HProjectDbContext>(HProjectDbContext.Create);
            app.CreatePerOwinContext<UserManagers>(UserManagers.Create);
            app.CreatePerOwinContext<RoleManagers>(RoleManagers.Create);

            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
            });
        }

    }
}