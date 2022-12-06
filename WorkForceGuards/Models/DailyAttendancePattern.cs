

using System.ComponentModel.DataAnnotations.Schema;
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

    public class DailyAttendancePatternsDTO: DailyAttendancePattern
    {
        public DailyAttendancePatternsDTO() { }
        public DailyAttendancePatternsDTO(DailyAttendancePattern model)
        {
            Id = model.Id;
            ScheduleId = model.ScheduleId;
            ScheduleName = model.Schedule.Name;
            StaffMemberId = model.StaffMemberId;
            StaffMemberName = model.StaffMember.Name;
            SublocationId = model.SublocationId;
            TransportationId = model.TransportationId;
            DayOffs = model.DayOffs;
        }
        public string ScheduleName { get; set; }
        public string StaffMemberName { get; set; }
    }
}
