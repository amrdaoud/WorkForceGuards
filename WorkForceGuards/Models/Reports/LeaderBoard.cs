using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkForceManagementV0.Models.Reports
{
    public class LeaderBoardWithSize
    {
        public int DataSize { get; set; }
        public List<LeaderBoard> Data { get; set; }
    }
    public class LeaderBoard
    {
        public int Rank { get; set; }
        public string AvatarUrl { get; set; }
        public int StaffId { get; set; }
        public string StaffName { get; set; }
        public double? Adherence { get; set; }
    }
}
