using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using WorkForceGuards.Models.DTO;
using WorkForceManagementV0.Contexts;
using WorkForceManagementV0.Migrations;
using WorkForceManagementV0.Models;
using WorkForceManagementV0.Models.Bindings;
using WorkForceManagementV0.Repositories.Interfaces;

namespace WorkForceManagementV0.Repositories
{
    public class SublocationService :ISublocationService
    {
      
        private readonly ApplicationDbContext db;
        public SublocationService (ApplicationDbContext context)
        {
            db = context;
        }

        public DataWithError GetAll()
        {

            DataWithError data = new DataWithError();
            var sub = db.SubLocations.Include(x => x.Location).Select(x => new SubLocationViewModel(x)).ToList();
            data.Result = sub;
            data.ErrorMessage = null;
            return data;

        }
        public DataWithError UpdateSublocation(SubLocation model)
        {
            DataWithError data = new DataWithError();
            if(!CheckUniqValue(model))
            {
                data.Result = null;
                data.ErrorMessage = "Sublocation with the same name exists!";
                return data;
            }
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            db.Entry(model).Reference(s => s.Location).Load();
            data.Result = new SubLocationViewModel(model);
            data.ErrorMessage = null;
            return data;

        }
        public DataWithError Add(SubLocation model)
        {
            DataWithError data = new DataWithError();
            if (!CheckUniqValue(model))
            {
                data.Result = null;
                data.ErrorMessage = "Sublocation with the same name exists!";
                return data;
            }
            db.SubLocations.Add(model);
            db.SaveChanges();
            db.Entry(model).Reference(s => s.Location).Load();
            data.Result = new SubLocationViewModel(model);
            data.ErrorMessage = null;
            return data;
        }

        //public bool CheckUniqValue(TransportationBinding value)
        //{
        //    var same = db.SubLocations.FirstOrDefault(a => (a.Name == value.) && a.Id != value.LocationId);
        //    if (same == null)
        //    {
        //        return true;
        //    }
        //    return false;
        //}

        public bool CheckUniqValue(SubLocation value)
        {
            var same = db.Locations.FirstOrDefault(a => a.Name.ToLower() == value.Name.ToLower() && a.Id != value.Id);
            if (same == null)
            {
                return true;
            }
            return false;

        }
        public bool CheckUniq(string value)
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
