using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkForceManagementV0.Models.DTO
{
    public class AttendanceSummary
    {
        public TimeSpan TimeMap { get; set; }
        public int? IntervalId { get; set; }
        public double Tolerance { get; set; }
        public int? AverageNeeded { get; set; }
        public int AverageAvailable { get; set; }
    }
    public class AttendanceSummaryNeeded
    {
        public TimeSpan TimeMap { get; set; }
        public int AverageNeeded { get; set; }
    }
    public class ActiveSchedule
    {
        public Schedule Schedule { get; set; }
        public int AttendanceStaffCount { get; set; }
    }
}
