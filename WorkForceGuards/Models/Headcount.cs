using System.ComponentModel.DataAnnotations.Schema;
using WorkForceManagementV0.Models;

namespace WorkForceGuards.Models
{
    public class Headcount
    {
        public int Id { get; set; }
        [ForeignKey("TransporationRoute")]
        public int TransportationRouteId { get; set; }
        [ForeignKey("Sublocation")]
        public int SublocationId { get; set; }
        public virtual TransportationRoute TransportationRoute { get; set; }
        public virtual SubLocation Sublocation { get; set; }
        public double Total { get; set; }
    }
}
