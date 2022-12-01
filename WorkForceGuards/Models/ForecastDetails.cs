using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WorkForceManagementV0.Models
{
    public class ForecastDetails
    {
        public int Id { get; set; }

        [ForeignKey("Forecast")]
        public int ForecastId { get; set; }

        [ForeignKey("Interval")]
        public int IntervalId { get; set; }
        public string DayoffWeek { get; set; }
        public int EmployeeCount { get; set; }
        public virtual Interval Interval { get; set; } 
        public virtual Forecast Forecast { get; set; }
        public ForecastDetails(int ForecastId, string DayoffWeek, int IntervalId, int EmployeeCount)
        {
            this.ForecastId = ForecastId;
            this.DayoffWeek = DayoffWeek;
            this.IntervalId = IntervalId;
            this.EmployeeCount = EmployeeCount;

        }
     
        public ForecastDetails(TmpForecastDetails model)
        {
            ForecastId = model.Id;
            IntervalId = model.IntervalId;
            DayoffWeek = model.DayoffWeek;
            EmployeeCount = model.EmployeeCount;

        }

    }
}
