using System;
using System.Collections.Generic;
using System.Security.Claims;
using WorkForceGuards.Models.Reports;
using WorkForceManagementV0.Models.Bindings;

namespace WorkForceManagementV0.Repositories.Interfaces
{
    public interface IReportAdheranceService
    {
        DataWithError Report(ReportFilter model, ClaimsPrincipal user);
        DataWithError ActivityReport(DateTime RequestDate, ClaimsPrincipal user);
        DataWithError StaffAttendanceReport(StaffAttendanceFilter filter, ClaimsPrincipal user);
        DataWithError StaffWorkingDaysReport(StaffAttendanceFilter filter, ClaimsPrincipal user);
        List<StaffAttendanceReport> StaffAttendanceReportDownload(StaffAttendanceFilter filter, ClaimsPrincipal user);
        List<StaffWorkingDaysReport> StaffWorkingDaysReportDownload(StaffAttendanceFilter filter, ClaimsPrincipal user);


    }
}
