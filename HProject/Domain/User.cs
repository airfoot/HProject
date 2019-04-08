using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNet.Identity.EntityFramework;

namespace HProject.Domain
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }
        public string IconUrl { get; set; }
        public virtual Department UserDepartment { get; set; }
    }
}