using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WorkForceManagementV0.Models.Backup
{
    public class BkpScheduleDetail
    {
        public BkpScheduleDetail()
        {

        }
        public BkpScheduleDetail(ScheduleDetail model)
        {
            Id = 0;
            DailyAttendanceId = model.DailyAttendanceId;
            ActivityId = model.ActivityId;
            IntervalId = model.IntervalId;
            Duration = model.Duration;
            ScheduleId = model.ScheduleId;
        }
        [Key]
        public int Id { get; set; }
        [ForeignKey("DailyAttendance")]
        public int DailyAttendanceId { get; set; }
        public virtual BkpDailyAttendance DailyAttendance { get; set; }
        [ForeignKey("Activity")]
        public int ActivityId { get; set; }

        [ForeignKey("Interval")]
        public int IntervalId { get; set; }

        public double? Duration { get; set; }
        public int ScheduleId { get; set; }

        public virtual Interval Interval { get; set; }
        public virtual Activity Activity { get; set; }
    }
}
