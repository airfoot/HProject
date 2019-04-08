using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HProject.Domain
{
    public class MenuItem
    {
        public Guid Id { get; set; }
        public string Area { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public virtual MenuItem Parent { get; set; }
    }
}