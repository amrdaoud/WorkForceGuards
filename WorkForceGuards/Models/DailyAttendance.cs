﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WorkForceGuards.Models;
using WorkForceManagementV0.Models.Backup;

namespace WorkForceManagementV0.Models
{
    public class DailyAttendance
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("StaffMember")]
        public int StaffMemberId { get; set; }   
        
        [ForeignKey("Schedule")]
        public int ScheduleId { get; set; }
        public DateTime Day { get; set; }
        [ForeignKey("AttendanceType")]

        public int AttendanceTypeId { get; set; }

        //[ForeignKey("Shift")]
        //public int ShiftId { get; set; }
        [ForeignKey("TransportationRoute")]
        public int? TransportationRouteId { get; set; }
        [ForeignKey("Sublocation")]
        public int? SublocationId { get; set; }
        public virtual SubLocation Sublocation { get; set; }
        [ForeignKey("HeadOfSection")]
        public int HeadOfSectionId { get; set; }

        public virtual StaffMember StaffMember { get; set; }

        public virtual AttendanceType AttendanceType { get; set; }

        public virtual Schedule Schedule { get; set; }

        //public virtual Shift Shift { get; set; }
        public virtual TransportationRoute TransportationRoute { get; set; }
        public virtual HeadOfSection HeadOfSection { get; set; }

        public bool IsDeleted { get; set; }
        public bool HaveBackup { get; set; }

        public virtual ICollection<ScheduleDetail> ScheduleDetails { get; set; }


        public DailyAttendance(int StaffMemberId, int ScheduleId, DateTime Day, int  AttendanceTypeId, int? TransportationRouteId, int HeadOfSectionId)
        {
            this.StaffMemberId = StaffMemberId;
            this.ScheduleId = ScheduleId;
            this.Day = Day;
            this.AttendanceTypeId = AttendanceTypeId;
            this.TransportationRouteId = TransportationRouteId;
            this.HeadOfSectionId = HeadOfSectionId;
            this.HaveBackup = false;
        }

        public DailyAttendance(StaffMember staffMember,
            Schedule schedule,
            DateTime Day,
            AttendanceType attendanceType,
            TransportationRoute transportationRoute,
            int sublocationId,
            int HeadOfSectionId)
        {
            this.StaffMemberId = staffMember.Id;
            this.ScheduleId = schedule.Id;
            this.Day = Day;
            this.AttendanceTypeId = attendanceType.Id;
            this.SublocationId = sublocationId;
            this.TransportationRouteId = transportationRoute.Id;
            this.HeadOfSectionId = HeadOfSectionId;
            this.HaveBackup = false;
            var scheduleDetails = new List<ScheduleDetail>();
            var currentInterval = transportationRoute.ArriveIntervalId;
            while (currentInterval <= transportationRoute.DepartIntervalId)
            {
                scheduleDetails.Add(new ScheduleDetail
                {
                    ActivityId = attendanceType.DefaultActivityId.Value,
                    IntervalId = currentInterval,
                    ScheduleId = schedule.Id
                });
                currentInterval++;
            }
            this.ScheduleDetails = scheduleDetails;
        }
        public DailyAttendance(BkpDailyAttendance model)
        {
            Id = model.DailyAttendanceId;
            StaffMemberId = model.StaffMemberId;
            ScheduleId = model.ScheduleId;
            Day = model.Day;
            AttendanceTypeId = model.AttendanceTypeId;
            SublocationId = model.SublocationId;
            TransportationRouteId = model.TransportationRouteId;
            HeadOfSectionId = model.HeadOfSectionId;
            IsDeleted = false;
            HaveBackup = true;
        }


    }
}
