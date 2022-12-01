using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkForceManagementV0.Models.DTO
{
    public class SwapRequestDto
    {
        public SwapRequestDto()
        {

        }
        public SwapRequestDto(SwapRequest request, string alias)
        {
            Id = request.Id;
            ScheduleName = request.Schedule!= null ? request.Schedule.Name : "";
            RequesterName = request.SourceDailyAttendance.StaffMember.Name;
            ResponderName = request.DestinationDailyAttendance.StaffMember.Name;
            RequestDate = request.IssueDate;
            RequesterDay = request.SourceDailyAttendance.Day;
            ResponderDay = request.DestinationDailyAttendance.Day;
            StatusId = request.StatusId;
            StatusName = request.Status.Name;
            CloseDate = request.Details.OrderBy(x => x.IssueDate).LastOrDefault()!.CloseDate;
            CanAction = !request.Status.IsComplete && request.Details.OrderBy(x => x.IssueDate).LastOrDefault()!.InvolvedAlias.ToLower() == alias.ToLower();
            CanReverse = false;
        }
        public int Id { get; set; }
        public string ScheduleName { get; set; }
        public string RequesterName { get; set; }
        public string ResponderName { get; set; }
        public DateTime RequestDate { get; set; }
        public DateTime? CloseDate { get; set; }
        public DateTime RequesterDay { get; set; }
        public DateTime ResponderDay { get; set; }
        public string StatusId { get; set; }
        public string StatusName { get; set; }
        public bool CanAction { get; set; }
        public bool CanReverse { get; set; }
    }
    public class SwapRequestWithDetailsDto
    {
        public int Id { get; set; }
        public string RequesterName { get; set; }
        public DailyAttendanceByDayDto SourceDailyAttendance { get; set; }
        public DailyAttendanceByDayDto DestinationDailyAttendance { get; set; }
        public bool CanAction { get; set; }
        public List<SwapRequestDetail> Details { get; set; }
        public bool IsClosed { get; set; }
        
    }

    public class SwapRequestDetailDto
    {
        public int Id { get; set; }
    }
}
