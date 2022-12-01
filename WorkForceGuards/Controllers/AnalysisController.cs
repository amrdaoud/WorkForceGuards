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
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AnalysisController : ControllerBase
    {
        private readonly IAnalysisService _analysisService;
        public AnalysisController(IAnalysisService analysisService)
        {
            _analysisService = analysisService;
        }
        [Authorize(Policy = "AdminOrHos")]
        [HttpGet("neededvsavailable")]
        public ActionResult getNeededVsAvailable(int scheduleId, DateTime day)
        {
            var result = _analysisService.NeededVsAvailable(scheduleId, day);
            if(!string.IsNullOrEmpty(result.ErrorMessage))
            {
                return StatusCode(480, new { ErrorMessage = result.ErrorMessage });
            }
            return Ok(result.Result);
        }
        [Authorize(Policy = "AdminOrHos")]
        [HttpGet("leaderboard/{id}")]
        public ActionResult getLeaderBoard(int id, int pageIndex, int pageSize)
        {
            var result = _analysisService.LeaderBoard(id,pageIndex, pageSize);
            if (!string.IsNullOrEmpty(result.ErrorMessage))
            {
                return StatusCode(480, new { ErrorMessage = result.ErrorMessage });
            }
            return Ok(result.Result);
        }
        [Authorize(Policy = "AdminOrHos")]
        [HttpGet("adherencebyhos/{id}")]
        public ActionResult getAdherenceByHos(int id)
        {
            var result = _analysisService.AdherenceByHos(id);
            if (!string.IsNullOrEmpty(result.ErrorMessage))
            {
                return StatusCode(480, new { ErrorMessage = result.ErrorMessage });
            }
            return Ok(result.Result);
        }
    }
}
