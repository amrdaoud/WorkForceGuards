using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkForceManagementV0.Models.Bindings;
using WorkForceManagementV0.Models.DTO;

namespace WorkForceManagementV0.Repositories.Interfaces
{
    public interface IDailyAttendanceService
    {
        DataWithError CreateScheduleAttendance(int scheduleId);
        DataWithError CreateAttendance();
        DataWithError CreateStaffScheduleAttendance(int scheduleId, int staffId);
        List<AttendanceSummary> GetAttendanceSummary(int scheduleId, int? forecastId);
        // DataWithError GetUnpublishedSchedule();
    }
}
