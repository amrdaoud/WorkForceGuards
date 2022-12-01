using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkForceManagementV0.Models.Bindings
{
    public class EditScheduleDetailsBinding
    {
        public List<int> ScheduleDetailsIds { get; set; }
        public int ActivityId { get; set; }
    }
    public class AddScheduleDetailsBinding
    {
        public int scheduleId { get; set; }
        public List<AddScheduleDetail> AddScheduleDetails { get; set; }
        public int ActivityId { get; set; }
    }
    public class AddScheduleDetail
    {
        public int DailyAttendanceId { get; set; }
        public int IntervalId { get; set; }
    }
    public class ScheduleDetailManipulate
    {
        public int DailyAttendanceId { get; set; }
        public int IntervalId { get; set; }
        public int? ActivityId { get; set; }
    }

    public class ManipulateDetails
    {
        public int ActivityId { get; set; }
        public List<ScheduleDetailManipulate> ScheduleDetailsManipulate { get; set; }

    }
    public class ManipulateDailyAttendance
    {
        public int SourceId { get; set; }
        public List<int> DestinationIds { get; set; }
    }
}
