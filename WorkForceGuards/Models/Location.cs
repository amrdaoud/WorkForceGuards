

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WorkForceManagementV0.Models
{
    public class Location
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50,MinimumLength =3, ErrorMessage ="Name between 3 and 50 characters")]
        public string Name { get; set; }
        public string Address { get; set; }
        public string ContactPhone { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<StaffMember> StaffMembers { get; set; }
        public virtual ICollection<Asset> Assets { get; set; }
        public virtual ICollection<SubLocation> SubLocations  { get; set; }
    }
}
