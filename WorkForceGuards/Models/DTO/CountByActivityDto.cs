using System;

namespace WorkForceManagementV0.Models.DTO
{
    public class CountByActivityDto
    {
        public string ActivityName { get; set; }
        public string ActivityColor { get; set; }
        public int ActivityId { get; set; }
       
        public int IntervalId { get; set; }
        public TimeSpan TimeMap { get; set; }
        public int StaffCount { get; set; }
    }

    
}
