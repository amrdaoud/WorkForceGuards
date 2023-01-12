using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WorkForceManagementV0.Contexts;
using WorkForceManagementV0.Models;
using WorkForceManagementV0.Models.Bindings;
using WorkForceManagementV0.Models.DTO;
using WorkForceManagementV0.Repositories.Identity;
using WorkForceManagementV0.Repositories.Interfaces;

namespace WorkForceManagementV0.Repositories
{
    public class ScheduleWithRuleService: IScheduleWithRuleService
    {
        private readonly ApplicationDbContext db;
        private readonly IUserService _userService;

        public ScheduleWithRuleService(ApplicationDbContext context, IUserService userService)
        {
            db = context;
            _userService = userService;
        }
        public ScudualeWithRule GetAll(ClaimsPrincipal user)
        {
            var appUser = _userService.GetUserInfo(user);
            ScudualeWithRule data = new ScudualeWithRule();
            var ScheduleList = db.Schedules.Where(x => true);
            if(appUser.Roles.Contains("Hos") && !(appUser.Roles.Contains("SuperUser") || appUser.Roles.Contains("Admin")))
            {
                ScheduleList = ScheduleList.Where(x => x.IsPublish && x.DailyAttendances.FirstOrDefault(d => d.HeadOfSection.Alias.ToLower() == appUser.Alias.ToLower()) != null);
            } else if (appUser.Roles.Contains("User") && !(appUser.Roles.Contains("SuperUser") || appUser.Roles.Contains("Admin")))
            {
                ScheduleList = ScheduleList.Where(x => x.IsPublish && x.DailyAttendances.FirstOrDefault(d => d.StaffMember.Alias.ToLower() == appUser.Alias.ToLower()) != null);
            }
            var ShiftRule = db.shiftRules.FirstOrDefault();
            data.ScheduleData = ScheduleList.ToList();
            data.ShiftRuleData = ShiftRule;
            return data;
        }

        public DataWithError Add(ScheduleWithRuleBinding model)
        {
            DataWithError result = new DataWithError();

            if (CheckLastPublishedSchedule() == true)
            {
                var timezone = TimeZoneInfo.FindSystemTimeZoneById("Arab Standard Time");
                model.ScheduleData.StartDate = TimeZoneInfo.ConvertTimeFromUtc(model.ScheduleData.StartDate, timezone);
                model.ScheduleData.EndDate = TimeZoneInfo.ConvertTimeFromUtc(model.ScheduleData.EndDate, timezone);
                model.ScheduleData.CreateDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timezone);
                // model.ScheduleData.UpdateDate= TimeZoneInfo.ConvertTimeFromUtc((DateTime)model.ScheduleData.UpdateDate, timezone);
                if((model.ScheduleData.EndDate - model.ScheduleData.StartDate).TotalDays > 56)
                {
                    return new DataWithError(null, "Range is too long max: 56 days!");
                }

                if (dateschedule(model.ScheduleData.StartDate,model.ScheduleData.EndDate,model.ScheduleData.Id))
                {
                    if (CheckUniqeValue(model.ScheduleData))
                    {
                        db.Schedules.Add(model.ScheduleData);
                        db.SaveChanges();

                        var rule = db.shiftRules.FirstOrDefault();
                        if (rule == null)
                        {
                            db.shiftRules.Add(model.ShiftRuleData);
                            db.SaveChanges();
                        }
                        else
                        {
                            rule.StartAfter = model.ShiftRuleData.StartAfter;
                            rule.EndBefore = model.ShiftRuleData.EndBefore;
                            rule.BreakBetween = model.ShiftRuleData.BreakBetween;

                            db.Entry(rule).State = EntityState.Modified;
                            db.SaveChanges();
                        }

                        result.Result = model;
                        result.ErrorMessage = null;
                        return result;
                    }
                    result.Result = null;
                    result.ErrorMessage = "Duplicated Schedule Name Inserted";
                    return result;
                }
            
               
                result.Result = null;
                result.ErrorMessage = "schedual date is included with another schedual";
                return result;
            }

            result.Result = null;
            result.ErrorMessage = "Last Schedule is not published";
            return result;

        }

        public DataWithError AddWithPatterns(ScheduleWithRuleBinding model)
        {
            DataWithError result = new DataWithError();

            if (CheckLastPublishedSchedule() == true)
            {
                var timezone = TimeZoneInfo.FindSystemTimeZoneById("Arab Standard Time");
                model.ScheduleData.StartDate = TimeZoneInfo.ConvertTimeFromUtc(model.ScheduleData.StartDate, timezone);
                model.ScheduleData.EndDate = TimeZoneInfo.ConvertTimeFromUtc(model.ScheduleData.EndDate, timezone);
                model.ScheduleData.CreateDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timezone);
                // model.ScheduleData.UpdateDate= TimeZoneInfo.ConvertTimeFromUtc((DateTime)model.ScheduleData.UpdateDate, timezone);
                if ((model.ScheduleData.EndDate - model.ScheduleData.StartDate).TotalDays > 56)
                {
                    return new DataWithError(null, "Range is too long max: 56 days!");
                }

                if (dateschedule(model.ScheduleData.StartDate, model.ScheduleData.EndDate, model.ScheduleData.Id))
                {
                    if (CheckUniqeValue(model.ScheduleData))
                    {
                        db.Schedules.Add(model.ScheduleData);
                        db.SaveChanges();

                        var rule = db.shiftRules.FirstOrDefault();
                        if (rule == null)
                        {
                            db.shiftRules.Add(model.ShiftRuleData);
                            db.SaveChanges();
                        }
                        else
                        {
                            rule.StartAfter = model.ShiftRuleData.StartAfter;
                            rule.EndBefore = model.ShiftRuleData.EndBefore;
                            rule.BreakBetween = model.ShiftRuleData.BreakBetween;

                            db.Entry(rule).State = EntityState.Modified;
                            
                        }

                        result.Result = model;
                        result.ErrorMessage = null;
                        //var earlierSchedule = db.Schedules.OrderByDescending(x => x.StartDate).FirstOrDefault(x => x.IsPublish);
                        //if(earlierSchedule != null)
                        //{
                        //    db.DailyAttendancePatterns.AddRange(db.DailyAttendancePatterns.Where(x => x.ScheduleId == earlierSchedule.Id));
                        //}
                        db.SaveChanges();
                        return result;
                    }
                    result.Result = null;
                    result.ErrorMessage = "Duplicated Schedule Name Inserted";
                    return result;
                }


                result.Result = null;
                result.ErrorMessage = "schedual date is included with another schedual";
                return result;
            }

            result.Result = null;
            result.ErrorMessage = "Last Schedule is not published";
            return result;

        }

        public DataWithError Edit(ScheduleWithRuleBinding model)
        {
            DataWithError result = new DataWithError();
            if (CheckUniqeValue(model.ScheduleData) && model.ScheduleData.IsPublish == false)
            {
                var timezone = TimeZoneInfo.FindSystemTimeZoneById("Arab Standard Time");
                model.ScheduleData.StartDate = TimeZoneInfo.ConvertTimeFromUtc(model.ScheduleData.StartDate, timezone);
                model.ScheduleData.EndDate = TimeZoneInfo.ConvertTimeFromUtc(model.ScheduleData.EndDate, timezone);
                // model.ScheduleData.CreateDate = TimeZoneInfo.ConvertTimeFromUtc((DateTime)model.ScheduleData.CreateDate, timezone);
                 model.ScheduleData.UpdateDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timezone);
                if ((model.ScheduleData.EndDate - model.ScheduleData.StartDate).TotalDays > 56)
                {
                    return new DataWithError(null, "Range is too long max: 56 days!");
                }
                var schedule = db.Schedules.FirstOrDefault(x => x.Id == model.ScheduleData.Id);
                var rule = db.shiftRules.FirstOrDefault();
                if (schedule == null || rule == null)
                {
                    return new DataWithError { Result = null, ErrorMessage = "Schedule Or Rules Not Found!" };
                }
                if (dateschedule(model.ScheduleData.StartDate, model.ScheduleData.EndDate,model.ScheduleData.Id))
                {
                    if (rule.StartAfter != model.ShiftRuleData.StartAfter
                            || rule.EndBefore != model.ShiftRuleData.EndBefore
                            || rule.BreakBetween != model.ShiftRuleData.BreakBetween
                            || model.ScheduleData.StartDate != schedule.StartDate
                            || model.ScheduleData.EndDate != schedule.EndDate)
                    {
                        model.ScheduleData.ForecastId = null;
                        var dailyAttendances = db.DailyAttendances.Where(x => x.ScheduleId == model.ScheduleData.Id);
                        db.DailyAttendances.RemoveRange(dailyAttendances);
                    }
                    rule.StartAfter = model.ShiftRuleData.StartAfter;
                    rule.EndBefore = model.ShiftRuleData.EndBefore;
                    rule.BreakBetween = model.ShiftRuleData.BreakBetween;
                    db.Entry(rule).State = EntityState.Modified;
                    db.Entry(schedule).CurrentValues.SetValues(model.ScheduleData);
                    db.SaveChanges();
                    result.Result = model;
                    result.ErrorMessage = null;
                    return result;
                    }
                    result.Result = null;
                    result.ErrorMessage = "schedual date is included with another schedual";
                    return result;

                }
            result.Result = null;
            result.ErrorMessage = "Duplicated Schedule Name or Schedule is published Inserted";
            return result;
        }
        public DataWithError EditPublished(ScheduleWithRuleBinding model)
        {
            var timezone = TimeZoneInfo.FindSystemTimeZoneById("Arab Standard Time");
            model.ScheduleData.EndDate = TimeZoneInfo.ConvertTimeFromUtc(model.ScheduleData.EndDate, timezone);
            if(model.ScheduleData.EndDate.Date < DateTime.Now.Date)
            {
                return new DataWithError(null, "Please choose future date");
            }
            var schedule = db.Schedules.FirstOrDefault(x => x.Id == model.ScheduleData.Id && x.IsPublish);
            if(schedule == null)
            {
                return new DataWithError(null, "Schedule not found!");
            }
            if((schedule.EndDate <= model.ScheduleData.EndDate))
            {
                return new DataWithError(null, "Please choose earlier than current End Date!");
            }
            var availableDateSchedule = db.Schedules.FirstOrDefault(s => s.Id != model.ScheduleData.Id && model.ScheduleData.EndDate >= s.StartDate && model.ScheduleData.EndDate <= s.EndDate);
            if(availableDateSchedule != null)
            {
                return new DataWithError(null, "Dates overlap with other schedules");
            }
            if ((model.ScheduleData.EndDate - model.ScheduleData.StartDate).TotalDays > 56)
            {
                return new DataWithError(null, "Range is too long max: 56 days!");
            }
            schedule.EndDate = model.ScheduleData.EndDate;
            db.SwapRequests.RemoveRange(db.SwapRequests.Where(s => s.ScheduleId == schedule.Id));
            db.DailyAttendances.RemoveRange(db.DailyAttendances.Where(d => d.ScheduleId == schedule.Id && d.Day > schedule.EndDate));
            db.SaveChanges();
            return new DataWithError(model, "");
        }

        public DataWithError DeleteSchedule(int id)
        {
            var schedule = db.Schedules.FirstOrDefault(x=>x.Id==id);
            if(schedule == null)
            {
                return new DataWithError(null, "Schedule Not Found!");
            }
            var timezone = TimeZoneInfo.FindSystemTimeZoneById("Arab Standard Time");
            var nowDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timezone);
            if (nowDate >= schedule.StartDate)
            {
                return new DataWithError(null, "Cannot delete earlier schedule!");
            }
            db.SwapRequests.RemoveRange(db.SwapRequests.Where(s => s.ScheduleId == id));
            db.Schedules.Remove(schedule);
            db.SaveChanges();
            return new DataWithError(true, "");
        }

        public bool CheckUniqeValue(Schedule model)
        {
            var data = db.Schedules.FirstOrDefault(x => x.Name.ToLower() == model.Name.ToLower() && x.Id != model.Id);
            if(data==null)
            {
                return true;
            }
            return false;
        }

        public ShiftRule UpdateRule(ShiftRule model)
        {
            db.Entry(model).State = EntityState.Modified;
            db.SaveChanges();
            return model;
          
        }

        public bool CheckVlaue(string name,string ignoreName)
          {

            var schedule = db.Schedules.FirstOrDefault(x=>x.Name.ToLower()!=ignoreName.ToLower() && x.Name.ToLower()==name.ToLower());

            if(schedule==null)
            {
                return true;
            }
            return false;
        }

        public ScheduleWithRuleBinding GetById(int id)
        {
            ScheduleWithRuleBinding data = new ScheduleWithRuleBinding();

            var schedule = db.Schedules.Find(id);

            if(schedule!=null)
            {
                var rule = db.shiftRules.FirstOrDefault();

                data.ScheduleData = schedule;
                data.ShiftRuleData = rule;
                return data;
            }

            return data;
        
        }

        public bool CheckLastPublishedSchedule()
        {
           
                var LastSchedule = db.Schedules.OrderByDescending(x => x.Id).Take(1).FirstOrDefault();
            if(LastSchedule!=null)
            {
                if (LastSchedule.IsPublish == false)
                {
                    return false;
                }
                return true;
            }

            return true;
            

        }

        public bool dateschedule(DateTime start, DateTime end,int id )
        {
           
            var schedulein = db.Schedules
                .FirstOrDefault(
                    x => x.Id != id && (
                        !(x.EndDate < start || x.StartDate > end)
                        ));
            return schedulein == null;

        }

        public DataWithError GetUnpublishedSchedule()
        {
            var activeSchedule = db.Schedules
                .Select(s => new ActiveSchedule { Schedule = s, AttendanceStaffCount = s.DailyAttendances.Select(x => x.StaffMemberId).Distinct().Count() })
                .FirstOrDefault(s => !s.Schedule.IsPublish);
            if (activeSchedule == null)
            {
                return new DataWithError(null, "All schedules are published");
            }
            return new DataWithError(activeSchedule, "");
        }

        public DataWithError GetCurrentSchedule()
        {
            var currentSchedule = db.Schedules
                .Where(s => s.IsPublish)
                .OrderBy(s => s.StartDate)
                .Select(s => new ActiveSchedule { Schedule = s, AttendanceStaffCount = s.DailyAttendances.Select(x => x.StaffMemberId).Distinct().Count() })
                .LastOrDefault();
            //if (currentSchedule == null)
            //{
            //    return new DataWithError(null, "No Schedule Found!");
            //}
            return new DataWithError(currentSchedule, "");
        }
        /*{
var schedulein = db.Schedules
.FirstOrDefault(
  x => x.Id != id && (
  start >= x.StartDate && start <= x.EndDate) || 
  (end >= x.StartDate && end <= x.EndDate) || 
  (start <= x.StartDate && end >= x.EndDate
));
return schedulein == null;*/
        //var scheduleinfo = db.Schedules.Where(x => x.Id != id).ToList();
        //if(scheduleinfo!=null)
        //{
        //    foreach (var s in scheduleinfo)
        //    {
        //        if (start < end && start > s.EndDate && start >= DateTime.Now)
        //        {
        //            return true;
        //        }
        //        return false;
        //    }

        //}
        //else if (start < end && start >= DateTime.Now)
        //{
        //    return true;
        //}
        //return false;

        //}


    }
}
