using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkForceManagementV0.Models.DTO
{
    public class ScheduleById
    {
        public int ScheduleId { get; set; }
        public string ScheduleName { get; set; }
        public List<DailyStaffDto> DailyStaffs { get; set; }
    }

    public class DailyStaffDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public List<DailyAttendanceByScheduleIdDto> DailyAttendances { get; set; }

    }
    public class DailyAttendanceByScheduleIdDto
    {
        public int Id { get; set; }
        public int AttendanceTypeId { get; set; }
        public string TransportationName { get; set; }
        public TimeSpan TransportationArriveTime { get; set; }
        public TimeSpan TransportationDepartTime { get; set; }
        public string AttendanceTypeName { get; set; }
        public string HeadOfSectionName { get; set; }
        public string? SublocationName { get; set; }
        public string Color { get; set; }
        public int ShiftId { get; set; }
        public bool IsAbsence { get; set; }
        public DateTime Day { get; set; }
        public bool HaveBackup { get; set; } = false;
    }
}
