using System.Collections.Generic;
using System.Threading.Tasks;
using WorkForceGuards.Models;
using WorkForceManagementV0.Models;
using WorkForceManagementV0.Models.Bindings;

namespace WorkForceGuards.Repositories.Interfaces
{
    public interface IDailyAttendancePatternService
    {
        DataWithError GetAll();
        DailyAttendancePatternsDTO AddUpdate(DailyAttendancePattern model);
        DataWithError Upload(List<DailyAttendancePatternsUpload> model);
        DataWithError Delete(int id);
        Task<DataWithError> GenerateSchedule();
        DataWithError GetEligibleStaffMembers(List<ScheduleDetailManipulate> models);
    }
}
