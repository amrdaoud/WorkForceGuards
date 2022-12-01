using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkForceManagementV0.Repositories.ActiveDirectory;

namespace WorkForceManagementV0.Controllers
{
    [Authorize(Policy="SuperUser")]
    [Route("api/[controller]")]
    [ApiController]
    public class ActiveDirectoryController : ControllerBase
    {
        private readonly IActiveDirectory _ad;
        public ActiveDirectoryController(IActiveDirectory ad)
        {
            _ad = ad;
        }
        [HttpGet("groups")]
        public IActionResult GetGroups()
        {
            return Ok(_ad.GetGroups());
        }
        [HttpGet("groups/{searchQuery}")]
        public IActionResult GetGroups(string searchQuery)
        {
            return Ok(_ad.GetGroupsBySearch(searchQuery));
        }

        [HttpGet("users")]
        public IActionResult GetUsers()
        {
            return Ok(_ad.GetUsers());
        }
        [HttpGet("users/{searchQuery}")]
        public IActionResult GetUsers(string searchQuery)
        {
            return Ok(_ad.GetUsersBySearch(searchQuery));
        }
    }
}
