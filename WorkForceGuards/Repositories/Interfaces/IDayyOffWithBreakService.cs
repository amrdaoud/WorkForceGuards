using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkForceManagementV0.Models;
using WorkForceManagementV0.Models.Bindings;
using WorkForceManagementV0.Models.DTO;

namespace WorkForceManagementV0.Repositories.Interfaces
{
   public  interface IDayyOffWithBreakService
    {
       List<DayOffWithBreakDto> GetAll();  // As Admin 

        DataWithError GetById(int empid);  // As user
        DataWithError GetByAlias(string alias);
        DataWithError Add(DayOffWithBreakOptionBinding model); // As User 

        DataWithError Edit(DayOffWithBreakOptionBinding model); // As User

        DayOffWithBreakOptionBinding GetStaff(int id);


        DataWithError ApprovedOption(DayOffApprovalBinding approval);  //As Admin

        bool SetDailyAttendence(DayOffApprovalBinding model);
        bool setForeCasting(int ScheduleId);
        bool setForecastDetails(int ForecastId);
        bool setForecasthistory();

        bool CheckStaffOptions();

        DataWithError CreateDailyAttendence();

        List<AttendanceType> GetAttendanceType();

        bool CheckDayOffOptions(DayOffWithBreakOptionBinding model);

        Dictionary<int, string> GetDaysOfWeek();

        DataWithError A_ApprovedOption(DayOffApprovalBinding model);
        DataWithError UploadDayOffWithBreaks(List<DayOffWithBreaksUpload> models);


    }
}
