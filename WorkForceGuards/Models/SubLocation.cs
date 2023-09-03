using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using WorkForceGuards.Models;

namespace WorkForceManagementV0.Models
{
    public class SubLocation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [ForeignKey("Location")]
        public int LocationId { get; set; }
        public virtual Location Location { get; set; }
        public virtual ICollection<TransportationRoute> TransportationRoutes { get; set; }
        public virtual ICollection<Headcount> Headcounts { get; set; }
    }
}
