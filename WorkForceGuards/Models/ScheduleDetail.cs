using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WorkForceManagementV0.Models.Backup;

namespace WorkForceManagementV0.Models
{
    public class ScheduleDetail
    {
        public int Id { get; set; }
      

        [ForeignKey ("DailyAttendance")]
        public int DailyAttendanceId { get; set; }
        public virtual DailyAttendance DailyAttendance { get; set; }

        [ForeignKey("Activity")]
        public int ActivityId { get; set; }
        public virtual Activity Activity { get; set; }
        [ForeignKey("Interval")]

        public int IntervalId { get; set; }
        public virtual Interval Interval { get; set; }
        public double? Duration  { get; set; }
        [ForeignKey("Schedule")]
        public int ScheduleId { get; set; }
        public virtual Schedule Schedule { get; set; }
        [ForeignKey("BackupStaff")]
        public int? BackupStaffId { get; set; }
        public StaffMember BackupStaff { get; set; }
        public ScheduleDetail()
        {

        }
        //public ScheduleDetail(TmpScheduleDetail model)
        //{
        //    DailyAttendanceId = model.DailyAttendanceId;
        //    ActivityId = model.ActivityId;
        //    IntervalId = model.IntervalId;
        //    Duration = model.Duration;
        //}
        public ScheduleDetail(int DailyAttendanceId, int ActivityId, int IntervalId, double? Duration, int ScheduleId)
        {
            this.DailyAttendanceId = DailyAttendanceId;
            this.ActivityId = ActivityId;
            this.IntervalId = IntervalId;
            this.Duration = Duration;
            this.ScheduleId = ScheduleId;
        }
        public ScheduleDetail(BkpScheduleDetail model)
        {
            Id = 0;
            DailyAttendanceId = model.DailyAttendanceId;
            ActivityId = model.ActivityId;
            IntervalId = model.IntervalId;
            Duration = null;
            ScheduleId = model.ScheduleId;
        }
    }
}
