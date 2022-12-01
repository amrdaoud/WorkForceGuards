using System;

namespace WorkForceManagementV0.Models
{
    public class IpccAgent
    {
        public int Id { get; set; }
        public int StaffMemberEmployeeId { get; set; }
        public string StaffMemberName { get; set; }
        public DateTime UtcDate { get; set; }
        public DateTime LocalDate { get; set; }
        public int Duration { get; set; }
    }
}
