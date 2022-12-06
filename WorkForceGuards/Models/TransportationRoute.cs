using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WorkForceManagementV0.Models
{
    public class TransportationRoute
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Name between 3 and 50 characters")]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        [ForeignKey("ArriveInterval")]
        public int ArriveIntervalId { get; set; }
        [ForeignKey("DepartInterval")]
        public int? DepartIntervalId { get; set; }
        public bool IsIgnored { get; set; }
        public bool IsDeleted { get; set; }
        public virtual Interval ArriveInterval { get; set; }
        public virtual Interval DepartInterval { get; set; }

        public virtual ICollection<StaffMember> StaffMembers { get; set; }

    }
}
