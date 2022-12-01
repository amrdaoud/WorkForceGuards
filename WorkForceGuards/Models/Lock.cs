using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkForceManagementV0.Models
{
    public class Lock
    {
        public int Id { get; set; }
        public int ScheduleDetailId { get; set; }
        public string EmployeeName { get; set; }
        public int OldActivityId { get; set; }
        public string OldName { get; set; }
        public int NewActivityId { get; set; }
        public string NewName { get; set; }
    }
}
