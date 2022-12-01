using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WorkForceManagementV0.Models.Bindings;

namespace WorkForceManagementV0.Models
{
    public class Forecast
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string  Name { get; set; }
        [Required]
        public double DurationTolerance { get; set; }
        [Required]
        public double OfferedTolerance { get; set; }
        [Required]
        public double ServiceLevel { get; set; }
        [Required]
        public int ServiceTime { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate{ get; set; }
        public String ExceptDates { get; set; }
        public bool IsSaved { get; set; }

        public virtual ICollection<ForecastDetails> ForecastDetails{ get; set; }
        public Forecast()
        {

        }
        public Forecast (ForecastBindingModel model)
        {
            Id = model.Id ?? 0;
            Name = model.Name;
            DurationTolerance = model.DurationTolerance;
            OfferedTolerance = model.OfferedTolerance;
            ServiceLevel = model.ServiceLevel;
            ServiceTime = model.ServiceTime;
            StartDate = model.StartDate;
            EndDate = model.EndDate;
            IsSaved = false;
            ExceptDates = model.ExceptDates == null ? "" : string.Join(",", model.ExceptDates.Select(x => x.ToString("dd/MM/yyyy")).ToArray());
        }
        

    }
}
