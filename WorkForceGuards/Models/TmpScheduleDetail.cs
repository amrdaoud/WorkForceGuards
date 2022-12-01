using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WorkForceManagementV0.Models
{
    public class TmpScheduleDetail
    {
        public TmpScheduleDetail(int dailyAttendanceId, int activityId, int intervalId)
        {
            DailyAttendanceId = dailyAttendanceId;
            ActivityId = activityId;
            IntervalId = intervalId;
            Id = 0;
            Duration = 0;
        }
        public int Id { get; set; }
        [ForeignKey("DailyAttendance")]
        public int DailyAttendanceId { get; set; }
        public virtual DailyAttendance DailyAttendance { get; set; }

        [ForeignKey("Activity")]
        public int ActivityId { get; set; }
        public virtual Activity Activity { get; set; }
        [ForeignKey("Interval")]

        public int IntervalId { get; set; }
        public virtual Interval Interval { get; set; }

        public double Duration { get; set; }
        
    }
}
