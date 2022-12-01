using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WorkForceManagementV0.Models
{
    public class People
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Employee Id is Mandatory")]
        public int EmployeeId { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Name between 3 and 50 characters")]
        public string Name { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Alias between 3 and 50 characters")]
        public string Alias { get; set; }
        public string Gender { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Language { get; set; }
        public string Address { get; set; }
        public string Note { get; set; }
        public bool IsDeleted { get; set; }

    }
}
