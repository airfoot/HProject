using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using HProject.Domain;

namespace HProject.Infrastructure
{
    public class UserManagers : UserManager<User>
    {
        public UserManagers(IUserStore<User> store) : base(store)
        {

        }
        public static UserManagers Create(IdentityFactoryOptions<UserManagers> options, IOwinContext context)
        {
            HProjectDbContext dbContext = context.Get<HProjectDbContext>();
            UserManagers userManagers = new UserManagers(new UserStore<User>(dbContext));

            userManagers.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
            };
            userManagers.UserValidator = new UserValidator<User>(userManagers)
            {
                AllowOnlyAlphanumericUserNames = true,
                RequireUniqueEmail = true
            };

            return userManagers;
        }
    }
}