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
    [Route("api/[controller]")]
    [ApiController]
    public class AssetTypesController : ControllerBase
    {
        private readonly IAssetTypeService _IAssetTypeService;
        public AssetTypesController(IAssetTypeService assetTypeService)
        {
            _IAssetTypeService = assetTypeService;
        }

        [HttpGet]
        public ActionResult Get()
        {
            return Ok(_IAssetTypeService.GetAll());
        }
        [HttpGet("{id}")]
        public ActionResult GetById(int id)
        {
            return Ok(_IAssetTypeService.GetById(id));
        }
        [HttpPost]
        public ActionResult Add(AssetType model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var action = _IAssetTypeService.Add(model);
            if (string.IsNullOrEmpty(action.ErrorMessage))
            {
                return Ok(action.Result);

            }
            else
            {

                return BadRequest(new { ErrorMessage = action.ErrorMessage });

            }
        }
        [HttpPut("{id}")]
        public ActionResult Edit(int id, AssetType model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (id != model.Id)
            {
                return BadRequest(new { message = "model id missmatch with request id " });
            }
            var action = _IAssetTypeService.Update(model);
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
            return Ok(_IAssetTypeService.CheckValue(value));
        }

    }
}
