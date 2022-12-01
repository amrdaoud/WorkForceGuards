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
    [Route("api/[controller]")]
    [ApiController]
    public class Test : ControllerBase
    {
        private readonly IFinalScheduleService _IFinalSchedule;
        private readonly IForecastService _IForecastService;
        public Test(IFinalScheduleService scheduler, IForecastService forecastService)
        {
            _IFinalSchedule = scheduler;
            _IForecastService = forecastService;
        }

        //[HttpPost("GenerateForecast")]
        //public ActionResult GenerateForecast(ForecastBindingModel model)
        //{
        //    return Ok(_IForecastService.GenerateForecast(model));
        //}
        [HttpGet("GenerateSchedule")]
        public ActionResult GenerateSchedule(int scheduleId, int forecastId)
        {
            return Ok(_IFinalSchedule.GenerateSchedule( scheduleId,  forecastId));
        }

        //[HttpGet("CreateAcceptedBreaks")]
        //public ActionResult CreateAcceptedBreaks(DateTime currentDay, double tolerance, int forecastId)
        //{
        //    return Ok(_IFinalSchedule.CreateAcceptedBreaks(currentDay, tolerance, forecastId));
        //}
        




    }
}
