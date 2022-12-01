using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkForceManagementV0.Models.DTO;

namespace WorkForceManagementV0.Repositories.ActiveDirectory
{
    public interface IActiveDirectory
    {
        List<string> GetGroups();
        List<string> GetGroupsBySearch(string searchQuery);
        List<ADUser> GetUsers();
        List<ADUser> GetUsersBySearch(string searchQuery);  
    }
}
