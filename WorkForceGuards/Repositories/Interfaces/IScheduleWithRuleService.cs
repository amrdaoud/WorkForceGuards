using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WorkForceManagementV0.Models;
using WorkForceManagementV0.Models.Bindings;

namespace WorkForceManagementV0.Repositories.Interfaces
{
   public interface IScheduleWithRuleService
    {
        ScudualeWithRule GetAll(ClaimsPrincipal user);
       
        DataWithError Add(ScheduleWithRuleBinding model);
        DataWithError AddWithPatterns(ScheduleWithRuleBinding model);
        DataWithError Edit(ScheduleWithRuleBinding model);
        DataWithError EditPublished(ScheduleWithRuleBinding model);
        DataWithError DeleteSchedule(int id);
        ShiftRule UpdateRule(ShiftRule model);
        bool CheckUniqeValue(Schedule model);
        bool CheckVlaue(string name, string ignoreName);
        ScheduleWithRuleBinding GetById(int id);
        bool CheckLastPublishedSchedule();
        bool dateschedule(DateTime start, DateTime end,int id );
        DataWithError GetUnpublishedSchedule();
        DataWithError GetCurrentSchedule();
    }
}
