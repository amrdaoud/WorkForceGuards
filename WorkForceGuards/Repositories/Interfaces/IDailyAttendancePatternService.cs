using System.Collections.Generic;
using WorkForceGuards.Models;

namespace WorkForceGuards.Repositories.Interfaces
{
    public interface IDailyAttendancePatternService
    {
        List<DailyAttendancePatternsDTO> GetAll();
        DailyAttendancePatternsDTO AddUpdate(DailyAttendancePattern model);
    }
}
