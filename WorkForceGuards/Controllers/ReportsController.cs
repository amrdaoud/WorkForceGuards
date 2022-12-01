using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using WorkForceManagementV0.Models.Bindings;
using WorkForceManagementV0.Repositories;
using WorkForceManagementV0.Repositories.Interfaces;

namespace WorkForceManagementV0.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class ReportsController : ControllerBase
    {
        private readonly IReportAdheranceService _IReportAdheranceService;
        public  ReportsController(IReportAdheranceService ReportAdheranceService)
        {
            _IReportAdheranceService = ReportAdheranceService;
        }
        [Authorize(Policy = "AdminOrHos")]
        [HttpPost("Adherance")]
        public ActionResult Report(ReportFilter model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Model Is Not Valid");
            }


            var action = _IReportAdheranceService.Report(model, User);
            if (string.IsNullOrEmpty(action.ErrorMessage))
            {
                return Ok(action.Result);

            }
            else
            {

                return BadRequest(new { ErrorMessage = action.ErrorMessage });

            }
        }
        [HttpGet("Activities")]
        public IActionResult ActivityReport(DateTime RequestDate)
        {
            var action = _IReportAdheranceService.ActivityReport(RequestDate, User);
            if(string.IsNullOrEmpty(action.ErrorMessage))
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
