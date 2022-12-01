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
    public class ActivityService: IActivityService
    {
        private readonly ApplicationDbContext db;
        public ActivityService(ApplicationDbContext context)
        {
            db = context;
        }

        public List<Activity> GetAll()
        {
            return db.Activities.Where(x => !x.IsUndefined).ToList();
        }

        public DataWithError Add(Activity model)
        {
            DataWithError result = new DataWithError();

            if(CheckUniqValue(model))
            {
                var timezone = TimeZoneInfo.FindSystemTimeZoneById("Arab Standard Time");
                model.CreateDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timezone);
               // model.UpdateDate = TimeZoneInfo.ConvertTimeFromUtc((DateTime)model.UpdateDate, timezone);

                db.Activities.Add(model);

                db.SaveChanges();

                result.Result = model;
                result.ErrorMessage = null;
                return result;

            }

            result.Result = null;
            result.ErrorMessage = "Duplicated Name or Color Activity Inserted";
            return result;

        }


        public DataWithError Update(Activity model)
        {
            DataWithError data = new DataWithError();
            if(CheckUniqValue(model))
            {
                var timezone = TimeZoneInfo.FindSystemTimeZoneById("Arab Standard Time");
                //model.CreateDate = TimeZoneInfo.ConvertTimeFromUtc((DateTime)model.CreateDate, timezone);
                model.UpdateDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timezone);

                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();

                data.Result = model;
                data.ErrorMessage = null;
                return data;
            }

            data.Result = null;
            data.ErrorMessage = "Duplicated Name or Color Activity Inserted";
            return data;
        }

        public Activity GetById(int id)
        {
            return db.Activities.Find(id);
        }

        public bool delete(int id)
        {
            var activity = db.Activities.FirstOrDefault(x => x.Id == id);

            if(activity!=null)
            {
                activity.IsDeleted = true;
                activity.Name = "#" + activity.Name+DateTime.Now;
                activity.Color = "#" + activity.Color+DateTime.Now;
                db.Entry(activity).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            }

            return false;
        }

        public bool CheckUniqValue(Activity value)
        {
            var same = db.Activities.FirstOrDefault(a => (a.Name == value.Name || a.Color==value.Color) && a.Id != value.Id);
            if (same == null)
            {
                return true;
            }
            return false;
        }

        public bool CheckValue(string value, string ignoreName)
        {
            var same = db.Activities.FirstOrDefault(x =>  x.Name.ToLower() == value.ToLower() && x.Name.ToLower()!=ignoreName.ToLower());

            if(same==null)
            {
                return true;
            }
             return false;
            
        }


        public bool CheckValueColor(string value, string ignoreName)
        {
            var same = db.Activities.FirstOrDefault(x => x.Color.Replace("#","").ToLower() == value.ToLower() && x.Color.Replace("#","").ToLower() != ignoreName.ToLower());

            if (same == null)
            {
                return true;
            }
            return false;
        }


        public List<Color> GetColors()
        {
           return db.colors.ToList();
        }

        public List<Color> AvailableColor(string CurrentColor)
        {
            Color data = new Color();
            var UsedColor = db.Activities.Select(y => y.Color).ToList();
            var Allcolors = db.colors.Select(x => x.ColorName).ToList();
            var Available = Allcolors.Where(i => !UsedColor.Any(e => i.Contains(e))).ToList();
            var ColorInfo = db.colors.Where(s => Available.Contains(s.ColorName)).ToList();

            if(CurrentColor!=null)
            {
                var Current = db.colors.FirstOrDefault(x=>x.ColorName== CurrentColor);

                ColorInfo.Add(Current);
            }

            return ColorInfo;



        }


    }
}
