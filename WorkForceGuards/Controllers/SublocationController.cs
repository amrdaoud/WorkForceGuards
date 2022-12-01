using Microsoft.AspNetCore.Mvc;
using WorkForceManagementV0.Models.Bindings;
using WorkForceManagementV0.Models;
using WorkForceManagementV0.Repositories.Interfaces;

namespace WorkForceManagementV0.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class SublocationController : ControllerBase
    {
      

        private readonly ISublocationService _SublocationService;
        public SublocationController(ISublocationService SublocationService)
        {
            _SublocationService = SublocationService;
        }

        [HttpPost("Add")]
        public IActionResult Add(SubLocation model)
        {
            var action = _SublocationService.Add(model);
            if (string.IsNullOrEmpty(action.ErrorMessage))
            {
                return Ok(action.Result);
            }
            else
            {
                return BadRequest(new { ErrorMessage = action.ErrorMessage });
            }
        }
        [HttpGet("All")]
        public IActionResult GetAll()
        {
            var action = _SublocationService.GetAll();
            if(string.IsNullOrEmpty(action.ErrorMessage))
            {
                return Ok(action.Result);
            }
            else
            {
                return BadRequest(new { ErrorMessage = action.ErrorMessage });
            }

        }
        [HttpPut("UpdateSublocation")]
        public IActionResult UpdateSublocation(SubLocation model)
        {
            var action = _SublocationService.UpdateSublocation(model);
            if(string.IsNullOrEmpty(action.ErrorMessage))
            {
                return Ok(action.Result);
            }
            else
            {
                return BadRequest(new { Errormessage = action.ErrorMessage });
            }
        }


    }
}
