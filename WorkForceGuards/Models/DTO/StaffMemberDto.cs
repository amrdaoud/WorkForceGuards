using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkForceManagementV0.Models.DTO
{
    public class StaffMemberDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string Language { get; set; }
        public string Address { get; set; }
        public string Note { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime LeaveDate { get; set; }
        public DateTime HireDate { get; set; }
        public int? EstimatedLeaveDays { get; set; }
        public string Religion { get; set; }
        public int StaffTypeId { get; set; }
        public string StaffTypeName { get; set; }

        public int ? TransportationRouteId { get; set; }
        public string TransportationRouteName { get; set; }

        public int ? LocationId { get; set; }
        public string LocationName { get; set; }

        public int ? HeadOfSectionId { get; set; }
        public string HeadOfSectionName { get; set; }

    }

    public class FullStaffMemberDto
    {
       public List<StaffMemberDto> Result { get; set; }

        public int ResultSize { get; set; }

    }
}
