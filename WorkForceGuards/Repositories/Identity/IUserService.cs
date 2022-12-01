using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkForceManagementV0.Models.Identity;
using System.Security.Claims;
namespace WorkForceManagementV0.Repositories.Identity
{
    public interface IUserService
    {
        AppUser GetUserInfo(ClaimsPrincipal user);
        string getAvatar(string alias);
    }
}
