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
    public class StaffTypeController : ControllerBase
    {
        private readonly IStaffTypeService _IStaffTypeService;
        public StaffTypeController(IStaffTypeService staffTypeService)
        {
            _IStaffTypeService = staffTypeService;
        }
        [HttpGet]
        public ActionResult Get()
        {
            return Ok(_IStaffTypeService.GetAll());
        }
        [HttpGet("{id}")]
        public ActionResult GetById(int id)
        {
            return Ok(_IStaffTypeService.GetById(id));
        }
        [Authorize(Policy = "Admin")]
        [HttpPost]
        public ActionResult Add(StaffType model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var action = _IStaffTypeService.Add(model);
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
        public ActionResult Edit(int id, StaffType model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (id != model.Id)
            {
                return BadRequest();
            }
            var action = _IStaffTypeService.Update(model);

            if (string.IsNullOrEmpty(action.ErrorMessage))
            {
                return Ok(action.Result);

            }
            else
            {

                return BadRequest(new { ErrorMessage = action.ErrorMessage });

            }
        }


        [HttpGet("checkUniq")]

        public ActionResult checkUniq(string value)
        {
            return Ok(_IStaffTypeService.CheckValue(value));
        }
    }
}
