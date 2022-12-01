using System;
using System.Collections.Generic;

namespace WorkForceManagementV0.Models.Bindings
{
    public class ReportFilter
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string FilterType { get; set; }
        public List<string> FilterValue { get; set; }
        public bool IsDaily { get; set; }
        public string Groupby { get; set; }
    }


}
