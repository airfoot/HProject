using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HProject.Domain
{
    public class Department
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public virtual User DepartmentManager { get; set; }
    }
}