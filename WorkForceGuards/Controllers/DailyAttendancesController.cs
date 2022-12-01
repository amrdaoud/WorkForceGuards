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
    public class DailyAttendancesController : ControllerBase
    {
        private readonly IDailyAttendanceService _iDailyAttendanceService;

        public DailyAttendancesController(IDailyAttendanceService iDailyAttendanceService)
        {
            _iDailyAttendanceService = iDailyAttendanceService;
        }
        [Authorize(Policy = "Admin")]
        [HttpGet("createattendance/{id}")]
        public ActionResult CreateScheduleAttendance(int id)
        {
            var result = _iDailyAttendanceService.CreateScheduleAttendance(id);
            if (!string.IsNullOrEmpty(result.ErrorMessage))
            {
                return BadRequest(new { ErrorMessage = result.ErrorMessage });
            }
            return Ok(result.Result);
        }
        [Authorize(Policy = "Admin")]
        [HttpGet("createattendance")]
        public ActionResult CreateAttendance()
        {
            var result = _iDailyAttendanceService.CreateAttendance();
            if (!string.IsNullOrEmpty(result.ErrorMessage))
            {
                return BadRequest(new { ErrorMessage = result.ErrorMessage });
            }
            return Ok(result.Result);
        }
        [Authorize(Policy = "Admin")]
        [HttpGet("createstaffattendance")]
        public ActionResult CreateStaffScheduleAttendance(int scheduleId, int staffId)
        {
            var result = _iDailyAttendanceService.CreateStaffScheduleAttendance(scheduleId, staffId);
            if (!string.IsNullOrEmpty(result.ErrorMessage))
            {
                return BadRequest(new { ErrorMessage = result.ErrorMessage });
            }
            return Ok(result.Result);
        }
        [Authorize(Policy = "Admin")]
        [HttpGet("summary")]
        public ActionResult GetSummary(int scheduleId, int? forecastId)
        {
            var result = _iDailyAttendanceService.GetAttendanceSummary(scheduleId, forecastId);
            //if (!string.IsNullOrEmpty(result.ErrorMessage))
            //{
            //    return BadRequest(new { ErrorMessage = result.ErrorMessage });
            //}
            return Ok(result);
        }
    }
}
