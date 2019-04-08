using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Security.Principal;
using HProject.Domain;

namespace HProject.Infrastructure
{
    public static class HProjectIdentityExtension
    {
        public static string GetUserFullName(this IIdentity identity)
        {
            UserManagers userManager = HttpContext.Current.GetOwinContext().GetUserManager<UserManagers>();
            User user = userManager.FindById(HttpContext.Current.User.Identity.GetUserId());
            if (user != null)
            {
                return user.FullName;
            }
            return "";
        }
    }
}