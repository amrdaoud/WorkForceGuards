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
    public class ForecastController : ControllerBase
    {
        private readonly IForecastService _IForecastService;
        public ForecastController(IForecastService Forecastservice)
        {
            _IForecastService = Forecastservice;
        }
        //[HttpPost("Save")]
        //public ActionResult SaveForecast(ForecastBindingModel model)
        //{

        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest();
        //    }
        //    else
        //    {
        //        var result = _IForecastService.SaveForecast(model);
        //        if(!result.Succeded)
        //        {
        //            return BadRequest(new { ErrorMessage = result.Message });
        //        }
        //        return Ok(true);
        //    }
        // }
        [Authorize(Policy = "Admin")]
        [HttpPost("Generate")]
        public async Task<ActionResult> GenerateForecastAsync(ForecastBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var action = await _IForecastService.GenerateForecastAsync(model);
            if (string.IsNullOrEmpty(action.ErrorMessage))
            {
                return Ok(action.Result);

            }
            else
            {

                return BadRequest(new { ErrorMessage = action.ErrorMessage });
            }
            //return Ok(true);
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest();
            //}
            //else
            //    return Ok(await _IForecastService.GenerateForecastAsync(model));
        }
        [Authorize(Policy = "Admin")]
        [HttpGet("GetAll")]
        public ActionResult GetAll()
        {
            var action = _IForecastService.GetAll();
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
        [HttpGet("GetById")]
        public ActionResult GetById(int id)
        {
            var action = _IForecastService.GetById(id);
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
        [HttpGet("GetUnSaved")]
        public ActionResult GetUnSaved()
        {
            var action = _IForecastService.GetUnSaved();
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
        [HttpGet("GetSaved")]
        public ActionResult GetSaved()
        {
            var action = _IForecastService.GetSaved();
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
        [HttpPut("UpdateInterval/{id}")]
        //EditForecast(int id, EditForecastBinding model)
        public ActionResult PutOneInterval(int id, EditForecastBinding model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (id != model.Id)
            {
                return BadRequest(new { message = "model id missmatch with request id " });
            }

            var action = _IForecastService.PutOneInterval(id, model);
            if (string.IsNullOrEmpty(action.ErrorMessage))
            {
                return Ok(action.Result);

            }
            else
            {

                return BadRequest(new { ErrorMessage = action.ErrorMessage });
            }
            //return Ok(true);

        }
        [Authorize(Policy = "Admin")]
        [HttpPut("{id}")]
        public ActionResult PutForecast(int id, ForecastBindingModel model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (id != model.Id)
            {
                return BadRequest(new { message = "model id missmatch with request id " });
            }

            var action = _IForecastService.PutForecast(id, model);
            if (string.IsNullOrEmpty(action.ErrorMessage))
            {
                return Ok(action.Result);

            }
            else
            {

                return BadRequest(new { ErrorMessage = action.ErrorMessage });
            }

        }

        //[HttpGet("CheckUniqeValue")]
        //public ActionResult CheckUniqeValue(ForecastBindingModel model)
        //{
        //    return Ok(_IForecastService.CheckUniqeValue(model));
        //}
        [Authorize(Policy = "Admin")]
        [HttpDelete("Remove/{id}")]
        public ActionResult Remove(int id)
        {
            var action = _IForecastService.Remove(id);
            if (string.IsNullOrEmpty(action.ErrorMessage))
            {
                return Ok(action.Result);

            }
            else
            {

                return BadRequest(new { ErrorMessage = action.ErrorMessage });
            }

        }

        [HttpGet("CheckUniqeValue")]
        public ActionResult CheckValue(string name, string ignoreName)
        {

            return Ok(_IForecastService.CheckValue(name, ignoreName));
        }

        [HttpGet("Save/{id}")]
        public ActionResult SaveForecast(int id)
        {
            var action = _IForecastService.SaveForecast(id);
            if (string.IsNullOrEmpty(action.ErrorMessage))
            {
                return Ok(action.Result);
            }
            else
            {
                return BadRequest(new { ErrorMessage = action.ErrorMessage });
            }
        }

        [HttpGet("GetIntervals")]
        public ActionResult GetIntervals()
        {
            var action = _IForecastService.GetIntervals();
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
        [HttpPost("EditTolerance")]
        public ActionResult EditTolerance( EditToleranceBinding model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var action = _IForecastService.EditTolerance(model);
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
        [HttpGet("EditAllTolerance")]
        public ActionResult  EditAllTolerance(double tolerance)
        {
            var action = _IForecastService.EditAllTolerance(tolerance);
            if(string.IsNullOrEmpty(action.ErrorMessage))
            {
                return Ok(action.Result);
            }
            else
            {
                return BadRequest(new { ErrorMessage = action.ErrorMessage });
            }
        }

        [Authorize(Policy = "Admin")]
        [HttpPost("RecommendAll")]
        public ActionResult EditAllTolerance(List<RecommendAll> recommendAll)
        {
            var action = _IForecastService.RecommendAllTolerances(recommendAll);
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
