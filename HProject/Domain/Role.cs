using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;

namespace HProject.Domain
{
    public class Role : IdentityRole
    {
        public virtual ICollection<MenuItem> MenuItems { get; set; }
        
        public Role(string roleName) : base(roleName)
        {
            this.MenuItems = new HashSet<MenuItem>();
        }
        public Role() : base()
        {
            this.MenuItems = new HashSet<MenuItem>();
        }
    }
}