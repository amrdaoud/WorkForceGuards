using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkForceManagementV0.Contexts;
using WorkForceManagementV0.Models;
using WorkForceManagementV0.Models.Bindings;
using WorkForceManagementV0.Models.Reports;
using WorkForceManagementV0.Repositories.Identity;
using WorkForceManagementV0.Repositories.Interfaces;

namespace WorkForceManagementV0.Repositories
{
    public class AnalysisService : IAnalysisService
    {
        private readonly ApplicationDbContext _db;
        private readonly IUserService _userService;
        private readonly IForecastService _forecastService;
        public AnalysisService(ApplicationDbContext db, IUserService userService, IForecastService forecastService)
        {
            _db = db;
            _userService = userService;
            _forecastService = forecastService;
        }
        public DataWithError NeededVsAvailable(int scheduleId, DateTime day)
        {
            var timezone = TimeZoneInfo.FindSystemTimeZoneById("Arab Standard Time");
            day = TimeZoneInfo.ConvertTimeFromUtc(day, timezone).Date;
            var schedule = _db.Schedules.Find(scheduleId);
            if(schedule == null)
            {
                return new DataWithError(null, "Schedule not exist!");
            }
            if(!(day >= schedule.StartDate && day <= schedule.EndDate))
            {
                return new DataWithError(null, "Please choose date between start & end of selected schedule!");
            }
            if(schedule.ForecastId == null)
            {
                return new DataWithError(null, "Schedule is not generated!");
            }
            var phoneActivities = _db.Activities.Where(x => x.IsPhone).Select(x => x.Id).ToList();
            if(phoneActivities.Count == 0)
            {
                return new DataWithError(null, "Phone Activity is not available!");
            }
            var forecast = _db.Forecasts.Find(schedule.ForecastId);
            if (forecast == null)
            {
                return new DataWithError(null, "Forecast is not available!");
            }
            var forecastDetails = _db.ForecastDetails.Include(x => x.Interval)
                .Where(x => x.ForecastId == schedule.ForecastId && x.DayoffWeek == day.DayOfWeek.ToString() && x.IntervalId <= 96);
            var realForecastDetails = _db.ForecastHistories.Include(x => x.Interval).Where(x => x.Day == day).ToList()
                .Select(x => new
                {
                    Day = x.Day,
                    Interval = x.Interval,
                    EmployeeCount = _forecastService.AgentCount(forecast.ServiceLevel, forecast.ServiceTime, x.Offered, x.Duration)
                });
            var result = _db.ScheduleDetail
                .Include(x => x.Interval)
                .Include(x => x.DailyAttendance)
                .Where(x => x.DailyAttendance.Day == day && x.DailyAttendance.ScheduleId == scheduleId && x.IntervalId <= 96)
                .Concat(
                    _db.ScheduleDetail
                    .Include(x => x.Interval)
                    .Include(x => x.DailyAttendance)
                    .Where(x => x.DailyAttendance.Day == day.AddDays(-1) &&  x.IntervalId > 96)
                )
                .ToList()
                .OrderBy(x => x.Interval.TimeMap)
                .GroupBy(x => new
                {
                    Day = day,
                    x.Interval.TimeMap,
                }).Select(g => new NeededVsAvailable
                {
                    Day = g.Key.Day,
                    TimeMap = g.Key.TimeMap,
                    Forecast = forecastDetails.FirstOrDefault(x => x.Interval.TimeMap == g.Key.TimeMap) != null ?
                    forecastDetails.FirstOrDefault(x => x.Interval.TimeMap == g.Key.TimeMap).EmployeeCount : null,
                    Needed = realForecastDetails.FirstOrDefault(x => x.Interval.TimeMap == g.Key.TimeMap) != null ?
                    realForecastDetails.FirstOrDefault(x => x.Interval.TimeMap == g.Key.TimeMap).EmployeeCount : null,
                    Available = g.Sum(x => phoneActivities.Contains(x.ActivityId) ? 1 : 0)
                });
            return new DataWithError(result, "");
            
        }
        public DataWithError LeaderBoard(int scheduleId, int pageIndex, int pageSize)
        {
            var timezone = TimeZoneInfo.FindSystemTimeZoneById("Arab Standard Time");
            var today = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timezone);
            try
            {
                var schedule = _db.Schedules.Find(scheduleId);
                if (schedule == null)
                {
                    return new DataWithError(null, "Schedule not exist!");
                }
                var phoneActivities = _db.Activities.Where(x => x.IsPhone).Select(x => x.Id).ToList();
                var result = new LeaderBoardWithSize();
                var data = _db.ScheduleDetail
                    .Where(x => x.ScheduleId == scheduleId && phoneActivities.Contains(x.ActivityId))
                    .Where(x => x.DailyAttendance.Day < today.Date || (x.DailyAttendance.Day == today.Date && x.Interval.TimeMap < today.AddHours(-0.75).TimeOfDay))
                    .GroupBy(x => new
                    {
                        StaffId = x.DailyAttendance.StaffMemberId,
                        StaffName = x.DailyAttendance.StaffMember.Name,
                        StaffAlias = x.DailyAttendance.StaffMember.Alias
                    }).Select((g) => new LeaderBoard
                    {
                    // Rank = i,
                    AvatarUrl = g.Key.StaffAlias,
                        StaffId = g.Key.StaffId,
                        StaffName = g.Key.StaffName,
                        Adherence = g.Sum(x => x.Duration / 15) / g.Count()
                    }).OrderByDescending(x => x.Adherence);
                result.DataSize = _db.StaffMembers.Where(s => s.DailyAttendances.Where(d => d.ScheduleId == scheduleId).FirstOrDefault() != null).Count();
                result.Data = data.Skip(pageIndex * pageSize).Take(pageSize).ToList().Select((x, i) => new LeaderBoard
                {
                    Rank = (pageIndex * pageSize) + i + 1,
                    AvatarUrl = _userService.getAvatar(x.AvatarUrl),
                    StaffId = x.StaffId,
                    StaffName = x.StaffName,
                    Adherence = x.Adherence
                }).ToList();
                return new DataWithError(result, "");
            }
            catch (Exception ex)
            {
                return new DataWithError(ex,ex.Message);
            }
            
        }

        public DataWithError AdherenceByHos(int scheduleId)
        {
            var timezone = TimeZoneInfo.FindSystemTimeZoneById("Arab Standard Time");
            var today = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timezone);
            var schedule = _db.Schedules.Find(scheduleId);
            if (schedule == null)
            {
                return new DataWithError(null, "Schedule not exist!");
            }
            var phoneActivities = _db.Activities.Where(x => x.IsPhone).Select(x => x.Id).ToList();
            var result = _db.ScheduleDetail
                .Where(x => x.ScheduleId == scheduleId && phoneActivities.Contains(x.ActivityId))
                .Where(x => x.DailyAttendance.Day < today.Date || (x.DailyAttendance.Day == today.Date && x.Interval.TimeMap < today.AddHours(-0.75).TimeOfDay))
                .GroupBy(x => new
                {
                    HosId = x.DailyAttendance.HeadOfSectionId,
                    HosName = x.DailyAttendance.HeadOfSection.Name
                }).Select(g => new AdherenceByHos
                {
                    HosId = g.Key.HosId,
                    HosName = g.Key.HosName,
                    Adherence = (g.Sum(x => x.Duration / 15) / g.Count()) * 100
                }).ToList();
            return new DataWithError(result, "");
        }
    }
}
