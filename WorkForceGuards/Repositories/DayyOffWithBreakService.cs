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
    public class DayyOffWithBreakService : IDayyOffWithBreakService
    {

        private readonly ApplicationDbContext db;
        public DayyOffWithBreakService(ApplicationDbContext context)
        {
            db = context;
        }
        public List<DayOffWithBreakDto> GetAll()
        {
            var schedule = db.Schedules.FirstOrDefault(x => !x.IsPublish);
            if (schedule == null)
            {
                return null;
            }
            var staffMembers = db.StaffMembers
                .Include(x => x.BreakTypeOption)
                .ThenInclude(x => x.AttendanceType)
                .Include(x => x.BreakTypeOption)
                .ThenInclude(x => x.TransportationRoute)
                .Include(x => x.DayOffOptions.Where(y => y.ScheduleId == schedule.Id))
                .Where(x => x.LeaveDate >= schedule.StartDate && x.StartDate <= schedule.EndDate).ToList();
            var result = new List<DayOffWithBreakDto>();
            foreach (var staffMemberInfo in staffMembers.ToList())
            {
                var breakTypeOption = db.breakTypeOptions.Include(x => x.Sublocation).Include(x => x.TransportationRoute).Include(x => x.AttendanceType).FirstOrDefault(x => x.ScheduleId == schedule.Id && x.StaffMemberId == staffMemberInfo.Id);
                var lastBreakTypeOption = db.breakTypeOptions.Include(x => x.Sublocation).Include(x => x.TransportationRoute).Include(x => x.Schedule).OrderByDescending(x => x.ScheduleId).FirstOrDefault(x => x.StaffMemberId == staffMemberInfo.Id && x.Schedule.IsPublish);
                var lastShift = lastBreakTypeOption != null ? lastBreakTypeOption.TransportationRoute : null;
                var shift = breakTypeOption != null && breakTypeOption.TransportationRoute != null ? breakTypeOption.TransportationRoute : (lastShift != null ? lastShift : null);
                var attendanceType = breakTypeOption != null ? breakTypeOption.AttendanceType : null;
                result.Add(
                    new DayOffWithBreakDto(staffMemberInfo.Name,
                    staffMemberInfo.Id,
                schedule.Name, schedule.Id,
                attendanceType != null ? attendanceType.Name : null,
                attendanceType != null ? attendanceType.Id : 0,
                shift != null ? shift.Name : null,
                shift != null ? shift.Id : 0,
                breakTypeOption.SublocationId,
                breakTypeOption.Sublocation.Name,
                staffMemberInfo.DayOffOptions != null && staffMemberInfo.DayOffOptions.Count >= 3 ? new DayOptionViewModel(staffMemberInfo.DayOffOptions.ToList()[0]) : null,
                staffMemberInfo.DayOffOptions != null && staffMemberInfo.DayOffOptions.Count >= 3 ? new DayOptionViewModel(staffMemberInfo.DayOffOptions.ToList()[1]) : null,
                staffMemberInfo.DayOffOptions != null && staffMemberInfo.DayOffOptions.Count >= 3 ? new DayOptionViewModel(staffMemberInfo.DayOffOptions.ToList()[2]) : null,
                staffMemberInfo.DayOffOptions != null && staffMemberInfo.DayOffOptions.Count > 3 ? new DayOptionViewModel(staffMemberInfo.DayOffOptions.ToList()[3]) :
                (staffMemberInfo.DayOffOptions != null && staffMemberInfo.DayOffOptions.Count == 1 ? new DayOptionViewModel(staffMemberInfo.DayOffOptions.ToList()[0]) : null)));
            }
            return result;




        }
        public DataWithError GetById(int empid)
        {
            // var staffMemberInfo = db.StaffMembers.Include(s => s.BreakTypeOption).FirstOrDefault(x => x.Id == empid);
            var schedulee = db.Schedules.OrderBy(x => x.Id).LastOrDefault(s => !s.IsPublish);
            if (schedulee == null)
            {
                return new DataWithError(null, "There is no unpublished schedule!");
            }
            return A_GetByStaffSchedule(empid, schedulee.Id);
            
        }
        public DataWithError GetByAlias(string alias)
        {
            var schedulee = db.Schedules.OrderBy(x => x.Id).LastOrDefault(s => !s.IsPublish);
            if(schedulee == null)
            {
                return new DataWithError(null, "There is no unpublished schedule!");
            }
            var staffMember = db.StaffMembers.FirstOrDefault(x => x.Alias == alias);
            if(staffMember == null)
            {
                return new DataWithError(null, "Staff Member Not Found!");
            }
            return A_GetByStaffSchedule(staffMember.Id, schedulee.Id);

        }
        public DataWithError A_GetByStaffSchedule(int staffId, int scheduleId)
        {
            var schedulee = db.Schedules.Find(scheduleId);
            var staffMemberInfo = db.StaffMembers
                    .Include(x => x.BreakTypeOption)
                    .ThenInclude(x => x.AttendanceType)
                    .Include(x => x.BreakTypeOption)
                    .ThenInclude(x => x.TransportationRoute)
                    .Include(x => x.DayOffOptions)
                    .FirstOrDefault(x => x.Id == staffId && !(x.LeaveDate < schedulee.StartDate || x.StartDate > schedulee.EndDate));
            if (staffMemberInfo == null)
            {
                return new DataWithError(null, "Staff Member Id Not Found");
            }
            DayOffWithBreakDto data;
            var schedule = db.Schedules.OrderBy(x => x.Id).LastOrDefault(s => !s.IsPublish);
            staffMemberInfo.BreakTypeOption = db.breakTypeOptions.Include(x => x.Sublocation).Include(x => x.TransportationRoute).Include(x => x.AttendanceType).Where(x => x.ScheduleId == schedule.Id && x.StaffMemberId == staffId).ToList();
            if(staffMemberInfo.BreakTypeOption.Count==0)
            {
                staffMemberInfo.BreakTypeOption = db.breakTypeOptions.Include(x => x.Sublocation).Include(x => x.TransportationRoute).Include(x => x.Schedule).OrderByDescending(x => x.ScheduleId).Where(x => x.StaffMemberId == staffId && x.Schedule.IsPublish==true).ToList();
                    
            }
            staffMemberInfo.DayOffOptions = db.dayOffOptions.Where(x => x.ScheduleId == schedule.Id && x.StaffMemberId == staffId).ToList();
            var attendanceType = staffMemberInfo.BreakTypeOption.Count != 0 ? staffMemberInfo.BreakTypeOption.ToList()[0].AttendanceType:null;
            var shift= staffMemberInfo.BreakTypeOption.Count != 0 ? staffMemberInfo.BreakTypeOption.ToList()[0].TransportationRoute : null;

            //var breakTypeOption = db.breakTypeOptions.Include(x => x.Shift).Include(x => x.AttendanceType).FirstOrDefault(x => x.ScheduleId == schedule.Id && x.StaffMemberId == staffId);
            //var lastBreakTypeOption = db.breakTypeOptions.Include(x => x.Shift).Include(x => x.Schedule).OrderByDescending(x => x.ScheduleId).FirstOrDefault(x => x.StaffMemberId == staffId && x.Schedule.IsPublish);
            //var lastShift = lastBreakTypeOption != null ? lastBreakTypeOption.Shift : null;
            //var shift = breakTypeOption != null ? (breakTypeOption.Shift ?? lastShift) : null;
            //var dayOffOptions = staffMemberInfo.DayOffOptions;
            //var attendanceType = breakTypeOption != null ? breakTypeOption.AttendanceType : null;

            data = new DayOffWithBreakDto(staffMemberInfo.Name, staffMemberInfo.Id,
                schedule.Name, schedule.Id,
                attendanceType != null ? attendanceType.Name : null,
                attendanceType != null ? attendanceType.Id : 0,
                shift != null ? shift.Name : null,
                shift != null ? shift.Id : 0,
                0,"Any",
                staffMemberInfo.DayOffOptions != null && staffMemberInfo.DayOffOptions.Count >= 3 ? new DayOptionViewModel(staffMemberInfo.DayOffOptions.ToList()[0]) : null,
                staffMemberInfo.DayOffOptions != null && staffMemberInfo.DayOffOptions.Count >= 3 ? new DayOptionViewModel(staffMemberInfo.DayOffOptions.ToList()[1]) : null,
                staffMemberInfo.DayOffOptions != null && staffMemberInfo.DayOffOptions.Count >= 3 ? new DayOptionViewModel(staffMemberInfo.DayOffOptions.ToList()[2]) : null,
                staffMemberInfo.DayOffOptions != null && staffMemberInfo.DayOffOptions.Count > 3 ? new DayOptionViewModel(staffMemberInfo.DayOffOptions.ToList()[3]) :
                (staffMemberInfo.DayOffOptions != null && staffMemberInfo.DayOffOptions.Count == 1 ? new DayOptionViewModel(staffMemberInfo.DayOffOptions.ToList()[0]) : null));
            return new DataWithError(data, null);
        }
        public DataWithError Add(DayOffWithBreakOptionBinding model)
        {
            DataWithError result = new DataWithError();
            var Schedule = db.Schedules.OrderBy(x => x.Id).LastOrDefault(s => !s.IsPublish);
            //var DayOptions = (DayOffWithBreakDto)A_GetByStaffSchedule(model.StaffMemberId, model.ScheduleId).Result;
            var dayOptions = db.dayOffOptions.Where(x => x.ScheduleId == Schedule.Id && x.StaffMemberId == model.StaffMemberId).ToList();
            var isApproved = dayOptions.FirstOrDefault(x => x.IsApproved);
            if(isApproved != null)
            {
                result.Result = null;
                result.ErrorMessage = "Dayoff is approved ";
                return result;

            }
      
                db.dayOffOptions.RemoveRange(dayOptions);

            var breaks=db.breakTypeOptions.Where(x => x.ScheduleId == Schedule.Id && x.StaffMemberId == model.StaffMemberId).ToList();
            if(breaks !=null)
            {
                db.breakTypeOptions.RemoveRange(breaks);
            }

             db.SaveChanges();

            var staffLeaveDate = db.StaffMembers.FirstOrDefault(x => x.Id == model.StaffMemberId).LeaveDate;
            if (staffLeaveDate > Schedule.StartDate)
            {
                var dayoff = db.dayOffOptions.Where(x => x.StaffMemberId == model.StaffMemberId && x.ScheduleId == Schedule.Id).FirstOrDefault();
                if (dayoff == null)
                {


                    var status = CheckDayOffOptions(model);
                    if (status == true)
                    {
                        DayOffOption dayOptions1 = new DayOffOption();
                        DayOffOption dayOptions2 = new DayOffOption();
                        DayOffOption dayOptions3 = new DayOffOption();
                        BreakTypeOption breakOption = new BreakTypeOption();


                        var staffInfo = db.StaffMembers.FirstOrDefault(x => x.Id == model.StaffMemberId);
                        var scheduleInfo = db.Schedules.FirstOrDefault(x => x.IsPublish == false);

                        var attendenceTypeInfo = db.AttendanceTypes.FirstOrDefault(x => x.Id == model.AttendenceTypeId);

                        if (staffInfo != null && scheduleInfo != null && attendenceTypeInfo != null)
                        {

                            breakOption.StaffMemberId = model.StaffMemberId;
                            breakOption.ScheduleId = scheduleInfo.Id;
                            breakOption.AttendenceTypeId = model.AttendenceTypeId.Value;

                            db.breakTypeOptions.Add(breakOption);



                            dayOptions1.StaffMemberId = model.StaffMemberId;
                            dayOptions1.ScheduleId = scheduleInfo.Id;
                            dayOptions1.DayOne = model.DayOption1.Days[0];
                            dayOptions1.DayTwo = model.DayOption1.Days[1];
                            db.dayOffOptions.Add(dayOptions1);

                            dayOptions2.StaffMemberId = model.StaffMemberId;
                            dayOptions2.ScheduleId = scheduleInfo.Id;
                            dayOptions2.DayOne = model.DayOption2.Days[0];
                            dayOptions2.DayTwo = model.DayOption2.Days[1];
                            db.dayOffOptions.Add(dayOptions2);

                            dayOptions3.StaffMemberId = model.StaffMemberId;
                            dayOptions3.ScheduleId = scheduleInfo.Id;
                            dayOptions3.DayOne = model.DayOption3.Days[0];
                            dayOptions3.DayTwo = model.DayOption3.Days[1];
                            db.dayOffOptions.Add(dayOptions3);
                            db.SaveChanges();

                            result.Result = model;
                            result.ErrorMessage = null;
                            return result;
                        }

                        result.Result = null;
                        result.ErrorMessage = "Check on Foreign keys ScheduleId or AttendenceType Id or StaffMemberId is not found";
                        return result;
                    }


                    result.Result = null;
                    result.ErrorMessage = "Duplicate in your Day-Off options...please check your options.";
                    return result;

                }
                result.Result = null;
                result.ErrorMessage = "your options has been selected ";
                return result;


            }
            result.Result = null;
            result.ErrorMessage = "Staff is not found in this schedule ";
            return result;
        }
        public DayOffWithBreakOptionBinding GetStaff(int id)
        {
            DayOffWithBreakOptionBinding data = new DayOffWithBreakOptionBinding();

            var staffMemberInfo = db.StaffMembers.FirstOrDefault(x => x.Id == id);
            var schedule = db.Schedules.OrderBy(x => x.Id).LastOrDefault(s => !s.IsPublish);

            if (staffMemberInfo != null && staffMemberInfo.LeaveDate > schedule.StartDate)
            {
                /// must add ispublish==true 
                var LastSchedule = db.Schedules.OrderBy(x => x.Id).LastOrDefault(s => s.IsPublish==true);
                var breakTypeOption = db.breakTypeOptions.FirstOrDefault(x => x.ScheduleId == LastSchedule.Id && x.StaffMemberId == id);
                var StaffDayOffOptions = db.dayOffOptions.Where(x => x.ScheduleId == LastSchedule.Id && x.StaffMemberId == id).
                                            Select(y => new { y.DayOne, y.DayTwo, y.IsApproved }).ToList();
                if (StaffDayOffOptions.Count == 3)
                {
                    data.StaffMemberId = id;
                    data.ScheduleId = LastSchedule.Id;
                    data.AttendenceTypeId = breakTypeOption.AttendenceTypeId;
                    data.ShiftId = breakTypeOption.TransportationRouteId;
                    data.DayOption1.IsApproved = StaffDayOffOptions[0].IsApproved;
                    data.DayOption1.Days.Add(StaffDayOffOptions[0].DayOne);
                    data.DayOption1.Days.Add(StaffDayOffOptions[0].DayTwo);
                    data.DayOption2.IsApproved = StaffDayOffOptions[1].IsApproved;
                    data.DayOption2.Days.Add(StaffDayOffOptions[1].DayOne);
                    data.DayOption2.Days.Add(StaffDayOffOptions[1].DayTwo);
                    data.DayOption3.IsApproved = StaffDayOffOptions[2].IsApproved;
                    data.DayOption3.Days.Add(StaffDayOffOptions[2].DayOne);
                    data.DayOption3.Days.Add(StaffDayOffOptions[2].DayTwo);
                    return data;
                }
                data.StaffMemberId = id;
                data.ScheduleId = LastSchedule.Id;
                data.AttendenceTypeId = breakTypeOption?.AttendenceTypeId;
                data.ShiftId = breakTypeOption.TransportationRouteId;
                data.DayOption1.IsApproved = StaffDayOffOptions[0].IsApproved;
                data.DayOption1.Days.Add(StaffDayOffOptions[0].DayOne);
                data.DayOption1.Days.Add(StaffDayOffOptions[0].DayTwo);
                data.DayOption2.IsApproved = StaffDayOffOptions[1].IsApproved;
                data.DayOption2.Days.Add(StaffDayOffOptions[1].DayOne);
                data.DayOption2.Days.Add(StaffDayOffOptions[1].DayTwo);
                data.DayOption3.IsApproved = StaffDayOffOptions[2].IsApproved;
                data.DayOption3.Days.Add(StaffDayOffOptions[2].DayOne);
                data.DayOption3.Days.Add(StaffDayOffOptions[2].DayTwo);

                data.DayOption4.IsApproved = StaffDayOffOptions[3].IsApproved;
                data.DayOption4.Days.Add(StaffDayOffOptions[3].DayOne);
                data.DayOption4.Days.Add(StaffDayOffOptions[3].DayTwo);
                return data;

            }

            return null;

        }
        public DataWithError Edit(DayOffWithBreakOptionBinding model)
        {
            DataWithError result = new DataWithError();
            var Schedule = db.Schedules.OrderBy(x => x.Id).LastOrDefault(s => !s.IsPublish);
            var DayOptions = (DayOffWithBreakDto)A_GetByStaffSchedule(model.StaffMemberId, model.ScheduleId).Result;
            if (Schedule != null && DayOptions != null)
            {
                if(Schedule.IsPublish)
                {
                    return new DataWithError(null, "Schedule is published!");
                }
                if(DayOptions.DayOption1?.IsApproved == true || DayOptions.DayOption2?.IsApproved == true || DayOptions.DayOption3?.IsApproved == true || DayOptions.DayOption4?.IsApproved == true)
                {
                    return new DataWithError(null, "Options are approved!");
                }
                
                    var breakOption = db.breakTypeOptions.FirstOrDefault(x => x.ScheduleId == Schedule.Id && x.StaffMemberId == model.StaffMemberId);
                    var dayOptions = db.dayOffOptions.Where(x => x.ScheduleId == Schedule.Id && x.StaffMemberId == model.StaffMemberId).ToList();
                    db.breakTypeOptions.Remove(breakOption);
                    db.dayOffOptions.RemoveRange(dayOptions);
                    var data = Add(model);
                    result.Result = data;
                    result.ErrorMessage = null;
                    return result;
            }
            result.Result = null;
            result.ErrorMessage = "Schedule or Day options not found for this employee ";
            return result;



        }
        public DataWithError ApprovedOption(DayOffApprovalBinding approval)
        {

            DayOffOption day = new DayOffOption();
            BreakTypeOption breakOpp = new BreakTypeOption();
            DataWithError data = new DataWithError();
            var schedule = db.Schedules.OrderBy(x => x.Id).LastOrDefault(s => !s.IsPublish);
            var staffMember = db.StaffMembers.FirstOrDefault(x => x.Id == approval.StaffMemberId && x.LeaveDate > schedule.StartDate);
            var breakOption = db.breakTypeOptions.FirstOrDefault(x => x.StaffMemberId == approval.StaffMemberId && x.ScheduleId == schedule.Id);
            // var dayOff = db.dayOffOptions.FirstOrDefault(x=>x.StaffMemberId==approval.StaffMemberId && x.ScheduleId==schedule.Id);


            if (staffMember != null && schedule != null && breakOption != null && breakOption.IsApproved == false)
            {
                day.StaffMemberId = approval.StaffMemberId;
                day.ScheduleId = schedule.Id;
                day.DayOne = approval.AccepedDayOffInfo.Day1;
                day.DayTwo = approval.AccepedDayOffInfo.Day2;
                day.IsApproved = true;

                if (approval.AccepedDayOffInfo.Id == 0)
                {
                    day.IsAdmin = true;
                    db.dayOffOptions.Add(day);
                }

                else
                {
                    day.Id = approval.AccepedDayOffInfo.Id;
                    db.Entry(day).State = EntityState.Modified;
                }


                breakOption.StaffMemberId = approval.StaffMemberId;
                breakOption.ScheduleId = schedule.Id;
                breakOption.AttendenceTypeId = approval.AcceptedBreakTypeOption;
                breakOption.IsAdmin = true;
                breakOption.IsApproved = true;
                breakOption.TransportationRouteId = approval.ShiftId;

                db.Entry(breakOption).State = EntityState.Modified;
                db.SaveChanges();

                var GResult = GetById(approval.StaffMemberId);
                if (GResult.ErrorMessage == null)
                {
                    data.Result = GResult.Result;
                    data.ErrorMessage = null;
                    return data;
                }

                data.Result = null;
                data.ErrorMessage = "This staff member not found";
                return data;

            }

            //staff member has not day off and brfeak

            else if (breakOption == null)
            {
                day.StaffMemberId = approval.StaffMemberId;

                day.ScheduleId = schedule.Id;
                day.DayOne = approval.AccepedDayOffInfo.Day1;
                day.DayTwo = approval.AccepedDayOffInfo.Day2;
                day.IsApproved = true;

                if (approval.AccepedDayOffInfo.Id == 0)
                {
                    day.IsAdmin = true;
                    db.dayOffOptions.Add(day);
                }

                else
                {
                    day.Id = approval.AccepedDayOffInfo.Id;
                    db.Entry(day).State = EntityState.Modified;
                }




                breakOpp.StaffMemberId = approval.StaffMemberId;
                breakOpp.ScheduleId = schedule.Id;
                breakOpp.AttendenceTypeId = approval.AcceptedBreakTypeOption;
                breakOpp.IsAdmin = true;
                breakOpp.IsApproved = true;
                breakOpp.TransportationRouteId = approval.ShiftId;

                db.breakTypeOptions.Add(breakOpp);
                db.SaveChanges();

                var GResult = GetById(approval.StaffMemberId);
                if (GResult.ErrorMessage == null)
                {
                    data.Result = GResult.Result;
                    data.ErrorMessage = null;
                    return data;
                }

                data.Result = null;
                data.ErrorMessage = " staff member is  not found";
                return data;
            }


            data.Result = null;
            data.ErrorMessage = "SchduleId or StaffMemberId or break option not found or approved";

            return data;


        }
        public DataWithError A_ApprovedOption(DayOffApprovalBinding model)
        {
            var schedule = db.Schedules.Find(model.ScheduleId);
            if (schedule == null)
            {
                return new DataWithError(null, "Schedule not found");
            }
            if (schedule.IsPublish)
            {
                return new DataWithError(null, "Schedule is published");
            }
            var staffBreakOption = db.breakTypeOptions.FirstOrDefault(x => x.ScheduleId == schedule.Id && x.StaffMemberId == model.StaffMemberId);
            if (staffBreakOption == null)
            {
                db.breakTypeOptions.Add(new BreakTypeOption(model));
            }
            else
            {
                staffBreakOption.AttendenceTypeId = model.AcceptedBreakTypeOption;
                staffBreakOption.TransportationRouteId = model.ShiftId;
                staffBreakOption.IsApproved = true;
                staffBreakOption.IsAdmin = true;
                db.Entry(staffBreakOption).State = EntityState.Modified;
            }
            var staffDayOffOptions = db.dayOffOptions.Where(x => x.StaffMemberId == model.StaffMemberId && x.ScheduleId == model.ScheduleId);
            foreach (var dayOffOption in staffDayOffOptions)
            {
                dayOffOption.IsApproved = false;
            }
            if (model.AccepedDayOffInfo.Id == 0)
            {
                db.dayOffOptions.RemoveRange(db.dayOffOptions
                                             .Where(x => x.StaffMemberId == model.StaffMemberId && x.ScheduleId == model.ScheduleId && x.IsAdmin == true)
                                             .ToList());
                db.dayOffOptions.Add(new DayOffOption(model));
            }
            else
            {
                var staffDayOffOption = db.dayOffOptions.Find(model.AccepedDayOffInfo.Id);
                if (staffDayOffOption == null)
                {
                    return new DataWithError(null, "Dayoff option not found");
                }
                staffDayOffOption.IsApproved = true;
                db.Entry(staffDayOffOption).State = EntityState.Modified;
            }
            db.SaveChanges();
            var result = A_GetByStaffSchedule(model.StaffMemberId, model.ScheduleId).Result;
            return new DataWithError(result, null);
        }
        public bool SetDailyAttendence(DayOffApprovalBinding model)
        {

            List<DailyAttendance> AttendeceData = new List<DailyAttendance>();
            DailyAttendance daily;
            var scheduleInfo = db.Schedules.FirstOrDefault(x => x.Id == model.ScheduleId);
            var ApprovalDayOff = db.dayOffOptions.FirstOrDefault(x => x.ScheduleId == model.ScheduleId && x.StaffMemberId == model.StaffMemberId && x.IsApproved == true);
            var ApprovalBraekOption = db.breakTypeOptions.FirstOrDefault(x => x.ScheduleId == model.ScheduleId && x.StaffMemberId == model.StaffMemberId && x.IsApproved == true);
            var dailyAttendenceInfo = db.DailyAttendances.FirstOrDefault(x => x.ScheduleId == model.ScheduleId && x.StaffMemberId == model.StaffMemberId && x.TransportationRouteId == model.ShiftId);
            var staffMemberInfo = db.StaffMembers.FirstOrDefault(x => x.Id == model.StaffMemberId);
            if (scheduleInfo != null && dailyAttendenceInfo == null)
            {
                if (staffMemberInfo.StartDate < scheduleInfo.StartDate && staffMemberInfo.LeaveDate > scheduleInfo.EndDate)
                {
                    for (var d = scheduleInfo.StartDate; d <= scheduleInfo.EndDate; d = d.AddDays(1))
                    {
                        if (d.DayOfWeek.ToString().ToUpper() == ApprovalDayOff.DayOne.ToString().ToUpper() || d.DayOfWeek.ToString().ToUpper() == ApprovalDayOff.DayTwo.ToUpper())
                        {
                            daily = new DailyAttendance(model.StaffMemberId, model.ScheduleId, d, 6, model.ShiftId, staffMemberInfo.HeadOfSectionId);

                        }
                        else
                        {
                            daily = new DailyAttendance(model.StaffMemberId, model.ScheduleId, d, ApprovalBraekOption.AttendenceTypeId, model.ShiftId, staffMemberInfo.HeadOfSectionId);


                        }

                        AttendeceData.Add(daily);


                    }
                }

                else if (staffMemberInfo.StartDate < scheduleInfo.StartDate && staffMemberInfo.LeaveDate < scheduleInfo.EndDate)
                {
                    for (var d = scheduleInfo.StartDate; d <= staffMemberInfo.LeaveDate; d = d.AddDays(1))
                    {
                        if (d.DayOfWeek.ToString().ToUpper() == ApprovalDayOff.DayOne.ToString().ToUpper() || d.DayOfWeek.ToString().ToUpper() == ApprovalDayOff.DayTwo.ToUpper())
                        {
                            daily = new DailyAttendance(model.StaffMemberId, model.ScheduleId, d, 6, model.ShiftId, staffMemberInfo.HeadOfSectionId);

                        }
                        else
                        {
                            daily = new DailyAttendance(model.StaffMemberId, model.ScheduleId, d, ApprovalBraekOption.AttendenceTypeId, model.ShiftId, staffMemberInfo.HeadOfSectionId);


                        }

                        AttendeceData.Add(daily);
                    }
                }


                else
                {
                    for (var d = staffMemberInfo.StartDate; d <= scheduleInfo.EndDate; d = d.AddDays(1))
                    {
                        if (d.DayOfWeek.ToString().ToUpper() == ApprovalDayOff.DayOne.ToString().ToUpper() || d.DayOfWeek.ToString().ToUpper() == ApprovalDayOff.DayTwo.ToUpper())
                        {
                            daily = new DailyAttendance(model.StaffMemberId, model.ScheduleId, d, 6, model.ShiftId, staffMemberInfo.HeadOfSectionId);

                        }
                        else
                        {
                            daily = new DailyAttendance(model.StaffMemberId, model.ScheduleId, d, ApprovalBraekOption.AttendenceTypeId, model.ShiftId, staffMemberInfo.HeadOfSectionId);


                        }

                        AttendeceData.Add(daily);
                    }



                }
                db.DailyAttendances.AddRange(AttendeceData);
                db.SaveChanges();

                return true;
            }

            return false;

        }
        public bool setForeCasting(int ScheduleId)
        {

            List<ForeCasting> data = new List<ForeCasting>();
            ForeCasting setdata;
            var scheduleinfo = db.Schedules.FirstOrDefault(x => x.Id == ScheduleId);
            var intervalinfo = db.Intervals;
            Random rnd = new Random();
            var day = scheduleinfo.StartDate;
            for (var q = intervalinfo.Select(x => x.Id).Min(); q <= intervalinfo.Select(x => x.Id).Max(); q = q + 1)
            //for (var d=scheduleinfo.StartDate; d < scheduleinfo.EndDate; d= d.AddMinutes(15))
            {
                //var day = scheduleinfo.StartDate.AddDays(1);
                //string hourMinute = d.ToString("HH:mm");
                //TimeSpan time = TimeSpan.Parse(hourMinute);
                //var datainterval = intervalinfo.FirstOrDefault(x => x.TimeMap == time);

                setdata = new ForeCasting(ScheduleId, day, q, rnd.Next(1, 5));

                data.Add(setdata);



            }
            db.ForeCastings.AddRange(data);
            db.SaveChanges();


            return true;
        }
        public bool setForecastDetails(int ForecastId)
        {

            List<ForecastDetails> data = new List<ForecastDetails>();
            ForecastDetails setdata;
            List<ForecastDetails> finaldata = new List<ForecastDetails>();
            //var scheduleinfo = db.Schedules.FirstOrDefault(x => x.Id == ScheduleId);
            var intervalinfo = db.Intervals;
            var Forecast = db.Forecasts.FirstOrDefault(x => x.Id == ForecastId);
            Random rnd = new Random();

            for (var day = Forecast.StartDate; day <= Forecast.EndDate; day = day.AddDays(1))
            {
                for (var q = intervalinfo.Select(x => x.Id).Min(); q < 97/*q <= intervalinfo.Select(x => x.Id).Max()*/; q = q + 1)
                {
                    var DayoffWeek = day.DayOfWeek.ToString();

                    setdata = new ForecastDetails(ForecastId, DayoffWeek, q, rnd.Next(1, 5));
                    data.Add(setdata);

                }
                for (var q = 97; q <= intervalinfo.Select(x => x.Id).Max(); q = q + 1)
                {
                    var DayoffWeek = day.DayOfWeek.ToString();
                    int value = q - 96;
                    foreach (var d in data)
                    {
                        if (value == d.IntervalId)
                        {
                            setdata = new ForecastDetails(d.ForecastId, d.DayoffWeek, q, d.EmployeeCount);
                            data.Add(setdata);
                        }

                    }

                }

            }
            var dayweek = new List<string>() { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };

            foreach (var d in dayweek)
            {
                for (var q = intervalinfo.Select(x => x.Id).Min(); q <= intervalinfo.Select(x => x.Id).Max(); q = q + 1)
                {
                    var final = data.FirstOrDefault(x => x.DayoffWeek == d && x.IntervalId == q);

                    finaldata.Add(final);
                }
            }

            db.ForecastDetails.AddRange(finaldata);
            db.SaveChanges();

            return true;
        }
        public bool setForecasthistory(int ForecastId)
        {
            var forecastdata = db.Forecasts.Where(x => x.Id == ForecastId);

            return true;
        }
        private List<string> dictionaryday(int ForecastId)
        {
            var forecastdata = db.Forecasts.Where(x => x.Id == ForecastId);
            var start = forecastdata.Select(x => x.StartDate);
            var END = forecastdata.Select(X => X.EndDate);

            var Days = new List<string>();
            return Days;
        }
        public bool CheckStaffOptions()
        {
            var staffMember = db.StaffMembers.Select(x => x.Id).ToList();
            var StaffDayOff = db.dayOffOptions.Select(x => x.StaffMemberId).Distinct().ToList();

            if (staffMember.Count == StaffDayOff.Count)
            {
                foreach (var s in staffMember)
                {
                    var approvedOption = db.dayOffOptions.FirstOrDefault(x => x.StaffMemberId == s && x.IsApproved == true);

                    if (approvedOption == null)
                    {
                        return false;
                    }
                }

                return true;
            }
            return false;

        }
        public DataWithError CreateDailyAttendence()
        {
            var LastSchedule = db.Schedules.OrderByDescending(x => x.Id).FirstOrDefault(x => !x.IsPublish);
            if (db.DailyAttendances.Count() > 0)
            {
                db.DailyAttendances.RemoveRange(db.DailyAttendances.Where(x => x.ScheduleId == LastSchedule.Id));
            }
            DataWithError data = new DataWithError();
            DayOffApprovalBinding binding = new DayOffApprovalBinding();
            if (LastSchedule == null)
            {
                return new DataWithError(null, "No New Schedules");
            }
            if (CheckStaffOptions() == true)
            {
                var StaffMembers = db.breakTypeOptions.Where(x => x.ScheduleId == LastSchedule.Id).ToList();
                foreach (var s in StaffMembers)
                {
                    binding.ShiftId = (int)s.TransportationRouteId;
                    binding.ScheduleId = LastSchedule.Id;
                    binding.StaffMemberId = s.StaffMemberId;
                    var dailyStatus = SetDailyAttendence(binding);
                    if (dailyStatus == false)
                    {

                        data.Result = null;
                        data.ErrorMessage = "Daily Attendance was created";
                        return data;
                    }
                }

                var DailyAttendence = db.DailyAttendances.FirstOrDefault(x => x.ScheduleId == LastSchedule.Id);

                data.Result = true;
                data.ErrorMessage = null;
                return data;
            }

            data.Result = null;
            data.ErrorMessage = "There are some staff Member have not day off options or some staff Members have not approved options";
            return data;
        }
        public List<AttendanceType> GetAttendanceType()
        {
            return db.AttendanceTypes.ToList();
        }
        public bool CheckDayOffOptions(DayOffWithBreakOptionBinding model)
        {
            var DicDay = GetDaysOfWeek();

            List<List<int>> Final = new List<List<int>>();
            List<List<string>> mainList = new List<List<string>>() { model.DayOption1.Days, model.DayOption2.Days, model.DayOption3.Days };


            foreach (var s in mainList)
            {
                List<int> DayNumber = new List<int>();

                for (int i = 0; i <= s.Count - 1; i++)
                {
                    var result = DicDay.Where(pair => pair.Value.ToLower() == s[i].ToLower())
                         .Select(pair => pair.Key);

                    DayNumber.Add(Convert.ToInt32(result.FirstOrDefault()));
                }

                DayNumber.Sort();

                Final.Add(DayNumber);
            }

            var CompareData = Final.Distinct(ListEqualityComparer<int>.Default).ToList();

            if (Final.Count != CompareData.Count)
            {
                return false;
            }


            return true;
        }
        public Dictionary<int, string> GetDaysOfWeek()
        {
            Dictionary<int, string> daysOfWeek = new Dictionary<int, string>();
            for (int i = 1; i <= 7; i++)
            {
                daysOfWeek.Add(i, Enum.GetName(typeof(DayOfWeek), i % 7));
            }

            return daysOfWeek;
        }
        public bool setForecasthistory()
        {
            throw new NotImplementedException();
        }
        public class ListEqualityComparer<T> : IEqualityComparer<List<T>>
        {
            private readonly IEqualityComparer<T> _itemEqualityComparer;

            public ListEqualityComparer() : this(null) { }

            public ListEqualityComparer(IEqualityComparer<T> itemEqualityComparer)
            {
                _itemEqualityComparer = itemEqualityComparer ?? EqualityComparer<T>.Default;
            }

            public static readonly ListEqualityComparer<T> Default = new ListEqualityComparer<T>();

            public bool Equals(List<T> x, List<T> y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null) || ReferenceEquals(y, null)) return false;
                return x.Count == y.Count && !x.Except(y, _itemEqualityComparer).Any();
            }

            public int GetHashCode(List<T> list)
            {
                int hash = 17;
                foreach (var itemHash in list.Select(x => _itemEqualityComparer.GetHashCode(x))
                                             .OrderBy(h => h))
                {
                    hash += 31 * itemHash;
                }
                return hash;
            }
        }
        public DataWithError UploadDayOffWithBreaks(List<DayOffWithBreaksUpload> models)
        {
            var days = new List<string> { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
            
            var schedule = db.Schedules.FirstOrDefault(x => !x.IsPublish);
            var breakOptions = new List<BreakTypeOption>();
            var dayOptions = new List<DayOffOption>();
            if(schedule == null)
            {
                return new DataWithError(null, "No open schedule found!");
            }
            foreach (var model in models)
            {
                if(model.EmployeeId == null)
                {
                    return new DataWithError(null, $"EmployeeId Missing!");
                }
                else
                {
                    if(!int.TryParse(model.EmployeeId, out int employeeId))
                    {
                        return new DataWithError(null, $"EmployeeId Must be Number!");
                    }
                }
                if(models.Where(x => x.EmployeeId == model.EmployeeId).Count() > 1)
                {
                    return new DataWithError(null, $"Duplication found for #{model.EmployeeId}!");
                }
                var staff = db.StaffMembers.FirstOrDefault(s => s.EmployeeId.ToString() == model.EmployeeId);
                if(staff == null)
                {
                    return new DataWithError(null, $"Staff Member: {model.EmployeeId} not found!");
                }
                if (!days.Contains(model.DayOne))
                {
                    return new DataWithError(null, $"{model.DayOne} is not a valid day for ${model.StaffMember}!");
                }
                if (!days.Contains(model.DayTwo))
                {
                    return new DataWithError(null, $"{model.DayTwo} is not a valid day for ${model.StaffMember}!");
                }
                var transportation = db.TransportationRoutes.FirstOrDefault(x => x.Name.ToLower() == model.Transportation.ToLower());
                if(transportation == null)
                {
                    return new DataWithError(null, $"Transportation {model.Transportation} not found for {model.StaffMember}!");
                }
                var attendanceType = db.AttendanceTypes.FirstOrDefault(x => x.Name.ToLower() == model.Attendance.ToLower() && x.Hidden);
                if (attendanceType == null)
                {
                    return new DataWithError(null, $"Attendance Type {model.Attendance} not found for {model.StaffMember}!");
                }
                var breakOption = new BreakTypeOption
                {
                    StaffMemberId = staff.Id,
                    ScheduleId = schedule.Id,
                    TransportationRouteId = transportation.Id,
                    AttendenceTypeId = attendanceType.Id,
                    Id = 0,
                    IsApproved = true,
                    IsAdmin = true
                };
                breakOptions.Add(breakOption);
                var dayOffOption = new DayOffOption
                {
                    Id = 0,
                    ScheduleId = schedule.Id,
                    StaffMemberId = staff.Id,
                    IsApproved = true,
                    IsAdmin = true,
                    DayOne = model.DayOne,
                    DayTwo = model.DayTwo
                };
                dayOptions.Add(dayOffOption);
            }
            db.dayOffOptions.RemoveRange(db.dayOffOptions.Where(d => d.ScheduleId == schedule.Id));
            db.breakTypeOptions.RemoveRange(db.breakTypeOptions.Where(d => d.ScheduleId == schedule.Id));
            db.dayOffOptions.AddRange(dayOptions);
            db.breakTypeOptions.AddRange(breakOptions);
            db.SaveChanges();
            return new DataWithError(true, "");
        }
    }
}
