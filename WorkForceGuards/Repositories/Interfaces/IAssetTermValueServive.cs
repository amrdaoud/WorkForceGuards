using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkForceManagementV0.Models;

namespace WorkForceManagementV0.Repositories.Interfaces
{
   public interface IAssetTermValueServive
    {
        List<AssetTermValue> GetAll();//GET()
        List<AssetTermValue> GetById(int id);// GET(ID)
        AssetTermValue Add(AssetTermValue model); //POST(AssetTerm)
        AssetTermValue Update(AssetTermValue model); //PUT(ID,AssetTerm)
        bool Delete(int id);//Delete(ID)
    }
}
