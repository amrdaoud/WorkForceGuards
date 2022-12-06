using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WorkForceManagementV0.Contexts;
using WorkForceManagementV0.Models;
using WorkForceManagementV0.Models.Backup;
using WorkForceManagementV0.Models.Bindings;
using WorkForceManagementV0.Models.DTO;
using WorkForceManagementV0.Repositories.Identity;
using WorkForceManagementV0.Repositories.Interfaces;

namespace WorkForceManagementV0.Repositories
{
    public class SuccessWithMessage
    {
        public bool Succeded { get; set; }
        public string Message { get; set; }
    }
    public class AcceptedBreak
    {
        public Interval Interval { get; set; }
        public int BreakCount { get; set; }
        public int staffavailable { get; set; }
    }
    public class DailyStaff
    {
        public int DailyAttendanceId { get; set; }
        public StaffMember StaffMember { get; set; }
        public TransportationRoute TransportationRoute { get; set; }
        public AttendanceType AttendanceType { get; set; }
        public TransportationRoute Shift { get; set; }
    }
    public class ScheduleViewModel
    {
        public string StaffmemberName { get; set; }
        public DateTime Day { get; set; }
        public int StartInterval { get; set; }
        public int EndInterval { get; set; }
        public List<int> Breaks { get; set; }
    }
    public class FinalScheduleService : IFinalScheduleService
    {
        private readonly bool isRandom;
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _config;
        private bool bypassZero = false;
        private int configBreakSize = 4;
        private readonly IUserService _userService;
        public FinalScheduleService(ApplicationDbContext db, IConfiguration config, IUserService userService)
        {
            _db = db;
            isRandom = false;
            _config = config;
            _userService = userService;
        }
        public SuccessWithMessage GenerateSchedule(int scheduleId, int forecastId)
        {
            bypassZero = _config.GetValue<bool>("BypassZeroBreaks");
            configBreakSize = _config.GetValue<int>("BreakSize");
            var forecastdata = _db.Forecasts.FirstOrDefault(x => x.Id == forecastId);
            var scheduledata = _db.Schedules.FirstOrDefault(x => x.Id == scheduleId);
            if (scheduledata == null || scheduledata.IsPublish == true || forecastdata == null || forecastdata.IsSaved == false)
            {
                return new SuccessWithMessage { Succeded = false, Message = "your schedule is not found or  published or your forecast is not found or saved !" };
            }
            var phoneActivityId = _db.Activities.FirstOrDefault(x => x.IsPhone)?.Id;
            var breakActivityId = _db.Activities.FirstOrDefault(x => x.IsBreak)?.Id;
            var absenceActivityId = _db.Activities.FirstOrDefault(x => x.IsAbsence)?.Id;
            var absenceTypeId = _db.AttendanceTypes.FirstOrDefault(x => x.IsAbsence)?.Id;
            var halfTypeId = _db.AttendanceTypes.FirstOrDefault(x => x.Name.ToLower() == "half-hour")?.Id;
            var rules = _db.shiftRules.FirstOrDefault();
            if (phoneActivityId == null || breakActivityId == null || absenceTypeId == null || rules == null)
            {
                return new SuccessWithMessage { Succeded = false, Message = "Missing Activity Or Rules!" };
            }
            var schedule = _db.Schedules.Include(s => s.DailyAttendances).FirstOrDefault(s => s.Id == scheduleId);

            List<ScheduleDetail> scheduleDetails = new List<ScheduleDetail>();
            if (schedule == null)
            {
                return new SuccessWithMessage { Succeded = false, Message = "Schedule not found!" };
            }
            for (DateTime currentDay = schedule.StartDate; currentDay <= schedule.EndDate; currentDay = currentDay.AddDays(1))
            {
                var dailyStaffs = CreateListOfDailyStaff(currentDay);
                var acceptedBreaks = CreateAcceptedBreaks(currentDay, forecastId /*dailyStaffs*/);
                var impossibleInterval = acceptedBreaks.FirstOrDefault(x => x.BreakCount < 0);
                if(bypassZero)
                {
                    if (impossibleInterval != null)
                    {
                        return new SuccessWithMessage
                        {
                            Succeded = false,
                            Message = $@"Impossible to proceed with {impossibleInterval.Interval.TimeMap} on {currentDay.ToString("dd/MM/yyyy")} !"
                        };
                    }
                }
                
                foreach (var dailyStaff in dailyStaffs)
                {
                    if (dailyStaff.AttendanceType.Id == absenceTypeId)
                    {
                        for (int i = dailyStaff.TransportationRoute.ArriveIntervalId; i <= dailyStaff.TransportationRoute.DepartIntervalId; i++)
                        {
                            scheduleDetails.Add(new ScheduleDetail(dailyStaff.DailyAttendanceId, absenceActivityId.Value, i, null, scheduleId));
                        }
                    }
                    else
                    {
                        try
                        {
                            if(dailyStaff.StaffMember.Id == 4190)
                            {
                                var q = 0;
                            } 
                            var breaks = CreateDailyStaffSchedule(dailyStaff, acceptedBreaks, rules, halfTypeId.Value);
                            for (int i = dailyStaff.TransportationRoute.ArriveIntervalId; i <= dailyStaff.TransportationRoute.DepartIntervalId; i++)
                            {
                                var activityId = breaks.Contains(i) ? breakActivityId : phoneActivityId;
                                scheduleDetails.Add(new ScheduleDetail(dailyStaff.DailyAttendanceId, activityId.Value, i, null, scheduleId));
                            }
                        }
                        catch
                        {
                            return new SuccessWithMessage
                            {
                                Succeded = false,
                                Message = $@"Schedule for {dailyStaff.StaffMember.Name } on {currentDay.ToString("dd/MM/yyyy")} failed "
                            };
                        }

                    }

                }

            }
            //// //_db.Database.ExecuteSqlRaw("TRUNCATE TABLE TmpScheduleDetails");

            // _db.TmpScheduleDetails.RemoveRange(_db.TmpScheduleDetails.ToList());
            // _db.TmpScheduleDetails.AddRange(result);
            _db.ScheduleDetail.RemoveRange(_db.ScheduleDetail.Where(x => x.ScheduleId == scheduleId));
            _db.ScheduleDetail.AddRange(scheduleDetails);
            schedule.ForecastId = forecastId;
            _db.SaveChanges();
            return new SuccessWithMessage { Succeded = true, Message = "Schedule Generated" };
        }

        private List<AcceptedBreak> CreateAcceptedBreaks(DateTime currentDay, int forecastId /*List<DailyStaff> dailyStaffs*/)
        {
            List<AcceptedBreak> data = new List<AcceptedBreak>();

            var Lastschedule = _db.Schedules.OrderByDescending(x => x.Id).FirstOrDefault(x => x.IsPublish);
            var currentschedule = _db.Schedules.OrderByDescending(x => x.Id).FirstOrDefault(x => !x.IsPublish);
            var routes = _db.DailyAttendances
                .Include(x => x.TransportationRoute)
                  //.ThenInclude(x => x.SubLocation.Location.Assets)
                  .Include(x => x.ScheduleDetails)
                  .Where(x => x.Day == currentDay && !x.AttendanceType.IsAbsence)
                  .Select(x => new { x.TransportationRoute, x.StaffMember, x.StaffMember.Location.Assets }).ToList();

            var routesbb = _db.DailyAttendances
                .Include(x => x.TransportationRoute)
                //.ThenInclude(x => x.SubLocation.Location.Assets)
                .Include(x => x.ScheduleDetails).ThenInclude(x => x.Activity)
                .Where(x => x.Day == currentDay.AddDays(-1) && !x.AttendanceType.IsAbsence
                 && x.TransportationRoute.ArriveIntervalId <= 97 && x.StaffMember.TransportationRoute.DepartIntervalId >= 97)
                 .Select(x => new { x.TransportationRoute, x.StaffMember, x.StaffMember.Location.Assets }).ToList();


            var s = _db.Intervals.ToList();
            //var forecast = setForecastDetails(forecastId);
            var finalforecast = _db.ForecastDetails.Where(x => x.DayoffWeek == currentDay.DayOfWeek.ToString() && x.ForecastId == forecastId).ToList();

            for (var q = s.Select(x => x.Id).Min(); q <= s.Select(x => x.Id).Max(); q = q + 1)
            {
                var x = new AcceptedBreak();

                var covermapa = s.FirstOrDefault(x => x.Id == q).CoverMap;

                var staffavailable = routes.Where(x => x.TransportationRoute.ArriveIntervalId <= q && x.TransportationRoute.DepartIntervalId >= q)
                        .Select(x => x.StaffMember.Id).Distinct().Count();
                if (q < 97)
                {
                    int staffb = routesbb.Where(y => y.TransportationRoute.ArriveIntervalId <= 96 + q && y.TransportationRoute.DepartIntervalId >= 96 + q)
                    .Select(x => x.StaffMember.Id).Distinct().Count();
                    staffavailable = staffavailable + staffb;
                }
                if (q == 5)
                {
                    var sssss = 0;
                }
                var forcastdata = finalforecast.FirstOrDefault(x => x.IntervalId == q);
                var availablebreak = 0.0;
                if (bypassZero)
                {
                    availablebreak = ((staffavailable * (1 + s.First(i => i.Id == q).Tolerance)) <= forcastdata.EmployeeCount ? 0 : ((staffavailable * (1 + s.First(i => i.Id == q).Tolerance)) - (forcastdata.EmployeeCount)));
                }
                else
                {
                    availablebreak = (((staffavailable * (1 + s.First(i => i.Id == q).Tolerance)) - (forcastdata.EmployeeCount)));
                }

                x.Interval = new Interval();
                x.Interval.CoverMap = s.FirstOrDefault(x => x.Id == q).CoverMap;
                x.Interval.Id = q;
                x.Interval.TimeMap = s.FirstOrDefault(x => x.Id == q).TimeMap;
                x.Interval.OrderMap = s.FirstOrDefault(x => x.Id == q).OrderMap;
                x.Interval.Tolerance = s.FirstOrDefault(x => x.Id == q).Tolerance;
                x.staffavailable = staffavailable;
                x.BreakCount = (int)Math.Ceiling(availablebreak);

                data.Add(x);





            }



            return data;
        }
        private List<DailyStaff> CreateListOfDailyStaff(DateTime currentDay)
        {

            // var Lastschedule = _db.Schedules.OrderByDescending(x => x.Id).FirstOrDefault(x => x.IsPublish);
            var result = new List<DailyStaff>();
            var today = _db.DailyAttendances
                .Include(x => x.TransportationRoute)
                .Include(x => x.StaffMember)
                // .ThenInclude(x => x.TransportationRoute)
                .Include(x => x.AttendanceType).Where(x => x.Day == currentDay)
                .Select(x => new DailyStaff { DailyAttendanceId = x.Id, AttendanceType = x.AttendanceType, Shift = x.TransportationRoute, StaffMember = x.StaffMember, TransportationRoute = x.TransportationRoute })
                .OrderBy(x => x.AttendanceType.Id).ToList();
            var routes = _db.TransportationRoutes.OrderBy(x => x.ArriveIntervalId).ToList();
            var i = 0;

            while (result.Count < today.Count)
            {
                var r = today.FirstOrDefault(x => x.TransportationRoute.Id == routes[i].Id && !(result.Select(x => x.StaffMember.Id)).Contains(x.StaffMember.Id));
                if (r != null)
                {
                    result.Add(r);
                }
                if (i < routes.Count - 1)
                {
                    i++;
                }
                else { i = 0; }
            }
            return result;


        }
        private List<int> CreateDailyStaffSchedule(DailyStaff dailyStaff, List<AcceptedBreak> acceptedBreaks, ShiftRule rules, int halfId)
        {

            var result = new List<int>();
            var zz = acceptedBreaks.Where(x => x.BreakCount == 0);
            result = GenerateSchedualQuarterNew(
                dailyStaff.TransportationRoute.ArriveIntervalId,
                dailyStaff.TransportationRoute.DepartIntervalId.Value,
                new List<int>(),
                acceptedBreaks, rules.StartAfter, rules.EndBefore, rules.BreakBetween, configBreakSize, dailyStaff.AttendanceType.Id == halfId, new List<int>());


            return result;
        }
        private bool GenerateRandom(int start, int end, List<int> ExecludeList, List<int> forbidden, List<int> failedBreaks, out int b)
        {
            var range = Enumerable.Range(start, end).Where(i => !ExecludeList.Contains(i) && !forbidden.Contains(i) && !failedBreaks.Contains(i));
            var rand = new Random();
            if (!(start <= end - (ExecludeList.Count + forbidden.Count + failedBreaks.Count) + 1))
            {
                b = 0;
                return false;
            }
            int index = rand.Next(start, end - (ExecludeList.Count + forbidden.Count + failedBreaks.Count) + 1);
            b = range.ElementAt(index - start);
            return true;
        }
        private bool GenerateMax(int start, int end, List<int> ExecludeList, List<int> forbidden, List<int> failedBreaks, List<AcceptedBreak> acceptedBreaks, out int b)
        {
            var range = Enumerable.Range(start, end - start + 1).Where(i => !ExecludeList.Contains(i) && !forbidden.Contains(i) && !failedBreaks.Contains(i));
            var accepted = acceptedBreaks.Where(x => range.Contains(x.Interval.Id));
            var result = accepted.FirstOrDefault(x => x.BreakCount == accepted.Max(x => x.BreakCount));
            if (result != null)
            {
                b = result.Interval.Id;
                return true;
            }
            b = 0;
            return false;

        }
        private List<int> GenerateForbidden(int start, int end, List<AcceptedBreak> acceptedBreaks)
        {
            if(bypassZero)
            {
                return new List<int>();
            }
            return acceptedBreaks.Where(a => a.Interval.Id >= start && a.Interval.Id <= end && a.BreakCount == 0).Select(a => a.Interval.Id).ToList();
        }
        public List<int> GenerateSchedualQuarterNew(int startInterval, int endInterval, List<int> ExecludeList,
            List<AcceptedBreak> acceptedBreaks, int StartAfter, int EndBefore, int Between, int breakSize, bool IsHalf, List<int> failedBreaks)
        {
            if (ExecludeList.Count == breakSize)
            {
                ExecludeList.Sort();
                return ExecludeList.ToList();
            }
            var forbidden = GenerateForbidden(startInterval + StartAfter, endInterval - EndBefore, acceptedBreaks);
            int b;
            var succeded = false;
            if (isRandom)
            {
                succeded = GenerateRandom(startInterval + StartAfter, endInterval - EndBefore,
                CreateExcludeIntervalRandom(ExecludeList, Between, startInterval, endInterval),
                forbidden, failedBreaks, out b);

            }
            else
            {
                succeded = GenerateMax(startInterval + StartAfter, endInterval - EndBefore,
                CreateExcludeIntervalMax(ExecludeList, Between, startInterval, endInterval, breakSize, IsHalf),
                forbidden, failedBreaks, acceptedBreaks, out b);
            }
            if (succeded)
            {
                ExecludeList.Add(b);

                if (ExecludeList.Count == breakSize - 1 && IsHalf)
                {
                    if (b - 1 > startInterval + StartAfter && !forbidden.Contains(b - 1) && checkBetweenBreaks(ExecludeList, b - 1, Between, "left"))
                    {
                        acceptedBreaks.FirstOrDefault(x => x.Interval.Id == b).BreakCount--;
                        acceptedBreaks.FirstOrDefault(x => x.Interval.Id == b - 1).BreakCount--;
                        ExecludeList.Add(b - 1);
                    }
                    else if (b + 1 < endInterval - EndBefore && !forbidden.Contains(b + 1) && checkBetweenBreaks(ExecludeList, b + 1, Between, "right"))
                    {
                        acceptedBreaks.FirstOrDefault(x => x.Interval.Id == b).BreakCount--;
                        acceptedBreaks.FirstOrDefault(x => x.Interval.Id == b + 1).BreakCount--;
                        ExecludeList.Add(b + 1);
                    }
                    else
                    {
                        ExecludeList.RemoveAt(ExecludeList.Count - 1);
                        // ExecludeList = new List<int>();
                        failedBreaks.Add(b);
                    }
                }
                else
                {
                    acceptedBreaks.FirstOrDefault(x => x.Interval.Id == b).BreakCount--;
                }
                return GenerateSchedualQuarterNew(startInterval, endInterval, ExecludeList, acceptedBreaks, StartAfter, EndBefore, Between, breakSize, IsHalf, failedBreaks);

            }
            else
            {
                return null;
            }
        }
        private bool checkBetweenBreaks(List<int> breakList, int current, int betweenBreaks, string direction)
        {
            if (direction == "left")
            {
                return breakList.Where(x => x <= current && current - betweenBreaks <= x).Count() == 0;
            }
            else
            {
                return breakList.Where(x => x >= current && current + betweenBreaks >= x).Count() == 0;
            }
        }
        private List<int> CreateExcludeIntervalRandom(List<int> ExList, int Between, int startInterval, int endInterval)
        {
            List<int> result = new List<int>();

            foreach (int e in ExList)
            {
                for (int i = e - Between; i <= e + Between; i++)
                {
                    if (i >= startInterval && i < endInterval && !result.Contains(i))
                    {
                        result.Add(i);

                    }
                }
            }
            if (result.Count >= endInterval - startInterval - 8) //Error
            {
                var q = 1;
            }
            return result;
        }
        private List<int> CreateExcludeIntervalMax(List<int> ExList, int Between, int startInterval, int endInterval, int breakSize, bool IsHalf)
        {
            List<int> result = new List<int>();
            // int slicer = (endInterval - startInterval - 1) / breakSize;
            //if(ExList.Count == 0)
            //{
            //    result.AddRange(Enumerable.Range(startInterval, 12));
            //    result.AddRange(Enumerable.Range(endInterval - 12, 12));
            //    return result;
            //}
            var count = (endInterval - startInterval + 1);
            var totalSlices = (count - breakSize) / (breakSize + 1.0);
            int slicer = (int)Math.Ceiling((decimal)(totalSlices - 1 + ((totalSlices - Between) * 2)));
            foreach (int e in ExList)
            {
                for (int i = e - Between; i <= e + Between; i++)
                {
                    if (i >= startInterval && i < endInterval && !result.Contains(i))
                    {
                        result.Add(i);

                    }
                }

            }
            if (ExList.Count == 0)
            {
                for (int i = startInterval + slicer + 1; i <= endInterval; i++)
                {
                    if (!result.Contains(i))
                    {
                        result.Add(i);

                    }
                }
            }
            else if (ExList.Count == 1)
            {
                for (int i = endInterval - slicer - 1; i >= startInterval; i--)
                {
                    if (!result.Contains(i))
                    {
                        result.Add(i);

                    }
                }
            }
            else if (!IsHalf)
            {
                int connector = (Between * (breakSize - ExList.Count)) + (breakSize - ExList.Count - 1);
                var copy = ExList.OrderBy(x => x).ToList();
                for (int i = 0; i < copy.Count - 1; i++)
                {
                    if (copy[i + 1] - copy[i] > connector)
                    {
                        result.AddRange(Enumerable.Range(copy[i] + 1, connector).Intersect(Enumerable.Range(copy[i + 1] - connector, connector)));
                    }
                }

            }
            if (result.Distinct().Count() >= endInterval - startInterval)
            {
                var q = "Error";
            }
            return result;
        }
        public DataWithError PublishSchedule(int id)
        {

            var details = _db.ScheduleDetail.FirstOrDefault(x => x.ScheduleId == id);
            if (details == null)
            {
                return new DataWithError(null, "Schedule is not generated!");
            }
            var schedule = _db.Schedules.Find(id);
            schedule.IsPublish = true;
            _db.SaveChanges();
            return new DataWithError(schedule, "");
        }
        public DataWithError schedulebystaff(int scheduleId, int staffId, ClaimsPrincipal user)
        {
            var appUser = _userService.GetUserInfo(user);
            var data = new DataWithError();
            var staffmember = _db.StaffMembers.FirstOrDefault(x => x.Id == staffId);
            if (appUser.Roles.Contains("User") && !(appUser.Roles.Contains("SuperUser") || appUser.Roles.Contains("Admin") || appUser.Roles.Contains("Hos")))
            {
                if (staffmember.Alias.ToLower() != appUser.Alias.ToLower())
                {
                    return new DataWithError(null, "Unauthorized");
                }
            }
            var checkschedule = _db.Schedules.FirstOrDefault(x => x.Id == scheduleId);
            var checkdailyAttendance = _db.DailyAttendances
                .Include(x => x.ScheduleDetails).ThenInclude(x => x.Activity)
                .Include(x => x.ScheduleDetails).ThenInclude(x => x.Interval)
                .Include(x => x.HeadOfSection)
                .Where(x => x.ScheduleId == scheduleId && x.StaffMemberId == staffId)
               .OrderBy(x => x.Day);
            //if (appUser.Roles.Contains("Hos") && !(appUser.Roles.Contains("SuperUser") || appUser.Roles.Contains("Admin")))
            //{
            //    checkdailyAttendance = checkdailyAttendance.Where(d => d.HeadOfSection.Alias.ToLower() == appUser.Alias.ToLower()).OrderBy(d => d.Day);
            //}
            if (staffmember == null || checkschedule == null || checkdailyAttendance.Count() == 0)
            {
                data.Result = null;
                data.ErrorMessage = "staff  or schedule is not found or DailyAttendance is not created  ";
                return data;
            }
            staffmember.DailyAttendances = checkdailyAttendance.ToList();
            var s = new ScheduleByStaffDto
            {

                Id = staffmember.Id,
                EmployeeId = staffmember.EmployeeId,
                Name = staffmember.Name,
                ScheduleId = checkschedule.Id,
                ScheduleName = checkschedule.Name,
                ScheduleStartDate = checkschedule.StartDate,
                ScheduleEndDate = checkschedule.EndDate,
                DailyAttendances = staffmember.DailyAttendances.OrderBy(s => s.Day).Select(y => new DailyAttendanceDto
                {
                    Day = y.Day,
                    Id = y.Id,
                    HaveBackup = y.HaveBackup,
                    HeadOfSectionName = y.HeadOfSection.Name,
                    ScheduleDetails = y.ScheduleDetails.OrderBy(y => y.IntervalId).ToDictionary(c => c.IntervalId, c => new ScheduleDetailDto
                    {
                        Id = c.Id,
                        IntervalId = c.IntervalId,
                        IntervalTimeMap = c.Interval.TimeMap,
                        ActivityId = c.ActivityId,
                        ActivityName = c.Activity.Name,
                        ActivityColor = c.Activity.Color,
                        Duration = c.Duration != null ? (int)c.Duration : null
                    })
                })
            };

            data.Result = s;
            data.ErrorMessage = null;
            return data;

        }
        public DataWithError schedulebyDate(int scheduleId, DateTime day)
        {
            var data = new DataWithError();
            var checkschedule = _db.Schedules.FirstOrDefault(x => x.Id == scheduleId);
            var checkdailyAttendance = _db.DailyAttendances.Include(x => x.StaffMember)
                .Include(x => x.ScheduleDetails).ThenInclude(x => x.Interval)
                .Include(x => x.ScheduleDetails).ThenInclude(x => x.Activity)
                .Where(x => x.ScheduleId == scheduleId && x.Day == day).ToList();
            if (checkschedule == null || checkdailyAttendance.Count == 0)
            {
                data.Result = null;
                data.ErrorMessage = "staff  or schedule is not found or DailyAttendance  is not created  ";
                return data;
            }

            var ss = new ScheduleByDateDto
            {
                Day = day,
                ScheduleId = checkschedule.Id,
                ScheduleName = checkschedule.Name,
                ScheduleStartDate = checkschedule.StartDate,
                ScheduleEndDate = checkschedule.EndDate,
                DailyAttendances = checkdailyAttendance.Select(y => new DailyAttendanceByDayDto
                {
                    Id = y.StaffMemberId,
                    EmployeeId = y.StaffMember.EmployeeId,
                    AttendanceId = y.Id,
                    Name = y.StaffMember.Name,
                    ScheduleDetails = y.ScheduleDetails.ToDictionary(c => c.IntervalId, c => new ScheduleDetailDto
                    {
                        Id = c.Id,
                        IntervalId = c.IntervalId,
                        IntervalTimeMap = c.Interval.TimeMap,
                        ActivityId = c.ActivityId,
                        ActivityName = c.Activity.Name,
                        ActivityColor = c.Activity.Color
                    })
                }).ToList()
            };

            data.Result = ss;
            data.ErrorMessage = null;
            return data;


        }
        public DataWithError schedulebyDatePage(int scheduleId, DateTime day, int pageIndex, int pageSize, string searchQuery, string type, ClaimsPrincipal user)
        {
            var appUser = _userService.GetUserInfo(user);
            var data = new DataWithError();
            var checkschedule = _db.Schedules.FirstOrDefault(x => x.Id == scheduleId);
            var dbDailyAttendance = _db.DailyAttendances
                .Include(x => x.StaffMember)
                .Include(x => x.HeadOfSection)
                .Include(x => x.ScheduleDetails).ThenInclude(x => x.Interval)
                .Include(x => x.ScheduleDetails).ThenInclude(x => x.Activity)
                .Where(x => x.ScheduleId == scheduleId && x.Day == day);
            if (appUser.Roles.Contains("Hos") && !(appUser.Roles.Contains("SuperUser") || appUser.Roles.Contains("Admin")))
            {
                dbDailyAttendance = dbDailyAttendance.Where(d => d.HeadOfSection.Alias.ToLower() == appUser.Alias.ToLower());
            }
            else if (appUser.Roles.Contains("User") && !(appUser.Roles.Contains("SuperUser") || appUser.Roles.Contains("Admin")))
            {
                dbDailyAttendance = dbDailyAttendance.Where(d => d.StaffMember.Alias.ToLower() == appUser.Alias.ToLower());
            }
            var dailyAttendances = new List<DailyAttendance>();
            if (checkschedule == null || dbDailyAttendance.Count() == 0)
            {
                data.Result = null;
                data.ErrorMessage = "staff  or schedule is not found or DailyAttendance  is not created  ";
                return data;
            }
            if (!string.IsNullOrEmpty(searchQuery))
            {
                if (type == "Staff")
                {
                    if (int.TryParse(searchQuery, out var employeeId))
                    {
                        dbDailyAttendance = dbDailyAttendance.Where(x => x.StaffMember.EmployeeId == employeeId).OrderBy(x => x.StaffMember.EmployeeId);
                    }
                    else
                    {
                        dbDailyAttendance = dbDailyAttendance.Where(x => x.StaffMember.Name.ToLower().StartsWith(searchQuery.ToLower())
                        || x.StaffMember.Alias.ToLower().StartsWith(searchQuery.ToLower()) || x.StaffMember.Email.ToLower().StartsWith(searchQuery.ToLower())).OrderBy(x => x.StaffMember.EmployeeId);
                    }
                }
                else if (type == "Hos")
                {
                    if (int.TryParse(searchQuery, out var employeeId))
                    {
                        dbDailyAttendance = dbDailyAttendance.Where(x => x.HeadOfSection.EmployeeId == employeeId).OrderBy(x => x.StaffMember.EmployeeId);
                    }
                    else
                    {
                        dbDailyAttendance = dbDailyAttendance.Where(x => x.HeadOfSection.Name.ToLower().StartsWith(searchQuery.ToLower())
                        || x.HeadOfSection.Alias.ToLower().StartsWith(searchQuery.ToLower()) || x.HeadOfSection.Email.ToLower().StartsWith(searchQuery.ToLower())).OrderBy(x => x.StaffMember.EmployeeId);
                    }
                }
                else if (type == "Transportation")
                {
                    if (int.TryParse(searchQuery, out var transportationId))
                    {
                        dbDailyAttendance = dbDailyAttendance.Where(x => x.TransportationRouteId == transportationId).OrderBy(x => x.StaffMember.EmployeeId);
                    }
                    else
                    {
                        dbDailyAttendance = dbDailyAttendance.Where(x => x.TransportationRoute.Name.ToLower().StartsWith(searchQuery.ToLower())).OrderBy(x => x.StaffMember.EmployeeId);
                    }
                }
                else if(type == "Time")
                {
                    if (TimeSpan.TryParse(searchQuery, out TimeSpan time))
                    {
                        dbDailyAttendance = dbDailyAttendance.Where(x => time >= x.TransportationRoute.ArriveInterval.TimeMap && time <= x.TransportationRoute.DepartInterval.TimeMap).OrderBy(x => x.StaffMember.EmployeeId);
                    }
                }
                else if (type == "Break Time")
                {
                    if (TimeSpan.TryParse(searchQuery, out TimeSpan time))
                    {
                        var breakActivityId = _db.Activities.FirstOrDefault(x => x.IsBreak)?.Id;
                        if(breakActivityId != null)
                        {
                            dbDailyAttendance = dbDailyAttendance
                                .Where(x => x.ScheduleDetails.FirstOrDefault(z => z.ActivityId == breakActivityId && z.Interval.TimeMap == time) != null)
                                .OrderBy(x => x.StaffMember.EmployeeId);
                        }
                    }
                }
                else if (type == "Location")
                {
                    if (int.TryParse(searchQuery, out var locationId))
                    {
                        dbDailyAttendance = dbDailyAttendance.Where(x => x.SublocationId == locationId).OrderBy(x => x.StaffMember.EmployeeId);
                    }
                    else
                    {
                        dbDailyAttendance = dbDailyAttendance.Where(x => x.Sublocation.Name.ToLower().StartsWith(searchQuery.ToLower())).OrderBy(x => x.StaffMember.EmployeeId);
                    }
                    
                }
                else if (type == "Activity Time")
                {
                    var filters = searchQuery.Split('-');
                    
                    if (filters.Length == 2 && TimeSpan.TryParse(filters[1], out TimeSpan time) && int.TryParse(filters[0], out int activityId))
                    {
                        if (activityId == 0)
                        {
                            dbDailyAttendance = dbDailyAttendance
                            .Where(x => x.ScheduleDetails.FirstOrDefault(z => z.Interval.TimeMap == time) != null)
                            .OrderBy(x => x.StaffMember.EmployeeId);
                        }
                        else
                        {
                            dbDailyAttendance = dbDailyAttendance
                            .Where(x => x.ScheduleDetails.FirstOrDefault(z => z.ActivityId == activityId && z.Interval.TimeMap == time) != null)
                            .OrderBy(x => x.StaffMember.EmployeeId);
                        }
                    }
                }
            }
            var size = dbDailyAttendance.Count();
            var ss = new ScheduleByDateDto
            {
                Day = day,
                ScheduleId = checkschedule.Id,
                ScheduleName = checkschedule.Name,
                ScheduleStartDate = checkschedule.StartDate,
                ScheduleEndDate = checkschedule.EndDate,
                DailyAttendances = dbDailyAttendance.OrderBy(x => x.StaffMember.Name).Skip(pageSize * pageIndex).Take(pageSize).ToList().Select(y => new DailyAttendanceByDayDto
                {

                    Id = y.StaffMemberId,
                    EmployeeId = y.StaffMember.EmployeeId,
                    Name = y.StaffMember.Name,
                    AttendanceId = y.Id,
                    HeadOfSectionName = y.HeadOfSection.Name,
                    ScheduleDetails = y.ScheduleDetails.OrderBy(y => y.IntervalId).ToDictionary(c => c.IntervalId, c => new ScheduleDetailDto
                    {
                        Id = c.Id,
                        IntervalId = c.IntervalId,
                        IntervalTimeMap = c.Interval.TimeMap,
                        ActivityId = c.ActivityId,
                        ActivityName = c.Activity.Name,
                        ActivityColor = c.Activity.Color,
                        Duration = c.Duration != null ? (int)c.Duration : null
                    })
                }).ToList()
            };

            data.Result = new { Result = ss, Size = size };
            data.ErrorMessage = null;
            return data;
        }
        public DataWithError schedulebystaffday(int scheduleId, int staffId, DateTime day, ClaimsPrincipal user)
        {
            var appUser = _userService.GetUserInfo(user);
            var data = new DataWithError();
            var staffmember = _db.StaffMembers.FirstOrDefault(x => x.Id == staffId);
            if (appUser.Roles.Contains("User") && !(appUser.Roles.Contains("SuperUser") || appUser.Roles.Contains("Admin") || appUser.Roles.Contains("Hos")))
            {
                if (staffmember.Alias.ToLower() != appUser.Alias.ToLower())
                {
                    return new DataWithError(null, "Unauthorized");
                }
            }
            var checkschedule = _db.Schedules.FirstOrDefault(x => x.Id == scheduleId);
            var checkdailyAttendance = _db.DailyAttendances
                .Include(x => x.ScheduleDetails).ThenInclude(x => x.Activity)
                .Include(x => x.ScheduleDetails).ThenInclude(x => x.Interval)
                .Include(x => x.HeadOfSection)
                .Where(x => x.ScheduleId == scheduleId && x.StaffMemberId == staffId && x.Day == day)
               .OrderBy(x => x.Day);
            //if (appUser.Roles.Contains("Hos") && !(appUser.Roles.Contains("SuperUser") || appUser.Roles.Contains("Admin")))
            //{
            //    if (checkdailyAttendance.FirstOrDefault(d => d.HeadOfSection.Alias.ToLower() == appUser.Alias.ToLower()) == null)
            //    {
            //        return new DataWithError(null, "Unauthorized");
            //    }
            //}
            if (staffmember == null || checkschedule == null || checkdailyAttendance.Count() == 0)
            {
                data.Result = null;
                data.ErrorMessage = "staff  or schedule is not found or DailyAttendance  is not created  ";
                return data;
            }
            staffmember.DailyAttendances = checkdailyAttendance.ToList();
            var s = new ScheduleByStaffDto
            {

                Id = staffmember.Id,
                EmployeeId = staffmember.EmployeeId,
                Name = staffmember.Name,
                ScheduleId = checkschedule.Id,
                ScheduleName = checkschedule.Name,
                ScheduleStartDate = checkschedule.StartDate,
                ScheduleEndDate = checkschedule.EndDate,
                DailyAttendances = staffmember.DailyAttendances.Select(y => new DailyAttendanceDto
                {
                    Day = y.Day,
                    Id = y.Id,
                    HaveBackup = y.HaveBackup,
                    HeadOfSectionName = y.HeadOfSection.Name,
                    ScheduleDetails = y.ScheduleDetails.OrderBy(y => y.IntervalId).ToDictionary(c => c.IntervalId, c => new ScheduleDetailDto
                    {
                        Id = c.Id,
                        IntervalId = c.IntervalId,
                        IntervalTimeMap = c.Interval.TimeMap,
                        ActivityId = c.ActivityId,
                        ActivityName = c.Activity.Name,
                        ActivityColor = c.Activity.Color,
                        Duration = c.Duration != null ? (int)c.Duration : null
                    })
                })
            };
            data.Result = s;
            data.ErrorMessage = null;
            return data;
        }
        public DataWithError schedulebyId(int scheduleId)
        {
            var data = new DataWithError();
            var checkschedule = _db.Schedules.FirstOrDefault(x => x.Id == scheduleId);
            if (checkschedule == null)
            {
                return new DataWithError(null, "Schedule not found!");
            }
            var staffMembers = _db.StaffMembers.Where(x => x.DailyAttendances.FirstOrDefault(s => s.ScheduleId == scheduleId) != null);

            var ResultSize = staffMembers.Count();
            var result = new ScheduleById
            {
                ScheduleId = scheduleId,
                ScheduleName = checkschedule.Name,
                DailyStaffs = staffMembers.Select(x => new DailyStaffDto
                {
                    Id = x.Id,
                    EmployeeId = x.EmployeeId,
                    Name = x.Name,
                    DailyAttendances = x.DailyAttendances.Where(s => s.ScheduleId == scheduleId).OrderBy(s => s.Day).Select(d => new DailyAttendanceByScheduleIdDto
                    {
                        Id = d.Id,
                        ShiftId = (int)d.TransportationRouteId,
                        AttendanceTypeId = d.AttendanceTypeId,
                        TransportationName = d.TransportationRoute.Name,
                        TransportationArriveTime = x.TransportationRoute.ArriveInterval.TimeMap,
                        TransportationDepartTime = x.TransportationRoute.DepartInterval.TimeMap,
                        AttendanceTypeName = d.AttendanceType.Name,
                        Day = d.Day,
                        IsAbsence = d.AttendanceType.IsAbsence
                    }).ToList()
                }).ToList()
            };


            data.Result = new { Result = result, Size = ResultSize };
            data.ErrorMessage = null;
            return data;


        }
        public DataWithError schedulebyIdpage(int scheduleId, int pageIndex, int pageSize, string searchQuery, string type, ClaimsPrincipal user)
        {
            var appUser = _userService.GetUserInfo(user);
            var data = new DataWithError();
            var checkschedule = _db.Schedules.FirstOrDefault(x => x.Id == scheduleId);
            if (checkschedule == null)
            {
                return new DataWithError(null, "Schedule not found!");
            }
            var staffMembers = _db.StaffMembers.Where(x => x.DailyAttendances.FirstOrDefault(s => s.ScheduleId == scheduleId) != null);
            if (appUser.Roles.Contains("Hos") && !(appUser.Roles.Contains("SuperUser") || appUser.Roles.Contains("Admin")))
            {
                staffMembers = staffMembers.Where(x => x.DailyAttendances.FirstOrDefault(s => s.ScheduleId == scheduleId && s.HeadOfSection.Alias.ToLower() == appUser.Alias.ToLower()) != null);
            }
            else if (appUser.Roles.Contains("User") && !(appUser.Roles.Contains("SuperUser") || appUser.Roles.Contains("Admin")))
            {
                staffMembers = staffMembers.Where(x => x.Alias.ToLower() == appUser.Alias.ToLower());
            }
            if (!string.IsNullOrEmpty(searchQuery))
            {

                if (type == "Staff")
                {
                    if (int.TryParse(searchQuery, out var employeeId))
                    {
                        staffMembers = staffMembers.Where(x => x.EmployeeId == employeeId).OrderBy(x => x.EmployeeId);
                    }
                    else
                    {
                        staffMembers = staffMembers.Where(x => x.Name.ToLower().StartsWith(searchQuery.ToLower())
                        || x.Alias.ToLower().StartsWith(searchQuery.ToLower()) || x.Email.ToLower().StartsWith(searchQuery.ToLower()));
                    }
                } else if (type == "Hos")
                {
                    if (int.TryParse(searchQuery, out var employeeId))
                    {
                        staffMembers = staffMembers.Where(x => x.DailyAttendances.FirstOrDefault(s => s.ScheduleId == scheduleId && s.HeadOfSection.EmployeeId == employeeId) != null);
                    }
                    else
                    {
                        staffMembers = staffMembers.Where(x => x.DailyAttendances.FirstOrDefault(s => s.ScheduleId == scheduleId && (s.HeadOfSection.Alias.ToLower().StartsWith(searchQuery.ToLower())
                        || s.HeadOfSection.Name.ToLower().StartsWith(searchQuery.ToLower()) || s.HeadOfSection.Email.StartsWith(searchQuery.ToLower()))) != null);
                    }
                } else if (type == "Transportation")
                {
                    if (int.TryParse(searchQuery, out var transportationId))
                    {
                        staffMembers = staffMembers.Where(x => x.DailyAttendances.FirstOrDefault(s => s.ScheduleId == scheduleId && s.TransportationRouteId == transportationId) != null);
                    }
                    else
                    {
                        staffMembers = staffMembers.Where(x => x.DailyAttendances.FirstOrDefault(s => s.ScheduleId == scheduleId && s.TransportationRoute.Name.ToLower().StartsWith(searchQuery.ToLower())) != null);
                    }
                }
                else if (type == "Location")
                {
                    if (int.TryParse(searchQuery, out var locationId))
                    {
                        staffMembers = staffMembers.Where(x => x.DailyAttendances.FirstOrDefault(s => s.ScheduleId == scheduleId && s.SublocationId == locationId) != null);
                    }
                    else
                    {
                        staffMembers = staffMembers.Where(x => x.DailyAttendances.FirstOrDefault(s => s.ScheduleId == scheduleId && s.Sublocation.Name.ToLower().StartsWith(searchQuery.ToLower())) != null);
                    }

                }
                else if(type == "Time")
                {
                    if(TimeSpan.TryParse(searchQuery, out TimeSpan time)) {
                        staffMembers = staffMembers.Where(x => x.DailyAttendances.FirstOrDefault(s => s.ScheduleId == scheduleId && time >= s.TransportationRoute.ArriveInterval.TimeMap && time <= s.TransportationRoute.DepartInterval.TimeMap) != null);
                    }
                }
                
            }

            var ResultSize = staffMembers.Count();
            var result = new ScheduleById
            {
                ScheduleId = scheduleId,
                ScheduleName = checkschedule.Name,
                DailyStaffs = staffMembers.OrderBy(x => x.Name).Skip(pageSize * pageIndex).Take(pageSize).Select(x => new DailyStaffDto
                {
                    Id = x.Id,
                    EmployeeId = x.EmployeeId,
                    Name = x.Name,
                    DailyAttendances = x.DailyAttendances.Where(s => s.ScheduleId == scheduleId).OrderBy(s => s.Day).Select(d => new DailyAttendanceByScheduleIdDto
                    {
                        Id = d.Id,
                        ShiftId = (int)d.TransportationRouteId,
                        HeadOfSectionName = d.HeadOfSection.Name,
                        AttendanceTypeId = d.AttendanceTypeId,
                        TransportationName = d.TransportationRoute.Name,
                        TransportationArriveTime = d.TransportationRoute.ArriveInterval.TimeMap,
                        TransportationDepartTime = d.TransportationRoute.DepartInterval.TimeMap,
                        AttendanceTypeName = d.AttendanceType.Name,
                        Day = d.Day,
                        Color = d.AttendanceType.DefaultActivity.Color,
                        IsAbsence = d.AttendanceType.IsAbsence,
                        HaveBackup = d.HaveBackup
                    }).ToList()
                }).ToList()
            };


            data.Result = new { Result = result, Size = ResultSize };
            data.ErrorMessage = null;
            return data;
        }
        public DataWithError EditScheduleDetails(int scheduleDetailId, int activityId, ClaimsPrincipal user)
        {
            var data = new DataWithError();
            var model = new ScheduleDetailDto();
            var dataschedule = _db.ScheduleDetail.FirstOrDefault(x => x.Id == scheduleDetailId);
            var lockmodel = new Lock();
            var activitydata = _db.Activities;
            if (dataschedule == null)
            {
                data.Result = null;
                data.ErrorMessage = "scheduledetails is not found ";
                return data;
            }
            var appUser = _userService.GetUserInfo(user);
            backupDailyAttendance(dataschedule.DailyAttendanceId, appUser.Alias);
            dataschedule.ActivityId = activityId;
            _db.SaveChanges();

            var Newdata = _db.ScheduleDetail.Include(X => X.Activity).Include(x => x.Interval).FirstOrDefault(x => x.Id == scheduleDetailId);
            model.Id = Newdata.Id;
            model.IntervalId = Newdata.IntervalId;
            model.ActivityId = Newdata.ActivityId;
            model.ActivityName = Newdata.Activity.Name;
            model.ActivityColor = Newdata.Activity.Color;
            model.Duration = Newdata.Duration != null ? (int)Newdata.Duration : null;
            model.IntervalTimeMap = Newdata.Interval.TimeMap;
            model.Id = Newdata.Id;
            model.IntervalId = Newdata.IntervalId;
            model.ActivityId = Newdata.ActivityId;
            model.ActivityName = Newdata.Activity.Name;
            model.ActivityColor = Newdata.Activity.Color;
            model.Duration = Newdata.Duration != null ? (int)Newdata.Duration : null;
            model.IntervalTimeMap = Newdata.Interval.TimeMap;
            //lockmodel.ScheduleDetailId = scheduleDetailId;
            //lockmodel.EmployeeName = "ola";
            //lockmodel.OldActivityId = activityId;
            //lockmodel.OldName = activitydata.FirstOrDefault(x => x.Id == activityId).Name;
            //lockmodel.NewActivityId = Newdata.ActivityId;
            //lockmodel.NewName = activitydata.FirstOrDefault(x => x.Id == Newdata.ActivityId).Name;
            //_db.Locks.Add(lockmodel);
            _db.SaveChanges();
            data.Result = model;
            data.ErrorMessage = null;
            return data;
        }
        public DataWithError EditScheduleDetailsMultiple(EditScheduleDetailsBinding model, ClaimsPrincipal user)
        {
            var appUser = _userService.GetUserInfo(user);
            var dailyAttendancesId = _db.DailyAttendances
                .Where(d => d.ScheduleDetails.Any(z => model.ScheduleDetailsIds.Contains(z.Id))).Distinct().Select(x => x.Id).ToList();
            if(dailyAttendancesId.Count == 0)
            {
                return new DataWithError(null, "Daily Attendance Not Found !");
            }
            foreach(int id in dailyAttendancesId)
            {
                backupDailyAttendance(id, appUser.Alias);
            }
            foreach(int id in model.ScheduleDetailsIds)
            {
                _db.ScheduleDetail.Find(id).ActivityId = model.ActivityId;
            }
            _db.SaveChanges();
            var result = _db.ScheduleDetail.Where(x => model.ScheduleDetailsIds.Contains(x.Id)).Select(x => new ScheduleDetailDto
            {
                Id = x.Id,
                ActivityColor = x.Activity.Color,
                ActivityId = x.ActivityId,
                ActivityName = x.Activity.Name,
                Duration = x.Duration != null ? (int)x.Duration : null,
                IntervalId = x.IntervalId,
                IntervalTimeMap = x.Interval.TimeMap

            }).ToList();
            return new DataWithError(result, "");

        }
        public DataWithError AddScheduleDetail(int dailyAttendanceId, int intervalId, int activityId, ClaimsPrincipal user)
        {
            var dailyAttendance = _db.DailyAttendances.Find(dailyAttendanceId);
            if (dailyAttendance == null)
            {
                return new DataWithError(null, "Daily Attendance not found!");
            }
            _db.Entry(dailyAttendance).Collection(d => d.ScheduleDetails).Load();
            var found = dailyAttendance.ScheduleDetails.FirstOrDefault(x => x.IntervalId == intervalId);
            if (found != null)
            {
                return new DataWithError(null, "Interval already exist!");
            }
            var appUser = _userService.GetUserInfo(user);
            backupDailyAttendance(dailyAttendanceId, appUser.Alias);
            var newDetail = new ScheduleDetail(dailyAttendanceId, activityId, intervalId, null, dailyAttendance.ScheduleId);
            _db.ScheduleDetail.Add(newDetail);
            _db.SaveChanges();
            var model = new ScheduleDetailDto();
            var Newdata = _db.ScheduleDetail.Include(X => X.Activity).Include(x => x.Interval).FirstOrDefault(x => x.Id == newDetail.Id);
            model.Id = Newdata.Id;
            model.IntervalId = Newdata.IntervalId;
            model.ActivityId = Newdata.ActivityId;
            model.ActivityName = Newdata.Activity.Name;
            model.ActivityColor = Newdata.Activity.Color;
            model.Duration = Newdata.Duration != null ? (int)Newdata.Duration : null;
            model.IntervalTimeMap = Newdata.Interval.TimeMap;
            model.Id = Newdata.Id;
            model.IntervalId = Newdata.IntervalId;
            model.ActivityId = Newdata.ActivityId;
            model.ActivityName = Newdata.Activity.Name;
            model.ActivityColor = Newdata.Activity.Color;
            model.Duration = Newdata.Duration != null ? (int)Newdata.Duration : null;
            model.IntervalTimeMap = Newdata.Interval.TimeMap;
            return new DataWithError(model, "");

        }

        public DataWithError AddScheduleDetailMultiple(AddScheduleDetailsBinding model, int scheduleId, ClaimsPrincipal user)
        {
            var schedule = _db.Schedules.Find(scheduleId);
            if(schedule == null)
            {
                return new DataWithError(null, "Schedule Not Found!");
            }
            var dailyAttendances = _db.DailyAttendances
                .Where(d => model.AddScheduleDetails.Select(x => x.DailyAttendanceId).Contains(d.Id)).ToList();
            if (dailyAttendances.Count == 0)
            {
                return new DataWithError(null, "Daily Attendance not found!");
            }
            var appUser = _userService.GetUserInfo(user);
            foreach (var dailyAttendanceId in dailyAttendances.Select(x => x.Id).Distinct())
            {
                backupDailyAttendance(dailyAttendanceId, appUser.Alias);
            }
            foreach(var element in model.AddScheduleDetails)
            {
                var available = _db.ScheduleDetail.FirstOrDefault(x => x.DailyAttendanceId == element.DailyAttendanceId && x.IntervalId == element.IntervalId);
                if(available != null)
                {
                    _db.ScheduleDetail
                    .Add(new ScheduleDetail(element.DailyAttendanceId, model.ActivityId, element.IntervalId, null, scheduleId));
                }
            }
            _db.SaveChanges();
            var result = _db.ScheduleDetail.Where(x => dailyAttendances
            .Select(x => x.Id).Distinct()
            .Contains(x.DailyAttendanceId)
            && model.AddScheduleDetails.Select(z => z.IntervalId).Contains(x.IntervalId)).Select(s => new ScheduleDetailDto
            {
                Id = s.Id,
                ActivityColor = s.Activity.Color,
                ActivityId = s.ActivityId,
                ActivityName = s.Activity.Name,
                Duration = s.Duration != null ? (int)s.Duration : null,
                IntervalId = s.IntervalId,
                IntervalTimeMap = s.Interval.TimeMap
            }).ToList();
            return new DataWithError(result, null);
        }
        public DataWithError EditDailyAttendance(int dailyAttendanceId, int attendanceTypeId, ClaimsPrincipal user)
        {
            var dailyAttendance = _db.DailyAttendances.Find(dailyAttendanceId);
            
            if (dailyAttendance == null)
            {
                return new DataWithError(null, "Daily Attendance not found!");
            }
            var attendanceType = _db.AttendanceTypes.Find(attendanceTypeId);
            if (attendanceType == null)
            {
                return new DataWithError(null, "Attendance Type not found!");
            }
            var appUser = _userService.GetUserInfo(user);
            if (backupDailyAttendance(dailyAttendanceId, appUser.Alias))
            {
                dailyAttendance.HaveBackup = true;
            }
            dailyAttendance.AttendanceTypeId = attendanceTypeId;
            _db.Entry(dailyAttendance).Reference(d => d.TransportationRoute).Load();
            _db.Entry(dailyAttendance).Reference(d => d.HeadOfSection).Load();
            _db.Entry(dailyAttendance).Collection(d => d.ScheduleDetails).Load();
            _db.Entry(attendanceType).Reference(a => a.DefaultActivity).Load();
            foreach (var scheduleDetail in dailyAttendance.ScheduleDetails)
            {
                scheduleDetail.ActivityId = attendanceType.DefaultActivityId.Value;
            }
            _db.Entry(dailyAttendance).State = EntityState.Modified;
            _db.SaveChanges();
            var result = new DailyAttendanceByScheduleIdDto
            {
                Id = dailyAttendance.Id,
                ShiftId = (int)dailyAttendance.TransportationRouteId,
                AttendanceTypeId = dailyAttendance.AttendanceTypeId,
                TransportationName = dailyAttendance.TransportationRoute.Name,
                TransportationArriveTime = new TimeSpan(),
                TransportationDepartTime = new TimeSpan(),
                AttendanceTypeName = attendanceType.Name,
                HeadOfSectionName = dailyAttendance.HeadOfSection.Name,
                Day = dailyAttendance.Day,
                Color = attendanceType.DefaultActivity.Color,
                IsAbsence = attendanceType.IsAbsence,
                HaveBackup = dailyAttendance.HaveBackup
            };
            return new DataWithError(result, "");
        }
        public DataWithError EditDailyAttendanceShift(int dailyAttendanceId, int transportationId, ClaimsPrincipal user)
        {
            var dailyAttendance = _db.DailyAttendances.Find(dailyAttendanceId);
            if (dailyAttendance == null)
            {
                return new DataWithError(null, "Daily Attendance not found!");
            }
            var transportation = _db.TransportationRoutes.Find(transportationId);
            if (transportation == null)
            {
                return new DataWithError(null, "Transportation not found!");
            }
            var appUser = _userService.GetUserInfo(user);
            if (backupDailyAttendance(dailyAttendanceId, appUser.Alias))
            {
                dailyAttendance.HaveBackup = true;
            }
            _db.Entry(dailyAttendance).Collection(d => d.ScheduleDetails).Load();
            _db.Entry(dailyAttendance).Reference(d => d.AttendanceType).Load();
            _db.Entry(dailyAttendance).Reference(d => d.HeadOfSection).Load();
            _db.Entry(dailyAttendance).Reference(d => d.TransportationRoute).Load();
            _db.Entry(dailyAttendance.AttendanceType).Reference(d => d.DefaultActivity).Load();
            var undefinedId = _db.Activities.FirstOrDefault(x => x.IsUndefined).Id;
            var oldScheduleDetails = dailyAttendance.ScheduleDetails.OrderBy(x => x.IntervalId).ToList();
            var transScheduleDetails = dailyAttendance.ScheduleDetails
                .Where(x => x.IntervalId >= dailyAttendance.TransportationRoute.ArriveIntervalId && x.IntervalId <= dailyAttendance.TransportationRoute.DepartIntervalId)
                .OrderBy(x => x.IntervalId)
                .Take(transportation.DepartIntervalId.Value - transportation.ArriveIntervalId + 1)
                .ToList();
            var transIntervals = Enumerable.Range((int)transportation.ArriveIntervalId, transScheduleDetails.Count).ToList();
            var newScheduleDetails = new List<ScheduleDetail>();
            for (int i = 0; i < transIntervals.Count; i++)
            {
                var oldMappedDetail = oldScheduleDetails.FirstOrDefault(x => x.IntervalId == transIntervals[i]);
                if(oldMappedDetail == null)
                {
                    newScheduleDetails.Add(new ScheduleDetail(dailyAttendanceId, transScheduleDetails[i].ActivityId, transIntervals[i], null, dailyAttendance.ScheduleId));
                }
                else
                {
                    newScheduleDetails.Add(new ScheduleDetail(dailyAttendanceId, transScheduleDetails[i].ActivityId, transIntervals[i], oldMappedDetail.Duration, dailyAttendance.ScheduleId));
                }
            }
            newScheduleDetails.AddRange(oldScheduleDetails.Where(x => !newScheduleDetails.Select(y => y.IntervalId).Contains(x.IntervalId) &&
            x.Duration != null).Select(s => { s.ActivityId = undefinedId; return s; }));
            dailyAttendance.ScheduleDetails = newScheduleDetails;
            dailyAttendance.TransportationRouteId = transportationId;
            _db.Entry(dailyAttendance).State = EntityState.Modified;
            _db.SaveChanges();
            var result = new DailyAttendanceByScheduleIdDto
            {
                Id = dailyAttendance.Id,
                ShiftId = (int)dailyAttendance.TransportationRouteId,
                AttendanceTypeId = dailyAttendance.AttendanceTypeId,
                HeadOfSectionName = dailyAttendance.HeadOfSection.Name,
                TransportationName = dailyAttendance.TransportationRoute.Name,
                TransportationArriveTime = new TimeSpan(),
                TransportationDepartTime = new TimeSpan(),
                AttendanceTypeName = dailyAttendance.AttendanceType.Name,
                Day = dailyAttendance.Day,
                Color = dailyAttendance.AttendanceType.DefaultActivity.Color,
                IsAbsence = dailyAttendance.AttendanceType.IsAbsence,
                HaveBackup = dailyAttendance.HaveBackup
            };
            return new DataWithError(result, "");
        }
        public DataWithError RemoveScheduleDetail(int scheduleDetailId, ClaimsPrincipal user)
        {
            var scheduleDetail = _db.ScheduleDetail.Find(scheduleDetailId);
            if(scheduleDetail == null)
            {
                return new DataWithError(null, "Interval  not found");
            }
            if(scheduleDetail.Duration != null)
            {
                return new DataWithError(null, "Cannot delete interval with duration");
            }
            var dailyAttendance = _db.DailyAttendances.Find(scheduleDetail.DailyAttendanceId);
            var appUser = _userService.GetUserInfo(user);
            backupDailyAttendance(dailyAttendance.Id, appUser.Alias);
            _db.ScheduleDetail.Remove(scheduleDetail);
            _db.SaveChanges();
            return new DataWithError(true, "");
        }
        private bool backupDailyAttendance(int dailyAttendanceId, string alias)
        {
            var dailyAttendance = _db.DailyAttendances.Find(dailyAttendanceId);
            if (dailyAttendance == null)
            {
                return false;
            }
            //var bkpDailyAttendance = _db.BkpDailyAttendances.Find(dailyAttendanceId);
            //if (bkpDailyAttendance != null)
            //{
            //    return true;
            //}
            dailyAttendance.HaveBackup = true;
            _db.Entry(dailyAttendance).Collection(d => d.ScheduleDetails).Load();
            var toAdd = new BkpDailyAttendance(dailyAttendance, alias);
            toAdd.ScheduleDetails = dailyAttendance.ScheduleDetails.Select(x => new BkpScheduleDetail(x)).ToList();
            _db.BkpDailyAttendances.Add(toAdd);
            return true;
        }
        public DataWithError UndoDailyAttendance(int bkpId, ClaimsPrincipal user)
        {
            var bkpDailyAttendance = _db.BkpDailyAttendances.Find(bkpId);
            if (bkpDailyAttendance == null)
            {
                return new DataWithError(null, "Backup not found");
            }
            var dailyAttendance = _db.DailyAttendances.Find(bkpDailyAttendance.DailyAttendanceId);
            if (dailyAttendance == null)
            {
                return new DataWithError(null, "Original Daily Attendance not found");
            }
            var appUser = _userService.GetUserInfo(user);
            backupDailyAttendance(dailyAttendance.Id, appUser.Alias);
            _db.Entry(bkpDailyAttendance).Collection(d => d.ScheduleDetails).Load();
            _db.Entry(dailyAttendance).Collection(d => d.ScheduleDetails).Load();
            _db.Entry(dailyAttendance).Reference(d => d.TransportationRoute).Load();
            _db.Entry(dailyAttendance).CurrentValues.SetValues(new DailyAttendance(bkpDailyAttendance));
            if (undoScheduleDetailsDuration(dailyAttendance.ScheduleDetails.ToList(), bkpDailyAttendance.ScheduleDetails.ToList(),
                dailyAttendance.ScheduleId,dailyAttendance.Id,out List<ScheduleDetail> newScheduleDetails))
            {
                dailyAttendance.ScheduleDetails = newScheduleDetails;
            }
            else
            {
                return new DataWithError(null, "Undefined activity not exist!");
            }

            // _db.BkpDailyAttendances.Remove(bkpDailyAttendance);
            // _db.Entry(bkpDailyAttendance).State = EntityState.Deleted;
            _db.Entry(dailyAttendance).State = EntityState.Modified;
            var attendanceType = _db.AttendanceTypes.Find(dailyAttendance.AttendanceTypeId);
            var transportationRoute = _db.TransportationRoutes.Find(dailyAttendance.TransportationRoute);
            _db.Entry(attendanceType).Reference(x => x.DefaultActivity).Load();
            var result = new DailyAttendanceByScheduleIdDto
            {
                Id = dailyAttendance.Id,
                ShiftId = (int)dailyAttendance.TransportationRouteId,
                AttendanceTypeId = dailyAttendance.AttendanceTypeId,
                TransportationName = transportationRoute.Name,
                TransportationArriveTime = new TimeSpan(),
                TransportationDepartTime = new TimeSpan(),
                AttendanceTypeName = attendanceType.Name,
                Day = dailyAttendance.Day,
                Color = attendanceType.DefaultActivity.Color,
                IsAbsence = attendanceType.IsAbsence,
                HaveBackup = dailyAttendance.HaveBackup
            };
            _db.SaveChanges();


            return new DataWithError(result, "");
        }
        public DataWithError GetDailyAttendanceBackups(int dailyAttendanceId)
        {
            var current = _db.DailyAttendances
                .Include(x => x.ScheduleDetails).ThenInclude(x => x.Activity)
                .Include(x => x.ScheduleDetails).ThenInclude(x => x.Interval)
                .Include(x => x.AttendanceType)
                .FirstOrDefault(x => x.Id == dailyAttendanceId);
            if(current == null)
            {
                return new DataWithError(null, "Daily Attendance Not Found");
            }
            
            var backups = _db.BkpDailyAttendances
                .Include(x => x.ScheduleDetails).ThenInclude(x => x.Activity)
                .Include(x => x.ScheduleDetails).ThenInclude(x => x.Interval)
                .Include(x => x.AttendanceType)
                .Where(x => x.DailyAttendanceId == dailyAttendanceId).OrderByDescending(x => x.ActionTime).ToList();
            var currentResult = new BkpDailyAttendanceDto
            {
                Id = 0,
                ActionTime = backups.FirstOrDefault() == null ? null : backups.FirstOrDefault().ActionTime,
                Alias = backups.FirstOrDefault() == null ? "System" : backups.FirstOrDefault().Alias,
                AttendanceType = current.AttendanceType.Name,
                ScheduleDetails = current.ScheduleDetails.OrderBy(y => y.IntervalId).ToDictionary(c => c.IntervalId, c => new ScheduleDetailDto
                {
                    Id = c.Id,
                    IntervalId = c.IntervalId,
                    IntervalTimeMap = c.Interval.TimeMap,
                    ActivityId = c.ActivityId,
                    ActivityName = c.Activity.Name,
                    ActivityColor = c.Activity.Color,
                    Duration = c.Duration != null ? (int)c.Duration : null
                })
            };
            var backupsResult = backups.Select(y => new BkpDailyAttendanceDto
            {
                Id = y.Id,
                Alias = backups.IndexOf(y) + 1 >= backups.Count ? "System" : backups[backups.ToList().IndexOf(y) + 1].Alias,
                ActionTime = backups.IndexOf(y) + 1 >= backups.Count ? null : backups[backups.ToList().IndexOf(y) + 1].ActionTime,
                AttendanceType = y.AttendanceType.Name,
                ScheduleDetails = y.ScheduleDetails.OrderBy(y => y.IntervalId).ToDictionary(c => c.IntervalId, c => new ScheduleDetailDto
                {
                    Id = c.Id,
                    IntervalId = c.IntervalId,
                    IntervalTimeMap = c.Interval.TimeMap,
                    ActivityId = c.ActivityId,
                    ActivityName = c.Activity.Name,
                    ActivityColor = c.Activity.Color,
                    Duration = c.Duration != null ? (int)c.Duration : null
                })
            });
            var result = backupsResult.ToList();
            result.Insert(0,currentResult);
            return new DataWithError(result, "");
        }
        public DataWithError UndoDailyAttendanceReturnDetails(int bkpId, ClaimsPrincipal user)
        {
            var bkpDailyAttendance = _db.BkpDailyAttendances.Find(bkpId);
            if (bkpDailyAttendance == null)
            {
                return new DataWithError(null, "Backup not found");
            }
            var dailyAttendance = _db.DailyAttendances.Find(bkpDailyAttendance.DailyAttendanceId);
            if (dailyAttendance == null)
            {
                return new DataWithError(null, "Original Daily Attendance not found");
            }
            var appUser = _userService.GetUserInfo(user);
            backupDailyAttendance(dailyAttendance.Id, appUser.Alias);
            _db.Entry(bkpDailyAttendance).Collection(d => d.ScheduleDetails).Load();
            _db.Entry(dailyAttendance).Collection(d => d.ScheduleDetails).Load();
            _db.Entry(dailyAttendance).CurrentValues.SetValues(new DailyAttendance(bkpDailyAttendance));
            if (undoScheduleDetailsDuration(dailyAttendance.ScheduleDetails.ToList(), bkpDailyAttendance.ScheduleDetails.ToList(),
                dailyAttendance.ScheduleId, dailyAttendance.Id, out List<ScheduleDetail> newScheduleDetails))
            {
                dailyAttendance.ScheduleDetails = newScheduleDetails;
            }
            else
            {
                return new DataWithError(null, "Undefined activity not exist!");
            }

            _db.Entry(dailyAttendance).State = EntityState.Modified;
            var attendanceType = _db.AttendanceTypes.Find(dailyAttendance.AttendanceTypeId);
            _db.Entry(attendanceType).Reference(x => x.DefaultActivity).Load();
            // _db.BkpDailyAttendances.Remove(bkpDailyAttendance);
            // _db.Entry(bkpDailyAttendance).State = EntityState.Deleted;
            _db.SaveChanges();
            var result = _db.DailyAttendances.Where(d => d.Id == dailyAttendance.Id)
                .Include(d => d.ScheduleDetails).ThenInclude(d => d.Interval)
                .Include(d => d.ScheduleDetails).ThenInclude(d => d.Activity)
                .ToList().Select(d =>
                new DailyAttendanceDto
                {
                    Day = d.Day,
                    Id = d.Id,
                    HaveBackup = d.HaveBackup,
                    ScheduleDetails = d.ScheduleDetails.OrderBy(y => y.IntervalId).ToDictionary(c => c.IntervalId, c => new ScheduleDetailDto
                    {
                        Id = c.Id,
                        IntervalId = c.IntervalId,
                        IntervalTimeMap = c.Interval.TimeMap,
                        ActivityId = c.ActivityId,
                        ActivityName = c.Activity.Name,
                        ActivityColor = c.Activity.Color,
                        Duration = c.Duration != null ? (int)c.Duration : null
                    })
                }).FirstOrDefault();
            return new DataWithError(result, "");
        }

        private bool undoScheduleDetailsDuration(List<ScheduleDetail> currentScheduleDetails,
            List<BkpScheduleDetail> bkpScheduleDetails,
            int scheduleId,
            int dailyAttendanceId,
            out List<ScheduleDetail> result)
        {
            var undefined = _db.Activities.FirstOrDefault(x => x.IsUndefined);
            if(undefined == null)
            {
                result = null;
                return false;
            }
            var retScheduleDetails = bkpScheduleDetails.Select(x => new ScheduleDetail(x)).ToList();
            foreach(var scheduleDetail in currentScheduleDetails.Where(x => x.Duration != null))
            {
                var toUpdate = retScheduleDetails.FirstOrDefault(x => x.IntervalId == scheduleDetail.IntervalId);
                if(toUpdate != null)
                {
                    toUpdate.Duration = scheduleDetail.Duration;
                }
                else
                {
                    retScheduleDetails.Add(new ScheduleDetail(dailyAttendanceId, undefined.Id, scheduleDetail.IntervalId, scheduleDetail.Duration, scheduleId));
                }
            }
            result = retScheduleDetails;
            return true;
        }
        public DataWithError schedulebyIdpageAll(int scheduleId, int pageIndex, int pageSize, string searchQuery, string type, ClaimsPrincipal user)
        {
            var appUser = _userService.GetUserInfo(user);
            var data = new DataWithError();
            var checkschedule = _db.Schedules.FirstOrDefault(x => x.Id == scheduleId);
            if (checkschedule == null)
            {
                return new DataWithError(null, "Schedule not found!");
            }
            var staffMembers = _db.StaffMembers.Where(x => x.DailyAttendances.FirstOrDefault(s => s.ScheduleId == scheduleId) != null);
            if (appUser.Roles.Contains("User") && !(appUser.Roles.Contains("SuperUser") || appUser.Roles.Contains("Admin") || appUser.Roles.Contains("Hos")))
            {
                staffMembers = staffMembers.Where(x => x.Alias.ToLower() == appUser.Alias.ToLower());
            }
            if (!string.IsNullOrEmpty(searchQuery))
            {
                if (type == "Staff")
                {
                    if (int.TryParse(searchQuery, out var employeeId))
                    {
                        staffMembers = staffMembers.Where(x => x.EmployeeId == employeeId).OrderBy(x => x.EmployeeId);
                    }
                    else
                    {
                        staffMembers = staffMembers.Where(x => x.Name.ToLower().StartsWith(searchQuery.ToLower())
                        || x.Alias.ToLower().StartsWith(searchQuery.ToLower()) || x.Email.ToLower().StartsWith(searchQuery.ToLower()));
                    }
                }
                else if (type == "Hos")
                {
                    if (int.TryParse(searchQuery, out var employeeId))
                    {
                        staffMembers = staffMembers.Where(x => x.DailyAttendances.FirstOrDefault(s => s.ScheduleId == scheduleId && s.HeadOfSection.EmployeeId == employeeId) != null);
                    }
                    else
                    {
                        staffMembers = staffMembers.Where(x => x.DailyAttendances.FirstOrDefault(s => s.ScheduleId == scheduleId && (s.HeadOfSection.Alias.ToLower().StartsWith(searchQuery.ToLower())
                        || s.HeadOfSection.Name.ToLower().StartsWith(searchQuery.ToLower()) || s.HeadOfSection.Email.StartsWith(searchQuery.ToLower()))) != null);
                    }
                }
                else if (type == "Transportation")
                {
                    if (int.TryParse(searchQuery, out var transportationId))
                    {
                        staffMembers = staffMembers.Where(x => x.DailyAttendances.FirstOrDefault(s => s.ScheduleId == scheduleId && s.TransportationRouteId == transportationId) != null);
                    }
                    else
                    {
                        staffMembers = staffMembers.Where(x => x.DailyAttendances.FirstOrDefault(s => s.ScheduleId == scheduleId && s.TransportationRoute.Name.ToLower().StartsWith(searchQuery.ToLower())) != null);
                    }
                }
                else if (type == "Location")
                {
                    if (int.TryParse(searchQuery, out var locationId))
                    {
                        staffMembers = staffMembers.Where(x => x.DailyAttendances.FirstOrDefault(s => s.ScheduleId == scheduleId && s.SublocationId == locationId) != null);
                    }
                    else
                    {
                        staffMembers = staffMembers.Where(x => x.DailyAttendances.FirstOrDefault(s => s.ScheduleId == scheduleId && s.Sublocation.Name.ToLower().StartsWith(searchQuery.ToLower())) != null);
                    }

                }
                else if (type == "Time")
                {
                    if (TimeSpan.TryParse(searchQuery, out TimeSpan time))
                    {
                        staffMembers = staffMembers.Where(x => x.DailyAttendances.FirstOrDefault(s => s.ScheduleId == scheduleId && time >= s.TransportationRoute.ArriveInterval.TimeMap && time <= s.TransportationRoute.DepartInterval.TimeMap) != null);
                    }
                }
            }

            var ResultSize = staffMembers.Count();
            var result = new ScheduleById
            {
                ScheduleId = scheduleId,
                ScheduleName = checkschedule.Name,
                DailyStaffs = staffMembers.OrderBy(x => x.Name).Skip(pageSize * pageIndex).Take(pageSize).Select(x => new DailyStaffDto
                {
                    Id = x.Id,
                    EmployeeId = x.EmployeeId,
                    Name = x.Name,
                    DailyAttendances = x.DailyAttendances.Where(s => s.ScheduleId == scheduleId).OrderBy(s => s.Day).Select(d => new DailyAttendanceByScheduleIdDto
                    {
                        Id = d.Id,
                        ShiftId = (int)d.TransportationRouteId,
                        AttendanceTypeId = d.AttendanceTypeId,
                        HeadOfSectionName = d.HeadOfSection.Name,
                        TransportationName = d.TransportationRoute.Name,
                        TransportationArriveTime = d.TransportationRoute.ArriveInterval.TimeMap,
                        TransportationDepartTime = d.TransportationRoute.DepartInterval.TimeMap,
                        AttendanceTypeName = d.AttendanceType.Name,
                        Day = d.Day,
                        Color = d.AttendanceType.DefaultActivity.Color,
                        IsAbsence = d.AttendanceType.IsAbsence,
                        HaveBackup = d.HaveBackup
                    }).ToList()
                }).ToList()
            };


            data.Result = new { Result = result, Size = ResultSize };
            data.ErrorMessage = null;
            return data;
        }
        public DataWithError schedulebyDatePageAll(int scheduleId, DateTime day, int pageIndex, int pageSize, string searchQuery, string type, ClaimsPrincipal user)
        {
            var appUser = _userService.GetUserInfo(user);
            var data = new DataWithError();
            var checkschedule = _db.Schedules.FirstOrDefault(x => x.Id == scheduleId);
            var dbDailyAttendance = _db.DailyAttendances
                .Include(x => x.StaffMember)
                .Include(x => x.HeadOfSection)
                .Include(x => x.ScheduleDetails).ThenInclude(x => x.Interval)
                .Include(x => x.ScheduleDetails).ThenInclude(x => x.Activity)
                .Where(x => x.ScheduleId == scheduleId && x.Day == day);
            
            if (appUser.Roles.Contains("User") && !(appUser.Roles.Contains("SuperUser") || appUser.Roles.Contains("Admin") || appUser.Roles.Contains("Hos")))
            {
                dbDailyAttendance = dbDailyAttendance.Where(d => d.StaffMember.Alias.ToLower() == appUser.Alias.ToLower());
            }
            var dailyAttendances = new List<DailyAttendance>();
            if (checkschedule == null || dbDailyAttendance.Count() == 0)
            {
                data.Result = null;
                data.ErrorMessage = "staff  or schedule is not found or DailyAttendance  is not created  ";
                return data;
            }
            if (!string.IsNullOrEmpty(searchQuery))
            {
                if (type == "Staff")
                {
                    if (int.TryParse(searchQuery, out var employeeId))
                    {
                        dbDailyAttendance = dbDailyAttendance.Where(x => x.StaffMember.EmployeeId == employeeId).OrderBy(x => x.StaffMember.EmployeeId);
                    }
                    else
                    {
                        dbDailyAttendance = dbDailyAttendance.Where(x => x.StaffMember.Name.ToLower().StartsWith(searchQuery.ToLower())
                        || x.StaffMember.Alias.ToLower().StartsWith(searchQuery.ToLower()) || x.StaffMember.Email.ToLower().StartsWith(searchQuery.ToLower())).OrderBy(x => x.StaffMember.EmployeeId);
                    }
                }
                else if (type == "Hos")
                {
                    if (int.TryParse(searchQuery, out var employeeId))
                    {
                        dbDailyAttendance = dbDailyAttendance.Where(x => x.HeadOfSection.EmployeeId == employeeId).OrderBy(x => x.StaffMember.EmployeeId);
                    }
                    else
                    {
                        dbDailyAttendance = dbDailyAttendance.Where(x => x.HeadOfSection.Name.ToLower().StartsWith(searchQuery.ToLower())
                        || x.HeadOfSection.Alias.ToLower().StartsWith(searchQuery.ToLower()) || x.HeadOfSection.Email.ToLower().StartsWith(searchQuery.ToLower())).OrderBy(x => x.StaffMember.EmployeeId);
                    }
                }
                else if (type == "Transportation")
                {
                    if (int.TryParse(searchQuery, out var transportationId))
                    {
                        dbDailyAttendance = dbDailyAttendance.Where(x => x.TransportationRouteId == transportationId).OrderBy(x => x.StaffMember.EmployeeId);
                    }
                    else
                    {
                        dbDailyAttendance = dbDailyAttendance.Where(x => x.TransportationRoute.Name.ToLower().StartsWith(searchQuery.ToLower())).OrderBy(x => x.StaffMember.EmployeeId);
                    }
                }
                else if (type == "Time")
                {
                    if (TimeSpan.TryParse(searchQuery, out TimeSpan time))
                    {
                        dbDailyAttendance = dbDailyAttendance.Where(x => time >= x.TransportationRoute.ArriveInterval.TimeMap && time <= x.TransportationRoute.DepartInterval.TimeMap).OrderBy(x => x.StaffMember.EmployeeId);
                    }
                }
                else if (type == "Break Time")
                {
                    if (TimeSpan.TryParse(searchQuery, out TimeSpan time))
                    {
                        var breakActivityId = _db.Activities.FirstOrDefault(x => x.IsBreak)?.Id;
                        if (breakActivityId != null)
                        {
                            dbDailyAttendance = dbDailyAttendance
                                .Where(x => x.ScheduleDetails.FirstOrDefault(z => z.ActivityId == breakActivityId && z.Interval.TimeMap == time) != null)
                                .OrderBy(x => x.StaffMember.EmployeeId);
                        }
                    }
                }
                else if (type == "Location")
                {
                    if (int.TryParse(searchQuery, out var locationId))
                    {
                        dbDailyAttendance = dbDailyAttendance.Where(x => x.SublocationId == locationId).OrderBy(x => x.StaffMember.EmployeeId);
                    }
                    else
                    {
                        dbDailyAttendance = dbDailyAttendance.Where(x => x.Sublocation.Name.ToLower().StartsWith(searchQuery.ToLower())).OrderBy(x => x.StaffMember.EmployeeId);
                    }

                }
                else if (type == "Activity Time")
                {
                    var filters = searchQuery.Split('-');

                    if (filters.Length == 2 && TimeSpan.TryParse(filters[1], out TimeSpan time) && int.TryParse(filters[0], out int activityId))
                    {
                        if(activityId == 0)
                        {
                            dbDailyAttendance = dbDailyAttendance
                            .Where(x => x.ScheduleDetails.FirstOrDefault(z => z.Interval.TimeMap == time) != null)
                            .OrderBy(x => x.StaffMember.EmployeeId);
                        }
                        else
                        {
                            dbDailyAttendance = dbDailyAttendance
                            .Where(x => x.ScheduleDetails.FirstOrDefault(z => z.ActivityId == activityId && z.Interval.TimeMap == time) != null)
                            .OrderBy(x => x.StaffMember.EmployeeId);
                        }
                        
                    }
                }
            }
            var size = dbDailyAttendance.Count();
            var ss = new ScheduleByDateDto
            {
                Day = day,
                ScheduleId = checkschedule.Id,
                ScheduleName = checkschedule.Name,
                ScheduleStartDate = checkschedule.StartDate,
                ScheduleEndDate = checkschedule.EndDate,
                DailyAttendances = dbDailyAttendance.OrderBy(x => x.StaffMember.Name).Skip(pageSize * pageIndex).Take(pageSize).ToList().Select(y => new DailyAttendanceByDayDto
                {

                    Id = y.StaffMemberId,
                    EmployeeId = y.StaffMember.EmployeeId,
                    Name = y.StaffMember.Name,
                    HeadOfSectionName = y.HeadOfSection.Name,
                    AttendanceId = y.Id,
                    ScheduleDetails = y.ScheduleDetails.OrderBy(y => y.IntervalId).ToDictionary(c => c.IntervalId, c => new ScheduleDetailDto
                    {
                        Id = c.Id,
                        IntervalId = c.IntervalId,
                        IntervalTimeMap = c.Interval.TimeMap,
                        ActivityId = c.ActivityId,
                        ActivityName = c.Activity.Name,
                        ActivityColor = c.Activity.Color,
                        Duration = c.Duration != null ? (int)c.Duration : null
                    })
                }).ToList()
            };

            data.Result = new { Result = ss, Size = size };
            data.ErrorMessage = null;
            return data;
        }
        public DataWithError ManipulateScheduleDetails(ManipulateDetails model, int scheduleId, ClaimsPrincipal user, bool isStaff)
        {
            var dailyAttendances = _db.DailyAttendances
                .Where(d => model.ScheduleDetailsManipulate.Select(x => x.DailyAttendanceId).Contains(d.Id)).Distinct();
            if(dailyAttendances.Count() == 0)
            {
                return new DataWithError(null, "Daily Attendances Not Found!");
            }
            var appUser = _userService.GetUserInfo(user);
            foreach(int dailyAttendanceId in dailyAttendances.Select(x => x.Id).ToList())
            {
                backupDailyAttendance(dailyAttendanceId, appUser.Alias);
            }
            var undefinedActivity = _db.Activities.FirstOrDefault(x => x.IsUndefined);
            if(undefinedActivity == null)
            {
                return new DataWithError(null, "Undefined Activity not Available!");
            }
            foreach(var element in model.ScheduleDetailsManipulate)
            {
                var scheduleDetail = _db.ScheduleDetail.FirstOrDefault(s => s.DailyAttendanceId == element.DailyAttendanceId && s.IntervalId == element.IntervalId);
                
                if (scheduleDetail == null)
                {
                    if(model.ActivityId != 0)
                    {
                        _db.ScheduleDetail.Add(new ScheduleDetail(element.DailyAttendanceId, model.ActivityId, element.IntervalId, null, scheduleId));
                    }
                }
                else
                {
                    if (model.ActivityId != 0)
                    {
                        scheduleDetail.ActivityId = model.ActivityId;
                        
                    }
                    else
                    {
                        if(scheduleDetail.Duration != null)
                        {
                            scheduleDetail.ActivityId = undefinedActivity.Id;
                        }
                        else
                        {
                            _db.ScheduleDetail.Remove(scheduleDetail);
                        }
                    }
                    
                }
                
            }
            _db.SaveChanges();
            if(isStaff)
            {
                var result = _db.DailyAttendances
                .Where(d => model.ScheduleDetailsManipulate.Select(x => x.DailyAttendanceId).Contains(d.Id)).Distinct()
                .Include(x => x.ScheduleDetails).ThenInclude(x => x.Activity)
                .Include(x => x.ScheduleDetails).ThenInclude(x => x.Interval)
                .Include(x => x.HeadOfSection)
                .ToList()
                .Select(y => new DailyAttendanceDto
                {
                    Day = y.Day,
                    Id = y.Id,
                    HaveBackup = y.HaveBackup,
                    HeadOfSectionName = y.HeadOfSection.Name,
                    ScheduleDetails = y.ScheduleDetails.OrderBy(y => y.IntervalId).ToDictionary(c => c.IntervalId, c => new ScheduleDetailDto
                    {
                        Id = c.Id,
                        IntervalId = c.IntervalId,
                        IntervalTimeMap = c.Interval.TimeMap,
                        ActivityId = c.ActivityId,
                        ActivityName = c.Activity.Name,
                        ActivityColor = c.Activity.Color,
                        Duration = c.Duration != null ? (int)c.Duration : null
                    })
                });
                return new DataWithError(result, null);
            }
            else
            {
                var result = _db.DailyAttendances
                .Where(d => model.ScheduleDetailsManipulate.Select(x => x.DailyAttendanceId).Contains(d.Id)).Distinct()
                .Include(x => x.StaffMember)
                .Include(x => x.ScheduleDetails).ThenInclude(x => x.Activity)
                .Include(x => x.ScheduleDetails).ThenInclude(x => x.Interval)
                .Include(x => x.HeadOfSection)
                .ToList()
                .Select(y => new DailyAttendanceByDayDto
                {
                    Id = y.StaffMemberId,
                    EmployeeId = y.StaffMember.EmployeeId,
                    Name = y.StaffMember.Name,
                    HeadOfSectionName = y.HeadOfSection.Name,
                    AttendanceId = y.Id,
                    ScheduleDetails = y.ScheduleDetails.ToDictionary(c => c.IntervalId, c => new ScheduleDetailDto
                    {
                        Id = c.Id,
                        IntervalId = c.IntervalId,
                        IntervalTimeMap = c.Interval.TimeMap,
                        ActivityId = c.ActivityId,
                        ActivityName = c.Activity.Name,
                        ActivityColor = c.Activity.Color,
                        Duration = c.Duration != null ? (int)c.Duration : null
                    })
                });
                return new DataWithError(result, null);
            }
            

        }
        public DataWithError CopyDailyAttendance(ManipulateDailyAttendance model, ClaimsPrincipal user)
        {
            if(model.SourceId == 0 || model.DestinationIds == null || model.DestinationIds.Count == 0)
            {
                return new DataWithError(null, "No Attendance Found to copy!");
            }
            var sourceAttendance = _db.DailyAttendances.Include(x => x.ScheduleDetails).FirstOrDefault(x => x.Id == model.SourceId);
            if(sourceAttendance == null)
            {
                return new DataWithError(null, "Source attendance not found!");
            }
            var destinationAttendances = _db.DailyAttendances.Include(x => x.ScheduleDetails)
                .Include(x => x.AttendanceType).ThenInclude(y => y.DefaultActivity)
                .Include(x => x.TransportationRoute)
                .Include(x => x.HeadOfSection)
                .Where(x => model.DestinationIds.Contains(x.Id));
            if(!destinationAttendances.Any())
            {
                return new DataWithError(null, "Destination attendances not found!");
            }
            int undefinedId = _db.Activities.FirstOrDefault(x => x.IsUndefined).Id;
            var result = new List<DailyAttendanceByScheduleIdDto>();
            var appUser = _userService.GetUserInfo(user);
            foreach (var destinationAttendance in destinationAttendances.ToList())
            {
                backupDailyAttendance(destinationAttendance.Id, appUser.UserName);
                setUndifinedActivity(destinationAttendance, undefinedId);
                destinationAttendance.AttendanceTypeId = sourceAttendance.AttendanceTypeId;
                destinationAttendance.TransportationRouteId = sourceAttendance.TransportationRouteId;
                foreach(var scheduleDetail in sourceAttendance.ScheduleDetails)
                {
                    var mappedDetail = destinationAttendance.ScheduleDetails.FirstOrDefault(x => x.IntervalId == scheduleDetail.IntervalId);
                    if(mappedDetail != null)
                    {
                        mappedDetail.ActivityId = scheduleDetail.ActivityId;
                    }
                    else
                    {
                        destinationAttendance.ScheduleDetails.Add(new ScheduleDetail(destinationAttendance.Id, scheduleDetail.ActivityId, scheduleDetail.IntervalId, null, destinationAttendance.ScheduleId));
                    }
                }
                result.Add(new DailyAttendanceByScheduleIdDto
                {
                    Id = destinationAttendance.Id,
                    ShiftId = (int)destinationAttendance.TransportationRouteId,
                    AttendanceTypeId = destinationAttendance.AttendanceTypeId,
                    TransportationName = destinationAttendance.TransportationRoute.Name,
                    TransportationArriveTime = new TimeSpan(),
                    TransportationDepartTime = new TimeSpan(),
                    AttendanceTypeName = destinationAttendance.AttendanceType.Name,
                    HeadOfSectionName = destinationAttendance.HeadOfSection.Name,
                    Day = destinationAttendance.Day,
                    Color = destinationAttendance.AttendanceType.DefaultActivity.Color,
                    IsAbsence = destinationAttendance.AttendanceType.IsAbsence,
                    HaveBackup = destinationAttendance.HaveBackup
                });
            }
            _db.SaveChanges();
            return new DataWithError(result, "");

        }
        private void setUndifinedActivity(DailyAttendance d, int activityId)
        {
            foreach (var scheduleDetail in d.ScheduleDetails.ToList())
            {
                if(scheduleDetail.Duration == null)
                {
                    d.ScheduleDetails.Remove(scheduleDetail);
                }
            }
            foreach (var scheduleDetail in d.ScheduleDetails)
            {
                scheduleDetail.ActivityId = activityId;
            }
        }
        
    }
}


