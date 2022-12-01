using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkForceManagementV0.Models;
using WorkForceManagementV0.Models.Bindings;

namespace WorkForceManagementV0.Repositories.Interfaces
{
   public interface IAssetTypeService
    {
        List<AssetType> GetAll();//GET()
        AssetType GetById(int id);// GET(ID)
        DataWithError Add(AssetType model); //POST(Location)
        DataWithError Update(AssetType model); //PUT(ID,Location)
        bool Delete(int id);//Delete(ID) 

        bool CheckUniqValue(AssetType value);

        bool CheckValue(string value);

    }
}
