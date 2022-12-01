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
    public class AssetTermController : ControllerBase
    {
        private readonly IAssetTermService _IAssetTermService;
        public AssetTermController(IAssetTermService assetTermService)
        {
            _IAssetTermService = assetTermService;
        }

        [HttpGet]
        public ActionResult Get()
        {
            return Ok(_IAssetTermService.GetAll());
        }
        [HttpGet("{id}")]
        public ActionResult GetById(int id)
        {
            return Ok(_IAssetTermService.GetById(id));
        }
        [HttpPost]
        public ActionResult Add(AssetTerm model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return Ok(_IAssetTermService.Add(model));
        }
        [HttpPut("{id}")]
        public ActionResult Edit(int id, AssetTerm model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (id != model.Id)
            {
                return BadRequest();
            }
            return Ok(_IAssetTermService.Update(model));
        }











    }
}
