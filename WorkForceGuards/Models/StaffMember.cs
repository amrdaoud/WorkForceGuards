using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WorkForceManagementV0.Models
{
    public class StaffMember: People
    {
        public DateTime StartDate { get; set; }
        public DateTime LeaveDate { get; set; }
        public DateTime HireDate { get; set; }
        public int? EstimatedLeaveDays { get; set; }
        public string Religion { get; set; }
        [ForeignKey("StaffType")]
        public int StaffTypeId { get; set; }
        public virtual StaffType StaffType { get; set; }
        [ForeignKey("TransportationRoute")]
        public int ? TransportationRouteId { get; set; }
        public virtual TransportationRoute TransportationRoute { get; set; }
        [ForeignKey("Location")]
        public int? LocationId { get; set; }
        public virtual Location Location { get; set; }
        [ForeignKey("HeadOfSection")]
        public int HeadOfSectionId { get; set; }
        public string PhoneNumber { get; set; }
        public virtual HeadOfSection HeadOfSection { get; set; }

        public virtual ICollection<DailyAttendance> DailyAttendances { get; set; }

        public virtual ICollection<DayOffOption> DayOffOptions { get; set; }

        public virtual ICollection<BreakTypeOption> BreakTypeOption { get; set; }
    }
}
