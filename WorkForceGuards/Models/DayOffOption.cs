using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WorkForceManagementV0.Models.Bindings;

namespace WorkForceManagementV0.Models
{
    public class DayOffOption
    {
        public DayOffOption()
        {

        }
        public DayOffOption(DayOffApprovalBinding model)
        {
            Id = 0;
            StaffMemberId = model.StaffMemberId;
            ScheduleId = model.ScheduleId;
            DayOne = model.AccepedDayOffInfo.Day1;
            DayTwo = model.AccepedDayOffInfo.Day2;
            IsApproved = true;
            IsAdmin = true;
        }
        [Key]
        public int Id { get; set; }

        [ForeignKey("StaffMember")]
        public int StaffMemberId { get; set; }

        [ForeignKey("Schedule")]
        public int ScheduleId { get; set; }

        public string DayOne { get; set; }

        public string DayTwo { get; set; }

        public bool IsApproved { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsAdmin { get; set; }

        public virtual StaffMember StaffMember { get; set; }

        public virtual Schedule Schedule { get; set; }

    }
}
