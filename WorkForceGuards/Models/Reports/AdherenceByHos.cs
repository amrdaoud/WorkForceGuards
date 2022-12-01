using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkForceManagementV0.Models.Reports
{
    public class AdherenceByHos
    {
        public int HosId { get; set; }
        public string HosName { get; set; }
        public double? Adherence { get; set; }
    }
}
