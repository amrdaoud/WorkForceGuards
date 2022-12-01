using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkForceManagementV0.Models;
using WorkForceManagementV0.Repositories.Interfaces;

namespace WorkForceManagementV0.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ActivityController : ControllerBase
    {
        private readonly IActivityService _IActivityService;
        public ActivityController(IActivityService activityService)
        {
            _IActivityService = activityService;
        }
        [HttpGet]
        public ActionResult GET()
        {
            return Ok(_IActivityService.GetAll());
        }
        [Authorize(Policy = "Admin")]
        [HttpPost]
        public ActionResult Add(Activity model)

        {
            model.CreateDate = DateTime.UtcNow;
            if (!ModelState.IsValid)
            {
                return BadRequest("Model Is Not Valid");
            }


            var action = _IActivityService.Add(model);
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
        public ActionResult Edite(int id,Activity model)
        {
            model.UpdateDate = DateTime.UtcNow;
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (id != model.Id)
            {
                return BadRequest(new { message = "model id missmatch with request id " });
            }

            var action = _IActivityService.Update(model);
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
            return Ok(_IActivityService.GetById(id));
        }

        [Authorize(Policy = "Admin")]
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            return Ok(_IActivityService.delete(id));
        }


        [HttpGet("checkUniqe")]
        public ActionResult CheckValue(string value,string ignoreName)
        {
            return Ok(_IActivityService.CheckValue(value, ignoreName));
        }


        [HttpGet("checkUniqeColor")]
        public ActionResult checkUniqeColor(string value, string ignoreName)
        {
            return Ok(_IActivityService.CheckValueColor(value, ignoreName));
        }



        [HttpGet("GetColors")]
        public ActionResult GetColors()
        {
            return Ok(_IActivityService.GetColors());
        }

        [HttpGet("AvailableColor")]
        public ActionResult AvailableColor(string CurrentColor)
        {
            return Ok(_IActivityService.AvailableColor(CurrentColor));
        }


    }
}
