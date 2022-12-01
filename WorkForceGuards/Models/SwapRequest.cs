using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WorkForceManagementV0.Models
{
    public class SwapRequest
    {
        public SwapRequest()
        {

        }
        public SwapRequest(DailyAttendance sourceDailyAttendance, DailyAttendance destinationDailyAttendance, DateTime issueDate, string requesterAlias ,int? scheduleId)
        {
            SourceDailyAttendanceId = sourceDailyAttendance.Id;
            DestinationDailyAttendanceId = destinationDailyAttendance.Id;
            IssueDate = issueDate;
            RequesterAlias = requesterAlias;
            RequesterName = sourceDailyAttendance.StaffMember.Name;
            ResponderName = destinationDailyAttendance.StaffMember.Name;
            StatusId = "APPRV1";
            Details = new List<SwapRequestDetail>();
            Details.Add(new SwapRequestDetail(destinationDailyAttendance.StaffMember.Alias, issueDate));
            ScheduleId = scheduleId;
        }
        public int Id { get; set; }
        [ForeignKey("Schedule")]
        public int? ScheduleId { get; set; }
        [ForeignKey("SourceDailyAttendance")]
        public int? SourceDailyAttendanceId { get; set; }
        [ForeignKey("DestinationDailyAttendance")]
        public int? DestinationDailyAttendanceId { get; set; }
        public DateTime IssueDate { get; set; }
        [ForeignKey("Status")]
        public string StatusId { get; set; }
        [Required]
        public string RequesterAlias { get; set; }
        public string RequesterName { get; set; }
        public string ResponderName { get; set; }
        public virtual Schedule Schedule { get; set; }
        public virtual DailyAttendance SourceDailyAttendance { get; set; }
        public virtual DailyAttendance DestinationDailyAttendance { get; set; }
        public virtual SwapRequestStatus Status { get; set; }
        public virtual ICollection<SwapRequestDetail> Details { get; set; }
    }
    public class SwapRequestDetail
    {
        public SwapRequestDetail()
        {

        }
        public SwapRequestDetail(string involvedAlias, DateTime issueDate)
        {
            InvolvedAlias = involvedAlias;
            IsApproved = false;
            IsDeclined = false;
            IssueDate = issueDate;
        }
        public int Id { get; set; }
        [ForeignKey("SwapRequest")]
        public int SwapRequestId { get; set; }
        [Required]
        public string InvolvedAlias { get; set; }
        public bool? IsApproved { get; set; }
        public bool? IsDeclined { get; set; }
        public string Reason { get; set; }
        [Required]
        public DateTime IssueDate { get; set; }
        public DateTime? CloseDate { get; set; }

        public virtual SwapRequest SwapRequest { get; set; }
    }
    public class SwapRequestStatus
    {
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public bool IsComplete { get; set; }
        [Required]
        public bool IsSuccess { get; set; }
    }
}
