using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkForceManagementV0.Contexts;
using WorkForceManagementV0.Models;
using WorkForceManagementV0.Repositories.Interfaces;

namespace WorkForceManagementV0.Repositories
{
    public class AssetTermValueServive: IAssetTermValueServive
    {

        private readonly ApplicationDbContext db;
        public AssetTermValueServive(ApplicationDbContext context)
        {
            db = context;
        }
        public List<AssetTermValue> GetAll()
        {
            return db.AssetTermValues.Include(x => x.AssetType).Include(y => y.AssetTerm).ToList();
        }

        public List<AssetTermValue> GetById(int id)
        {
            return db.AssetTermValues.Where(z=>z.Id==id).Include(x => x.AssetType).Include(y => y.AssetTerm).ToList();

        }




        public AssetTermValue Add(AssetTermValue model)
        {
            db.AssetTermValues.Add(model);
            db.SaveChanges();
            return model;
        }


        public AssetTermValue Update(AssetTermValue model)
        {
            db.Entry(model).State = EntityState.Modified;

            db.SaveChanges();

            return model;

        }


        public bool Delete(int id)
        {
            var value = db.AssetTermValues.Find(id);

            db.AssetTermValues.Remove(value);

            db.SaveChanges();

            return true;
        }









    }
}
