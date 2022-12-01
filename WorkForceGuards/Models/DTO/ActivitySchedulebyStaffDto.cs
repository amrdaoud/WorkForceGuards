using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkForceManagementV0.Models.DTO
{
    public class ScheduleByStaffDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string  Name { get; set; }
        public int ScheduleId { get; set; }
        public string ScheduleName { get; set; }
        public DateTime ScheduleStartDate { get; set; }
        public DateTime ScheduleEndDate { get; set; }
        public IEnumerable<DailyAttendanceDto> DailyAttendances { get; set; }



    }

    public class DailyAttendanceDto
    {
        public DateTime Day { get; set; }
        public int Id { get; set; }
        public bool HaveBackup { get; set; }
        public string HeadOfSectionName { get; set; }
       public Dictionary<int, ScheduleDetailDto> ScheduleDetails { get;set; }
         //public List<ScheduleDetailDto> ScheduleDetails { get; set; }
    }

    public class BkpDailyAttendanceDto
    {
        public int Id { get; set; }
        public string Alias { get; set; }
        public DateTime? ActionTime { get; set; }
        public string AttendanceType { get; set; }
        public Dictionary<int, ScheduleDetailDto> ScheduleDetails { get; set; }
    }

    public class ScheduleDetailDto
    {
        public int Id { get; set; }
        public int IntervalId { get; set; }
        public TimeSpan IntervalTimeMap { get; set; }
        public int ActivityId { get; set; }
        public string ActivityColor { get; set; }
        public string ActivityName { get; set; }
        public int? Duration { get; set; }
    }
}
