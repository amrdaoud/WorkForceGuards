using System;

namespace WorkForceManagementV0.Models.DTO
{
    public class AdherancebyTransportationDto
    {
         
        public int HosId { get; set; }
        public string HosName { get; set; }
        public string StaffName { get; set; }
        public int StaffId { get; set; }
        public int? TransportId { get; set; }
        public DateTime? Day { get; set; }
        public double? Adherance { get; set; }
    
}
}
