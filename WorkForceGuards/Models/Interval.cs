using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkForceManagementV0.Models
{
    public class Interval
    {
        public int Id { get; set; }
        public TimeSpan TimeMap { get; set; }
        public int OrderMap { get; set; }
        public int CoverMap { get; set; }
        public double Tolerance { get; set; } = 0;
    }
}
