using Entity.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utility.StaticData;

namespace HRMApi.Controllers
{
    [ApiController]
    [Route(StaticData.API_CONTROLLER_ROUTE), Authorize]
    public class LeaveRequestController : ControllerBase
    {
        private readonly ILeaveRequestService _leaveRequestService;
        public LeaveRequestController(ILeaveRequestService leaveRequestService)
        {
            _leaveRequestService = leaveRequestService;
        }

        // GET: api/LeaveRequest
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = await _leaveRequestService.GetLeaveRequests();
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        // GET api/LeaveRequest/5
        [HttpGet(StaticData.ID)]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                var result = await _leaveRequestService.GetLeaveRequest(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        // POST: api/LeaveRequest
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]DataAccess.Models.LeaveRequest leave)
        {
            try
            {
                var result = await _leaveRequestService.AddLeaveRequest(leave);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // PUT api/LeaveRequest
        [HttpPut]
        public async Task<IActionResult> Edit([FromBody]LeaveRequestVm leave)
        {
            try
            {
                var result = await _leaveRequestService.EditLeaveRequest(leave);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // PUT api/LeaveRequest/Approve
        [HttpPut]
        public async Task<IActionResult> Approve([FromBody] LeaveRequestVm leave)
        {
            try
            {
                var result = await _leaveRequestService.ApproveLeaveRequest(leave);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        // DELETE api/LeaveRequest
        [HttpDelete(StaticData.ID)]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var result = await _leaveRequestService.DeleteLeaveRequest(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
