using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
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
            var sub = db.SubLocations.ToList();
            data.Result = sub;
            data.ErrorMessage = null;
            return data;

        }
        public DataWithError UpdateSublocation(SubLocation model)
        {
            DataWithError data = new DataWithError();
            //if (CheckUniqValue(model))

            //{

                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();
                data.Result = model;
                data.ErrorMessage = null;
                return data;

            //}
            //data.Result = null;
            //data.ErrorMessage = "Please check your data ";
            //return data;




        }
        public DataWithError Add(SubLocation model)
        {
            SubLocation subdata = new SubLocation();
            DataWithError data = new DataWithError();
            //if (CheckUniqValue(model))
            //{
                //var locationName = db.Locations.FirstOrDefault(x => x.Id == model.locationId).Name;
                //subdata.LocationId = model.locationId;
                //subdata.Name = locationName +"_" + model.Name +"_"+ model.FromDate +"_"+ model.ToDate;
                db.SubLocations.Add(model);
                db.SaveChanges();
                data.Result = model;
                data.ErrorMessage = null;
                return data;
            //}
            //data.Result = null;
            //data.ErrorMessage = "Please check your data ";
            //return data;



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

      

      
    }
}
