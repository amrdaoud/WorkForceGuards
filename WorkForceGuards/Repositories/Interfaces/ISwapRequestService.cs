using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WorkForceManagementV0.Models;
using WorkForceManagementV0.Models.Bindings;

namespace WorkForceManagementV0.Repositories.Interfaces
{
    public interface ISwapRequestService
    {
        DataWithError GetMySchedules(string alias);
        DataWithError GetMyDayOffs(string alias, int scheduleId);
        DataWithError GetSiblings(string alias, int scheduleId);
        DataWithError GetDestinationDayOffs(int staffId, int scheduleId);
        DataWithError AddRequest(SwapRequest model);
        DataWithError Approve(SwapRequestApprovalBinding model, ClaimsPrincipal user);
        DataWithError GetInvolvedRequests(ClaimsPrincipal user);
        //DataWithError ReverseSwapRequest(int requestId, string alias);
        //DataWithError GetRequestDetails(int requestId);
    }
}
