using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WorkForceManagementV0.Models
{
    public class Activity
    {
        [Key]
        public int Id { get; set; }
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Name between 3 and 50 characters")]
        public string Name { get; set; }

        public string Color { get; set; }

        public bool IsPhone { get; set; }

        public bool IsAbsence { get; set; }

        public bool IsPaid { get; set; }

        public bool IsWorkTime { get; set; }
        public bool IsBreak { get; set; } = false;

        public bool IsDeleted { get; set; }
        public bool DisableEdit { get; set; } = false;
        public bool IsUndefined { get; set; } = false;
        public string CreatedBy { get; set; }
        public bool NeedBackup { get; set; }
        public DateTime? CreateDate { get; set; }

        public string? UpdatedBy { get; set; }

        public DateTime? UpdateDate { get; set; }

    }
}
