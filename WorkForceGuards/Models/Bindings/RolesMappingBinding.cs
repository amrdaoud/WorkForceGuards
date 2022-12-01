using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkForceManagementV0.Models.Bindings
{
    public class RolesMappingBinding
    {
        public RolesMappingChild Groups { get; set; }
        public RolesMappingChild Users { get; set; }
    }
    public class RolesMappingChild
    {
        public string Admin { get; set; }
        public string User { get; set; }
        public string Hos { get; set; }
    }
}
