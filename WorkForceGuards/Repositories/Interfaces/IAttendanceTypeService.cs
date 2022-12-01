using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkForceManagementV0.Models;
using WorkForceManagementV0.Models.Bindings;

namespace WorkForceManagementV0.Repositories.Interfaces
{
    public interface IAttendanceTypeService
    {
        List<AttendanceType> GetAll();
        AttendanceType GetById(int id);
        DataWithError Add(AttendanceType model);
        DataWithError Update(AttendanceType model);
        bool CheckUniqueName(string name);
        bool CheckUniqueModel(AttendanceType model);
    }
}
