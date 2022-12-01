using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkForceManagementV0.Models.Bindings
{
    public class SwapRequestApprovalBinding
    {
        public int RequestId { get; set; }
        public bool IsApproved { get; set; }
        public string Reason { get; set; }

    }
}
