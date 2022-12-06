using System.Collections.Generic;
using System.Linq;
using WorkForceGuards.Models;
using WorkForceGuards.Repositories.Interfaces;
using WorkForceManagementV0.Contexts;

namespace WorkForceGuards.Repositories
{
    public class DailyAttendancePatternService : IDailyAttendancePatternService
    {
        private readonly ApplicationDbContext _db;
        public DailyAttendancePatternService(ApplicationDbContext context)
        {
            _db = context;
        }
        public DailyAttendancePatternsDTO AddUpdate(DailyAttendancePattern model)
        {
            throw new System.NotImplementedException();
        }

        public List<DailyAttendancePatternsDTO> GetAll()
        {
            //var scheduleId = _db.Schedules.OrderByDescending(x => x.StartDate).FirstOrDefault(x => !x.IsPublish);
            //var result = _db.
            throw new System.NotImplementedException();
        }
    }
}
