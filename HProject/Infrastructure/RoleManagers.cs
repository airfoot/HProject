using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using HProject.Domain;

namespace HProject.Infrastructure
{
    public class RoleManagers : RoleManager<Role>, IDisposable
    {
        public RoleManagers(RoleStore<Role> store) : base(store)
        {

        }

        public static RoleManagers Create(IdentityFactoryOptions<RoleManagers> options, IOwinContext context)
        {
            return new RoleManagers(new RoleStore<Role>(context.Get<HProjectDbContext>()));
        }
    }
}