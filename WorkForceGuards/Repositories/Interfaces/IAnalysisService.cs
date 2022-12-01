using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkForceManagementV0.Models.Bindings;

namespace WorkForceManagementV0.Repositories.Interfaces
{
    public interface IAnalysisService
    {
        DataWithError NeededVsAvailable(int scheduleId, DateTime day);
        DataWithError LeaderBoard(int scheduleId, int pageIndex, int pageSize);
        DataWithError AdherenceByHos(int scheduleId);
    }
}
