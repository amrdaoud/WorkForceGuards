using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using WorkForceManagementV0.Models.Bindings;

namespace WorkForceManagementV0.Models
{
    public class BreakTypeOption
    {
        public BreakTypeOption()
        {

        }
        public BreakTypeOption(DayOffApprovalBinding model)
        {
            Id = 0;
            StaffMemberId = model.StaffMemberId;
            ScheduleId = model.ScheduleId;
            AttendenceTypeId = model.AcceptedBreakTypeOption;
            TransportationRouteId = model.ShiftId;
            SublocationId = model.SublocationId;
            IsApproved = true;
            IsAdmin = true;
        }
        [Key]
        public int Id { get; set; }

        [ForeignKey("StaffMember")]
        public int StaffMemberId { get; set; }

        [ForeignKey("Schedule")]
        public int ScheduleId { get; set; }

        [ForeignKey("AttendanceType")]
        public int AttendenceTypeId { get; set; }
        [ForeignKey("Sublocation")]
        public int SublocationId { get; set; }

        //[ForeignKey("Shift")]
        //public int ? ShiftId { get; set; }

        [ForeignKey("TransportationRoute")]
        public int ? TransportationRouteId { get; set; }
        public bool IsApproved { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsAdmin { get; set; }
        public virtual StaffMember StaffMember { get; set; }
        public virtual Schedule Schedule { get; set; }
        public virtual AttendanceType AttendanceType { get; set; }
        public virtual SubLocation Sublocation { get; set; }
        //public virtual Shift Shift { get; set; }
        public virtual TransportationRoute TransportationRoute { get; set; }
    }
}
