using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkForceManagementV0.Models;
using WorkForceManagementV0.Models.Bindings;
using WorkForceManagementV0.Repositories.Identity;
using WorkForceManagementV0.Repositories.Interfaces;

namespace WorkForceManagementV0.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SwapRequestsController : ControllerBase
    {
        private readonly ISwapRequestService _swapRequestService;
        private readonly IUserService _userService;
        public SwapRequestsController(ISwapRequestService swapRequestService, IUserService userService)
        {
            _swapRequestService = swapRequestService;
            _userService = userService;
        }
       
        [HttpPost]
        public IActionResult Add(SwapRequest model)
        {
            var appUser = _userService.GetUserInfo(User);
            model.RequesterAlias = appUser.Alias;
            var result = _swapRequestService.AddRequest(model);
            if (!string.IsNullOrEmpty(result.ErrorMessage))
            {
                return BadRequest(new { ErrorMessage = result.ErrorMessage });
            }
            return Ok(result.Result);
        }
       
        [Authorize(Policy ="Hos")]
        [HttpPost("handle")]
        public IActionResult Handle(SwapRequestApprovalBinding model)
        {
            var result = _swapRequestService.Approve(model, User);
            if (!string.IsNullOrEmpty(result.ErrorMessage))
            {
                return BadRequest(new { ErrorMessage = result.ErrorMessage });
            }
            return Ok(result.Result);
        }
       
        [HttpGet("involved")]
        public IActionResult GetInvolved(int scheduleId)
        {
            var result = _swapRequestService.GetInvolvedRequests(User);
            if (!string.IsNullOrEmpty(result.ErrorMessage))
            {
                return BadRequest(new { ErrorMessage = result.ErrorMessage });
            }
            return Ok(result.Result);
        }
        
        [HttpGet("myschedules")]
        public IActionResult GetSchedules()
        {
            var appUser = _userService.GetUserInfo(User);
            var result = _swapRequestService.GetMySchedules(appUser.Alias);
            if (!string.IsNullOrEmpty(result.ErrorMessage))
            {
                return BadRequest(new { ErrorMessage = result.ErrorMessage });
            }
            return Ok(result.Result);
        }
       
        [HttpGet("MyDayOffs/{scheduleId}")]
        public IActionResult GetMyDayOffs(int scheduleId)
        {
            var appUser = _userService.GetUserInfo(User);
            var result = _swapRequestService.GetMyDayOffs(appUser.Alias, scheduleId);
            if(!string.IsNullOrEmpty(result.ErrorMessage))
            {
                return BadRequest(new { ErrorMessage = result.ErrorMessage });
            }
            return Ok(result.Result);
        }
       
        [HttpGet("MySiblings/{scheduleId}")]
        public IActionResult GetMySiblings(int scheduleId)
        {
            var appUser = _userService.GetUserInfo(User);
            var result = _swapRequestService.GetSiblings(appUser.Alias, scheduleId);
            if (!string.IsNullOrEmpty(result.ErrorMessage))
            {
                return BadRequest(new { ErrorMessage = result.ErrorMessage });
            }
            return Ok(result.Result);
        }
        [HttpGet("DestDayOffs/{staffId}/{scheduleId}")]
        public IActionResult GetDestinationDayOffs(int staffId, int scheduleId)
        {
            var result = _swapRequestService.GetDestinationDayOffs(staffId, scheduleId);
            if (!string.IsNullOrEmpty(result.ErrorMessage))
            {
                return BadRequest(new { ErrorMessage = result.ErrorMessage });
            }
            return Ok(result.Result);
        }
        //[HttpGet("reverse/{id}")]
        //public IActionResult ReverseRequest(int id)
        //{
        //    var appUser = _userService.GetUserInfo(User);
        //    var result = _swapRequestService.ReverseSwapRequest(id, appUser.Alias);
        //    if (!string.IsNullOrEmpty(result.ErrorMessage))
        //    {
        //        return BadRequest(new { ErrorMessage = result.ErrorMessage });
        //    }
        //    return Ok(result.Result);
        //}

    }
}
