using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkForceManagementV0.Models;
using WorkForceManagementV0.Models.Bindings;

namespace WorkForceManagementV0.Repositories.Interfaces
{
   public interface IStaffTypeService
    {
        List<StaffType> GetAll();//GET()
        StaffType GetById(int id);// GET(ID)
        DataWithError Add(StaffType model); //POST(Location)
        DataWithError Update(StaffType model); //PUT(ID,Location)
        bool Delete(int id);//Delete(ID)

        bool CheckUniqValue(StaffType value);
        bool CheckValue(string value);

    }
}
