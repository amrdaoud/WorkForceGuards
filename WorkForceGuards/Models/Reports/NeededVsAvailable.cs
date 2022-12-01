using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkForceManagementV0.Models.Reports
{
    public class NeededVsAvailable
    {
        public DateTime Day { get; set; }
        public TimeSpan TimeMap { get; set; }
        public int? Forecast { get; set; }
        public int? Needed { get; set; }
        public int? Available { get; set; }

    }
}
