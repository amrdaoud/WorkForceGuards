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
    public class AttendanceTypesController : ControllerBase
    {
        private readonly IAttendanceTypeService _attendanceTypeService;
        public AttendanceTypesController(IAttendanceTypeService attendanceTypeService)
        {
            _attendanceTypeService = attendanceTypeService;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _attendanceTypeService.GetAll();
            return Ok(result);
        }
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var result = _attendanceTypeService.GetById(id);
            if(result == null)
            {
                return NotFound();
            }
            return Ok(id);
        }
        [Authorize(Policy ="Admin")]
        [HttpPost]
        public IActionResult Add(AttendanceType model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = _attendanceTypeService.Add(model);
            if(!string.IsNullOrEmpty(result.ErrorMessage))
            {
                return BadRequest(new { ErrorMessage = result.ErrorMessage });
            }
            return Ok(result.Result);
        }
        [Authorize(Policy = "Admin")]
        [HttpPut("{id}")]
        public IActionResult Update(int id, AttendanceType model)
        {
            if(!ModelState.IsValid || id != model.Id)
            {
                return BadRequest();
            }
            var result = _attendanceTypeService.Update(model);
            if (!string.IsNullOrEmpty(result.ErrorMessage))
            {
                return BadRequest(new { ErrorMessage = result.ErrorMessage });
            }
            return Ok(result.Result);
        }
        [HttpGet("checkname")]
        public IActionResult CheckUniqueName(string name)
        {
            return Ok(_attendanceTypeService.CheckUniqueName(name));
        }
    }
}
