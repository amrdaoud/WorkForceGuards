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
    public class HeadOfSectionController : ControllerBase
    {
        private readonly IHeadOfSectionService _IHeadOfSectionService;
        public HeadOfSectionController(IHeadOfSectionService headOfSectionService)
        {
            _IHeadOfSectionService = headOfSectionService;
        }
        [HttpGet]
        public ActionResult Get()
        {
            return Ok(_IHeadOfSectionService.GetAll());
        }
        [HttpGet("{id}")]
        public ActionResult GetById(int id)
        {
            return Ok(_IHeadOfSectionService.GetById(id));
        }
        [Authorize(Policy = "Admin")]
        [HttpPost]
        public ActionResult Add(HeadOfSection model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var action = _IHeadOfSectionService.Add(model);
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
        public ActionResult Edit(int id, HeadOfSection model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (id != model.Id)
            {
                return BadRequest();
            }
            var action = _IHeadOfSectionService.Update(model);
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

        public ActionResult checkUniq(int employeeId)
        {
            return Ok(_IHeadOfSectionService.CheckIdValue(employeeId));
        }
        [HttpGet("checkEmailvalue")]

        public ActionResult CheckUniqEmail(string Email)
        {
            return Ok(_IHeadOfSectionService.CheckEmailValue(Email));
        }
        [HttpGet("checkAliasValue")]

        public ActionResult CheckUniqAlias(string Alias)
        {
            return Ok(_IHeadOfSectionService.CheckAliasValue(Alias));
        }
        [HttpGet("CheckNameValue")]

        public ActionResult CheckUniqName(string Name)
        {
            return Ok(_IHeadOfSectionService.CheckNameValue(Name));
        }

    }
}
