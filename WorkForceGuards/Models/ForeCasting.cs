using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WorkForceManagementV0.Models
{
    public class ForeCasting
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Schedule")]
        public int ScheduleId { get; set; }

        [ForeignKey("Interval")]
        public int IntervalId { get; set; }

        public DateTime Day { get; set; }
        public int EmployeeCount { get; set; }

        public virtual Schedule Schedule { get; set; }
        public virtual Interval Interval { get; set; }
        public ForeCasting(int ScheduleId, DateTime Day,int IntervalId, int EmployeeCount)
        {
            this.ScheduleId = ScheduleId;
            this.Day = Day;
            this.IntervalId = IntervalId;
            this.EmployeeCount = EmployeeCount;
            
        }
    }
}
