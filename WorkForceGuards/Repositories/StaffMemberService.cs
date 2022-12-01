using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkForceManagementV0.Contexts;
using WorkForceManagementV0.Models;
using WorkForceManagementV0.Models.Bindings;
using WorkForceManagementV0.Models.DTO;
using WorkForceManagementV0.Repositories.Interfaces;

namespace WorkForceManagementV0.Repositories
{
    public class StaffMemberService: IStaffMemberService
    {
        private readonly ApplicationDbContext db;
        public StaffMemberService(ApplicationDbContext context)
        {
            db = context;
        }
        public FullStaffMemberDto GetAll(FilterModel f)
        {
            var staffMembers = db.StaffMembers.Include(x => x.TransportationRoute).
                Include(y => y.HeadOfSection).Include(z => z.StaffType).Include(v => v.Location).AsQueryable();


            if (!string.IsNullOrEmpty(f.SearchQuery))
            {
                if (int.TryParse(f.SearchQuery, out var id))
                {
                    staffMembers = staffMembers.Where(x => x.EmployeeId == id);
                }
                else
                {
                    staffMembers = staffMembers.Where(r => r.Name.ToLower().StartsWith(f.SearchQuery.ToLower()) ||
                    r.Alias.ToLower().StartsWith(f.SearchQuery.ToLower()) || r.Email.ToLower().StartsWith(f.SearchQuery.ToLower()));
                }
            }

            var q = staffMembers.ToList();
            if (f.Filters.Count != 0)
            {
                foreach (Filter c in f.Filters)
                {
                    var filterKeyProperty = typeof(StaffMember).GetProperty(c.Key);
                    q = q.Where(x => c.Values.Contains(filterKeyProperty.GetValue(x).ToString())).ToList();

                }
            }

            var result = q.Select(x => new StaffMemberDto
            {
                Id = x.Id,
                EmployeeId = x.EmployeeId,
                Name = x.Name,
                Alias = x.Alias,
                Gender = x.Gender,
                Email = x.Email,
                Language = x.Language,
                Address = x.Address,
                Note = x.Note,
                StartDate = x.StartDate,
                LeaveDate = x.LeaveDate,
                HireDate = x.HireDate,
                EstimatedLeaveDays = x.EstimatedLeaveDays,
                Religion = x.Religion,
                StaffTypeId = x.StaffTypeId,
                StaffTypeName = x.StaffType!=null?x.StaffType.Name:"Deleted",
                PhoneNumber = x.PhoneNumber,
                TransportationRouteId = x.TransportationRouteId,
                TransportationRouteName = x.TransportationRoute!=null?x.TransportationRoute.Name:"Deleted",
                LocationId = x.LocationId,
                LocationName = x.Location!=null?x.Location.Name:"Deleted",
                HeadOfSectionId = x.HeadOfSectionId,
                HeadOfSectionName = x.HeadOfSection!=null?x.HeadOfSection.Alias:"Deleted"


            }).ToList();
            FullStaffMemberDto ResultDto = new FullStaffMemberDto();


            if (!string.IsNullOrEmpty(f.Sort))
            {
                List<StaffMemberDto> data = new List<StaffMemberDto>();

                var sortProperty = typeof(StaffMemberDto).GetProperty(f.Sort);
                if (sortProperty != null && f.Order == "asc")
                {
                    data = result.OrderBy(x => sortProperty.GetValue(x)).ToList();
                    data = data.Skip(f.PageIndex * f.PageSize).Take(f.PageSize).ToList();
                }

                else if (sortProperty != null && f.Order == "desc")
                {
                    data = result.OrderByDescending(x => sortProperty.GetValue(x)).ToList();
                    data = data.Skip(f.PageIndex * f.PageSize).Take(f.PageSize).ToList();
                }

                

                ResultDto.Result = data;
                ResultDto.ResultSize = result.Count;

                return ResultDto;
            }


            else
            {
                List<StaffMemberDto> data = new List<StaffMemberDto>();


                data = result.Skip(f.PageIndex * f.PageSize).Take(f.PageSize).ToList();
               
                 ResultDto.Result = data;
                ResultDto.ResultSize = result.Count;

                return ResultDto;
            }



        }

        public StaffMemberDto GetById(int id)
        {
          

            var result = db.StaffMembers.Select(x => new StaffMemberDto
            {
                Id = x.Id,
                EmployeeId = x.EmployeeId,
                Name = x.Name,
                Alias = x.Alias,
                Gender = x.Gender,
                Email = x.Email,
                Language = x.Language,
                Address = x.Address,
                Note = x.Note,
                StartDate = x.StartDate,
                LeaveDate = x.LeaveDate,
                HireDate = x.HireDate,
                EstimatedLeaveDays = x.EstimatedLeaveDays,
                Religion = x.Religion,
                PhoneNumber = x.PhoneNumber,
                StaffTypeId = x.StaffTypeId,
                StaffTypeName = x.StaffType != null ? x.StaffType.Name : "Deleted",
                TransportationRouteId = x.TransportationRouteId,
                TransportationRouteName = x.TransportationRoute != null ? x.TransportationRoute.Name : "Deleted",
                LocationId = x.LocationId,
                LocationName = x.Location != null ? x.Location.Name : "Deleted",
                HeadOfSectionId = x.HeadOfSectionId,
                HeadOfSectionName = x.HeadOfSection != null ? x.HeadOfSection.Alias : "Deleted"


            }).FirstOrDefault(x=>x.Id==id);

            return result;

        }


        public DataWithError Add(StaffMember model)
        {
            DataWithError data = new DataWithError();

            if (CheckUniqValue(model))
            {
                var timezone = TimeZoneInfo.FindSystemTimeZoneById("Arab Standard Time");
                model.StartDate = TimeZoneInfo.ConvertTimeFromUtc(model.StartDate, timezone);
                model.LeaveDate = TimeZoneInfo.ConvertTimeFromUtc(model.LeaveDate, timezone);
                model.HireDate = TimeZoneInfo.ConvertTimeFromUtc(model.HireDate, timezone);

                db.StaffMembers.Add(model);
                db.SaveChanges();

                var result = db.StaffMembers.Select(x => new StaffMemberDto
                {
                    Id = x.Id,
                    EmployeeId = x.EmployeeId,
                    Name = x.Name,
                    Alias = x.Alias,
                    Gender = x.Gender,
                    Email = x.Email,
                    Language = x.Language,
                    Address = x.Address,
                    Note = x.Note,
                    StartDate = x.StartDate,
                    LeaveDate = x.LeaveDate,
                    HireDate = x.HireDate,
                    PhoneNumber = x.PhoneNumber,
                    EstimatedLeaveDays = x.EstimatedLeaveDays,
                    Religion = x.Religion,
                    StaffTypeId = x.StaffTypeId,
                    StaffTypeName = x.StaffType.Name,
                    TransportationRouteId = x.TransportationRouteId,
                    TransportationRouteName = x.TransportationRoute.Name,
                    LocationId = x.LocationId,
                    LocationName = x.Location.Name,
                    HeadOfSectionId = x.HeadOfSectionId,
                    HeadOfSectionName = x.HeadOfSection.Name


                }).FirstOrDefault(c => c.Id == model.Id);

                data.Result = result;
                data.ErrorMessage = null;
                return data;
            }

            data.Result = null;
            data.ErrorMessage = "Duplicated Employee Id Inserted";
            return data;

        }

        public DataWithError Update(StaffMember model)
        {
            DataWithError data = new DataWithError();
            if(CheckUniqValue(model))
            {
                var timezone = TimeZoneInfo.FindSystemTimeZoneById("Arab Standard Time");
                model.StartDate = TimeZoneInfo.ConvertTimeFromUtc(model.StartDate, timezone);
                model.LeaveDate = TimeZoneInfo.ConvertTimeFromUtc(model.LeaveDate, timezone);
                model.HireDate = TimeZoneInfo.ConvertTimeFromUtc(model.HireDate, timezone);

                db.Entry(model).State = EntityState.Modified;
                db.SaveChanges();

                var result = db.StaffMembers.Select(x => new StaffMemberDto
                {
                    Id = x.Id,
                    EmployeeId = x.EmployeeId,
                    Name = x.Name,
                    Alias = x.Alias,
                    Gender = x.Gender,
                    Email = x.Email,
                    Language = x.Language,
                    Address = x.Address,
                    Note = x.Note,
                    StartDate = x.StartDate,
                    LeaveDate = x.LeaveDate,
                    HireDate = x.HireDate,
                    PhoneNumber = x.PhoneNumber,
                    EstimatedLeaveDays = x.EstimatedLeaveDays,
                    Religion = x.Religion,
                    StaffTypeId = x.StaffTypeId,
                    StaffTypeName = x.StaffType.Name,
                    TransportationRouteId = x.TransportationRouteId,
                    TransportationRouteName = x.TransportationRoute.Name,
                    LocationId = x.LocationId,
                    LocationName = x.Location.Name,
                    HeadOfSectionId = x.HeadOfSectionId,
                    HeadOfSectionName = x.HeadOfSection.Name


                }).FirstOrDefault(c => c.Id == model.Id);
                data.Result = result;
                data.ErrorMessage = null;
                return data;
            }

            data.Result = null;
            data.ErrorMessage = "Duplicated Employee Id Inserted";
            return data;

        }

       
        public bool Delete(int id)
        {
            var staffMember = db.StaffMembers.Find(id);
            db.StaffMembers.Remove(staffMember);
            return true;
        }

       


        public List<int> GetEmployeeId()
        {
            var EmployeeIds = db.StaffMembers.Select(x => x.EmployeeId).ToList();
            var HeadEmpId = db.HeadOfSections.Select(x => x.EmployeeId).ToList();

            var Result = EmployeeIds.Concat(HeadEmpId).ToList();

            return Result;
        }

        public bool CheckUniqValue(StaffMember value)
        {
            var same = db.StaffMembers.FirstOrDefault(a => a.EmployeeId == value.EmployeeId && a.Id != value.Id);
            if (same == null)
            {
                return true;
            }
            return false;

        }
        public bool CheckUniqName(string Name)
        {
            var same = db.StaffMembers.FirstOrDefault(a => a.Name == Name );
            if (same == null)
            {
                return true;
            }
            return false;

        }

        public bool CheckUniqAlias(string Alias)
        {
            var same = db.StaffMembers.FirstOrDefault(a => a.Alias == Alias);
            if (same == null)
            {
                return true;
            }
            return false;

        }
        public bool CheckUniqEmail(string Email)
        {
            var same = db.StaffMembers.FirstOrDefault(a => a.Email == Email );
            if (same == null)
            {
                return true;
            }
            return false;

        }

        public bool CheckValue(string value)
        {
            var same = db.StaffMembers.FirstOrDefault(a => a.EmployeeId == Convert.ToInt64(value));
            if (same == null)
            {
                return true;
            }
            return false;
        }

    }
}
