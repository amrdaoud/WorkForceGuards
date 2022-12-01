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
    public class DailyAttendanceService : IDailyAttendanceService
    {
        private readonly ApplicationDbContext _db;
        public DailyAttendanceService(ApplicationDbContext db)
        {
            _db = db;
        }
        public DataWithError CreateAttendance()
        {
            var schedule = _db.Schedules.FirstOrDefault(x => !x.IsPublish);
            if (schedule == null)
            {
                return new DataWithError(null, "All schedules are published");
            }
            return CreateScheduleAttendance(schedule.Id);
        }

        public DataWithError CreateScheduleAttendance(int scheduleId)
        {
            var schedule = _db.Schedules.Find(scheduleId);
            if (schedule == null)
            {
                return new DataWithError(null, "Schedule not found!");
            }
            if (schedule.IsPublish)
            {
                return new DataWithError(null, "Schedule is published!");
            }
            var includedStaffMembers = _db.StaffMembers.Where(x => !(x.LeaveDate < schedule.StartDate || x.StartDate > schedule.EndDate)).ToList();
            schedule = _db.Schedules.Find(scheduleId);
            _db.Entry(schedule).Collection(s => s.DayOffOptions).Load();
            _db.Entry(schedule).Collection(s => s.BreakTypeOptions).Load();
            var notFoundStaff = includedStaffMembers.FirstOrDefault(x =>
                    (schedule.DayOffOptions.FirstOrDefault(d => d.StaffMemberId == x.Id && d.IsApproved) == null) ||
                    (schedule.BreakTypeOptions.FirstOrDefault(d => d.StaffMemberId == x.Id && d.IsApproved) == null)
            );
            //if (notFoundStaff != null)
            //{
            //    return new DataWithError(null, "Not all staff members have approved Day-Offs or Break Types!");
            //}
            var result = new List<DailyAttendance>();
            var absenceId = _db.AttendanceTypes.FirstOrDefault(x => x.IsAbsence).Id;
            foreach (var staff in includedStaffMembers)
            {
                result.AddRange(CreateStaffScheduleAttendance(schedule, staff, absenceId));
            }
            schedule.ForecastId = null;
            // Remove Details
            //_db.RemoveRange(_db.ScheduleDetail.Where(x => x.ScheduleId == scheduleId));
            // Remove Daily Attendance
            _db.DailyAttendances.RemoveRange(_db.DailyAttendances.Where(x => x.ScheduleId == scheduleId));
            _db.DailyAttendances.AddRange(result);
            _db.SaveChanges();
            return new DataWithError(true, "");
        }

        public DataWithError CreateStaffScheduleAttendance(int scheduleId, int staffId)
        {
            var schedule = _db.Schedules.Find(scheduleId);
            if (schedule == null)
            {
                return new DataWithError(null, "Schedule not found!");
            }
            if (schedule.IsPublish)
            {
                return new DataWithError(null, "Schedule is published!");
            }
            var staff = _db.StaffMembers.Find(staffId);
            if (staff.LeaveDate < schedule.StartDate || staff.StartDate > schedule.EndDate)
            {
                return new DataWithError(null, "Staff member is not active in this schedule");
            }
            schedule = _db.Schedules.Include(s => s.DayOffOptions).Include(s => s.BreakTypeOptions).FirstOrDefault(s => s.Id == scheduleId);
            if (schedule.DayOffOptions.FirstOrDefault(x => x.StaffMemberId == staff.Id && x.IsApproved) == null ||
                schedule.BreakTypeOptions.FirstOrDefault(x => x.StaffMemberId == staff.Id && x.IsApproved) == null)
            {
                return new DataWithError(null, "Staff member Day-Offs or Break Type is not approved");
            }
            var absenceId = _db.AttendanceTypes.FirstOrDefault(x => x.IsAbsence).Id;
            var result = CreateStaffScheduleAttendance(schedule, staff, absenceId);
            _db.RemoveRange(_db.ScheduleDetail.Where(x => x.ScheduleId == scheduleId));
            _db.DailyAttendances.RemoveRange(_db.DailyAttendances.Where(x => x.ScheduleId == scheduleId && x.StaffMemberId == staffId));
            _db.AddRange(result);
            _db.SaveChanges();
            return new DataWithError(true, "");
        }

        public List<AttendanceSummary> GetAttendanceSummary(int scheduleId, int? forecastId)
        {
            List<AttendanceSummaryNeeded> needed = null;
            if (forecastId != null)
            {
                needed = _db.ForecastDetails.Include(x => x.Interval).Where(x => x.ForecastId == forecastId.Value).ToList()
                .GroupBy(x => new { x.Interval.TimeMap })
                .Select(g => new AttendanceSummaryNeeded
                {
                    TimeMap = g.Key.TimeMap,
                    AverageNeeded = (int)Math.Ceiling(g.Average(z => z.EmployeeCount))
                }).ToList();
            }

            var available = _db.DailyAttendances.Include(d => d.TransportationRoute).Where(x => x.ScheduleId == scheduleId && !x.AttendanceType.IsAbsence).ToList()
                .GroupBy(x => new { x.Day, x.TransportationRoute })
                .Select(g => new
                {
                    g.Key.Day,
                    g.Key.TransportationRoute,
                    dailySum = g.Count()
                }).ToList()
                .GroupBy(z => new { z.TransportationRoute })
                .Select(g => new
                {
                    g.Key.TransportationRoute,
                    dailyAvg = g.Average(z => z.dailySum)
                }).ToList()
                .SelectMany(s => _db.Intervals.Where(i => i.Id >= s.TransportationRoute.ArriveIntervalId && i.Id <= s.TransportationRoute.DepartIntervalId)
                .Select(q => new { Interval = q, Count = s.dailyAvg }))
                .GroupBy(z => new { z.Interval.TimeMap, z.Interval.Tolerance })
                .Select(g => new AttendanceSummary
                {
                    TimeMap = g.Key.TimeMap,
                    IntervalId = _db.Intervals.FirstOrDefault(x => x.Id < 97 && x.TimeMap == g.Key.TimeMap)?.Id,
                    Tolerance = g.Key.Tolerance,
                    AverageAvailable = (int)Math.Ceiling(g.Sum(z => z.Count)),
                    AverageNeeded = needed?.FirstOrDefault(x => x.TimeMap == g.Key.TimeMap)?.AverageNeeded
                }).OrderBy(x => x.TimeMap).ToList();
            return available;
        }

        private List<DailyAttendance> CreateStaffScheduleAttendance(Schedule schedule, StaffMember staff, int absenceId)
        {
            var result = new List<DailyAttendance>();
            var dayOffs = schedule.DayOffOptions.FirstOrDefault(x => x.StaffMemberId == staff.Id && x.IsApproved);
            var attendanceType = schedule.BreakTypeOptions.FirstOrDefault(x => x.StaffMemberId == staff.Id && x.IsApproved);
            if(dayOffs == null || attendanceType==null)
            {
                return new List<DailyAttendance>();
            }
            for (DateTime currentDay = schedule.StartDate.Date; currentDay <= schedule.EndDate.Date; currentDay = currentDay.AddDays(1))
            {
                if (currentDay >= staff.StartDate && currentDay <= staff.LeaveDate)
                {
                    var isDayOff = (currentDay.DayOfWeek.ToString() == dayOffs.DayOne || currentDay.DayOfWeek.ToString() == dayOffs.DayTwo);
                    result.Add(new DailyAttendance(staff.Id, schedule.Id, currentDay, isDayOff ? absenceId : attendanceType.AttendenceTypeId, attendanceType.TransportationRouteId.Value, staff.HeadOfSectionId));
                }
            }
            return result;
        }
    }
}
