using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WorkForceGuards.Models;
using WorkForceGuards.Repositories.Interfaces;
using WorkForceManagementV0.Models.Bindings;

namespace WorkForceGuards.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DailyAttendancePatternController : ControllerBase
    {
        private readonly IDailyAttendancePatternService _pattern;
        public DailyAttendancePatternController(IDailyAttendancePatternService pattern)
        {
            _pattern = pattern;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _pattern.GetAll();
            if (!string.IsNullOrEmpty(result.ErrorMessage))
            {
                return BadRequest(new { ErrorMessage = result.ErrorMessage });
            }
            return Ok(result.Result);
        }
        [HttpPost]
        public IActionResult AddUpdate(DailyAttendancePattern model)
        {
            return Ok(_pattern.AddUpdate(model));
        }
        [HttpPost("upload")]
        public IActionResult upload(List<DailyAttendancePatternsUpload> lst)
        {
            var result = _pattern.Upload(lst);
            if(!string.IsNullOrEmpty(result.ErrorMessage))
            {
                return BadRequest(new { ErrorMessage = result.ErrorMessage });
            }
            return Ok(result.Result);
        }
        [HttpDelete("{id}")]
        public IActionResult delete(int id)
        {
            var result = _pattern.Delete(id);
            if(!string.IsNullOrEmpty(result.ErrorMessage))
            {
                return BadRequest(new { ErrorMessage = result.ErrorMessage });
            }
            return Ok(result.Result);
        }
        [HttpGet("generate")]
        public async Task<IActionResult> generateSchedule()
        {
            var result = await _pattern.GenerateSchedule();
            if (!string.IsNullOrEmpty(result.ErrorMessage))
            {
                return BadRequest(new { ErrorMessage = result.ErrorMessage });
            }
            return Ok(result.Result);
        }
        [HttpPost("eligible")]
        public IActionResult getEligibleStaffMembers(List<ScheduleDetailManipulate> models)
        {
            var result = _pattern.GetEligibleStaffMembers(models);
            if (!string.IsNullOrEmpty(result.ErrorMessage))
            {
                return BadRequest(new { ErrorMessage = result.ErrorMessage });
            }
            return Ok(result.Result);
        }
        [HttpGet("headcount")]
        public IActionResult getHeadCount()
        {
            var result = _pattern.GetHeadcount();
            if (!string.IsNullOrEmpty(result.ErrorMessage))
            {
                return BadRequest(new { ErrorMessage = result.ErrorMessage });
            }
            return Ok(result.Result);
        }
        [HttpPost("headcount")]
        public IActionResult setHeadCount(Headcount model)
        {
            var result = _pattern.SetHeadcount(model);
            if (!string.IsNullOrEmpty(result.ErrorMessage))
            {
                return BadRequest(new { ErrorMessage = result.ErrorMessage });
            }
            return Ok(result.Result);
        }
        [HttpPost("headcount/bulk")]
        public IActionResult bulkHeadCount(List<Dictionary<string, string>> models)
        {
            var result = _pattern.BulkHeadcount(models);
            if (!string.IsNullOrEmpty(result.ErrorMessage))
            {
                return BadRequest(new { ErrorMessage = result.ErrorMessage });
            }
            return Ok(result.Result);
        }
        [HttpPost("headcount/upload")]
        public IActionResult bulkHeadCount([FromForm] IFormFile file)
        {
            var ff = file;

            return Ok();
        }
    }
}
