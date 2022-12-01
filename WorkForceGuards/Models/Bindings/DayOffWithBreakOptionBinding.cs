using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkForceManagementV0.Models.Bindings
{
    public class DayOffWithBreakOptionBinding
    {
       public int StaffMemberId { get; set; }

        public int  ScheduleId { get; set; }

        public int? AttendenceTypeId { get; set; }

        public int ? ShiftId { get; set; }

        public DayOption DayOption1 { get; set; } = new DayOption();

        public DayOption DayOption2 { get; set; } = new DayOption();
        public DayOption DayOption3 { get; set; } = new DayOption();
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]

        public DayOption ? DayOption4 { get; set; } = new DayOption();


    }


    public class DayOption
    {
        public List<string> Days { get; set; } = new List<string>();

        public bool ?  IsApproved { get; set; }
    }



}
