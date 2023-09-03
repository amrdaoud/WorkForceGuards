using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkForceManagementV0.Models.Bindings;
using WorkForceManagementV0.Repositories.Interfaces;

namespace WorkForceManagementV0.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ActiveScheduleController : ControllerBase
    {
        private readonly IFinalScheduleService _finalScheduleService;
        public ActiveScheduleController(IFinalScheduleService finalScheduleService)
        {
            _finalScheduleService = finalScheduleService;
        }
        [Authorize(Policy ="Admin")]
        [HttpGet("Generate")]
        public ActionResult GenerateSchedule(int scheduleId, int forecastId)
        {
            var dataWithError = _finalScheduleService.GenerateSchedule(scheduleId, forecastId);
            if (!dataWithError.Succeded)
            {
                return BadRequest(new { ErrorMessage = dataWithError.Message });
            }
            return Ok(scheduleId);
        }
        [Authorize(Policy = "Admin")]
        [HttpGet("publish/{id}")]
        public ActionResult PublishSchedule(int id)
        {
            var dataWithError = _finalScheduleService.PublishSchedule(id);
            if (!string.IsNullOrEmpty(dataWithError.ErrorMessage))
            {
                return BadRequest(new { ErrorMessage = dataWithError.ErrorMessage });
            }
            return Ok(dataWithError.Result);
        }
        [HttpGet("schedulebystaff")]

        public ActionResult schedulebystaff(int scheduleId, int staffId)
        {
            var dataWithError = _finalScheduleService.schedulebystaff(scheduleId, staffId, User);
            if (string.IsNullOrEmpty(dataWithError.ErrorMessage))
            {
                return Ok(dataWithError.Result);
            }
            else if(dataWithError.ErrorMessage.ToLower() == "unauthorized")
            {
                return Unauthorized(new { ErrorMessage = "You are not authorized!" });
            }
            else
            {
                return BadRequest(new { ErrorMessage = dataWithError.ErrorMessage });
            }

        }

        [HttpGet("schedulebyDayPage")]
        public ActionResult schedulebyDatePage(int scheduleId, DateTime day, int pageIndex, int pageSize, string searchQuery, string type)
        {
            var dataWithError = _finalScheduleService.schedulebyDatePage(scheduleId, day, pageIndex, pageSize, searchQuery, type, User);
            if (string.IsNullOrEmpty(dataWithError.ErrorMessage))
            {
                return Ok(dataWithError.Result);
            }
            else
            {
                return BadRequest(new { ErrorMessage = dataWithError.ErrorMessage });
            }

        }
        [HttpGet("schedulebyDayPage/all")]
        public ActionResult schedulebyDatePageAll(int scheduleId, DateTime day, int pageIndex, int pageSize, string searchQuery, string type)
        {
            var dataWithError = _finalScheduleService.schedulebyDatePageAll(scheduleId, day, pageIndex, pageSize, searchQuery, type, User);
            if (string.IsNullOrEmpty(dataWithError.ErrorMessage))
            {
                return Ok(dataWithError.Result);
            }
            else
            {
                return BadRequest(new { ErrorMessage = dataWithError.ErrorMessage });
            }

        }
        [HttpGet("schedulebystaffday")]
        public ActionResult schedulebystaffday(int scheduleId, int staffId, DateTime day)
        {
            var dataWithError = _finalScheduleService.schedulebystaffday(scheduleId, staffId, day, User);
            if (string.IsNullOrEmpty(dataWithError.ErrorMessage))
            {
                return Ok(dataWithError.Result);
            }
            else if (dataWithError.ErrorMessage.ToLower() == "unaothorized")
            {
                return Unauthorized(new { ErrorMessage = "You are not authorized!" });
            }
            else
            {
                return BadRequest(new { ErrorMessage = dataWithError.ErrorMessage });
            }

        }

        [HttpGet("schedulebyIdpage")]
        public ActionResult schedulebyIdpage(int scheduleId, int pageIndex, int pageSize, string searchQuery, string type)
        {
            var dataWithError = _finalScheduleService.schedulebyIdpage(scheduleId, pageIndex, pageSize, searchQuery, type, User);
            if (string.IsNullOrEmpty(dataWithError.ErrorMessage))
            {
                return Ok(dataWithError.Result);
            }
            else
            {
                return BadRequest(new { ErrorMessage = dataWithError.ErrorMessage });
            }
        }

        [HttpGet("schedulebyIdpage/all")]
        public ActionResult schedulebyIdpageAll(int scheduleId, int pageIndex, int pageSize, string searchQuery, string type)
        {
            var dataWithError = _finalScheduleService.schedulebyIdpageAll(scheduleId, pageIndex, pageSize, searchQuery, type, User);
            if (string.IsNullOrEmpty(dataWithError.ErrorMessage))
            {
                return Ok(dataWithError.Result);
            }
            else
            {
                return BadRequest(new { ErrorMessage = dataWithError.ErrorMessage });
            }
        }

        [Authorize(Policy = "AdminOrHos")]
        [HttpGet("EditScheduleDetails")]

        public ActionResult EditScheduleDetails(int scheduleDetailId, int activityId)
        {
            var dataWithError = _finalScheduleService.EditScheduleDetails(scheduleDetailId, activityId, User);
            if (string.IsNullOrEmpty(dataWithError.ErrorMessage))
            {
                return Ok(dataWithError.Result);
            }
            else
            {
                return BadRequest(new { ErrorMessage = dataWithError.ErrorMessage });
            }
        }

        [Authorize(Policy = "AdminOrHos")]
        [HttpPost("EditScheduleDetailsMultiple")]

        public ActionResult EditScheduleDetails(EditScheduleDetailsBinding model)
        {
            var dataWithError = _finalScheduleService.EditScheduleDetailsMultiple(model,User);
            if (string.IsNullOrEmpty(dataWithError.ErrorMessage))
            {
                return Ok(dataWithError.Result);
            }
            else
            {
                return BadRequest(new { ErrorMessage = dataWithError.ErrorMessage });
            }
        }

        [Authorize(Policy = "AdminOrHos")]
        [HttpDelete("DeleteScheduleDetail/{id}")]

        public ActionResult RemoveScheduleDetail(int id)
        {
            var dataWithError = _finalScheduleService.RemoveScheduleDetail(id, User);
            if (string.IsNullOrEmpty(dataWithError.ErrorMessage))
            {
                return Ok(dataWithError.Result);
            }
            else
            {
                return BadRequest(new { ErrorMessage = dataWithError.ErrorMessage });
            }
        }

        [Authorize(Policy = "AdminOrHos")]
        [HttpGet("AddScheduleDetails")]

        public ActionResult AddScheduleDetails(int dailyAttendanceId, int intervalId, int activityId)
        {
            var dataWithError = _finalScheduleService.AddScheduleDetail(dailyAttendanceId,intervalId, activityId, User);
            if (string.IsNullOrEmpty(dataWithError.ErrorMessage))
            {
                return Ok(dataWithError.Result);
            }
            else
            {
                return BadRequest(new { ErrorMessage = dataWithError.ErrorMessage });
            }
        }

        [Authorize(Policy = "AdminOrHos")]
        [HttpPost("manipulatescheduledetails/staff/{scheduleId}")]

        public ActionResult AddScheduleDetailsMultipleStaff(ManipulateDetails model, int scheduleId)
        {
            var dataWithError = _finalScheduleService.ManipulateScheduleDetails(model, scheduleId, User, true);
            if (string.IsNullOrEmpty(dataWithError.ErrorMessage))
            {
                return Ok(dataWithError.Result);
            }
            else
            {
                return BadRequest(new { ErrorMessage = dataWithError.ErrorMessage });
            }
        }

        [Authorize(Policy = "AdminOrHos")]
        [HttpPost("manipulatescheduledetails/day/{scheduleId}")]

        public ActionResult AddScheduleDetailsMultipleDay(ManipulateDetails model, int scheduleId)
        {
            var dataWithError = _finalScheduleService.ManipulateScheduleDetails(model, scheduleId, User, false);
            if (string.IsNullOrEmpty(dataWithError.ErrorMessage))
            {
                return Ok(dataWithError.Result);
            }
            else
            {
                return BadRequest(new { ErrorMessage = dataWithError.ErrorMessage });
            }
        }



        [Authorize(Policy = "AdminOrHos")]
        [HttpGet("EditDailyAttendance")]
        public ActionResult EditDailyAttendance(int dailyAttendanceId, int attendanceTypeId)
        {
            var dataWithError = _finalScheduleService.EditDailyAttendance(dailyAttendanceId, attendanceTypeId, User);
            if (string.IsNullOrEmpty(dataWithError.ErrorMessage))
            {
                return Ok(dataWithError.Result);
            }
            else
            {
                return BadRequest(new { ErrorMessage = dataWithError.ErrorMessage });
            }
        }

        [Authorize(Policy = "AdminOrHos")]
        [HttpGet("EditSublocation")]
        public ActionResult EditSublocation(int dailyAttendanceId, int sublocationId)
        {
            var dataWithError = _finalScheduleService.EditSublocation(dailyAttendanceId, sublocationId, User);
            if (string.IsNullOrEmpty(dataWithError.ErrorMessage))
            {
                return Ok(dataWithError.Result);
            }
            else
            {
                return BadRequest(new { ErrorMessage = dataWithError.ErrorMessage });
            }
        }

        [Authorize(Policy = "AdminOrHos")]
        [HttpGet("EditDailyAttendanceShift")]
        public ActionResult EditDailyAttendanceShift(int dailyAttendanceId, int transportationId)
        {
            //var dataWithError = _finalScheduleService.EditDailyAttendanceShift(dailyAttendanceId, transportationId, User);
            var dataWithError = _finalScheduleService.EditDailyAttendanceShiftGrd(dailyAttendanceId, transportationId, User);
            if (string.IsNullOrEmpty(dataWithError.ErrorMessage))
            {
                return Ok(dataWithError.Result);
            }
            else
            {
                return BadRequest(new { ErrorMessage = dataWithError.ErrorMessage });
            }
        }

        [HttpGet("backups/{id}")]
        public ActionResult GetDailyAttendanceBackups(int id)
        {
            var dataWithError = _finalScheduleService.GetDailyAttendanceBackups(id);
            if (string.IsNullOrEmpty(dataWithError.ErrorMessage))
            {
                return Ok(dataWithError.Result);
            }
            else
            {
                return BadRequest(new { ErrorMessage = dataWithError.ErrorMessage });
            }
        }
        [Authorize(Policy = "AdminOrHos")]
        [HttpGet("undoDailyAttendance")]
        public ActionResult UndoDailyAttendance(int dailyAttendanceId, bool? withDetails)
        {
            
            var dataWithError = withDetails != null && withDetails == true ?
                _finalScheduleService.UndoDailyAttendanceReturnDetails(dailyAttendanceId, User) :
                _finalScheduleService.UndoDailyAttendance(dailyAttendanceId, User);
            ;
            if (string.IsNullOrEmpty(dataWithError.ErrorMessage))
            {
                return Ok(dataWithError.Result);
            }
            else
            {
                return BadRequest(new { ErrorMessage = dataWithError.ErrorMessage });
            }
        }
        [Authorize(Policy = "AdminOrHos")]
        [HttpPost("copydailyattendance")]
        public ActionResult CopyDailyAttendance(ManipulateDailyAttendance model)
        {
            var dataWithError = _finalScheduleService.CopyDailyAttendance(model, User);
            if(string.IsNullOrEmpty(dataWithError.ErrorMessage))
            {
                return Ok(dataWithError.Result);
            }
            else
            {
                return BadRequest(new {ErrorMessage= dataWithError.ErrorMessage});
            }
        }


    }
}
