using Microsoft.AspNetCore.Mvc;
using WorkForceManagementV0.Models.Bindings;
using WorkForceManagementV0.Models;
using WorkForceManagementV0.Repositories.Interfaces;
using WorkForceManagementV0.Repositories;

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

        [HttpPost]
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
        [HttpGet]
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
        [HttpPut("{id}")]
        public IActionResult UpdateSublocation(int id, SubLocation model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (id != model.Id)
            {
                return BadRequest();
            }
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

        [HttpGet("checkUniq")]

        public ActionResult CheckUniq(string value)
        {
            return Ok(_SublocationService.CheckUniq(value));
        }


    }
}
