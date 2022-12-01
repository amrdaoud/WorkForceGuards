using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkForceManagementV0.Models;
using WorkForceManagementV0.Models.Bindings;

namespace WorkForceManagementV0.Repositories.Interfaces
{
   public interface IHeadOfSectionService
    {
        List<HeadOfSection> GetAll();//GET()
        HeadOfSection GetById(int id);// GET(ID)
        DataWithError Add(HeadOfSection model); //POST(Location)
        DataWithError Update(HeadOfSection model); //PUT(ID,Location)
        bool Delete(int id);//Delete(ID)

        bool CheckUniqValue(HeadOfSection value);

        bool CheckIdValue(int employeeId);
        bool CheckNameValue(string value);

        bool CheckAliasValue(string alias);

        bool CheckEmailValue(string email);

    }
}
