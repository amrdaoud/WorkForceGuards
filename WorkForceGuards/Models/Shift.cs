using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WorkForceManagementV0.Models
{
    public class Shift
    {
        [Key]
        public int Id { get; set; }
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Name between 3 and 50 characters")]
        public string Name { get;set;}
        [ForeignKey("EarlyStartInterval")]
        public int? EarlyStartIntervalId { get; set; }
        [ForeignKey("LateEndInterval")]
        public int? LateEndIntervalId { get; set; }
        public double Duration { get; set; }

        public string CreatedBy { get; set; }
        public DateTime? CreateDate { get; set; }

        public string UpdatedBy { get; set; }

        public DateTime? UpdateDate { get; set; }
        public bool  IsDeleted { get; set; }
        public virtual Interval EarlyStartInterval { get; set; }
        public virtual Interval LateEndInterval { get; set; }


        //public virtual ICollection<DailyAttendance> DailyAttendances { get; set; }

        //public virtual ICollection<BreakTypeOption>  BreakTypeOptions{ get; set; }
    }
}
