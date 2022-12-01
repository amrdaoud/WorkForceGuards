using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WorkForceManagementV0.Models.Bindings
{
    public class ForecastBindingModel
    {
        public int? Id { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]

        public DateTime EndDate { get; set; }
        public List<DateTime> ExceptDates { get; set; }
        [Required]
        public double DurationTolerance { get; set; }
        [Required]
        public double OfferedTolerance { get; set; }
        [Required]
        public double ServiceLevel { get; set; }
        [Required]
        public int ServiceTime { get; set; }
    }
}
