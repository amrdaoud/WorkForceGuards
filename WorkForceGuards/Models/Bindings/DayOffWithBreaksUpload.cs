using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkForceManagementV0.Models.Bindings
{
    public class DayOffWithBreaksUpload
    {
        public string EmployeeId { get; set; }
        public string StaffMember { get; set; }
        public string Transportation { get; set; }
        public string Attendance { get; set; }
        public string DayOne { get; set; }
        public string DayTwo { get; set; }
    }
}
