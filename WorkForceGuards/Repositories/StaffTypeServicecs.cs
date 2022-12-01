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
    public class StaffTypeServicecs : IStaffTypeService
    {
        private readonly ApplicationDbContext db;
        public StaffTypeServicecs(ApplicationDbContext context)
        {
            db = context;
        }
        public DataWithError Add(StaffType model)
        {
            DataWithError data = new DataWithError();
            if (CheckUniqValue(model))
            {
                db.StaffTypes.Add(model);
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
            var staff = db.StaffTypes.Find(id);
            db.StaffTypes.Remove(staff);
            db.SaveChanges();
            return true;
        }

        public List<StaffType> GetAll()
        {
            return db.StaffTypes.ToList();
        }

        public StaffType GetById(int id)
        {
            return db.StaffTypes.Find(id);
        }

        public DataWithError Update(StaffType model)
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

        public bool CheckUniqValue(StaffType value)
        {
            var same = db.StaffTypes.FirstOrDefault(a => a.Name.ToLower() == value.Name.ToLower() && a.Id != value.Id);
            if (same == null)
            {
                return true;
            }
            return false;

        }

        public bool CheckValue(string value)
        {
            var same = db.StaffTypes.FirstOrDefault(a => a.Name.ToLower()==value.ToLower());
            if (same == null)
            {
                return true;
            }
            return false;
        }

    }
}
