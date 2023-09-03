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
    public class StaffAttendanceFilter
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public int? LocationId { get; set; }
        public int? SublocationId { get; set; }
        public int? HosId { get; set; }
        public int? StaffId { get; set; }
        public int? EmployeeId { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string Sort { get; set; }
        public string Order { get; set; }
    }


}
