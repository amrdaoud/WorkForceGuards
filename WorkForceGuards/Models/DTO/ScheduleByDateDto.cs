using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkForceManagementV0.Models.DTO
{
    public class ScheduleByDateDto
    {
        public DateTime Day { get; set; }
        public int ScheduleId { get; set; }
        public string ScheduleName { get; set; }
        public DateTime ScheduleStartDate { get; set; }
        public DateTime ScheduleEndDate { get; set; }
        public List<DailyAttendanceByDayDto> DailyAttendances { get; set; }


    }
    public class DailyAttendanceByDayDto
    {
        public int AttendanceId { get; set; }
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public string HeadOfSectionName { get; set; }
        public Dictionary<int,ScheduleDetailDto> ScheduleDetails { get; set; }
    }
 
  
}
