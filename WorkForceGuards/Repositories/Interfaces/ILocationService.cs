using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkForceManagementV0.Models;
using WorkForceManagementV0.Models.Bindings;

namespace WorkForceManagementV0.Repositories.Interfaces
{
    public interface ILocationService
    {
        List<Location> GetAll();//GET()
        Location GetById(int id);// GET(ID)
        DataWithError Add(Location model); //POST(Location)
        DataWithError Update(Location model); //PUT(ID,Location)
        bool Delete(int id);//Delete(ID)

        bool CheckUniqValue(Location value);

        bool CheckValue(string value);

    }
}
