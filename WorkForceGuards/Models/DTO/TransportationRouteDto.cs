using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkForceManagementV0.Models.DTO
{
    public class TransportationRouteDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LocationName { get; set; }
        public int LocationId { get; set; }

        public int ArriveTimeInterval_Id { get; set; }
        public int? DepartTimeInterval_Id { get; set; }
        public bool IsIgnored { get; set; }

    }
}
