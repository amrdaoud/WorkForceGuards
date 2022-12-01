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
    
    public class AttendanceTypeService : IAttendanceTypeService
    {
        private readonly ApplicationDbContext _db;
        public AttendanceTypeService(ApplicationDbContext db)
        {
            _db = db;
        }
        public DataWithError Add(AttendanceType model)
        {
            if(!CheckUniqueModel(model))
            {
                return new DataWithError(null, "Name already exist!");
            }
            _db.Add(model);
            _db.SaveChanges();
            return new DataWithError(model, "");
        }
        public bool CheckUniqueModel(AttendanceType model)
        {
            var same = _db.AttendanceTypes.FirstOrDefault(x => x.Name.ToLower() == model.Name.ToLower() && x.Id != model.Id);
            if (same == null)
            {
                return true;
            }
            return false;
        }

        public bool CheckUniqueName(string name)
        {
            var same = _db.AttendanceTypes.FirstOrDefault(x => x.Name.ToLower() == name.ToLower());
            if(same == null)
            {
                return true;
            }
            return false;
        }

        public List<AttendanceType> GetAll()
        {
            var result = _db.AttendanceTypes.Where(x => !x.Hidden).ToList();
            return result;
        }

        public AttendanceType GetById(int id)
        {
            var result = _db.AttendanceTypes.Find(id);
            return result;
        }

        public DataWithError Update(AttendanceType model)
        {
            if(!CheckUniqueModel(model))
            {
                return new DataWithError(null,"Name already exist!");
            }
            _db.Entry(model).State = EntityState.Modified;
            _db.SaveChanges();
            return new DataWithError(model, "");

        }
    }
}
