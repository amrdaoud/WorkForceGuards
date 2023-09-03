using Microsoft.AspNetCore.Mvc;
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
using WorkForceGuards.Models.Reports;
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
        private readonly List<Interval> _intervalList;



        public ReportAdheranceService(ApplicationDbContext db, IUserService userService)
        {
            var timezone = TimeZoneInfo.FindSystemTimeZoneById("Arab Standard Time");
            today = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timezone);
            _db = db;
            _userService = userService;
            _intervalList = _db.Intervals.ToList();
        }


        public DataWithError Report(ReportFilter model, ClaimsPrincipal user)
        {
            var timezone = TimeZoneInfo.FindSystemTimeZoneById("Arab Standard Time");
            var appUser = _userService.GetUserInfo(user);

            DataWithError data = new DataWithError();
            model.DateFrom = TimeZoneInfo.ConvertTimeFromUtc(model.DateFrom, timezone).Date;
            model.DateTo = TimeZoneInfo.ConvertTimeFromUtc(model.DateTo, timezone).Date;
            if (model.FilterType.Count() != 0 && model.FilterValue.Count() != 0)
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
            else if (model.FilterValue.Count() == 0 && model.FilterType.Count() != 0)
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
              .Where(x => x.DailyAttendance.Day >= model.DateFrom && x.DailyAttendance.Day <= model.DateTo && model.FilterValue.Contains(x.DailyAttendance.StaffMember.Name));

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
                            PhonebyHour = y.Sum(x => x.Duration) / 60,
                            ActivitybyHour = (y.Count()) / 4.0,

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
            if (dbScheduleDetail.Count() != 0)
            {
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
                .Include(x => x.Activity)
                .Include(x => x.Interval)
                .Include(x => x.DailyAttendance.StaffMember)
                .Where(x => (x.DailyAttendance.Day == RequestDate
                                                                       && x.IntervalId <= 96) ||
                                                                       (x.DailyAttendance.Day == RequestDate.AddDays(-1)
                                                                        && x.IntervalId > 96));

            var Requested = currentScheduleDetails.GroupBy(x => new { x.IntervalId, x.ActivityId, x.Activity.Name, x.Interval.TimeMap, x.Activity.Color })

                .Select(y => new CountByActivityDto
                {

                    ActivityId = y.Key.ActivityId,
                    TimeMap = y.Key.TimeMap,
                    StaffCount = y.Count(),
                    ActivityColor = y.Key.Color,
                    ActivityName = y.Key.Name,
                    IntervalId = y.Key.IntervalId,





                }
            ).ToList();

            data.Result = Requested;
            data.ErrorMessage = null;
            return data;




        }

        public DataWithError StaffAttendanceReport(StaffAttendanceFilter filter, ClaimsPrincipal user)
        {
            var timezone = TimeZoneInfo.FindSystemTimeZoneById("Arab Standard Time");
            filter.DateFrom = TimeZoneInfo.ConvertTimeFromUtc(filter.DateFrom, timezone);
            filter.DateTo = TimeZoneInfo.ConvertTimeFromUtc(filter.DateTo, timezone);
            var sortProperty = typeof(StaffAttendanceReport).GetProperty(filter.Sort);
            var result = new List<StaffAttendanceReport>();
            var breakActivity = _db.Activities.FirstOrDefault(x => x.IsBreak);
            var backupActivity = _db.Activities.FirstOrDefault(x => x.Name == "Backup");
            var attendance = _db.ScheduleDetail
                            .Include(x => x.DailyAttendance.StaffMember)
                            .ThenInclude(x => x.StaffType)
                            .Include(x => x.DailyAttendance.AttendanceType)
                            .Include(x => x.DailyAttendance.HeadOfSection)
                            .Include(x => x.DailyAttendance.TransportationRoute)
                            .Include(x => x.DailyAttendance.Sublocation)
                            .Include(x => x.Activity)
                            .Include(x => x.Interval)
                            .Include(x => x.BackupStaff)
                            .Include(x => x.BackupToStaff)
                              .Where(x => x.DailyAttendance.Day >= filter.DateFrom && x.DailyAttendance.Day <= filter.DateTo)
                              .Where(x => (filter.SublocationId != null ? x.DailyAttendance.SublocationId == filter.SublocationId : true))
                              .Where(x => (filter.LocationId != null ? x.DailyAttendance.Sublocation.LocationId == filter.LocationId : true))
                              .Where(x => (filter.HosId != null ? x.DailyAttendance.HeadOfSectionId == filter.HosId : true))
                              .Where(x => (filter.StaffId != null ? x.DailyAttendance.StaffMemberId == filter.StaffId : true))
                              .Where(x => (filter.EmployeeId != null ? x.DailyAttendance.StaffMember.EmployeeId == filter.EmployeeId : true))
                              .ToList()
                              .GroupBy(x => new
                              {
                                  x.DailyAttendance.Day,
                                  x.DailyAttendance.StaffMemberId,
                                  x.DailyAttendance.StaffMember.EmployeeId,
                                  x.DailyAttendance.StaffMember.PhoneNumber,
                                  StaffMemberName = x.DailyAttendance.StaffMember.Name,
                                  StaffTypeName = x.DailyAttendance.StaffMember.StaffType.Name,
                                  ShiftName = x.DailyAttendance.TransportationRoute.Name,
                                  HeadOfSectionName = x.DailyAttendance.HeadOfSection.Name,
                                  AttendanceTypeName = x.DailyAttendance.AttendanceType.Name,
                                  SublocationName = x.DailyAttendance.Sublocation.Name
                                  //ActivityName = x.Activity.Name
                              })
                            .Select(g => new StaffAttendanceReport
                            {
                                StaffMemberId = g.Key.StaffMemberId,
                                EmployeeId = g.Key.EmployeeId,
                                PhoneNumber = g.Key.PhoneNumber,
                                Day = g.Key.Day,
                                StaffMemberName = g.Key.StaffMemberName,
                                HeadOfSectionName = g.Key.HeadOfSectionName,
                                StaffTypeName = g.Key.StaffTypeName,
                                ShiftName = g.Key.ShiftName,
                                AttendanceTypeName = g.Key.AttendanceTypeName,
                                SublocationName = g.Key.SublocationName,
                                //g.Key.ActivityName,
                                //MinInterval = getSplittedText(splitIntoIntervals(g.Select(x => x.Interval).OrderBy(x => x.Id).ToList()))
                                ActualShift = g.Select(x => x.Interval).OrderBy(x => x.Id).First().TimeMap.ToString().Substring(0,5) + "-" + g.Select(x => x.Interval).OrderBy(x => x.Id).Last().TimeMap.Add(new TimeSpan(0,14,59)).ToString().Substring(0, 5),
                                Activities = g.GroupBy(x => new {
                                    ActivityName = x.Activity.Name,
                                    BackupStaffMemberName = (breakActivity?.Id) == x.ActivityId ? x.BackupStaff?.Name : null,
                                    BackupToStaffMemberName = (backupActivity?.Id) == x.ActivityId ? x.BackupToStaff?.Name : null
                                })
                                              .Select(g2 => g2.Key.ActivityName + (g2.Key.BackupStaffMemberName != null ? $@"({g2.Key.BackupStaffMemberName})" : g2.Key.BackupToStaffMemberName != null ? $@"({g2.Key.BackupToStaffMemberName})" : "") + ": "
                                              + getSplittedText(splitIntoIntervals(g2.Select(x => x.Interval).OrderBy(x => x.Id).ToList())).Aggregate((a,b) => a + "|" + b))
                                              .Aggregate((a,b) => a + " , " + b)
                            });
            //.ToList();
            if (sortProperty != null && filter.Order == "asc")
            {
                result = attendance.OrderBy(x => sortProperty.GetValue(x)).ToList();
                result = result.Skip(filter.PageIndex * filter.PageSize).Take(filter.PageSize).ToList();
            }

            else if (sortProperty != null && filter.Order == "desc")
            {
                result = attendance.OrderByDescending(x => sortProperty.GetValue(x)).ToList();
                result = result.Skip(filter.PageIndex * filter.PageSize).Take(filter.PageSize).ToList();
            }
            return new DataWithError(new StaffAttendanceReportWithSize { Result= result, ResultSize = attendance.Count() }, "");

        }
        public List<StaffAttendanceReport> StaffAttendanceReportDownload(StaffAttendanceFilter filter, ClaimsPrincipal user)
        {
            var timezone = TimeZoneInfo.FindSystemTimeZoneById("Arab Standard Time");
            filter.DateFrom = TimeZoneInfo.ConvertTimeFromUtc(filter.DateFrom, timezone);
            filter.DateTo = TimeZoneInfo.ConvertTimeFromUtc(filter.DateTo, timezone);
            var sortProperty = typeof(StaffAttendanceReport).GetProperty(filter.Sort);
            var result = new List<StaffAttendanceReport>();
            var breakActivity = _db.Activities.FirstOrDefault(x => x.IsBreak);
            var backupActivity = _db.Activities.FirstOrDefault(x => x.Name == "Backup");
            var attendance = _db.ScheduleDetail
                            .Include(x => x.DailyAttendance.StaffMember)
                            .ThenInclude(x => x.StaffType)
                            .Include(x => x.DailyAttendance.AttendanceType)
                            .Include(x => x.DailyAttendance.HeadOfSection)
                            .Include(x => x.DailyAttendance.TransportationRoute)
                            .Include(x => x.DailyAttendance.Sublocation)
                            .Include(x => x.Activity)
                            .Include(x => x.Interval)
                            .Include(x => x.BackupStaff)
                            .Include(x => x.BackupToStaff)
                              .Where(x => x.DailyAttendance.Day >= filter.DateFrom && x.DailyAttendance.Day <= filter.DateTo)
                              .Where(x => (filter.SublocationId != null ? x.DailyAttendance.SublocationId == filter.SublocationId : true))
                              .Where(x => (filter.LocationId != null ? x.DailyAttendance.Sublocation.LocationId == filter.LocationId : true))
                              .Where(x => (filter.HosId != null ? x.DailyAttendance.HeadOfSectionId == filter.HosId : true))
                              .Where(x => (filter.StaffId != null ? x.DailyAttendance.StaffMemberId == filter.StaffId : true))
                              .Where(x => (filter.EmployeeId != null ? x.DailyAttendance.StaffMember.EmployeeId == filter.EmployeeId : true))
                              .ToList()
                              .GroupBy(x => new
                              {
                                  x.DailyAttendance.Day,
                                  x.DailyAttendance.StaffMemberId,
                                  x.DailyAttendance.StaffMember.EmployeeId,
                                  x.DailyAttendance.StaffMember.PhoneNumber,
                                  StaffMemberName = x.DailyAttendance.StaffMember.Name,
                                  StaffTypeName = x.DailyAttendance.StaffMember.StaffType.Name,
                                  ShiftName = x.DailyAttendance.TransportationRoute.Name,
                                  HeadOfSectionName = x.DailyAttendance.HeadOfSection.Name,
                                  AttendanceTypeName = x.DailyAttendance.AttendanceType.Name,
                                  SublocationName = x.DailyAttendance.Sublocation.Name
                                  //ActivityName = x.Activity.Name
                              })
                            .Select(g => new StaffAttendanceReport
                            {
                                StaffMemberId = g.Key.StaffMemberId,
                                EmployeeId = g.Key.EmployeeId,
                                PhoneNumber = g.Key.PhoneNumber,
                                Day = g.Key.Day,
                                StaffMemberName = g.Key.StaffMemberName,
                                HeadOfSectionName = g.Key.HeadOfSectionName,
                                StaffTypeName = g.Key.StaffTypeName,
                                ShiftName = g.Key.ShiftName,
                                AttendanceTypeName = g.Key.AttendanceTypeName,
                                SublocationName = g.Key.SublocationName,
                                //g.Key.ActivityName,
                                //MinInterval = getSplittedText(splitIntoIntervals(g.Select(x => x.Interval).OrderBy(x => x.Id).ToList()))
                                ActualShift = g.Select(x => x.Interval).OrderBy(x => x.Id).First().TimeMap.ToString().Substring(0, 5) + "-" + g.Select(x => x.Interval).OrderBy(x => x.Id).Last().TimeMap.Add(new TimeSpan(0, 14, 59)).ToString().Substring(0, 5),
                                Activities = g.GroupBy(x => new {
                                    ActivityName = x.Activity.Name,
                                    BackupStaffMemberName = (breakActivity?.Id) == x.ActivityId ? x.BackupStaff?.Name : null,
                                    BackupToStaffMemberName = (backupActivity?.Id) == x.ActivityId ? x.BackupToStaff?.Name : null
                                })
                                              .Select(g2 => g2.Key.ActivityName + (g2.Key.BackupStaffMemberName != null ? $@"({g2.Key.BackupStaffMemberName})" : g2.Key.BackupToStaffMemberName != null ? $@"({g2.Key.BackupToStaffMemberName})" : "") + ": "
                                              + getSplittedText(splitIntoIntervals(g2.Select(x => x.Interval).OrderBy(x => x.Id).ToList())).Aggregate((a, b) => a + "|" + b))
                                              .Aggregate((a, b) => a + " , " + b)
                            });
            //.ToList();
            if (sortProperty != null && filter.Order == "asc")
            {
                result = attendance.OrderBy(x => sortProperty.GetValue(x)).ToList();
            }

            else if (sortProperty != null && filter.Order == "desc")
            {
                result = attendance.OrderByDescending(x => sortProperty.GetValue(x)).ToList();
            }
            return result;

        }

        public DataWithError StaffWorkingDaysReport(StaffAttendanceFilter filter, ClaimsPrincipal user)
        {
            var timezone = TimeZoneInfo.FindSystemTimeZoneById("Arab Standard Time");
            filter.DateFrom = TimeZoneInfo.ConvertTimeFromUtc(filter.DateFrom, timezone);
            filter.DateTo = TimeZoneInfo.ConvertTimeFromUtc(filter.DateTo, timezone);
            var sortProperty = typeof(StaffWorkingDaysReport).GetProperty(filter.Sort);
            var result = new List<StaffWorkingDaysReport>();
            var attendance = _db.DailyAttendances
                              .Include(x => x.StaffMember)
                              .ThenInclude(s => s.StaffType)
                              .Include(x => x.HeadOfSection)
                              .Include(x => x.AttendanceType)
                              .Include(x => x.TransportationRoute)
                              .Include(x => x.Sublocation)
                              .Include(x => x.ScheduleDetails)
                              .ThenInclude(x => x.Interval)
                              .Where(x => x.Day >= filter.DateFrom && x.Day <= filter.DateTo)
                              .Where(x => (filter.SublocationId != null ? x.SublocationId == filter.SublocationId : true))
                              .Where(x => (filter.LocationId != null ? x.Sublocation.LocationId == filter.LocationId : true))
                              .Where(x => (filter.HosId != null ? x.HeadOfSectionId == filter.HosId : true))
                              .Where(x => (filter.StaffId != null ? x.StaffMemberId == filter.StaffId : true))
                              .ToList()
                              .GroupBy(x => new
                              {
                                  x.StaffMember.EmployeeId,
                                  StaffMemberName = x.StaffMember.Name,
                                  StaffTypeName = x.StaffMember.StaffType.Name,
                                  ShiftName = x.TransportationRoute.Name,
                                  HeadOfSectionName = x.HeadOfSection.Name,
                                  SublocationName = x.Sublocation.Name
                              })
                              .Select(g => new StaffWorkingDaysReport
                              {
                                  EmployeeId = g.Key.EmployeeId,
                                  StaffMemberName = g.Key.StaffMemberName,
                                  HeadOfSectionName =g.Key.HeadOfSectionName,
                                  StaffTypeName = g.Key.StaffTypeName,
                                  ShiftName = g.Key.ShiftName,
                                  SublocationName = g.Key.SublocationName,
                                  Attendance = g.GroupBy(x => x.AttendanceType.Name)
                                                .Select(g2 => g2.Key + ": " + g2.Count()).Aggregate((a, b) => a + " , " + b)
                              });
            if (sortProperty != null && filter.Order == "asc")
            {
                result = attendance.OrderBy(x => sortProperty.GetValue(x)).ToList();
                result = result.Skip(filter.PageIndex * filter.PageSize).Take(filter.PageSize).ToList();
            }

            else if (sortProperty != null && filter.Order == "desc")
            {
                result = attendance.OrderByDescending(x => sortProperty.GetValue(x)).ToList();
                result = result.Skip(filter.PageIndex * filter.PageSize).Take(filter.PageSize).ToList();
            }
            return new DataWithError(new StaffWorkingDaysReporttWithSize { Result = result, ResultSize = attendance.Count() }, "");
        }

        public List<StaffWorkingDaysReport> StaffWorkingDaysReportDownload(StaffAttendanceFilter filter, ClaimsPrincipal user)
        {
            var timezone = TimeZoneInfo.FindSystemTimeZoneById("Arab Standard Time");
            filter.DateFrom = TimeZoneInfo.ConvertTimeFromUtc(filter.DateFrom, timezone);
            filter.DateTo = TimeZoneInfo.ConvertTimeFromUtc(filter.DateTo, timezone);
            var sortProperty = typeof(StaffWorkingDaysReport).GetProperty(filter.Sort);
            var result = new List<StaffWorkingDaysReport>();
            var attendance = _db.DailyAttendances
                              .Include(x => x.StaffMember)
                              .ThenInclude(s => s.StaffType)
                              .Include(x => x.HeadOfSection)
                              .Include(x => x.AttendanceType)
                              .Include(x => x.TransportationRoute)
                              .Include(x => x.Sublocation)
                              .Include(x => x.ScheduleDetails)
                              .ThenInclude(x => x.Interval)
                              .Where(x => x.Day >= filter.DateFrom && x.Day <= filter.DateTo)
                              .Where(x => (filter.SublocationId != null ? x.SublocationId == filter.SublocationId : true))
                              .Where(x => (filter.LocationId != null ? x.Sublocation.LocationId == filter.LocationId : true))
                              .Where(x => (filter.HosId != null ? x.HeadOfSectionId == filter.HosId : true))
                              .Where(x => (filter.StaffId != null ? x.StaffMemberId == filter.StaffId : true))
                              .ToList()
                              .GroupBy(x => new
                              {
                                  x.StaffMember.EmployeeId,
                                  StaffMemberName = x.StaffMember.Name,
                                  StaffTypeName = x.StaffMember.StaffType.Name,
                                  ShiftName = x.TransportationRoute.Name,
                                  HeadOfSectionName = x.HeadOfSection.Name,
                                  SublocationName = x.Sublocation.Name
                              })
                              .Select(g => new StaffWorkingDaysReport
                              {
                                  EmployeeId = g.Key.EmployeeId,
                                  StaffMemberName = g.Key.StaffMemberName,
                                  HeadOfSectionName = g.Key.HeadOfSectionName,
                                  StaffTypeName = g.Key.StaffTypeName,
                                  ShiftName = g.Key.ShiftName,
                                  SublocationName = g.Key.SublocationName,
                                  Attendance = g.GroupBy(x => x.AttendanceType.Name)
                                                .Select(g2 => g2.Key + ": " + g2.Count()).Aggregate((a, b) => a + " , " + b)
                              });
            if (sortProperty != null && filter.Order == "asc")
            {
                result = attendance.OrderBy(x => sortProperty.GetValue(x)).ToList();
            }

            else if (sortProperty != null && filter.Order == "desc")
            {
                result = attendance.OrderByDescending(x => sortProperty.GetValue(x)).ToList();
            }
            return result;
        }
        private List<string> getSplittedText(List<List<Interval>> intervals)
        {
            return intervals.Select(x => x.OrderBy(z => z.Id).First().TimeMap.ToString().Substring(0, 5) + '-' + x.OrderBy(z => z.Id).Last().TimeMap.Add(new TimeSpan(0,14,59)) .ToString().Substring(0, 5)).ToList();
        }
        private static List<List<Interval>> splitIntoIntervals(List<Interval> dates)
        {
            List<List<Interval>> intervals = new List<List<Interval>>();
            List<Interval> currentInterval = new List<Interval>();

            for (int i = 0; i < dates.Count; i++)
            {
                if (i == 0 || (dates[i].Id - dates[i - 1].Id) == 1)
                {
                    currentInterval.Add(dates[i]);
                }
                else
                {
                    intervals.Add(currentInterval);
                    currentInterval = new List<Interval>() { dates[i] };
                }
            }

            intervals.Add(currentInterval);

            return intervals;
        }
    }

}

