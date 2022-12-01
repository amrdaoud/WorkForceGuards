using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkForceManagementV0.Models;
using WorkForceManagementV0.Models.Bindings;
using WorkForceManagementV0.Models.DTO;

namespace WorkForceManagementV0.Repositories.Interfaces
{
   public interface IStaffMemberService
    {
        FullStaffMemberDto GetAll(FilterModel f);
        StaffMemberDto GetById(int id);// GET(ID)
        DataWithError Add(StaffMember model); //POST(staff)
        DataWithError Update(StaffMember model); //PUT(ID,staff)
        bool Delete(int id);//Delete(ID) 
        List<int> GetEmployeeId();
        bool CheckUniqValue(StaffMember value);
        bool CheckValue(string value);
        bool CheckUniqAlias(string Alias);
        bool CheckUniqName(string Name);
        bool CheckUniqEmail(string Email);



    }
}
