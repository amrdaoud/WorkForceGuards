using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WorkForceManagementV0.Models
{
    public class TmpForecastDetails
    {
        public int Id { get; set; }
        public int TmpForecastId { get; set; }
        [ForeignKey("Interval")]
        public int IntervalId { get; set; }
        public string DayoffWeek { get; set; }
        public int EmployeeCount { get; set; }
        public virtual Interval Interval { get; set; }
        public TmpForecastDetails() { }
        public TmpForecastDetails(int TmpForecastId, string DayoffWeek, int IntervalId, int EmployeeCount)
        {
            this.TmpForecastId = TmpForecastId;
            this.DayoffWeek = DayoffWeek;
            this.IntervalId = IntervalId;
            this.EmployeeCount = EmployeeCount;

        }
    }
}
