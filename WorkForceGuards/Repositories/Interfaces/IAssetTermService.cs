using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkForceManagementV0.Models;

namespace WorkForceManagementV0.Repositories.Interfaces
{
   public interface IAssetTermService
    {
        List<AssetTerm> GetAll();//GET()
        AssetTerm GetById(int id);// GET(ID)
        AssetTerm Add(AssetTerm model); //POST(AssetTerm)
        AssetTerm Update(AssetTerm model); //PUT(ID,AssetTerm)
        bool Delete(int id);//Delete(ID)
    }
}
