using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkForceManagementV0.Contexts;
using WorkForceManagementV0.Models;

namespace WorkForceManagementV0.Helpers
{
    public class SchedulerHelper : IFinalSchedule

    {
        private readonly ApplicationDbContext db;
        private Dictionary<int, int> AcceptedBreakCount { get; set; }

        public SchedulerHelper(ApplicationDbContext context)
        {
            db = context;
        }
        public bool BuildShedule(int scheduleId)
        {
            throw new NotImplementedException();
        }
        private bool BuildScheduleDay (int scheduleId, DateTime day)
        {
            return true;
        }
      
      
        public bool/*List<DataInserted>*/ BuildAcceptedBreakCount(DateTime day)
        {
           
            //    List<DataInserted> data = new List<DataInserted>();


            //    var routes = db.DailyAttendances
            //        .Include(x => x.StaffMember.TransportationRoute)
            //        .ThenInclude(x => x.Location.Assets).Include(x => x.ScheduleDetails)
            //        .Where(x => x.Day == day && !x.AttendanceType.IsAbsence)
            //        .Select(x => new { x.StaffMember.TransportationRoute, x.StaffMember, x.StaffMember.Location.Assets }).ToList();

            //    var routesbb = db.DailyAttendances
            //        .Include(x => x.StaffMember.TransportationRoute)
            //        .ThenInclude(x => x.Location.Assets).Include(x => x.ScheduleDetails)
            //        .Where(x => x.Day == day.AddDays(-1) && !x.AttendanceType.IsAbsence
            //        && x.StaffMember.TransportationRoute.ArriveIntervalId <= 97 && x.StaffMember.TransportationRoute.DepartIntervalId > 97)
            //        .Select(x => new { x.StaffMember.TransportationRoute, x.StaffMember, x.StaffMember.Location.Assets }).ToList();


            //    var s = db.Intervals;
            //    var foreCast = db.ForeCastings.Where(x => x.Day >= day && x.Day < day.AddDays(1));
            //    for (var q = s.Select(x => x.Id).Min(); q <= s.Select(x => x.Id).Max(); q = q + 1)
            //    {
            //        var x = new DataInserted() ;

            //        var covermapa = s.FirstOrDefault(x => x.Id == q).CoverMap;



            //        var staffavailable = routes.Where(x => x.TransportationRoute.ArriveIntervalId <= q && x.TransportationRoute.DepartIntervalId >= q)
            //            .Select(x => x.StaffMember.Id).Distinct().Count();
            //        var forcastdata = foreCast.FirstOrDefault(x => x.IntervalId == q);
            //        var availablebreak = staffavailable <= forcastdata.EmployeeCount ? 0 : staffavailable - (forcastdata.EmployeeCount);


            //        x.IntervalId = q;
            //        x.Covermapa = covermapa;
            //        x.availablebreak = availablebreak;
            //        x.finalstaffavailable = staffavailable;

            //        data.Add(x);
            //        if (q >= 97)
            //        {
            //            int staffb = routesbb.Where(y => y.TransportationRoute.ArriveIntervalId <= q && y.TransportationRoute.DepartIntervalId >= q)
            //                 .Select(x => x.StaffMember.Id).Distinct().Count();

            //           int covermapb = s.FirstOrDefault(x => x.Id == q).CoverMap;

            //            foreach(var d in data)
            //            {

            //                if (d.Covermapa==covermapb) {
            //                    d.finalstaffavailable += staffb;
            //                   d.availablebreak = staffavailable <= forcastdata.EmployeeCount ? 0 : staffavailable - (forcastdata.EmployeeCount);

            //                }

            //            }
            //        } 



            //}
            //return data;
            return true;



        }
        private bool RealCount(DateTime day,int scheduleId)
        {
            //var forcastDay = db.ForeCastings.FirstOrDefault(x => x.ScheduleId == scheduleId && day == x.TimebyQuarter);
            //foreach(var s in forcastDay.TimebyQuarter.ToString())
            //{
            //    var RealCount=db.dayOffOptions.Where(x => x.ScheduleId== scheduleId && (x.day))
            //}

            //for (var d= start; d <= end ; d =d.AddMinutes(15))
            //{

            //}

            //var routerbb= db.DailyAttendances
            //    .Include(x => x.StaffMember.TransportationRoute)
            //    .ThenInclude(x => x.Location.Assets).Include(x => x.ScheduleDetails)
            //    //.Include(x => x.StaffMember.TransportationRoute).Include(x => x.StaffMember.Location.Assets).Include(x => x.StaffMember.TransportationRoute.)
            //    .Where(x => x.Day == day.AddDays(-1) && !x.AttendanceType.IsAbsence 
            //    && x.StaffMember.TransportationRoute.ArriveIntervalId <= 97 && x.StaffMember.TransportationRoute.DepartIntervalId> 97)
            //    .Select(x => new { x.StaffMember.TransportationRoute, x.StaffMember, x.StaffMember.Location.Assets });
            //var dataintervalbb = db.Intervals.Where(x => x.Id >= 97);
            return true;
        }



    }
}
