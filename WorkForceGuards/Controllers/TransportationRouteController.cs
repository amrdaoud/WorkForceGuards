using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using WorkForceManagementV0.Models.Bindings;
using WorkForceManagementV0.Repositories.Interfaces;

namespace WorkForceManagementV0.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TransportationRouteController : ControllerBase
    {
        private readonly ITransportationRouteService _ITransportationRouteService;
        public TransportationRouteController(ITransportationRouteService transportationRouteService)
        {
            _ITransportationRouteService = transportationRouteService;
        }
        [HttpGet]
        public ActionResult Get()
        {
            return Ok(_ITransportationRouteService.GetAll());
        }
        [HttpGet("{id}")]
        public ActionResult GetById(int id)
        {
            return Ok(_ITransportationRouteService.GetById(id));
        }
        [Authorize(Policy = "Admin")]
        [HttpPost]
        public ActionResult Add(TransportationRouteBinding model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var action = _ITransportationRouteService.Add(model);
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
        public ActionResult Edit(int id, TransportationRouteBinding model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (id != model.Id)
            {
                return BadRequest();
            }
            var action = _ITransportationRouteService.Update(model);
            if (string.IsNullOrEmpty(action.ErrorMessage))
            {
                return Ok(action.Result);

            }
            else
            {

                return BadRequest(new { ErrorMessage = action.ErrorMessage });

            }
        }
        //[HttpGet("checkUniq")]

        //public ActionResult checkUniq(string value)
        //{
        //    return Ok(_ITransportationRouteService.CheckUniqValue(value));
        //}

        [HttpGet("ConvertTimeToendtInterval")]

        public ActionResult ConvertTimeToendtInterval(DateTime start,DateTime end)
        {
            return Ok(_ITransportationRouteService.ConvertTimeToendtInterval(start, end));
        }

        [HttpGet("ConvertintoDatetime")]

        public ActionResult ConvertintoDatetime(int id)
        {
            return Ok(_ITransportationRouteService.ConvertintoDatetime(id));
        }
        
        [HttpPost("AddTransportation")]
        public IActionResult AddTransportation(TransportationBinding model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
            var action = _ITransportationRouteService.AddTransportation(model);
            if(string.IsNullOrEmpty(action.ErrorMessage))
            {
                return Ok(action.Result);
            }
            else
            {
                 return BadRequest(new { Errormessag = action.ErrorMessage });
            }

        }
        [HttpGet("checkSublocValue")]
        public IActionResult checkSublocValue(TransportationBinding model)
        {
            return Ok(_ITransportationRouteService.checkSublocValue(model));
        }

    }
}
