using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ShiftController : ControllerBase
    {
        private readonly IShiftService _IShiftService;

        public ShiftController(IShiftService shiftService)
        {
            _IShiftService = shiftService;
        }


        [HttpGet]
        public ActionResult GET()
        {
            return Ok(_IShiftService.GetAll());
        }
        [Authorize(Policy = "Admin")]
        [HttpPost]
        public ActionResult Add(ShiftBinding model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Model Is Not Valid");
            }


            var action = _IShiftService.Add(model);
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
        public ActionResult Edite(int id, ShiftBinding model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (id != model.Id)
            {
                return BadRequest(new { message = "model id missmatch with request id " });
            }

            var action = _IShiftService.Edit(model);
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
            return Ok(_IShiftService.GetById(id));
        }

        [Authorize(Policy = "Admin")]
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            return Ok(_IShiftService.Delete(id));
        }


        [HttpGet("checkUniqe")]
        public ActionResult CheckValue(string value, string ignoreName)
        {
            return Ok(_IShiftService.CheckValue(value, ignoreName));
        }

        [HttpGet("ConvertTimeToendtInterval")]
        public ActionResult ConvertTimeToendtInterval(TimeSpan start, TimeSpan end)
        {
            return Ok(_IShiftService.ConvertTimeToendtInterval(start, end));
        }





    }
}
