using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkForceManagementV0.Contexts;
using WorkForceManagementV0.Models;
using WorkForceManagementV0.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using WorkForceManagementV0.Models.Bindings;

namespace WorkForceManagementV0.Repositories
{
    public class LocationService: ILocationService
    {
        private readonly ApplicationDbContext db;
        public LocationService(ApplicationDbContext context)
        {
            db = context;
        }

        public DataWithError Add(Location model)
        {
            DataWithError data = new DataWithError();
              if (CheckUniqValue(model))
              {
                db.Locations.Add(model);
                db.SaveChanges();
                data.Result = model;
                data.ErrorMessage = null;
                return data;
               }

             
            data.Result = null;
            data.ErrorMessage = "Duplicated Name Inserted";
            return data;


        }

        public bool Delete(int id)
        {
            var location = db.Locations.Find(id);
            try
            {
                db.Locations.Remove(location);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
            
        }

        public List<Location> GetAll()
        {
            return db.Locations.ToList();
        }

        public Location GetById(int id)
        {
            var location = db.Locations.Find(id);
            return location;
        }

        public DataWithError Update(Location model)
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
            data.ErrorMessage = "Duplicated Name Inserted";
            return data;

        }

        public bool CheckUniqValue(Location value)
        {
            var same = db.Locations.FirstOrDefault(a => a.Name.ToLower() == value.Name.ToLower() && a.Id != value.Id);
            if (same == null)
            {
                return true;
            }
            return false;

        }

        public bool CheckValue(string value)
        {
            var same = db.Locations.FirstOrDefault(a => a.Name.ToLower() == value.ToLower());
            if (same == null)
            {
                return true;
            }
            return false;
        }

    }
}
