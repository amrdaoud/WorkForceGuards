using System;
using System.Security.Claims;
using WorkForceManagementV0.Models.Bindings;

namespace WorkForceManagementV0.Repositories.Interfaces
{
    public interface IReportAdheranceService
    {
        DataWithError Report(ReportFilter model, ClaimsPrincipal user);
       DataWithError ActivityReport(DateTime RequestDate, ClaimsPrincipal user);



    }
}
