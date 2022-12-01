using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkForceManagementV0.Models;

namespace WorkForceManagementV0.Helpers
{
    public interface IFinalSchedule
    {
        bool BuildShedule(int scheduleId);
       bool /* List<DataInserted>*/ BuildAcceptedBreakCount(DateTime day);
    }
}
