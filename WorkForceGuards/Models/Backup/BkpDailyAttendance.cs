using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace WorkForceManagementV0.Models.Backup
{
    public class BkpDailyAttendance
    {
        public BkpDailyAttendance()
        {

        }
        public BkpDailyAttendance(DailyAttendance model, string alias)
        {
            var timezone = TimeZoneInfo.FindSystemTimeZoneById("Arab Standard Time");
            ActionTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timezone);
            Id = 0;
            DailyAttendanceId = model.Id;
            Alias = alias;
            StaffMemberId = model.StaffMemberId;
            ScheduleId = model.ScheduleId;
            Day = model.Day;
            AttendanceTypeId = model.AttendanceTypeId;
            TransportationRouteId = model.TransportationRouteId;
            HeadOfSectionId =   model.HeadOfSectionId;
            IsDeleted = false;
        }
        [Key]
        public int Id { get; set; }
        public int DailyAttendanceId { get; set; }
        public string Alias { get; set; }
        public DateTime ActionTime { get;set; }

        public int StaffMemberId { get; set; }


        public int ScheduleId { get; set; }
        public DateTime Day { get; set; }

        [ForeignKey("AttendanceType")]
        public int AttendanceTypeId { get; set; }


        public int? TransportationRouteId { get; set; }

        public int HeadOfSectionId { get; set; }
        public virtual AttendanceType AttendanceType { get; set; }









        public bool IsDeleted { get; set; }

        public virtual ICollection<BkpScheduleDetail> ScheduleDetails { get; set; }
    }
}
