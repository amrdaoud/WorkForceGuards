

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using WorkForceManagementV0.Models;

namespace WorkForceGuards.Models
{
    public class DailyAttendancePattern
    {
        public int Id { get; set; }
        [ForeignKey("Schedule")]
        public int ScheduleId { get; set; }
        [ForeignKey("StaffMember")]
        public int StaffMemberId { get; set; }
        [ForeignKey("Sublocation")]
        public int SublocationId { get; set; }
        [ForeignKey("Transportation")]
        public int TransportationId { get; set; }
        public string[] DayOffs { get; set; }
        public virtual StaffMember StaffMember { get; set; }
        public virtual Schedule Schedule { get; set; }
        public virtual SubLocation Sublocation { get; set; }
        public virtual TransportationRoute Transportation { get; set; }
    }

    public class DailyAttendancePatternsDTO
    {
        public DailyAttendancePatternsDTO() { }
        public DailyAttendancePatternsDTO(DailyAttendancePattern model)
        {
            Id = model.Id;
            ScheduleId = model.ScheduleId;
            ScheduleName = model.Schedule.Name;
            StaffMemberId = model.StaffMemberId;
            StaffMemberEmployeeId = model.StaffMember.EmployeeId;
            StaffMemberName = model.StaffMember.Name;
            SublocationId = model.SublocationId;
            SublocationName = model.Sublocation.Name;
            TransportationId = model.TransportationId;
            TransportationName = model.Transportation.Name;
            DayOffs = model.DayOffs;
        }
        public DailyAttendancePatternsDTO(Schedule schedule, StaffMember staffMember)
        {
            Id = 0;
            ScheduleId = schedule.Id;
            ScheduleName = schedule.Name;
            StaffMemberId = staffMember.Id;
            StaffMemberName = staffMember.Name;
            SublocationId = 0;
            TransportationId = 0;
            DayOffs = new List<string>().ToArray();
        }
        public int Id { get; set; }
        public int? ScheduleId { get; set; }
        public int? StaffMemberId { get; set; }
        public int? SublocationId { get; set; }
        public int? TransportationId { get; set; }
        public string[] DayOffs { get; set; }
        public string ScheduleName { get; set; }
        public int StaffMemberEmployeeId { get; set; }
        public string StaffMemberName { get; set; }
        public string SublocationName { get; set; }
        public string TransportationName { get; set; }
    }
    public class DailyAttendancePatternsUpload
    {
        public string EmployeeId { get; set; }
        public string Sublocation { get; set; }
        public string Shift { get; set; }
        public string DayOffs { get; set; }
    }

    public class DailyAttendancePatternBinding
    {
        public int Id { get; set; }
        public int ScheduleId { get; set; }
        public int StaffMemberId { get; set; }
        public int SublocationId { get; set; }
        public int TransportationId { get; set; }
        public DateTime[] DayOffs { get; set; }
        public virtual StaffMember StaffMember { get; set; }
        public virtual Schedule Schedule { get; set; }
        public virtual SubLocation Sublocation { get; set; }
        public virtual TransportationRoute Transportation { get; set; }
    }
}
