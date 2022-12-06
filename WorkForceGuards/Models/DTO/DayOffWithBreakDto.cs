using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkForceManagementV0.Models.DTO
{

    public class DayOffWithBreakDto
    {
        public DayOffWithBreakDto()
        {

        }
        public DayOffWithBreakDto(string staffName, int staffId, string scheduleName, int scheduleId, string attendanceTypeName, int attendanceTypeId,
            string shiftName, int shiftId, int sublocationId, string sublocationName ,DayOptionViewModel option1,
            DayOptionViewModel option2, DayOptionViewModel option3, DayOptionViewModel? option4)
        {
            StaffMemberName = staffName;
            StaffMemberId = staffId;
            ScheduleName = scheduleName;
            ScheduleId = scheduleId;
            AttendenceTypeName = attendanceTypeName;
            AttendenceTypeId = attendanceTypeId;
            SublocationId = sublocationId;
            SublocationName = sublocationName;
            ShiftName = shiftName;
            ShiftId = shiftId;
            DayOption1 = option1;
            DayOption2 = option2;
            DayOption3 = option3;
            DayOption4 = option4;

        }
        public string StaffMemberName { get; set; }

        public int StaffMemberId { get; set; }

        public string ScheduleName { get; set; }
        public int ScheduleId { get; set; }

        public string AttendenceTypeName { get; set; }
        public int AttendenceTypeId { get; set; }
        public int SublocationId { get; set; }
        public string SublocationName { get; set; }
        public string ShiftName { get; set; }

        public int ShiftId { get; set; }

        public DayOptionViewModel DayOption1 { get; set; } = new DayOptionViewModel();

        public DayOptionViewModel DayOption2 { get; set; } = new DayOptionViewModel();
        public DayOptionViewModel DayOption3 { get; set; } = new DayOptionViewModel();
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DayOptionViewModel? DayOption4 { get; set; } = new DayOptionViewModel();

    }

    public class DayOptionViewModel
    {
        public DayOptionViewModel()
        {

        }
        public DayOptionViewModel(DayOffOption op)
        {
            Id = op.Id;
            Days = new List<string>();
            Days.Add(op.DayOne);
            Days.Add(op.DayTwo);
            IsApproved = op.IsApproved;
        }

        public int Id { get; set; }
        public List<string> Days { get; set; } = new List<string>();
        public bool? IsApproved { get; set; }
    }
}