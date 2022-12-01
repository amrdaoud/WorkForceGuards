using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WorkForceManagementV0.Models
{
    public class ForecastHistory
    {
        public int Id { get; set; }
        public DateTime Day { get; set; }
        public int Offered { get; set; }
        public double Duration { get; set; }
        [ForeignKey("Interval")]
        public int IntervalId { get; set; }
        public virtual Interval Interval { get; set; }

    }
}
