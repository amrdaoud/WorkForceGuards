using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkForceManagementV0.Models.Bindings
{
    public class ScheduleDetailBinding
    {
        public int Id { get; set; }
        public int StaffId { get; set; }
        public int IntervalId { get; set; }
        public int DailyAttendenceId { get; set; }
    }
}
