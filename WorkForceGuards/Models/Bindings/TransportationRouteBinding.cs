using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkForceManagementV0.Models.Bindings
{
    public class TransportationRouteBinding
    {
        public int Id { get; set; }
 

        public string Name { get; set; }
        public string Description { get; set; }
      
        public int? LocationId { get; set; }
        public string LocationName { get; set; }


        public DateTime ArriveTime { get; set; }
       
        public DateTime DepartTime { get; set; }
        public bool IsIgnored { get; set; }
        public bool IsDeleted { get; set; }
    }
}
