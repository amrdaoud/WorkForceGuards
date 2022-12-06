using System;

namespace WorkForceManagementV0.Models.Bindings
{
    public class TransportationBinding
    {
        public int Id { get; set; }
        //public int SublocationId { get; set; }
        public DateTime ArriveTime { get; set; }
        public DateTime DepartTime { get; set; }
    }
}
