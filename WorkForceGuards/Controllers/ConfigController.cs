using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkForceManagementV0.Models.Bindings;
using WorkForceManagementV0.Repositories.Config;

namespace WorkForceManagementV0.Controllers
{
    [Authorize(Policy = "SuperUser")]
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigController : ControllerBase
    {
        private readonly IAppConfig _appConfig;
        public ConfigController(IAppConfig appConfig)
        {
            _appConfig = appConfig;
        }
        [HttpGet("rolesmapping")]
        public IActionResult GetRolesMapping()
        {
            return Ok(_appConfig.GetRolesMapping());
        }
        [HttpPost("rolesmapping")]
        public IActionResult PostRolesMapping(RolesMappingBinding model)
        {
            return Ok(_appConfig.SetRolesMapping(model));
        }
    }
}
