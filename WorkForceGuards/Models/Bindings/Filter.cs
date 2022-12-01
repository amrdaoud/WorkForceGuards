using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkForceManagementV0.Models.Bindings
{
    public class Filter
    {
        public string Key { get; set; }
        public List<string> Values { get; set; }
    }
}
