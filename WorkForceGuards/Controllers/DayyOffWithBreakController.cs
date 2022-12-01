using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkForceManagementV0.Models;
using WorkForceManagementV0.Models.Bindings;
using WorkForceManagementV0.Repositories.Identity;
using WorkForceManagementV0.Repositories.Interfaces;

namespace WorkForceManagementV0.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DayyOffWithBreakController : ControllerBase
    {
        private readonly IDayyOffWithBreakService _IDayyOffWithBreakService;
        private readonly IUserService _userService;
        public DayyOffWithBreakController(IDayyOffWithBreakService dayyOffWithBreakService, IUserService userService)
        {
            _IDayyOffWithBreakService = dayyOffWithBreakService;
            _userService = userService;
        }
        [Authorize(Policy = "Admin")]
        [HttpGet]

        public ActionResult GetAll()
        {
            return Ok(_IDayyOffWithBreakService.GetAll());
        }


        [HttpPost]

        public ActionResult Add(DayOffWithBreakOptionBinding model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }


            var action = _IDayyOffWithBreakService.Add(model);
            if (string.IsNullOrEmpty(action.ErrorMessage))
            {
                return Ok(action.Result);

            }
            else
            {

                return BadRequest(new { ErrorMessage = action.ErrorMessage });
            }
        }



        [HttpPost("Edit")]

        public ActionResult Edit(DayOffWithBreakOptionBinding model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            

            var action = _IDayyOffWithBreakService.Edit(model);
            if (string.IsNullOrEmpty(action.ErrorMessage))
            {
                return Ok(action.Result);

            }
            else
            {

                return BadRequest(new { ErrorMessage = action.ErrorMessage });
            }
        }

        [HttpGet("GetById")]

        public ActionResult GetById(int empid)
        {
           
            var action = _IDayyOffWithBreakService.GetById(empid);
            if (string.IsNullOrEmpty(action.ErrorMessage))
            {
                return Ok(action.Result);

            }
            else
            {

                return BadRequest(new { ErrorMessage = action.ErrorMessage });
            }
        }
        [Authorize]
        [HttpGet("GetByAlias")]

        public ActionResult GetByAlias()
        {
            var appUser = _userService.GetUserInfo(User);
            var action = _IDayyOffWithBreakService.GetByAlias(appUser.Alias);
            if (string.IsNullOrEmpty(action.ErrorMessage))
            {
                return Ok(action.Result);

            }
            else
            {

                return BadRequest(new { ErrorMessage = action.ErrorMessage });
            }
        }

        [Authorize(Policy = "Admin")]
        [HttpPost("Appoved")]
        public ActionResult Appoved(DayOffApprovalBinding model)
        {
            var action = _IDayyOffWithBreakService.ApprovedOption(model);
            if (string.IsNullOrEmpty(action.ErrorMessage))
            {
                return Ok(action.Result);

            }
            else
            {

                return BadRequest(new { ErrorMessage = action.ErrorMessage });
            }
        }
        [Authorize(Policy = "Admin")]
        [HttpPost("Approve")]

        public ActionResult Approve(DayOffApprovalBinding model)
        {
            var action = _IDayyOffWithBreakService.A_ApprovedOption(model);
            if (string.IsNullOrEmpty(action.ErrorMessage))
            {
                return Ok(action.Result);

            }
            else
            {

                return BadRequest(new { ErrorMessage = action.ErrorMessage });
            }
        }

        [Authorize(Policy = "Admin")]
        [HttpGet("CreateAttendance")]

        public ActionResult CreateAttendance()
        {
            var action = _IDayyOffWithBreakService.CreateDailyAttendence();
            if (string.IsNullOrEmpty(action.ErrorMessage))
            {
                return Ok(action.Result);

            }
            else
            {

                return BadRequest(new { ErrorMessage = action.ErrorMessage });
            }
        }
        [HttpGet("GetAttendanceType")]
        public ActionResult GetAttendanceType()
        {
            return Ok(_IDayyOffWithBreakService.GetAttendanceType());
        }
        [Authorize(Policy = "Admin")]
        [HttpPost("SetForeCasting")]
        public ActionResult setForeCasting(int ScheduleId)
        {
            return Ok(_IDayyOffWithBreakService.setForeCasting(ScheduleId));
        }
        [Authorize(Policy = "Admin")]
        [HttpPost("SetForecastDetails")]
        public ActionResult setForecastDetails(int ForecastId)
        {
            return Ok(_IDayyOffWithBreakService.setForecastDetails(ForecastId));
        }
        [Authorize(Policy = "Admin")]
        [HttpPost("upload")]

        public ActionResult Upload(List<DayOffWithBreaksUpload> models)
        {
            var action = _IDayyOffWithBreakService.UploadDayOffWithBreaks(models);
            if (string.IsNullOrEmpty(action.ErrorMessage))
            {
                return Ok(action.Result);
            }
            else
            {
                return BadRequest(new { ErrorMessage = action.ErrorMessage });
            }
        }


    }
}
