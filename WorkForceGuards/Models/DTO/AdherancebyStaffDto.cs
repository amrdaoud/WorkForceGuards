using System;
using System.Collections.Generic;

namespace WorkForceManagementV0.Models.DTO
{
    public class AdherancebyStaffDto
    {
        public int HosId { get; set; }
        public string HosName { get; set; }
        public string StaffName { get; set; }
        public int? StaffId { get; set; }
        public DateTime? Day { get; set; }
        public string TransportationRoute { get; set; }
        public double? Adherance { get; set; }
        public double?  PhonebyHour {get;set;}
        public double ActivitybyHour { get; set; }
    }
}
