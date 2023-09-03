using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using WorkForceGuards.Models;
using WorkForceGuards.Repositories.Interfaces;
using WorkForceManagementV0.Contexts;
using WorkForceManagementV0.Migrations;
using WorkForceManagementV0.Models;
using WorkForceManagementV0.Models.Bindings;

namespace WorkForceGuards.Repositories
{
    public class DailyAttendancePatternService : IDailyAttendancePatternService
    {
        private readonly ApplicationDbContext _db;
        public DailyAttendancePatternService(ApplicationDbContext context)
        {
            _db = context;
        }
        public DailyAttendancePatternsDTO AddUpdate(DailyAttendancePattern model)
        {
            var timezone = TimeZoneInfo.FindSystemTimeZoneById("Arab Standard Time");
            var currentPattern = _db.DailyAttendancePatterns.Find(model.Id);
            if(currentPattern == null)
            {
                // var q = model.DayOffs.Select(x => DateTime.ParseExact(x, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                model.DayOffs = model.DayOffs.Select(x => DateTime.ParseExact(x, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("dd/MM/yyyy")).ToArray();
                _db.DailyAttendancePatterns.Add(model);
                _db.SaveChanges();
                _db.Entry(model).Reference(d => d.Schedule).Load();
                _db.Entry(model).Reference(d => d.StaffMember).Load();
                _db.Entry(model).Reference(d => d.Transportation).Load();
                _db.Entry(model).Reference(d => d.Sublocation).Load();
                return new DailyAttendancePatternsDTO(model);
            }
            _db.Entry(currentPattern).State = EntityState.Detached;
            _db.Entry(model).State = EntityState.Modified;
            _db.SaveChanges();
            _db.Entry(model).Reference(d => d.Schedule).Load();
            _db.Entry(model).Reference(d => d.StaffMember).Load();
            _db.Entry(model).Reference(d => d.Transportation).Load();
            _db.Entry(model).Reference(d => d.Sublocation).Load();
            return new DailyAttendancePatternsDTO(model);
        }

        public DataWithError GetAll()
        {
            var schedule = _db.Schedules.OrderByDescending(x => x.StartDate).FirstOrDefault(x => !x.IsPublish);
            if(schedule == null)
            {
                return new DataWithError(null, "Please create new schedule!");
            }
            var staffMembers = getInvolvedStaffMembers(schedule);
            var currentPattern = _db.DailyAttendancePatterns.Where(x => x.ScheduleId == schedule.Id);
            var result = from staff in staffMembers
                         join cp in currentPattern on staff.Id equals cp.StaffMemberId into staffCpJoin
                         from p in staffCpJoin.DefaultIfEmpty()
                         select new DailyAttendancePatternsDTO
                         {
                             Id = p == null ? 0 : p.Id,
                             ScheduleId = schedule.Id,
                             ScheduleName = schedule.Name,
                             StaffMemberId = staff.Id,
                             StaffMemberName = staff.Name,
                             SublocationId = p.SublocationId,
                             SublocationName = p.Sublocation.Name,
                             TransportationId = p.TransportationId,
                             TransportationName = p.Transportation.Name,
                             StaffMemberEmployeeId = staff.EmployeeId,
                             DayOffs = p == null || p.DayOffs.Length == 0 ? new List<string>().ToArray() : 
                            //new List<DateTime>().ToArray()
                            p.DayOffs
                         };
            var earlierScheduleId = _db.Schedules.OrderByDescending(x => x.StartDate).FirstOrDefault(x => x.IsPublish)?.Id;

            if(earlierScheduleId != null)
            {
                var earlierPattern = _db.DailyAttendancePatterns.Where(x => x.ScheduleId == earlierScheduleId);
                result = from r in result
                         join e in earlierPattern on r.StaffMemberId equals e.StaffMemberId into staffCpJoin
                         from p in staffCpJoin.DefaultIfEmpty()
                         select new DailyAttendancePatternsDTO
                         {
                             Id = r == null ? 0 : r.Id,
                             ScheduleId = schedule.Id,
                             ScheduleName = schedule.Name,
                             StaffMemberId = r.StaffMemberId,
                             StaffMemberName = r.StaffMemberName,
                             SublocationId = p.SublocationId,
                             SublocationName = p.Sublocation.Name,
                             TransportationId = p.TransportationId,
                             TransportationName = p.Transportation.Name,
                             StaffMemberEmployeeId = r.StaffMemberEmployeeId,
                             DayOffs = new List<string>().ToArray()
                         };
            }
            return new DataWithError(result, "");
        }

        public DataWithError Upload(List<DailyAttendancePatternsUpload> lst)
        {
            var schedule = _db.Schedules.OrderByDescending(x => x.StartDate).FirstOrDefault(x => !x.IsPublish);
            var days = Enumerable.Range(0, 1 + schedule.EndDate.Subtract(schedule.StartDate).Days)
                      .Select(offset => schedule.StartDate.AddDays(offset).ToString("dd/MM/yyyy"))
                      .ToList();
            if (schedule == null)
            {
                return new DataWithError(false, "Please Create a Schedule First");
            }
            var distinctCheck = lst.Select(x => x.EmployeeId).Distinct().Count();
            if(distinctCheck < lst.Count)
            {
                return new DataWithError(false, "Multiple Rows With Same Employee Id");
            }
            var staffMembers = getInvolvedStaffMembers(schedule).ToList();
            var transportations = _db.TransportationRoutes.ToList();
            var sublocations = _db.SubLocations.ToList();
            var addList = new List<DailyAttendancePattern>();
            for(int i = 0; i < lst.Count; i++)
            {
                var staffMemberTry = int.TryParse(lst[i].EmployeeId, out int staffMemberId);
                if(!staffMemberTry)
                {
                    return new DataWithError(false, "Staff Member in Row " + (i + 1) + " Cannot be Parsed To Number");
                }
                var staffMember = staffMembers.FirstOrDefault(x => x.EmployeeId == staffMemberId);
                if(staffMember == null)
                {
                    return new DataWithError(false, "Staff Member Not Found Row " + (i + 1));
                }
                var subLocation = sublocations.FirstOrDefault(x => x.Name.ToLower() == lst[i].Sublocation.ToLower());
                if(subLocation == null)
                {
                    return new DataWithError(false, "Sublocation '" + lst[i].Sublocation + "' Not Found Row " + (i + 1));
                }
                var transportation = transportations.FirstOrDefault(x => x.Name.ToLower() == lst[i].Shift.ToLower());
                if (transportation == null)
                {
                    return new DataWithError(false, "Shift '" + lst[i].Shift + "' Not Found Row " + (i + 1));
                }
                if (!checkDayOffs(lst[i].DayOffs, days, out string[] dayOffsArray))
                {
                    return new DataWithError(false, "Check Day Offs in Row " + (i + 1));
                }
                addList.Add(new DailyAttendancePattern {
                    Id = 0,
                    ScheduleId = schedule.Id,
                    StaffMemberId = staffMember.Id,
                    SublocationId = subLocation.Id,
                    TransportationId = transportation.Id,
                    DayOffs = dayOffsArray
                });
            }
            _db.DailyAttendancePatterns.RemoveRange(_db.DailyAttendancePatterns.Where(x => x.ScheduleId == schedule.Id));
            _db.DailyAttendancePatterns.AddRange(addList);
            _db.SaveChanges();
            return new DataWithError(true, "");
        }
        public DataWithError Delete(int id)
        {
            var pattern = _db.DailyAttendancePatterns.Find(id);
            if(pattern == null)
            {
                return new DataWithError(false, "Pattern is not available!");
            }
            _db.DailyAttendancePatterns.Remove(pattern);
            _db.SaveChanges();
            return new DataWithError(true, "");
        }
        public async Task<DataWithError> GenerateSchedule()
        {
            var schedule = _db.Schedules.OrderByDescending(x => x.StartDate).FirstOrDefault(x => !x.IsPublish);

            var overallDailyAttendance = new List<DailyAttendance>();
            if(schedule == null)
            {
                return new DataWithError(false, "Please Create a Schedule First!");
            }
            //if(schedule.StartDate < DateTime.Today)
            //{
            //    return new DataWithError(false, "Schedule Already Started!");
            //}
            var defaultForecast = _db.Forecasts.FirstOrDefault();
            if(defaultForecast == null)
            {
                return new DataWithError(false, "Default Forecast is not set!");
            }
            var defaultAttendanceType = _db.AttendanceTypes.FirstOrDefault(x => !x.IsAbsence);
            if(defaultAttendanceType == null)
            {
                return new DataWithError(false, "Default Attendance Type is not set!");
            }
            var defaultAbsenceType = _db.AttendanceTypes.FirstOrDefault(x => x.IsAbsence);
            if (defaultAbsenceType == null)
            {
                return new DataWithError(false, "Default Absence Type is not set!");
            }
            var patterns = _db.DailyAttendancePatterns.Include(x => x.Transportation).Include(x => x.StaffMember).Where(x => x.ScheduleId == schedule.Id);
            foreach(var pattern in patterns)
            {
                _db.DailyAttendances.RemoveRange(_db.DailyAttendances.Where(x => x.ScheduleId == pattern.ScheduleId && x.StaffMemberId == pattern.StaffMemberId));
                _db.DailyAttendances.AddRange(this.GenerateDailyAttendance(pattern,defaultAttendanceType, defaultAbsenceType));
            }
            schedule.ForecastId = defaultForecast.Id;
            await _db.SaveChangesAsync();
            return new DataWithError(overallDailyAttendance, "");
        }
        public List<DailyAttendance> GenerateDailyAttendance(DailyAttendancePattern model, AttendanceType attendanceType, AttendanceType absenceType)
        {
            var result = new List<DailyAttendance>();
            var currentDay = model.Schedule.StartDate;
            while (currentDay <= model.Schedule.EndDate)
            {
                if(model.DayOffs.Select(x => DateTime.ParseExact(x, "dd/MM/yyyy", CultureInfo.InvariantCulture)).Contains(currentDay))
                {
                    result.Add(new DailyAttendance(
                            model.StaffMember,
                            model.Schedule,
                            currentDay,
                            absenceType,
                            model.Transportation,
                            model.SublocationId,
                            model.StaffMember.HeadOfSectionId
                           ));
                }
                else
                {
                    result.Add(new DailyAttendance(
                            model.StaffMember,
                            model.Schedule,
                            currentDay,
                            attendanceType,
                            model.Transportation,
                            model.SublocationId,
                            model.StaffMember.HeadOfSectionId
                           ));
                }
                
                currentDay = currentDay.AddDays(1);
            }
            return result;
        }
        private IQueryable<StaffMember> getInvolvedStaffMembers(Schedule schedule)
        {
            return _db.StaffMembers.Where(x => x.LeaveDate >= schedule.StartDate && x.StartDate <= schedule.EndDate);
        }
        private bool checkDayOffs(string dayOffsString, List<string> days, out string[] lst)
        {
            var dayOffList = dayOffsString.Split(',').Select(x => x.Trim());
            
            if (days.FirstOrDefault(x => dayOffList.Contains(x)) == null)
            {
                lst = null;
                return false;
            }
            lst = dayOffList.ToArray();
            return true;
            
        }
        public DataWithError GetEligibleStaffMembers(List<ScheduleDetailManipulate> models)
        {
            var da = _db.DailyAttendances.Where(x => models.Select(d => d.DailyAttendanceId).Contains(x.Id));
            var daStaffs = da.Select(x => x.StaffMemberId).ToList();
            var locations = da.Select(x => x.Sublocation.Location).Distinct().ToList();
            if (locations.Count() > 1)
            {
                return new DataWithError(null, "Cannot Find Backup For Multiple Locations");
            }
            //var timeMaps = _db.Intervals.Where(x => models.Select(x => x.IntervalId).Contains(x.Id)).Select(x => x.TimeMap).Distinct();
            //var tt = _db.Intervals.ToList();
            //var days = da.Select(x => x.Day).Distinct();
            var mm = models.GroupBy(x => x.DailyAttendanceId).Select(g => {
                var day = da.FirstOrDefault(d => d.Id == g.Key).Day;
                var intervals = g.Select(y => _db.Intervals.FirstOrDefault(i => i.Id == y.IntervalId).TimeMap);
                var dailyAttendances = _db.DailyAttendances.Where(da => !daStaffs.Contains(da.StaffMemberId) && !da.AttendanceType.IsAbsence && da.Day == day && da.Sublocation.LocationId == locations[0].Id)
                .Where(da => da.ScheduleDetails.FirstOrDefault(sd => sd.ActivityId == 35 && intervals.Contains(sd.Interval.TimeMap)) != null)
                ;
                return dailyAttendances.Select(x => x.StaffMember).ToList();
            }).Aggregate((a,b) => a.Intersect(b).ToList()).ToList();
            //var mm = models.GroupBy(x => x.DailyAttendanceId).Select(g => new {
            //    day = da.FirstOrDefault(d => d.Id == g.Key).Day,
            //    timeMaps = g.Select(y => _db.Intervals.FirstOrDefault(i => i.Id == y.IntervalId).TimeMap)
            //}).ToList();
            //var dd = _db.StaffMembers.Where(s => s.DailyAttendances.Where(d => !d.AttendanceType.IsAbsence && mm.Select(m => m.day).Contains(d.Day)).Count() == mm.Count).ToList();
                //&& m.timeMaps.All(t => d.ScheduleDetails.Where(sd => sd.ActivityId == 1).Select(sd => sd.Interval.TimeMap).ToList().Contains(t))) != null).ToList();
            //&& d.ScheduleDetails.Where(sd => sd.ActivityId == 1 && m.timeMaps.Contains(sd.Interval.TimeMap)).Count() == m.timeMaps.Count())).ToList();
            //var ss = _db.StaffMembers.Where(x => x.DailyAttendances.All(d => mm.Select(m => m.day).Contains(d.Day))).ToList();
            //var mm = da.Select(x => new
            //{
            //    Day = x.Day,
            //    TimeMaps = x.ScheduleDetails.Where(sd => sd.DailyAttendanceId == x.Id && )
            //})
            //var staffMembers = _db.StaffMembers.Where(staff => 
            //staff.DailyAttendances.Where(d =>
            //!d.AttendanceType.IsAbsence &&
            //days.Contains(d.Day)).Count() == days.Count());
            //var dailyAttendances = _db.DailyAttendances.Where(x => !x.AttendanceType.IsAbsence &&
            //!models.Select(d => d.DailyAttendanceId).Contains(x.Id) &&
            //days.Contains(x.Day) &&
            //x.ScheduleDetails.Where(s => s.Activity.IsPhone && 
            //timeMaps.Contains(s.Interval.TimeMap)).Count() == timeMaps.Count());
            //var result = dailyAttendances.Select(x => x.StaffMember).Intersect(staffMembers).Distinct().OrderBy(x => x.Name);
            return new DataWithError(mm, "");

        }

        public DataWithError GetHeadcount()
        {
            var pureResult = _db.Headcounts.Select(x => new
            {
                x.TransportationRouteId,
                x.SublocationId,
                TransportationRouteName = x.TransportationRoute.Name,
                SublocationName = x.Sublocation.Name,
                x.Total
            });
            var r = _db.Headcounts.Include(x => x.TransportationRoute).Include(x => x.Sublocation).ToList().GroupBy(x => x.Sublocation).Select(g => new
            {
                SublocationName = g.Key.Name,
                ShiftCount = g.Select(d => new
                {
                    ShiftName = d.TransportationRoute.Name,
                    d.Total
                })
            }).ToList();

            return new DataWithError(pureResult, "");
        }

        public DataWithError SetHeadcount(Headcount model)
        {
            var available = _db.Headcounts.FirstOrDefault(x => x.TransportationRouteId == model.TransportationRouteId && x.SublocationId == model.SublocationId);
            if(available != null)
            {
                available.Total = model.Total;
                return new DataWithError(available, "");
            }
            else
            {
                _db.Headcounts.Add(model);
                return new DataWithError(model, "");
            }
        }

        public DataWithError BulkHeadcount(List<Dictionary<string, string>> models)
        {
            //_db.Database.ExecuteSqlRaw("TRUNCATE TABLE [Headcounts]");
            var sublocations = _db.SubLocations.ToList();
            var transportationRoutes = _db.TransportationRoutes.ToList();
            var addResult = new List<Headcount>();
            foreach(var m in models)
            {
                var sublocation = sublocations.FirstOrDefault(s => s.Name == m["Sublocation"]);
                if(sublocation == null)
                {
                    return new DataWithError($"Sublocation {m["Sublocation"]} not found", null);
                }
                foreach(KeyValuePair<string,string> entry in m)
                {
                    if(entry.Key != "Sublocation")
                    {
                        var transportation = transportationRoutes.FirstOrDefault(x => x.Name == entry.Key);
                        if(transportation == null)
                        {
                            return new DataWithError($"Shift {entry.Key} not found", null);
                        }
                        addResult.Add(new Headcount { Id = 0, SublocationId = sublocation.Id, TransportationRouteId = transportation.Id, Total = Convert.ToDouble(entry.Value) });
                    }
                }
            }
            _db.Database.ExecuteSqlRaw("TRUNCATE TABLE [Headcounts]");
            _db.Headcounts.AddRange(addResult);
            _db.SaveChanges();
            return new DataWithError(true, "");
        }
    }
}
