using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkForceManagementV0.Contexts;
using WorkForceManagementV0.Models;
using WorkForceManagementV0.Models.Bindings;
using WorkForceManagementV0.Repositories.Interfaces;

namespace WorkForceManagementV0.Repositories
{
    public class AssetTypeService: IAssetTypeService
    {
        private readonly ApplicationDbContext db;
        public AssetTypeService(ApplicationDbContext context)
        {
            db = context;
        }
        public List<AssetType> GetAll()
        {
            return db.AssetTypes.ToList();
        }


        public AssetType GetById(int id)
        {
            return db.AssetTypes.Find(id);
        }


        public DataWithError Add(AssetType model)
        {
            DataWithError data = new DataWithError();
            if (CheckUniqValue(model))
            {
                db.AssetTypes.Add(model);
                db.SaveChanges();
                data.Result = model;
                data.ErrorMessage = null;
                return data;
            }
            data.Result = null;
            data.ErrorMessage = "Duplicated Barcode Inserted";
            return data;

        }




        public DataWithError Update(AssetType model)
        {
            DataWithError data = new DataWithError();
            if (CheckUniqValue(model))
            {
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                data.Result = model;
                data.ErrorMessage = null;
                return data;
            }



            data.Result = null;
            data.ErrorMessage = "Duplicated Barcode Inserted";
            return data;
        }

        public bool Delete(int id)
        {
            var assetType = db.AssetTypes.Find(id);
            db.AssetTypes.Remove(assetType);
            db.SaveChanges();
            return true;

        }

        public bool CheckUniqValue(AssetType value)
        {
            var same = db.AssetTypes.FirstOrDefault(a => a.Name.ToLower() == value.Name.ToLower() && a.Id != value.Id);
            if (same == null)
            {
                return true;
            }
            return false;

        }

        public bool CheckValue(string value)
        {
            var same = db.AssetTypes.FirstOrDefault(a => a.Name.ToLower() == value.ToLower());
            if (same == null)
            {
                return true;
            }
            return false;
        }


    }
}
