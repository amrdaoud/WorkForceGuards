using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WorkForceManagementV0.Models.Bindings
{
    public class DayOffApprovalBinding
    {
        [Required]
        public int StaffMemberId { get; set; }
        [Required]
        public int ScheduleId { get; set; }
        [Required]
        public int AcceptedBreakTypeOption { get; set; }
        [Required]
        public int ShiftId { get; set; }
        [Required]
        public AccepedDayOff AccepedDayOffInfo { get; set; } = new AccepedDayOff();
    }

    public class AccepedDayOff
    {
        [Required]
        public int  Id { get; set; }
        [Required]
        public string  Day1 { get; set; }
        [Required]
        public string  Day2 { get; set; }
    }
}
