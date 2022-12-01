using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkForceManagementV0.Models;
using WorkForceManagementV0.Models.Bindings;
using WorkForceManagementV0.Models.DTO;

namespace WorkForceManagementV0.Repositories.Interfaces
{
   public interface IAssetService
    {
        List<AssetDto> GetAll();//GET()
        AssetDto GetById(int id);// GET(ID)
        DataWithError Add(Asset model); //POST(Location)
        DataWithError Update(Asset model); //PUT(ID,Location)
        bool Delete(int id);//Delete(ID)
        FullAssetDto GetAll(FilterModel f);

        bool CheckUniqValue(Asset value);

        bool CheckValue(string value);

    }
}
