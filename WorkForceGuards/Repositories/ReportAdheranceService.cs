using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using WorkForceManagementV0.Contexts;
using WorkForceManagementV0.Models;
using WorkForceManagementV0.Models.Bindings;
using WorkForceManagementV0.Models.DTO;
using WorkForceManagementV0.Models.Reports;
using WorkForceManagementV0.Repositories.Identity;
using WorkForceManagementV0.Repositories.Interfaces;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WorkForceManagementV0.Repositories
{
    public class ReportAdheranceService : IReportAdheranceService
    {
        public readonly ApplicationDbContext _db;
        public readonly IUserService _userService;
        private readonly DateTime today;



        public ReportAdheranceService(ApplicationDbContext db, IUserService userService)
        {
            var timezone = TimeZoneInfo.FindSystemTimeZoneById("Arab Standard Time");
            today = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timezone);
            _db = db;
            _userService = userService;
        }


        public DataWithError Report(ReportFilter model, ClaimsPrincipal user)
        {
            var timezone = TimeZoneInfo.FindSystemTimeZoneById("Arab Standard Time");
            var appUser = _userService.GetUserInfo(user);
            
            DataWithError data = new DataWithError();
            model.DateFrom = TimeZoneInfo.ConvertTimeFromUtc(model.DateFrom, timezone).Date;
            model.DateTo = TimeZoneInfo.ConvertTimeFromUtc(model.DateTo, timezone).Date;
            if (model.FilterType.Count() !=0 && model.FilterValue.Count() !=0)
            {
                
                    switch (model.FilterType)
                    {

                        case ("Staff"):
                            return AdherancebyStaff(model, user);
                        case ("Hos"):
                            return AdherancebyHos(model, user);

                        case ("Transportation"):
                          return AdherancebyTransportation(model, user);

                        default:
                            data.Result = null;
                            data.ErrorMessage = "invalid filter value";
                            return data;
                    }
               

            } 
            else if (model.FilterValue.Count() == 0  && model.FilterType.Count() !=0)
            {
                return AdherancebyAll(model, user);

            }
            data.Result = null;
            data.ErrorMessage = " your request is not found  enter filter value please ";
            return data;



        }

        private DataWithError AdherancebyStaff(ReportFilter model, ClaimsPrincipal user)
        {
            DataWithError data = new DataWithError();

            //var rrr = int.TryParse(model.FilterValue, out int value);
            var dbScheduleDetail = _db.ScheduleDetail
              .Include(x => x.Interval)
              .Include(x => x.Activity)
              .Include(x => x.DailyAttendance)
              .Include(x => x.DailyAttendance).ThenInclude(x => x.StaffMember)
              .Include(x => x.DailyAttendance).ThenInclude(x => x.HeadOfSection)
              .Where(x => x.DailyAttendance.Day >= model.DateFrom && x.DailyAttendance.Day <= model.DateTo  && model.FilterValue.Contains(x.DailyAttendance.StaffMember.Name));

            if (dbScheduleDetail.Count() != 0)
            {

                var staffMember = _db.StaffMembers.Where(x => model.FilterValue.Contains(x.Name));



                if (staffMember == null)
                {
                    data.Result = null;
                    data.ErrorMessage = "staffmembers is not found ";
                    return data;
                }

                else if (model.IsDaily == true)
                {


                    var adh = dbScheduleDetail.Where(x => x.Activity.IsPhone)
                        .GroupBy(x => new
                        {
                            Date = x.DailyAttendance.Day,
                            Groupby = model.Groupby == "Staff" ? x.DailyAttendance.StaffMember.Name
                            : model.Groupby == "Hos" ? x.DailyAttendance.HeadOfSection.Name
                            : x.DailyAttendance.TransportationRoute.Name
                        })
                        .Select(y => new AdherancebyStaffDto
                        {

                            HosName = model.Groupby == "Hos" ? y.Key.Groupby : null,
                            StaffName = model.Groupby == "Staff" ? y.Key.Groupby : null,
                            Day = y.Key.Date,
                            TransportationRoute = model.Groupby == "TransportationRoute" ? y.Key.Groupby : null,
                            Adherance = y.Sum(x => x.Duration / 15) / y.Count(),
                            PhonebyHour = y.Sum(x => x.Duration) / 60,
                            ActivitybyHour = (y.Count()) / 4.0
                            //y.Sum(x => x.Duration) // / y.Count()
                        }).ToList();
                    data.Result = adh;
                    data.ErrorMessage = "";
                    return data;
                }
                var adhe = dbScheduleDetail.Where(x => x.Activity.IsPhone)
                        .GroupBy(x => new
                        {
                            //Date = x.DailyAttendance.Day,
                            Groupby = model.Groupby == "Staff" ? x.DailyAttendance.StaffMember.Name
                            : model.Groupby == "Hos" ? x.DailyAttendance.HeadOfSection.Name
                            : x.DailyAttendance.TransportationRoute.Name
                        })
                        .Select(y => new AdherancebyStaffDto
                        {

                            HosName = model.Groupby == "Hos" ? y.Key.Groupby : null,
                            StaffName = model.Groupby == "Staff" ? y.Key.Groupby : null,
                            Day = null,
                            TransportationRoute = model.Groupby == "TransportationRoute" ? y.Key.Groupby : null,
                            PhonebyHour=y.Sum(x=> x.Duration)/60,
                            ActivitybyHour=(y.Count())/ 4.0,    
                            
                            Adherance = y.Sum(x => x.Duration / 15) / y.Count()
                            //y.Sum(x => x.Duration) // / y.Count()
                        }).ToList();

                




                data.Result = adhe;
                data.ErrorMessage = "";
                return data;
            }
            data.Result = null;
            data.ErrorMessage = " no data please check your DateFrom , DateTo ,FilterValue";
            return data;



        }

        private DataWithError AdherancebyHos(ReportFilter model, ClaimsPrincipal user)
        {
            DataWithError data = new DataWithError();
            var Hos = _db.HeadOfSections.Where(x => model.FilterValue.Contains(x.Name));
            var dbScheduleDetail = _db.ScheduleDetail
             .Include(x => x.Interval)
             .Include(x => x.Activity)
             .Include(x => x.DailyAttendance)
             .Include(x => x.DailyAttendance).ThenInclude(x => x.StaffMember)
             .Include(x => x.DailyAttendance).ThenInclude(x => x.HeadOfSection)
             .Where(x => x.DailyAttendance.Day >= model.DateFrom && x.DailyAttendance.Day <= model.DateTo && model.FilterValue.Contains(x.DailyAttendance.HeadOfSection.Name));
            if (dbScheduleDetail.Count() != 0)
            {

              
                if (Hos == null)
                {
                    data.Result = null;
                    data.ErrorMessage = "Hos Id is not Found";
                    return data;

                }
                else if (model.IsDaily == true)
                {




                    var adhe = dbScheduleDetail.Where(x => x.Activity.IsPhone)
                        .GroupBy(x => new
                        {
                            HosId = x.DailyAttendance.HeadOfSectionId,
                            HosName = x.DailyAttendance.HeadOfSection.Name,
                            //StaffId = x.DailyAttendance.StaffMember.Id,
                            //StaffName = x.DailyAttendance.StaffMember.Name,
                            Date = x.DailyAttendance.Day,
                            Groupby = model.Groupby == "Staff" ? x.DailyAttendance.StaffMember.Name
                            : model.Groupby == "Hos" ? x.DailyAttendance.HeadOfSection.Name
                            : x.DailyAttendance.TransportationRoute.Name
                        })
                        .Select(y => new AdherancebyStaffDto
                        {
                            HosId = y.Key.HosId,
                            HosName = y.Key.HosName,
                            Day = y.Key.Date,
                            StaffName = model.Groupby == "Staff" ? y.Key.Groupby : null,
                            TransportationRoute = model.Groupby == "TransportationRoute" ? y.Key.Groupby : null,
                            Adherance = y.Sum(x => x.Duration / 15) / y.Count(),
                            PhonebyHour = y.Sum(x => x.Duration) / 60,
                            ActivitybyHour = (y.Count()) / 4.0
                            //y.Sum(x => x.Duration) // / y.Count()
                        }).ToList();
                    data.Result = adhe;
                    data.ErrorMessage = "";
                    return data;



                }
                var adh = dbScheduleDetail.Where(x => x.Activity.IsPhone)
                           .GroupBy(x => new
                           {
                               HosId = x.DailyAttendance.HeadOfSectionId,
                               HosName = x.DailyAttendance.HeadOfSection.Name,
                               //StaffId = x.DailyAttendance.StaffMember.Id,
                               //StaffName = x.DailyAttendance.StaffMember.Name,
                               //Date = x.DailyAttendance.Day,
                               Groupby = model.Groupby == "Staff" ? x.DailyAttendance.StaffMember.Name
                               : model.Groupby == "Hos" ? x.DailyAttendance.HeadOfSection.Name
                               : x.DailyAttendance.TransportationRoute.Name
                           })
                           .Select(y => new AdherancebyStaffDto
                           {
                               HosId = y.Key.HosId,
                               HosName = y.Key.HosName,
                               //Day = y.Key.Date,
                               StaffName = model.Groupby == "Staff" ? y.Key.Groupby : null,
                               TransportationRoute = model.Groupby == "TransportationRoute" ? y.Key.Groupby : null,
                               Adherance = y.Sum(x => x.Duration / 15) / y.Count(),
                               PhonebyHour = y.Sum(x => x.Duration) / 60,
                               ActivitybyHour = (y.Count()) / 4.0
                               //y.Sum(x => x.Duration) // / y.Count()
                           }).ToList();
                data.Result = adh;
                data.ErrorMessage = "";
                return data;
            }
            data.Result = null;
            data.ErrorMessage = " no data please check your DateFrom , DateTo ,FilterValue";
            return data;

        }
        private DataWithError AdherancebyTransportation(ReportFilter model, ClaimsPrincipal user)
        {
            DataWithError data = new DataWithError();

            var dbScheduleDetail = _db.ScheduleDetail
             .Include(x => x.Interval)
             .Include(x => x.Activity)
             .Include(x => x.DailyAttendance)
             .Include(x => x.DailyAttendance).ThenInclude(x => x.StaffMember)
             .Include(x => x.DailyAttendance).ThenInclude(x => x.HeadOfSection)
             .Where(x => x.DailyAttendance.Day >= model.DateFrom && x.DailyAttendance.Day <= model.DateTo && model.FilterValue.Contains(x.DailyAttendance.TransportationRoute.Name));
            if (dbScheduleDetail.Count() != 0)
            {

                var Transportation = _db.TransportationRoutes.Where(x => model.FilterValue.Contains(x.Name));
                if (Transportation == null)
                {
                    data.Result = null;
                    data.ErrorMessage = "TransportationId  is not Found";
                    return data;
                }
                else if (model.IsDaily == true)
                {

                    var adh = dbScheduleDetail.Where(x => x.Activity.IsPhone == true)
                           .GroupBy(x => new
                           {
                               Date = x.DailyAttendance.Day,
                               Groupby = model.Groupby == "Staff" ? x.DailyAttendance.StaffMember.Name
                                : model.Groupby == "Hos" ? x.DailyAttendance.HeadOfSection.Name
                                : x.DailyAttendance.TransportationRoute.Name
                           })
                            .Select(y => new AdherancebyStaffDto
                            {

                                HosName = model.Groupby == "Hos" ? y.Key.Groupby : null,
                                StaffName = model.Groupby == "Staff" ? y.Key.Groupby : null,
                                Day = y.Key.Date,
                                TransportationRoute = model.Groupby == "TransportationRoute" ? y.Key.Groupby : null,
                                Adherance = y.Sum(x => x.Duration / 15) / y.Count(),
                                PhonebyHour = y.Sum(x => x.Duration) / 60,
                                ActivitybyHour = (y.Count()) / 4.0


                            }).ToList();
                    data.Result = adh;
                    data.ErrorMessage = "";
                    return data;

                }
                var adhe = dbScheduleDetail.Where(x => x.Activity.IsPhone == true)
                         .GroupBy(x => new
                         {
                             //Date = x.DailyAttendance.Day,
                             Groupby = model.Groupby == "Staff" ? x.DailyAttendance.StaffMember.Name
                              : model.Groupby == "Hos" ? x.DailyAttendance.HeadOfSection.Name
                              : x.DailyAttendance.TransportationRoute.Name
                         })
                          .Select(y => new AdherancebyStaffDto
                          {

                              HosName = model.Groupby == "Hos" ? y.Key.Groupby : null,
                              StaffName = model.Groupby == "Staff" ? y.Key.Groupby : null,
                              //Day = y.Key.Date,
                              TransportationRoute = model.Groupby == "TransportationRoute" ? y.Key.Groupby : null,
                              Adherance = y.Sum(x => x.Duration / 15) / y.Count(),
                              PhonebyHour = y.Sum(x => x.Duration) / 60,
                              ActivitybyHour = (y.Count()) / 4.0


                          }).ToList();
                data.Result = adhe;
                data.ErrorMessage = "";
                return data;
            }
            data.Result = null;
            data.ErrorMessage = " no data please check your DateFrom , DateTo ,FilterValue";
            return data;


        }
        private DataWithError AdherancebyAll(ReportFilter model, ClaimsPrincipal user)
        {
            DataWithError data = new DataWithError();
          
            var dbScheduleDetail = _db.ScheduleDetail
             .Include(x => x.Interval)
             .Include(x => x.Activity)
             .Include(x => x.DailyAttendance)
             .Include(x => x.DailyAttendance).ThenInclude(x => x.StaffMember)
             .Include(x => x.DailyAttendance).ThenInclude(x => x.HeadOfSection)
             .Where(x => x.DailyAttendance.Day >= model.DateFrom && x.DailyAttendance.Day <= model.DateTo);
            if(dbScheduleDetail.Count() !=0)
            {
                var adhe = dbScheduleDetail.Where(x => x.Activity.IsPhone)
                       .GroupBy(x => new {

                           //Date = x.DailyAttendance.Day,
                           Groupby = model.Groupby == "Staff" ? x.DailyAttendance.StaffMember.Name
                           : model.Groupby == "Hos" ? x.DailyAttendance.HeadOfSection.Name
                           : x.DailyAttendance.TransportationRoute.Name
                       })
                       .Select(y => new AdherancebyStaffDto
                       {
                           HosName = model.Groupby == "Hos" ? y.Key.Groupby : null,
                          // Day = y.Key.Date,
                           StaffName = model.Groupby == "Staff" ? y.Key.Groupby : null,
                           TransportationRoute = model.Groupby == "TransportationRoute" ? y.Key.Groupby : null,
                           Adherance = y.Sum(x => x.Duration / 15) / y.Count(),
                           PhonebyHour = y.Sum(x => x.Duration) / 60,
                           ActivitybyHour = (y.Count()) / 4.0
                           //y.Sum(x => x.Duration) // / y.Count()
                       }).ToList();
                data.Result = adhe;
                data.ErrorMessage = "";
                return data;

            }
            data.Result = null;
            data.ErrorMessage = " no data please check your DateFrom , DateTo ";
            return data;

        }

        public DataWithError ActivityReport(DateTime RequestDate, ClaimsPrincipal user)

        {
            DataWithError data = new DataWithError();
            var timezone = TimeZoneInfo.FindSystemTimeZoneById("Arab Standard Time");
            RequestDate = TimeZoneInfo.ConvertTimeFromUtc(RequestDate, timezone).Date;


            var currentScheduleDetails = _db.ScheduleDetail
                .Include(x=> x.Activity)
                .Include(x=> x.Interval)
                .Include(x=> x.DailyAttendance.StaffMember)
                . Where(x => (x.DailyAttendance.Day == RequestDate
                                                                       && x.IntervalId <= 96) ||
                                                                       (x.DailyAttendance.Day == RequestDate.AddDays(-1)
                                                                        && x.IntervalId > 96));

            var Requested = currentScheduleDetails.GroupBy(x => new { x.IntervalId, x.ActivityId, x.Activity.Name,x.Interval.TimeMap ,x.Activity.Color})

                .Select(y => new CountByActivityDto
                {

                    ActivityId = y.Key.ActivityId,
                    TimeMap=y.Key.TimeMap,
                    StaffCount= y.Count(),
                    ActivityColor=y.Key.Color,
                    ActivityName = y.Key.Name,
                    IntervalId=y.Key.IntervalId,

                 
                    


                }
            ).ToList();

            data.Result = Requested;
            data.ErrorMessage = null;
            return data;


        
                                        
        }
    }

}

