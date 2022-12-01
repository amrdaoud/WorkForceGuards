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
    public class StaffMemberController : ControllerBase
    {
        private readonly IStaffMemberService _IStaffMemberService;
        public StaffMemberController(IStaffMemberService staffMemberService)
        {
            _IStaffMemberService = staffMemberService;
        }
        [HttpPost("getFilter")]
        
        public ActionResult GetFilter(FilterModel f)
        {
            return Ok(_IStaffMemberService.GetAll(f));
        }
        [HttpGet("{id}")]
        public ActionResult GetById(int id)
        {
            return Ok(_IStaffMemberService.GetById(id));
        }
        [Authorize(Policy = "Admin")]
        [HttpPost]
        public ActionResult Add(StaffMember model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var action = _IStaffMemberService.Add(model);

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
        public ActionResult Edit(int id, StaffMember model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (id != model.Id)
            {
                return BadRequest();
            }
            var action = _IStaffMemberService.Update(model);

            if (string.IsNullOrEmpty(action.ErrorMessage))
            {
                return Ok(action.Result);

            }
            else
            {

                return BadRequest(new { ErrorMessage = action.ErrorMessage });

            }
        }

        
        

        [HttpGet("getEmployeeIds")]

        public ActionResult getEmployeeIds()
        {
            return Ok(_IStaffMemberService.GetEmployeeId());
        }

        [HttpGet("checkUniq")]

        public ActionResult checkUniq(string value)
        {
            return Ok(_IStaffMemberService.CheckValue(value));
        }
        [HttpGet("checkEmailvalue")]

        public ActionResult CheckUniqEmail(string Email)
        {
            return Ok(_IStaffMemberService.CheckUniqEmail(Email));
        }
        [HttpGet("checkAliasValue")]

        public ActionResult CheckUniqAlias(string Alias)
        {
            return Ok(_IStaffMemberService.CheckUniqAlias(Alias));
        }
        [HttpGet("CheckNameValue")]

        public ActionResult CheckUniqName(string Name)
        {
            return Ok(_IStaffMemberService.CheckUniqName(Name));
        }
    }
}
