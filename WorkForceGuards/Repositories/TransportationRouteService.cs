using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using WorkForceManagementV0.Contexts;
using WorkForceManagementV0.Models;
using WorkForceManagementV0.Models.Bindings;
using WorkForceManagementV0.Models.DTO;
using WorkForceManagementV0.Repositories.Interfaces;

namespace WorkForceManagementV0.Repositories
{
    public class TransportationRouteService : ITransportationRouteService
    {
        private readonly ApplicationDbContext db;
        public TransportationRouteService(ApplicationDbContext context)
        {
            db = context;
        }
        public List<TransportationRouteBinding> GetAll()
        {
            List<TransportationRouteBinding> Result = new List<TransportationRouteBinding>();
            var transportations = db.TransportationRoutes.Include(x => x.SubLocation).Where(x => !x.IsDeleted).ToList();



            foreach (var s in transportations)
            {
                TransportationRouteBinding data = new TransportationRouteBinding();

                data.Id = s.Id;
                data.Name = s.Name;
                data.Description = s.Description;
                data.LocationName = s.SubLocation != null ? s.SubLocation.Name : "Deleted";
                data.LocationId = s.SubLocation != null ? s.SubLocation.Id : 0;
                data.ArriveTime = ConvertintoDatetime((int)s.ArriveIntervalId);
                data.DepartTime = ConvertintoDatetime((int)s.DepartIntervalId);
                data.IsIgnored = s.IsIgnored;
                data.IsDeleted = s.IsDeleted;

                Result.Add(data);

            }
            return Result;
        }


        public List<TransportationRouteBinding> GetById(int id)
        {
            var trans = db.TransportationRoutes.Where(y => y.Id == id).Include(x => x.SubLocation).ToList();
            List<TransportationRouteBinding> Result = new List<TransportationRouteBinding>();
            foreach (var s in trans)
            {
                TransportationRouteBinding data = new TransportationRouteBinding();

                data.Id = s.Id;
                data.Name = s.Name;
                data.Description = s.Description;
                data.LocationName = s.SubLocation != null ? s.SubLocation.Name : "Deleted";
                data.LocationId = s.SubLocation != null ? s.SubLocation.Id : 0;
                data.ArriveTime = ConvertintoDatetime((int)s.ArriveIntervalId);
                data.DepartTime = ConvertintoDatetime((int)s.DepartIntervalId);
                data.IsIgnored = s.IsIgnored;
                data.IsDeleted = s.IsDeleted;

                Result.Add(data);

            }
            return Result;


        }

        public DataWithError Add(TransportationRouteBinding model)
        {
            DataWithError data = new DataWithError();
            if (CheckUniqValue(model))
            {


                TransportationRoute InsertRoute = new TransportationRoute();
                TransportationRouteBinding ResponseRoute = new TransportationRouteBinding();

                //var startQuarter = ConvertTimeToNumber(model.EarlyStart);
                //var endQuarter= ConvertTimeToNumber(model.LateEnd);
                var StartInterval = ConvertTimeTostartInterval(model.ArriveTime, model.DepartTime);
                var EndInterval = ConvertTimeToendtInterval(model.ArriveTime, model.DepartTime);

                InsertRoute.Name = model.Name;
                InsertRoute.Description = model.Description;
                InsertRoute.SubLocationId = model.LocationId;
                InsertRoute.ArriveIntervalId = StartInterval;
                InsertRoute.DepartIntervalId = EndInterval;
                InsertRoute.IsDeleted = model.IsDeleted;
                InsertRoute.IsIgnored = model.IsIgnored;


                db.TransportationRoutes.Add(InsertRoute);
                db.SaveChanges();
                ResponseRoute.Id = InsertRoute.Id;
                ResponseRoute.Name = model.Name;
                ResponseRoute.Description = model.Description;
                ResponseRoute.LocationId = model.LocationId;
                ///return the model 
                ResponseRoute.ArriveTime = model.ArriveTime;
                ResponseRoute.DepartTime = model.DepartTime;
                ResponseRoute.IsDeleted = model.IsDeleted;
                ResponseRoute.IsIgnored = model.IsIgnored;
                ResponseRoute.LocationName = db.Locations.FirstOrDefault(x => x.Id == model.LocationId).Name;

                data.Result = ResponseRoute;
                data.ErrorMessage = null;
                return data;
                //db.TransportationRoutes.Add(model);

                //db.SaveChanges();

                //var trans = db.TransportationRoutes.Select(x => new TransportationRouteDto
                //{
                //    Id = x.Id,
                //    Name = x.Name,
                //    Description = x.Description,
                //    LocationName = x.Location!=null?x.Location.Name:"Deleted",
                //    LocationId = x.Location != null ? x.Location.Id :0,
                //    ArriveIntervalId = x.ArriveIntervalId,
                //    DepartIntervalId = x.DepartIntervalId ,
                //    IsIgnored = x.IsIgnored

                //}).FirstOrDefault(x => x.Id == model.Id);

                //data.Result = trans;
                //data.ErrorMessage = null;
                //return data;
            }

            data.Result = null;
            data.ErrorMessage = "Duplicated Name Inserted";
            return data;


        }


        public DataWithError Update(TransportationRouteBinding model)
        {
            TransportationRouteBinding Response = new TransportationRouteBinding();
            DataWithError data = new DataWithError();

            if (CheckUniqValue(model))
            {
                TransportationRoute UpdateRoute = new TransportationRoute();
                var StartInterval = ConvertTimeTostartInterval(model.ArriveTime, model.DepartTime);
                var EndInterval = ConvertTimeToendtInterval(model.ArriveTime, model.DepartTime);
                UpdateRoute.Id = model.Id;
                UpdateRoute.Name = model.Name;
                UpdateRoute.Description = model.Description;
                UpdateRoute.SubLocationId = model.LocationId;
                ///return the model 
                UpdateRoute.ArriveIntervalId = StartInterval;
                UpdateRoute.DepartIntervalId = EndInterval;
                UpdateRoute.IsDeleted = model.IsDeleted;
                UpdateRoute.IsIgnored = model.IsIgnored;


                db.Entry(UpdateRoute).State = EntityState.Modified;
                db.SaveChanges();

                Response.Id = model.Id;
                Response.Name = model.Name;
                Response.ArriveTime = ConvertintoDatetime((int)UpdateRoute.ArriveIntervalId);
                Response.DepartTime = ConvertintoDatetime((int)UpdateRoute.DepartIntervalId);
                Response.Description = model.Description;
                Response.LocationId = model.LocationId;
                Response.LocationName = db.Locations.FirstOrDefault(x => x.Id == model.LocationId).Name;
                Response.IsDeleted = model.IsDeleted;
                Response.IsIgnored = model.IsIgnored;
                //UpdateRoute.IsDeleted = model.IsDeleted;
                //UpdateRoute.IsIgnored = model.IsIgnored;

                data.Result = Response;
                data.ErrorMessage = null;
                return data;
                //    db.Entry(model).State = EntityState.Modified;

                //    db.SaveChanges();

                //    var trans = db.TransportationRoutes.Select(x => new TransportationRouteDto
                //    {
                //        Id = x.Id,
                //        Name = x.Name,
                //        Description = x.Description,
                //        LocationName = x.Location.Name,
                //        LocationId = x.Location.Id,
                //        ArriveIntervalId = x.ArriveIntervalId,
                //        DepartIntervalId = x.DepartIntervalId,
                //        IsIgnored = x.IsIgnored

                //    }).FirstOrDefault(x => x.Id == model.Id);
                //    data.Result = trans;
                //    data.ErrorMessage = null;
                //    return data;
            }

            data.Result = null;
            data.ErrorMessage = "Duplicated Name Inserted";
            return data;
        }

        public bool Delete(int id)
        {
            var trans = db.TransportationRoutes.Find(id);

            db.TransportationRoutes.Remove(trans);

            db.SaveChanges();

            return true;
        }

        public bool CheckUniqValue(TransportationRouteBinding value)
        {
            var same = db.TransportationRoutes.FirstOrDefault(a => a.Name.ToLower() == value.Name.ToLower() && a.Id != value.Id);
            if (same == null)
            {
                return true;
            }
            return false;

        }

        public bool CheckValue(string value)
        {
            var same = db.TransportationRoutes.FirstOrDefault(a => a.Name.ToLower() == value.ToLower());
            if (same == null)
            {
                return true;
            }
            return false;
        }

        public int ConvertTimeTostartInterval(DateTime Start, DateTime End)
        {
            string hourMinute = Start.ToString("HH:mm");
            TimeSpan time = TimeSpan.Parse(hourMinute);

            var data = db.Intervals.FirstOrDefault(x => x.TimeMap == time);
            return data.Id;

        }

        public int ConvertTimeToendtInterval(DateTime Start, DateTime End)
        {
            string hourStart = Start.ToString("HH:mm");
            string hourEnd = End.ToString("HH:mm");

            TimeSpan timeStart = TimeSpan.Parse(hourStart);
            TimeSpan timeEnd = TimeSpan.Parse(hourEnd);
            var data = (timeStart > timeEnd) ? db.Intervals.Where(x => x.OrderMap >= 95).FirstOrDefault(x => x.TimeMap == timeEnd) : db.Intervals.FirstOrDefault(x => x.TimeMap == timeEnd);
            return data.Id;

        }

        public DateTime ConvertintoDatetime(int IntervalId)
        {
            var data = db.Intervals.FirstOrDefault(x => x.Id == IntervalId);
            return Convert.ToDateTime(data.TimeMap.ToString());
        }
        public DataWithError AddTransportation(TransportationBinding model)
        {
            DataWithError data = new DataWithError();

            TransportationRoute trans = new TransportationRoute();

            var FromDate = ConvertTimeTostartInterval(model.FromDate, model.ToDate);
            var ToDate = ConvertTimeToendtInterval(model.FromDate, model.ToDate);
            var subloc = db.SubLocations.FirstOrDefault(x => x.Id == model.SublocationId);
            var LocName = db.Locations.FirstOrDefault(x => x.Id == subloc.LocationId).Name;
            var Name = LocName + "_" + subloc.Name + "_" + FromDate + "_" + ToDate;
            if (CheckUniqValue(model.SublocationId, Name))
            {
                trans.ArriveIntervalId = FromDate;
                trans.DepartIntervalId = ToDate;
                trans.Name = Name;
                trans.Description = null;
                trans.SubLocationId = model.SublocationId;
                trans.IsDeleted = false;
                trans.IsIgnored = false;
                db.TransportationRoutes.Add(trans);
                db.SaveChanges();

                data.Result = trans;
                data.ErrorMessage = null;
                return data;
            }
            data.Result = null;
            data.ErrorMessage = "the insert SublocationId is already created";
            return data;



        }
        public bool CheckUniqValue(int Id, string Name)
        {
            //var FromDate = ConvertTimeTostartInterval(model.FromDate, model.ToDate);
            //var ToDate = ConvertTimeToendtInterval(model.FromDate, model.ToDate);
            //var subloc = db.SubLocations.FirstOrDefault(x => x.Id == model.SublocationId);
            //var LocName = db.Locations.FirstOrDefault(x => x.Id == subloc.LocationId).Name;
            //var Name = LocName + "_" + subloc.Name + "_" + FromDate + "_" + ToDate;
            var sam = db.TransportationRoutes.FirstOrDefault(x => x.SubLocationId == Id && x.Name == Name);
            if (sam == null)
            {
                return true;
            }

            return false;
        }
        public bool checkSublocValue(TransportationBinding model)
        {
            var FromDate = ConvertTimeTostartInterval(model.FromDate, model.ToDate);
            var ToDate = ConvertTimeToendtInterval(model.FromDate, model.ToDate);
            var subloc = db.SubLocations.FirstOrDefault(x => x.Id == model.SublocationId);
            var LocName = db.Locations.FirstOrDefault(x => x.Id == subloc.LocationId).Name;
            var Name = LocName + "_" + subloc.Name + "_" + FromDate + "_" + ToDate;
            var sam = db.TransportationRoutes.FirstOrDefault(x => x.SubLocationId == model.SublocationId && x.Name == Name);
            if (sam == null)
            {
                return true;
            }

            return false;

        }

    }
}
