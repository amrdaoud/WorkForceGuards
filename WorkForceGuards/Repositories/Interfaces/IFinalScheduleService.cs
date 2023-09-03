using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WorkForceManagementV0.Models.Bindings;

namespace WorkForceManagementV0.Repositories.Interfaces
{
    public interface IFinalScheduleService
    {
       public  SuccessWithMessage GenerateSchedule(int scheduleId, int forecastId);
       
        public DataWithError PublishSchedule(int id );
        public DataWithError schedulebystaff(int scheduleId, int staffId, ClaimsPrincipal user);
        public DataWithError schedulebyDate(int scheduleId, DateTime day);
        public DataWithError schedulebyDatePage(int scheduleId, DateTime day, int pageIndex, int pageSize, string searchQuery, string type, ClaimsPrincipal user);
        public DataWithError schedulebystaffday(int scheduleId, int staffId, DateTime day, ClaimsPrincipal user);
        public DataWithError  schedulebyId(int scheduleId);
        public DataWithError schedulebyIdpage(int scheduleId, int pageIndex, int pageSize, string searchQuery, string type, ClaimsPrincipal user);
        public DataWithError EditScheduleDetails(int scheduleDetailId, int activityId, ClaimsPrincipal user);
        DataWithError EditScheduleDetailsMultiple(EditScheduleDetailsBinding model, ClaimsPrincipal user);
        DataWithError RemoveScheduleDetail(int scheduleDetailId, ClaimsPrincipal user);
        public DataWithError EditDailyAttendance(int dailyAttendanceId, int attendanceTypeId, ClaimsPrincipal user);
        public DataWithError EditSublocation(int dailyAttendanceId, int sublocationId, ClaimsPrincipal user);
        public DataWithError EditDailyAttendanceShift(int dailyAttendanceId, int transportationId, ClaimsPrincipal user);
        public DataWithError EditDailyAttendanceShiftGrd(int dailyAttendanceId, int transportationId, ClaimsPrincipal user);
        public DataWithError UndoDailyAttendance(int bkpId, ClaimsPrincipal user);
        public DataWithError UndoDailyAttendanceReturnDetails(int dailyAttendanceId, ClaimsPrincipal user);
        public DataWithError GetDailyAttendanceBackups(int dailyAttendanceId);
        public DataWithError AddScheduleDetail(int dailyAttendanceId, int intervalId, int activityId, ClaimsPrincipal user);
        DataWithError AddScheduleDetailMultiple(AddScheduleDetailsBinding model, int scheduleId, ClaimsPrincipal user);
        DataWithError schedulebyIdpageAll(int scheduleId, int pageIndex, int pageSize, string searchQuery, string type, ClaimsPrincipal user);
        DataWithError schedulebyDatePageAll(int scheduleId, DateTime day, int pageIndex, int pageSize, string searchQuery, string type, ClaimsPrincipal user);
        DataWithError ManipulateScheduleDetails(ManipulateDetails model, int scheduleId, ClaimsPrincipal user, bool isStaff);
        DataWithError CopyDailyAttendance(ManipulateDailyAttendance model, ClaimsPrincipal user);

    }
}
