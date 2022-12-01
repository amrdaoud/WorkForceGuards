using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using WorkForceManagementV0.Contexts;
using WorkForceManagementV0.Repositories.Identity;
using WorkForceManagementV0.Repositories.Interfaces;

namespace WorkForceManagementV0.Repositories
{
    public class AdherenceService : IAdherenceService
    {
        private readonly ApplicationDbContext _db;
        private readonly IUserService _userService;
        private readonly DateTime today;
        public AdherenceService(ApplicationDbContext db, IUserService userService)
        {
            var timezone = TimeZoneInfo.FindSystemTimeZoneById("Arab Standard Time");
            today = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timezone);
            _db = db;
            _userService = userService;
        }
        public decimal? AdherenceByStaffDay(int scheduleId, int staffId, DateTime day, ClaimsPrincipal user)
        {
            
            var appUser = _userService.GetUserInfo(user);
            var staffMember = _db.StaffMembers.Find(staffId);
            if (staffMember == null)
            {
                return null;
            }
            if (appUser.Roles.Contains("User") && !(appUser.Roles.Contains("SuperUser") || appUser.Roles.Contains("Admin") || appUser.Roles.Contains("Hos")))
            {
                if (staffMember.Alias.ToLower() != appUser.Alias.ToLower())
                {
                    return null;
                }
            }
            var adh = _db.ScheduleDetail
                .Where(d => (day != today.Date ? d.DailyAttendance.Day == day : d.DailyAttendance.Day == day && d.Interval.TimeMap < today.AddHours(-0.75).TimeOfDay) && d.DailyAttendance.StaffMemberId == staffId && d.DailyAttendance.ScheduleId == scheduleId && d.Activity.IsPhone)
                .Select(d => d.Duration / 15)
                .Sum() / _db.ScheduleDetail
                .Where(d => (day != today.Date ? d.DailyAttendance.Day == day : d.DailyAttendance.Day == day && d.Interval.TimeMap < today.AddHours(-0.75).TimeOfDay) && d.DailyAttendance.StaffMemberId == staffId && d.DailyAttendance.ScheduleId == scheduleId && d.Activity.IsPhone).Count();
            return adh != null && !double.IsNaN(adh.Value) ? Math.Round((decimal)adh, 2) : null;
        }

        public decimal? AdherenceByDay(int scheduleId, DateTime day, ClaimsPrincipal user)
        {
            var appUser = _userService.GetUserInfo(user);
            var scheduleDetails = _db.ScheduleDetail
               .Where(d => (day != today.Date ? d.DailyAttendance.Day == day : d.DailyAttendance.Day == day && d.Interval.TimeMap < today.AddHours(-0.75).TimeOfDay) && d.DailyAttendance.ScheduleId == scheduleId && d.Activity.IsPhone);

            if (appUser.Roles.Contains("Hos") && !(appUser.Roles.Contains("SuperUser") || appUser.Roles.Contains("Admin")))
            {
                scheduleDetails = scheduleDetails.Where(d => d.DailyAttendance.HeadOfSection.Alias.ToLower() == appUser.Alias.ToLower());
            }
            else if (appUser.Roles.Contains("User") && !(appUser.Roles.Contains("SuperUser") || appUser.Roles.Contains("Admin")))
            {
                scheduleDetails = scheduleDetails.Where(d => d.DailyAttendance.StaffMember.Alias.ToLower() == appUser.Alias.ToLower());
            }

            var adh = scheduleDetails
               .Select(d => d.Duration / 15)
               .Sum() / scheduleDetails.Count();
            return adh != null && !double.IsNaN(adh.Value) ? Math.Round((decimal)adh, 2) : null;
        }

        public decimal? AdherenceByStaff(int scheduleId, int staffId, ClaimsPrincipal user)
        {
            var appUser = _userService.GetUserInfo(user);
            var staffMember = _db.StaffMembers.Find(staffId);
            if(staffMember == null)
            {
                return null;
            }
            if (appUser.Roles.Contains("User") && !(appUser.Roles.Contains("SuperUser") || appUser.Roles.Contains("Admin") || appUser.Roles.Contains("Hos")))
            {
                if (staffMember.Alias.ToLower() != appUser.Alias.ToLower())
                {
                    return null;
                }
            }
            var adh = _db.ScheduleDetail
               .Where(d => d.DailyAttendance.StaffMemberId == staffId && d.DailyAttendance.ScheduleId == scheduleId && d.Activity.IsPhone)
               .Where(x => x.DailyAttendance.Day < today.Date || (x.DailyAttendance.Day == today.Date && x.Interval.TimeMap < today.AddHours(-0.75).TimeOfDay))
               .Select(d => d.Duration / 15)
               .Sum() / _db.ScheduleDetail
               .Where(d => d.DailyAttendance.StaffMemberId == staffId && d.DailyAttendance.ScheduleId == scheduleId && d.Activity.IsPhone)
               .Where(x => x.DailyAttendance.Day < today.Date || (x.DailyAttendance.Day == today.Date && x.Interval.TimeMap < today.AddHours(-0.75).TimeOfDay)).Count();
            return adh != null && !double.IsNaN(adh.Value) ? Math.Round((decimal)adh, 2) : null;
        }

        public decimal? AdherenceBySchedule(int scheduleId, ClaimsPrincipal user)
        {
            var appUser = _userService.GetUserInfo(user);

            var scheduleDetails = _db.ScheduleDetail
               .Where(d => d.DailyAttendance.ScheduleId == scheduleId && d.Activity.IsPhone)
               .Where(x => x.DailyAttendance.Day < today.Date || (x.DailyAttendance.Day == today.Date && x.Interval.TimeMap < today.AddHours(-0.75).TimeOfDay));

            if (appUser.Roles.Contains("Hos") && !(appUser.Roles.Contains("SuperUser") || appUser.Roles.Contains("Admin")))
            {
                scheduleDetails = scheduleDetails.Where(d => d.DailyAttendance.HeadOfSection.Alias.ToLower() == appUser.Alias.ToLower());
            }
            else if (appUser.Roles.Contains("User") && !(appUser.Roles.Contains("SuperUser") || appUser.Roles.Contains("Admin")))
            {
                scheduleDetails = scheduleDetails.Where(d => d.DailyAttendance.StaffMember.Alias.ToLower() == appUser.Alias.ToLower());
            }

            var adh = scheduleDetails
               .Select(d => d.Duration / 15)
               .Sum() / scheduleDetails.Count();
            return adh != null && !double.IsNaN(adh.Value) ? Math.Round((decimal)adh, 2) : null;
        }

        public decimal? AdherenceByScheduleAll(int scheduleId, ClaimsPrincipal user)
        {
            var appUser = _userService.GetUserInfo(user);

            var scheduleDetails = _db.ScheduleDetail
               .Where(d => d.DailyAttendance.ScheduleId == scheduleId && d.Activity.IsPhone)
               .Where(x => x.DailyAttendance.Day < today.Date || (x.DailyAttendance.Day == today.Date && x.Interval.TimeMap < today.AddHours(-0.75).TimeOfDay));

            if (appUser.Roles.Contains("User") && !(appUser.Roles.Contains("SuperUser") || appUser.Roles.Contains("Admin") || appUser.Roles.Contains("Hos")))
            {
                scheduleDetails = scheduleDetails.Where(d => d.DailyAttendance.StaffMember.Alias.ToLower() == appUser.Alias.ToLower());
            }

            var adh = scheduleDetails
               .Select(d => d.Duration / 15)
               .Sum() / scheduleDetails.Count();
            return adh != null && !double.IsNaN(adh.Value) ? Math.Round((decimal)adh, 2) : null;
        }

        public decimal? AdherenceByDayAll(int scheduleId, DateTime day, ClaimsPrincipal user)
        {
            var appUser = _userService.GetUserInfo(user);
            var scheduleDetails = _db.ScheduleDetail
               .Where(d => (day != today.Date ? d.DailyAttendance.Day == day : d.DailyAttendance.Day == day && d.Interval.TimeMap < today.AddHours(-0.75).TimeOfDay) && d.DailyAttendance.ScheduleId == scheduleId && d.Activity.IsPhone);
            if (appUser.Roles.Contains("User") && !(appUser.Roles.Contains("SuperUser") || appUser.Roles.Contains("Admin") || appUser.Roles.Contains("Hos")))
            {
                scheduleDetails = scheduleDetails.Where(d => d.DailyAttendance.StaffMember.Alias.ToLower() == appUser.Alias.ToLower());
            }

            var adh = scheduleDetails
               .Select(d => d.Duration / 15)
               .Sum() / scheduleDetails.Count();
            return adh != null && !double.IsNaN(adh.Value) ? Math.Round((decimal)adh, 2) : null;
        }
    }
}
