using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkForceManagementV0.Models;
using WorkForceManagementV0.Models.Bindings;
using WorkForceManagementV0.Repositories.Interfaces;

namespace WorkForceManagementV0.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleWithRuleController : ControllerBase
    {
        private readonly IScheduleWithRuleService _IScheduleWithRuleService;
        private readonly IFinalScheduleService _IFinalSchedule;
        public ScheduleWithRuleController(IScheduleWithRuleService scheduleWithRuleService, IFinalScheduleService scheduler)
        {
            _IScheduleWithRuleService = scheduleWithRuleService;
            _IFinalSchedule = scheduler;
        }

        [HttpGet]
        [Authorize]
        public ActionResult GetAll()
        {
            return Ok(_IScheduleWithRuleService.GetAll(User));
        }

        [Authorize(Policy = "Admin")]
        [HttpPost]
        public ActionResult Add(ScheduleWithRuleBinding model)
        {
            model.ScheduleData.CreateDate = DateTime.UtcNow;
            if (!ModelState.IsValid)
            {
                return BadRequest("Model Is Not Valid");
            }


            var action = _IScheduleWithRuleService.Add(model);
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
        [HttpPut("{id}")]
        public ActionResult Edite(int id, ScheduleWithRuleBinding model)
        {
            model.ScheduleData.UpdateDate = DateTime.UtcNow;
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (id != model.ScheduleData.Id)
            {
                return BadRequest(new { message = "model id missmatch with request id " });
            }

            var action = _IScheduleWithRuleService.Edit(model);
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
        [HttpPut("published/{id}")]
        public ActionResult EditPublished(int id, ScheduleWithRuleBinding model)
        {
            model.ScheduleData.UpdateDate = DateTime.UtcNow;
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (id != model.ScheduleData.Id)
            {
                return BadRequest(new { message = "model id missmatch with request id " });
            }

            var action = _IScheduleWithRuleService.EditPublished(model);
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
        public ActionResult GetById(int id)
        {
            return Ok(_IScheduleWithRuleService.GetById(id));
        }


        [HttpGet("checkUniqe")]
        public ActionResult CheckValue(string value, string ignoreName)
        {
            return Ok(_IScheduleWithRuleService.CheckVlaue(value, ignoreName));
        }

        [Authorize(Policy = "Admin")]
        [HttpPost("EditRule")]

        public ActionResult EditRule(ShiftRule model)
        {
            return Ok(_IScheduleWithRuleService.UpdateRule(model));
        }

        [HttpGet("dateschedule")]
        public ActionResult dateschedule(DateTime start, DateTime end, int id)
        {
            return Ok(_IScheduleWithRuleService.dateschedule(start, end, id));
        }
        [Authorize(Policy = "Admin")]
        [HttpGet("Generate")]
        public ActionResult GenerateSchedule(int scheduleId, int forecastId)
        {
            return Ok(_IFinalSchedule.GenerateSchedule(scheduleId, forecastId));
        }
        [HttpGet("unpublished")]
        public ActionResult GetUnpublishedSchedule()
        {
            var result = _IScheduleWithRuleService.GetUnpublishedSchedule();
            if (!string.IsNullOrEmpty(result.ErrorMessage))
            {
                return BadRequest(new { ErrorMessage = result.ErrorMessage });
            }
            return Ok(result.Result);
        }

        [HttpGet("current")]
        public ActionResult GetCurrentSchedule()
        {
            var result = _IScheduleWithRuleService.GetCurrentSchedule();
            if (!string.IsNullOrEmpty(result.ErrorMessage))
            {
                return BadRequest(new { ErrorMessage = result.ErrorMessage });
            }
            return Ok(result.Result);
        }
        [Authorize(Policy = "Admin")]
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var result = _IScheduleWithRuleService.DeleteSchedule(id);
            if(!string.IsNullOrEmpty(result.ErrorMessage)) {
                return BadRequest(new { ErrorMessage = result.ErrorMessage });
            }
            return Ok(result.Result);
        }

        //[HttpGet("CreateAcceptedBreaks")]
        //public ActionResult CreateAcceptedBreaks(DateTime currentDay, double tolerance, int forecastId)
        //{
        //    return Ok(_IFinalSchedule.CreateAcceptedBreaks(currentDay, tolerance, forecastId));
        //}








    }
}
