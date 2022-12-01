using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WorkForceManagementV0.Models
{
    public class AttendanceType
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public bool IsAbsence { get; set; }

        public bool IsDeleted { get; set; }
        public bool DisableEdit { get; set; } = false;
        public bool Hidden { get; set; } = false;
        [ForeignKey("DefaultActivity")]
        public int? DefaultActivityId { get; set; }

        public virtual ICollection<DailyAttendance> DailyAttendances { get; set; }

        public virtual ICollection<BreakTypeOption> BreakTypeOptions { get; set; }
        public virtual Activity DefaultActivity { get; set; }
    }
}
