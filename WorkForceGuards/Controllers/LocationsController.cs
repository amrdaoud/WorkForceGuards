using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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

    public class LocationsController : ControllerBase
    {
        private readonly ILocationService _locationService;
        public LocationsController(ILocationService locationService)
        {
            _locationService = locationService;
        }
        [HttpGet]
        public ActionResult Get()
        {
            return Ok(_locationService.GetAll());
        }
        [HttpGet("{id}")]
        public ActionResult GetById(int id)
        {
            return Ok(_locationService.GetById(id));
        }
        [Authorize(Policy = "Admin")]
        [HttpPost]
        public ActionResult Add(Location model)
        {
            if(!ModelState.IsValid)
            {

                return BadRequest();
            }
            var action = _locationService.Add(model);
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
        public ActionResult Edit(int id, Location model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { ErrorMessage = "Name between 3 and 50 characters" });
            }
            if (id != model.Id)
            {
                return BadRequest();
            }
            var action = _locationService.Update(model);
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
            return Ok(_locationService.CheckValue(value));
        }


    }
}
