using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkForceManagementV0.Models;
using WorkForceManagementV0.Models.Bindings;
using WorkForceManagementV0.Repositories.Interfaces;

namespace WorkForceManagementV0.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetsController : ControllerBase
    {
        private readonly IAssetService _IAssetService;
        public AssetsController(IAssetService assetService)
        {
            _IAssetService = assetService;
        }

        [HttpGet]
        public ActionResult Get()
        {
            return Ok(_IAssetService.GetAll());
        }
        [HttpGet("{id}")]
        public ActionResult GetById(int id)
        {
            return Ok(_IAssetService.GetById(id));
        }
        [HttpPost]
        public ActionResult Add(Asset model)
        {
           
                if (!ModelState.IsValid)
                {
                    return BadRequest("Model Is Not Valid");
                }


            var action = _IAssetService.Add(model);
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
        public ActionResult Edit( int id,Asset model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (id != model.Id)
            {
                return BadRequest(new { message = "model id missmatch with request id " });
            }

            var action = _IAssetService.Update(model);
            if (string.IsNullOrEmpty(action.ErrorMessage))
            {
                return Ok(action.Result);

            }
            else
            {

                return BadRequest(new { ErrorMessage = action.ErrorMessage });
            }

        }

        [HttpPost("getFilter")]

        public ActionResult GetFilter(FilterModel f)
        {
            return Ok(_IAssetService.GetAll(f));
        }


       [HttpGet("checkUniq")]

       public ActionResult checkUniq(string value)
        {
            return Ok(_IAssetService.CheckValue(value));
        }


    }
}
