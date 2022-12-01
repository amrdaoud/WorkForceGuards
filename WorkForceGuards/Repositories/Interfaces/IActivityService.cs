using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkForceManagementV0.Models;
using WorkForceManagementV0.Models.Bindings;

namespace WorkForceManagementV0.Repositories.Interfaces
{
   public  interface IActivityService
    {

        List<Activity> GetAll();
        DataWithError Add(Activity model);
        DataWithError Update(Activity model);
        Activity GetById(int id);
        bool delete(int id);
        bool CheckUniqValue(Activity value);
        bool CheckValue(string value,string ignoreName);
        bool CheckValueColor(string value, string ignoreName);
        List<Color> AvailableColor(string CurrentColor);
        List<Color> GetColors();

    }
}
