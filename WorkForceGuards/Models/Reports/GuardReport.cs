using System;
using System.Collections.Generic;

namespace WorkForceGuards.Models.Reports
{
    public class StaffWorkingDaysReport
    {
        public int EmployeeId { get; set; }
        public string StaffMemberName { get; set; }
        public string PhoneNumber { get; set; }
        public string HeadOfSectionName { get; set; }
        public string StaffTypeName { get; set; }
        public string ShiftName { get; set; }
        public string SublocationName { get; set; }
        public string Attendance { get; set; }
    }

    public class StaffAttendanceReport
    {
        public int StaffMemberId { get; set; }
        public int EmployeeId { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime Day { get; set; }
        public string StaffMemberName { get; set; }
        public string HeadOfSectionName { get; set; }
        public string StaffTypeName { get; set; }
        public string ShiftName { get; set; }
        public string AttendanceTypeName { get; set; }
        public string ActualShift { get; set; }
        public string SublocationName { get; set; }
        public string Activities { get; set; }
    }

    public class StaffAttendanceReportWithSize
    {
        public List<StaffAttendanceReport> Result { get; set; }
        public int ResultSize { get; set; }
    }

    public class StaffWorkingDaysReporttWithSize
    {
        public List<StaffWorkingDaysReport> Result { get; set; }
        public int ResultSize { get; set; }
    }
}
