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
    public class AssetTermValueController : ControllerBase
    {
        private readonly IAssetTermValueServive _IAssetTermValueServive;
        public AssetTermValueController(IAssetTermValueServive assetTermValueServive)
        {
            _IAssetTermValueServive = assetTermValueServive;
        }

        [HttpGet]
        public ActionResult Get()
        {
            return Ok(_IAssetTermValueServive.GetAll());
        }
        [HttpGet("{id}")]
        public ActionResult GetById(int id)
        {
            return Ok(_IAssetTermValueServive.GetById(id));
        }
        [HttpPost]
        public ActionResult Add(AssetTermValue model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return Ok(_IAssetTermValueServive.Add(model));
        }
        [HttpPut("{id}")]
        public ActionResult Edit(int id, AssetTermValue model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (id != model.Id)
            {
                return BadRequest(new { message = "model id missmatch with request id " });
            }
            return Ok(_IAssetTermValueServive.Update(model));
        }

        
    }
}
