using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WorkForceManagementV0.Models
{
    public class ShiftRule
    {
        [Key]
        public int Id { get; set; }

        [Range(typeof(int), "0", "8")]

        public int StartAfter { get; set; }

        [Range(typeof(int), "0", "8")]

        public int EndBefore { get; set; }

        [Range(typeof(int), "0", "8")]

        public int BreakBetween { get; set; }

        public bool IsDeleted { get; set; }

    }
}
