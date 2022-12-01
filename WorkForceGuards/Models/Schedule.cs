using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WorkForceManagementV0.Models
{
    public class Schedule
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string? CreatedBy { get; set; }
        public DateTime? CreateDate { get; set; }

        public string? UpdatedBy { get; set; }

        public DateTime? UpdateDate { get; set; }

        public bool IsPublish { get; set; }
        public int? ForecastId {get;set;}
        [NotMapped]
        public bool HaveAttendance { get; set; } = false;


        public bool IsDeleted { get; set; }

        public virtual ICollection<DailyAttendance> DailyAttendances { get; set; }

        public virtual ICollection<DayOffOption> DayOffOptions { get; set; }

        public virtual ICollection<BreakTypeOption> BreakTypeOptions { get; set; }
        public virtual ICollection<ForeCasting> ForeCastings { get; set; }
        public virtual ICollection<ScheduleDetail> ScheduleDetails { get; set; }

        public Schedule ()
        {
            HaveAttendance = this.DailyAttendances?.FirstOrDefault() != null;
        }
        
    }
}
