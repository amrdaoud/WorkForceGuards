using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkForceManagementV0.Models.Bindings
{
    public class ShiftBinding
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public TimeSpan EarlyStart { get; set; }

        public TimeSpan LateEnd { get; set; }

        public double ShiftDuration { get; set; }


    }
}
