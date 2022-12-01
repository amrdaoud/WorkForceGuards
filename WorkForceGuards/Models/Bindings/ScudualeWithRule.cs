using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkForceManagementV0.Models.Bindings
{
    public class ScudualeWithRule
    {
       public List<Schedule> ScheduleData { get; set; }

        public ShiftRule ShiftRuleData { get; set; }
    }
}
