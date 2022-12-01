using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WorkForceManagementV0.Repositories.Interfaces
{
    public interface IAdherenceService
    {
        decimal? AdherenceByStaffDay(int scheduleId, int staffId, DateTime day, ClaimsPrincipal user);
        decimal? AdherenceByDay(int scheduleId, DateTime day, ClaimsPrincipal user);
        decimal? AdherenceByStaff(int scheduleId, int staffId, ClaimsPrincipal user);
        decimal? AdherenceBySchedule(int scheduleId, ClaimsPrincipal user);
        decimal? AdherenceByScheduleAll(int scheduleId, ClaimsPrincipal user);
        decimal? AdherenceByDayAll(int scheduleId, DateTime day, ClaimsPrincipal user);
    }
}
