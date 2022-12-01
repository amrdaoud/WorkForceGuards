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
    public class SwapRequestService : ISwapRequestService
    {
        private readonly ApplicationDbContext _db;
        private readonly TimeZoneInfo timezone;
        private readonly IUserService _userService;
        public SwapRequestService(ApplicationDbContext db, IUserService userService)
        {
            _db = db;
            timezone = TimeZoneInfo.FindSystemTimeZoneById("Arab Standard Time");
            _userService = userService;
        }
        public DataWithError AddRequest(SwapRequest model)
        {
            var sourceDailyAttendance = _db.DailyAttendances.Find(model.SourceDailyAttendanceId);
            var destinationDailyAttendance = _db.DailyAttendances.Find(model.DestinationDailyAttendanceId);
            if (sourceDailyAttendance.ScheduleId != destinationDailyAttendance.ScheduleId)
            {
                return new DataWithError(null, "Please select attendance from the same schedule!");
            }
            _db.Entry(sourceDailyAttendance).Reference(d => d.Schedule).Load();
            if (!sourceDailyAttendance.Schedule.IsPublish)
            {
                return new DataWithError(null, "Schedule is not published!");
            }
            //var sameRequestDays = _db.SwapRequests.FirstOrDefault(x =>
            //x.ScheduleId == model.ScheduleId &&
            //!x.Status.IsComplete &&
            //    (
            //        x.SourceDailyAttendance.StaffMemberId == sourceDailyAttendance.StaffMemberId ||
            //        x.DestinationDailyAttendance.StaffMemberId == destinationDailyAttendance.StaffMemberId ||
            //        x.SourceDailyAttendance.StaffMemberId == destinationDailyAttendance.StaffMemberId ||
            //        x.DestinationDailyAttendance.StaffMemberId == sourceDailyAttendance.StaffMemberId
            //    ) &&
            
            //    (
            //        x.SourceDailyAttendanceId == model.SourceDailyAttendanceId ||
            //        x.SourceDailyAttendance.Day == 
            //    )
            //);
            var timezone = TimeZoneInfo.FindSystemTimeZoneById("Arab Standard Time");
            model.IssueDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timezone);
            _db.Entry(sourceDailyAttendance).Reference(d => d.HeadOfSection).Load();
            _db.Entry(sourceDailyAttendance).Reference(d => d.StaffMember).Load();
            _db.Entry(destinationDailyAttendance).Reference(d => d.StaffMember).Load();
            var swapRequest = new SwapRequest(
                sourceDailyAttendance, destinationDailyAttendance, model.IssueDate, model.RequesterAlias, model.ScheduleId
                );
            _db.SwapRequests.Add(swapRequest);
            _db.SaveChanges();
            return new DataWithError(swapRequest, "");
        }
        public DataWithError Approve(SwapRequestApprovalBinding model, ClaimsPrincipal user)
        {
            //User must be here
            var request = _db.SwapRequests
                .Include(x => x.Details)
                .Include(x => x.Status)
                .FirstOrDefault(x => x.Id == model.RequestId);
            if (request == null)
            {
                return new DataWithError(null, "Swap request is not exist!");
            }
            if (request.Status.IsComplete)
            {
                return new DataWithError(null, "Swap request already handled!");
            }
            var lastDetail = request.Details.OrderBy(x => x.IssueDate).LastOrDefault();
            var currentDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timezone);
            if (request.StatusId == "APPRV1")
            {
                lastDetail.IsApproved = model.IsApproved;
                lastDetail.IsDeclined = !model.IsApproved;
                lastDetail.CloseDate = currentDate;
                lastDetail.Reason = model.Reason;
                if (model.IsApproved)
                {
                    _db.Entry(request).Reference(x => x.SourceDailyAttendance).Load();
                    _db.Entry(request).Reference(x => x.DestinationDailyAttendance).Load();
                    var sourceStaffId = request.SourceDailyAttendance.StaffMemberId;
                    var sourceHeadOfSectionId = request.SourceDailyAttendance.HeadOfSectionId;
                    var destStaffId = request.DestinationDailyAttendance.StaffMemberId;
                    var destHeadOfSectionId = request.DestinationDailyAttendance.HeadOfSectionId;
                    var sourceAlt = _db.DailyAttendances.Include(x => x.ScheduleDetails)
                        .FirstOrDefault(x => x.Day == request.SourceDailyAttendance.Day && x.StaffMemberId == request.DestinationDailyAttendance.StaffMemberId);
                    if (sourceAlt == null)
                    {
                        return new DataWithError(null, "Swap request cannot be applied!");
                    }
                    var destAlt = _db.DailyAttendances.Include(x => x.ScheduleDetails)
                        .FirstOrDefault(x => x.Day == request.DestinationDailyAttendance.Day && x.StaffMemberId == sourceStaffId);
                    if (destAlt == null)
                    {
                        return new DataWithError(null, "Swap request cannot be applied!");
                    }
                    _db.Entry(request.SourceDailyAttendance).Collection(sd => sd.ScheduleDetails).Load();
                    _db.Entry(request.DestinationDailyAttendance).Collection(sd => sd.ScheduleDetails).Load();
                    foreach(ScheduleDetail d in request.SourceDailyAttendance.ScheduleDetails)
                    {
                        d.Duration = null;
                    }
                    foreach (ScheduleDetail d in request.DestinationDailyAttendance.ScheduleDetails)
                    {
                        d.Duration = null;
                    }

                    foreach (ScheduleDetail d in sourceAlt.ScheduleDetails)
                    {
                        d.Duration = null;
                    }
                    foreach (ScheduleDetail d in destAlt.ScheduleDetails)
                    {
                        d.Duration = null;
                    }
                    request.SourceDailyAttendance.StaffMemberId = destStaffId;
                    request.SourceDailyAttendance.HeadOfSectionId = destHeadOfSectionId;
                    request.DestinationDailyAttendance.StaffMemberId = sourceStaffId;
                    request.DestinationDailyAttendance.HeadOfSectionId = sourceHeadOfSectionId;

                    try
                    {
                        _db.SaveChanges();
                    }
                    catch
                    {
                        return new DataWithError(null, "Requester Day-Off has the same Responder Day-Off. Please Decline!");
                    }
                    sourceAlt.StaffMemberId = sourceStaffId;
                    sourceAlt.HeadOfSectionId = sourceHeadOfSectionId;

                    destAlt.StaffMemberId = destStaffId;
                    destAlt.HeadOfSectionId = destHeadOfSectionId;

                    request.StatusId = "ACCPTD";
                }
                else
                {
                    request.StatusId = "DCLND";
                }

            }
            _db.SaveChanges();
            return new DataWithError(TransformToDto(request.Id, user), "");


        }
        public DataWithError GetInvolvedRequests(ClaimsPrincipal user)
        {
            try
            {
                var currentSchedule = _db.Schedules.OrderBy(x => x.StartDate).LastOrDefault(s => s.IsPublish);
                if (currentSchedule == null)
                {
                    return new DataWithError(null, "No published schedule found");
                }
                var appUser = _userService.GetUserInfo(user);
                var currentDay = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timezone);
                var result = _db.SwapRequests.Where(x => x.IssueDate >= currentDay.AddDays(-30));
                if(!appUser.Roles.Contains("Hos") && !appUser.Roles.Contains("SuperUser"))
                {
                    result = result.Where(x => x.Details.Any(d => d.InvolvedAlias.ToLower() == appUser.Alias.ToLower()) || x.RequesterAlias.ToLower() == appUser.Alias.ToLower());
                }

                // .Where(x => x.SourceDailyAttendance.ScheduleId == currentSchedule.Id)
                var lastResult = result.Select(model => new SwapRequestDto
                {
                    Id = model.Id,
                    ScheduleName = model.Schedule != null ? model.Schedule.Name : "",
                    RequesterName = model.RequesterName,
                    ResponderName = model.ResponderName,
                    RequestDate = model.IssueDate,
                    RequesterDay = model.SourceDailyAttendance.Day,
                    ResponderDay = model.DestinationDailyAttendance.Day,
                    StatusId = model.StatusId,
                    StatusName = model.Status.Name,
                    CloseDate = model.Details.OrderBy(x => x.IssueDate).LastOrDefault()!.CloseDate,
                    CanAction = !model.Status.IsComplete && (appUser.Roles.Contains("Hos") || appUser.Roles.Contains("SuperUser")),
                    CanReverse = false
                }).ToList();
                return new DataWithError(lastResult, "");
            }
            catch
            {
                return new DataWithError(null, "Something Went Wrong!");
            }
        }
        public DataWithError GetMySchedules(string alias)
        {
            var staff = _db.StaffMembers.FirstOrDefault(x => x.Alias.ToLower() == alias.ToLower());
            if (staff == null)
            {
                return new DataWithError(null, "Staff Not Found");
            }
            var schedules = _db.Schedules.Where(s => s.IsPublish && !(staff.LeaveDate < s.StartDate || staff.StartDate > s.EndDate));
            return new DataWithError(schedules, "");
        }
        public DataWithError GetMyDayOffs(string alias, int scheduleId)
        {
            var staff = _db.StaffMembers.FirstOrDefault(x => x.Alias.ToLower() == alias.ToLower());
            if(staff == null)
            {
                return new DataWithError(null, "Staff Not Found");
            }
            var currentSchedule = _db.Schedules.Find(scheduleId);
            if (currentSchedule == null || !currentSchedule.IsPublish)
            {
                return new DataWithError(null, "No published schedule found");
            }
            var currentDay = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timezone);
            
            //if (currentSchedule.EndDate <= currentDay)
            //{
            //    return new DataWithError(null, "Schedule Ends  earlier than today");
            //}
            var dayOffs = getDayOffsByAliasSchedule(alias, currentDay, scheduleId);
            var result = new DailyStaffDto
            {
                Id = staff.Id,
                EmployeeId = staff.EmployeeId,
                Name = staff.Name,
                DailyAttendances = dayOffs.Select(d => new DailyAttendanceByScheduleIdDto
                {
                    Id = d.Id,
                    ShiftId = (int)d.TransportationRouteId,
                    AttendanceTypeId = d.AttendanceTypeId,
                    TransportationArriveTime = new TimeSpan(),
                    TransportationDepartTime = new TimeSpan(),
                    AttendanceTypeName = "DayOff",
                    Day = d.Day,
                    IsAbsence = true
                }).ToList()
            };
            return new DataWithError(result, "");
        }
        public DataWithError GetSiblings(string alias, int scheduleId)
        {
            var currentSchedule = _db.Schedules.Find(scheduleId);
            if (currentSchedule == null || !currentSchedule.IsPublish)
            {
                return new DataWithError(null, "No published schedule found");
            }
            var staff = _db.StaffMembers.FirstOrDefault(x => x.Alias.ToLower() == alias.ToLower());
            if(staff == null)
            {
                return new DataWithError(null, "Staff Not Found");
            }
            var siblings = _db.StaffMembers.Where(x => x.Alias.ToLower() != alias.ToLower() &&
            !(x.LeaveDate < currentSchedule.StartDate || x.StartDate > currentSchedule.EndDate));
            return new DataWithError(siblings, "");
        }
        public DataWithError GetDestinationDayOffs(int staffId, int scheduleId)
        {
            var staff = _db.StaffMembers.Find(staffId);
            if(staff == null)
            {
                return new DataWithError(null, "Staff Not Found");
            }
            var currentSchedule = _db.Schedules.Find(scheduleId);
            if (currentSchedule == null || !currentSchedule.IsPublish)
            {
                return new DataWithError(null, "No published schedule found");
            }
            var currentDay = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timezone);
            
            //if (currentSchedule.EndDate <= currentDay)
            //{
            //    return new DataWithError(null, "Schedule Ends  earlier than today");
            //}
            var dayOffs = getDayOffsByAliasSchedule(staff.Alias, currentDay, scheduleId);
            return new DataWithError(dayOffs, "");
        }
        private List<DailyAttendance> getDayOffsByAliasSchedule(string alias, DateTime currentDay, int scheduleId)
        {
            var dayOffs = _db.DailyAttendances
                .Where(d => d.Schedule.IsPublish && d.ScheduleId == scheduleId && d.StaffMember.Alias.ToLower() == alias.ToLower() && d.AttendanceType.IsAbsence)
                .OrderBy(d => d.Day);
            return dayOffs.ToList();
        }
        private SwapRequestDto TransformToDto(int swapRequestId, ClaimsPrincipal user)
        {
            var appUser = _userService.GetUserInfo(user);
            var result = _db.SwapRequests.Select(request => new SwapRequestDto
            {
                Id = request.Id,
                ScheduleName = request.Schedule != null ? request.Schedule.Name : "",
                RequesterName = request.RequesterName,
                ResponderName = request.ResponderName,
                RequestDate = request.IssueDate,
                RequesterDay = request.SourceDailyAttendance.Day,
                ResponderDay = request.DestinationDailyAttendance.Day,
                StatusId = request.StatusId,
                StatusName = request.Status.Name,
                CloseDate = request.Details.OrderBy(x => x.IssueDate).LastOrDefault()!.CloseDate,
                CanAction = !request.Status.IsComplete && (appUser.Roles.Contains("Hos") || appUser.Roles.Contains("SuperUser")),
                CanReverse = false
        }).FirstOrDefault(x => x.Id == swapRequestId);
            return result;
        }
        //public DataWithError ReverseSwapRequest(int requestId, string alias)
        //{
        //    var request = _db.SwapRequests
        //        .Include(x => x.Details)
        //        .Include(x => x.Status)
        //        .FirstOrDefault(x => x.Id == requestId);
        //    if (request == null)
        //    {
        //        return new DataWithError(null, "Swap request is not exist!");
        //    }
        //    if(request.StatusId != "ACCPTD")
        //    {
        //        return new DataWithError(null, "Request is not Accepted!");
        //    }
        //    _db.Entry(request).Reference(x => x.SourceDailyAttendance).Load();
        //    _db.Entry(request).Reference(x => x.DestinationDailyAttendance).Load();
        //    var sourceStaffId = request.DestinationDailyAttendance.StaffMemberId;
        //    var sourceHeadOfSectionId = request.DestinationDailyAttendance.HeadOfSectionId;
        //    var destStaffId = request.SourceDailyAttendance.StaffMemberId;
        //    var destHeadOfSectionId = request.SourceDailyAttendance.HeadOfSectionId;
        //    var sourceAlt = _db.DailyAttendances
        //        .FirstOrDefault(x => x.Day == request.DestinationDailyAttendance.Day && x.StaffMemberId == request.SourceDailyAttendance.StaffMemberId);
        //    if (sourceAlt == null)
        //    {
        //        return new DataWithError(null, "Swap request cannot be reversed!");
        //    }
        //    var destAlt = _db.DailyAttendances
        //        .FirstOrDefault(x => x.Day == request.SourceDailyAttendance.Day && x.StaffMemberId == sourceStaffId);
        //    if (destAlt == null)
        //    {
        //        return new DataWithError(null, "Swap request cannot be reversed!");
        //    }


        //    request.SourceDailyAttendance.StaffMemberId = destStaffId;
        //    request.SourceDailyAttendance.HeadOfSectionId = destHeadOfSectionId;
        //    request.DestinationDailyAttendance.StaffMemberId = sourceStaffId;
        //    request.DestinationDailyAttendance.HeadOfSectionId = sourceHeadOfSectionId;

        //    try
        //    {
        //        _db.SaveChanges();
        //    }
        //    catch
        //    {
        //        return new DataWithError(null, "Requester Day-Off has the same Responder Day-Off. Please Decline!");
        //    }




        //    sourceAlt.StaffMemberId = sourceStaffId;

        //    destAlt.StaffMemberId = destStaffId;

        //    request.StatusId = "RVRSD";
        //    _db.SaveChanges();
        //    return new DataWithError(TransformToDto(request.Id, alias), "");
        //}
    }
}
