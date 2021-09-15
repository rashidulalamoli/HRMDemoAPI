using DataAccess.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service;
using System;
using System.Threading.Tasks;
using Utility.StaticData;

namespace HRMApi.Controllers
{
    [ApiController]
    [Route(StaticData.API_CONTROLLER_ROUTE), Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/User
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = await _userService.GetUsers();
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // POST: api/User
        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> Post([FromBody]User user)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest("Invalid model");
                }
                var result = await _userService.AddUser(user);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // PUT: api/User
        [HttpPut]
        public async Task<IActionResult> Put([FromBody]User user)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    return BadRequest("Invalid model");
                }
                var result = await _userService.AddUser(user);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
