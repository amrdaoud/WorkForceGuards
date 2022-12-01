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
    public class HeadOfSectionService: IHeadOfSectionService
    {
        private readonly ApplicationDbContext db;
        public HeadOfSectionService(ApplicationDbContext context)
        {
            db = context;
        }

        public DataWithError Add(HeadOfSection model)
        {
            DataWithError data = new DataWithError();
            if(CheckUniqValue(model))
            {
                db.HeadOfSections.Add(model);
                db.SaveChanges();
                data.Result = model;
                data.ErrorMessage = null;
                return data;
            }
          
                data.Result = null;
                data.ErrorMessage = "Duplicated Employee Id Inserted";
                return data;
            
               
            
           
        }

        public bool Delete(int id)
        {
            var head = db.HeadOfSections.Find(id);
            try
            {
                db.HeadOfSections.Remove(head);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }

        }

        public List<HeadOfSection> GetAll()
        {
            return db.HeadOfSections.ToList();
        }

        public HeadOfSection GetById(int id)
        {
            var head = db.HeadOfSections.Find(id);
            return head;
        }

        public DataWithError Update(HeadOfSection model)
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
            data.ErrorMessage = "Duplicated Employee Id Inserted";
            return data;

        }

       

        public bool CheckUniqValue(HeadOfSection value)
        {
            var same = db.HeadOfSections.FirstOrDefault(a => a.EmployeeId == value.EmployeeId && a.Id != value.Id);
            if (same == null)
            {
                return true;
            }
            return false;

        }

        public bool CheckIdValue(int employeeId)
        {
            var same = db.HeadOfSections.FirstOrDefault(a => a.EmployeeId == employeeId);
            if (same == null)
            {
                return true;
            }
            return false;
        }
        public bool CheckNameValue(string value)
        {
            var same = db.HeadOfSections.FirstOrDefault(a => a.Name.ToLower() == value.ToLower());
            if (same == null)
            {
                return true;
            }
            return false;
        }

        public bool CheckAliasValue(string alias)
        {
            var same = db.HeadOfSections.FirstOrDefault(a => a.Alias.ToLower() == alias.ToLower());
            if (same == null)
            {
                return true;
            }
            return false;
        }

        public bool CheckEmailValue(string email)
        {
            var same = db.HeadOfSections.FirstOrDefault(a => a.Email.ToLower() == email.ToLower());
            if (same == null)
            {
                return true;
            }
            return false;
        }

    }
}
