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
    public class AssetTermService: IAssetTermService
    {
        private readonly ApplicationDbContext db;
        public AssetTermService(ApplicationDbContext context)
        {
            db = context;
        }
        public List<AssetTerm> GetAll()
        {
            return db.AssetTerms.Include(x=>x.Values).ToList();
        }

        public AssetTerm GetById(int id)
        {
            return db.AssetTerms.Find(id);
        }


        public AssetTerm Add(AssetTerm model)
        {
            db.AssetTerms.Add(model);

            db.SaveChanges();
            return model;
        }



        public AssetTerm Update(AssetTerm model)
        {
            
                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
            
           
            return model;
        }


        public bool Delete(int id)
        {
            var term = db.AssetTerms.Find(id);
            db.AssetTerms.Remove(term);

            db.SaveChanges();

            return true;
        }




    }
}
