using ChatNest.Models.Common;
using ChatNest.Models.Domain;
using ChatNest.Models.DTO;
using ChatNest.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatNest.Controllers
{
    [Authorize(Roles = "admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService adminService;

        public AdminController(IAdminService adminService)
        {
            this.adminService = adminService;
        }

        [HttpPost]
        [Route("AdminChangeUserRole")]
        public async Task<ActionResult<UserResponseModel>> AdminChangeUserRoleAsync([FromBody] AdminChangeUserRoleDTO adminChangeUserRoleDTO)
        {
            if (adminChangeUserRoleDTO == null)
                return BadRequest(new { MessageId = -6, MessageDescription = "Post data is required." });

            var response = await adminService.AdminChangeUserRoleAsync(adminChangeUserRoleDTO);

            return response.MessageID switch
            {
                1 => Ok(response),
                -5 => Forbid(),
                -99 => StatusCode(500, response),
                -100 => StatusCode(500, response),
                _ => BadRequest(response)
            };
        }

        [HttpPost]
        [Route("AdminToggleUserStatus")]
        public async Task<ActionResult<BaseResponse>> AdminToggleUserStatusAsync([FromBody] AdminToggleUserStatusDTO adminToggleUserStatusDTO)
        {
            if (adminToggleUserStatusDTO == null)
                return BadRequest(new { MessageId = -6, MessageDescription = "Post data is required." });

            var response = await adminService.AdminToggleUserStatusAsync(adminToggleUserStatusDTO);

            return response.MessageID switch
            {
                1 => Ok(response),
                -5 => Forbid(),
                -99 => StatusCode(500, response),
                -100 => StatusCode(500, response),
                _ => BadRequest(response)
            };
        }

        [HttpPost]
        [Route("AdminGetUsers")]
        public async Task<ActionResult<UserResponseModelList>> AdminGetUsersAsync(Guid AdminID)
        {
            var response = await adminService.AdminGetUsersAsync(AdminID);
            return response.MessageID switch
            {
                1 => Ok(response),
                -5 => Forbid(),
                -99 => StatusCode(500, response),
                -100 => StatusCode(500, response),
                _ => BadRequest(response)
            };
        }

    }
}
