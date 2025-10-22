using ChatNest.Models.Domain;
using ChatNest.Models.DTO;
using ChatNest.Repositories;
using ChatNest.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatNest.Controllers
{
    [Authorize(Roles = "admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminRepository adminRepository;

        public AdminController(IAdminRepository adminRepository)
        {
            this.adminRepository = adminRepository;
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [Route("AdminChangeUserRole")]
        public async Task<ActionResult<UserResponseModel>> AdminChangeUserRoleAsync([FromBody] AdminChangeUserRoleDTO adminChangeUserRoleDTO)
        {
            if (adminChangeUserRoleDTO == null)
                return BadRequest(new { MessageId = -6, MessageDescription = "Post data is required." });

            var userResponseModelDetailed = await adminRepository.AdminChangeUserRoleAsync(adminChangeUserRoleDTO);

            if (userResponseModelDetailed.MessageID == -5)
                return BadRequest(new { MessageId = userResponseModelDetailed.MessageID, MessageDescription = userResponseModelDetailed.MessageDescription });

            if (userResponseModelDetailed.MessageID == -99)
                return StatusCode(500, new { MessageId = userResponseModelDetailed.MessageID, MessageDescription = userResponseModelDetailed.MessageDescription });

            if (userResponseModelDetailed.MessageID == -100)
                return StatusCode(500, new { MessageId = userResponseModelDetailed.MessageID, MessageDescription = userResponseModelDetailed.MessageDescription });

            return Ok(new { MessageId = userResponseModelDetailed.MessageID, MessageDescription = userResponseModelDetailed.MessageDescription, User = userResponseModelDetailed.User });
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [Route("AdminToggleUserStatus")]
        public async Task<ActionResult<AdminResponseModelDTO>> AdminToggleUserStatusAsync([FromBody] AdminToggleUserStatusDTO adminToggleUserStatusDTO)
        {
            if (adminToggleUserStatusDTO == null)
                return BadRequest(new { MessageId = -6, MessageDescription = "Post data is required." });

            var adminResponseModel = await adminRepository.AdminToggleUserStatusAsync(adminToggleUserStatusDTO);

            if (adminResponseModel.MessageID == -5)
                return BadRequest(new { MessageId = adminResponseModel.MessageID, MessageDescription = adminResponseModel.MessageDescription });

            if (adminResponseModel.MessageID == -99)
                return StatusCode(500, new { MessageId = adminResponseModel.MessageID, MessageDescription = adminResponseModel.MessageDescription });

            if (adminResponseModel.MessageID == -100)
                return StatusCode(500, new { MessageId = adminResponseModel.MessageID, MessageDescription = adminResponseModel.MessageDescription });

            return Ok(new { MessageId = adminResponseModel.MessageID, MessageDescription = adminResponseModel.MessageDescription });

        }

    }
}
