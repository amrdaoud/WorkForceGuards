using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkForceManagementV0.Models.Bindings;

namespace WorkForceManagementV0.Repositories.Config
{
    public interface IAppConfig
    {
        dynamic GetRolesMapping();
        bool SetRolesMapping(RolesMappingBinding model);
    }
}
