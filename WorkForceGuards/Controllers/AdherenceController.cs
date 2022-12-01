using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkForceManagementV0.Repositories.Interfaces;

namespace WorkForceManagementV0.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AdherenceController : ControllerBase
    {
        private readonly IAdherenceService _adherenceService;
        public AdherenceController(IAdherenceService adherenceService)
        {
            _adherenceService = adherenceService;
        }
        [HttpGet("bystaffday")]
        public ActionResult AdherenceByStaffDay(int scheduleId, int staffId, DateTime day)
        {
            return Ok(_adherenceService.AdherenceByStaffDay(scheduleId, staffId, day, User));
        }
        [HttpGet("byday")]
        public ActionResult AdherenceByDay(int scheduleId, DateTime day)
        {
            return Ok(_adherenceService.AdherenceByDay(scheduleId, day, User));
        }
        [HttpGet("bystaff")]
        public ActionResult AdherenceByStaff(int scheduleId, int staffId)
        {
            return Ok(_adherenceService.AdherenceByStaff(scheduleId, staffId, User));
        }
        [HttpGet("byschedule")]
        public ActionResult AdherenceBySchedule(int scheduleId)
        {
            return Ok(_adherenceService.AdherenceBySchedule(scheduleId, User));
        }
        [HttpGet("byschedule/all")]
        public ActionResult AdherenceByScheduleAll(int scheduleId)
        {
            return Ok(_adherenceService.AdherenceByScheduleAll(scheduleId, User));
        }
        [HttpGet("byday/all")]
        public ActionResult AdherenceByDayAll(int scheduleId, DateTime day)
        {
            return Ok(_adherenceService.AdherenceByDayAll(scheduleId, day, User));
        }
    }
}
