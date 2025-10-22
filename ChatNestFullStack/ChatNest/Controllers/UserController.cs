using Azure;
using ChatNest.Models.Domain;
using ChatNest.Models.DTO;
using ChatNest.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatNest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpPost]
        [Route("AddUser")]
        public async Task<ActionResult<UserResponseModel>> AddUserAsync([FromBody] CreateUserRequestDto createUserDto)
        {
            if (createUserDto == null)
                return BadRequest(new { MessageId = -6, MessageDescription = "Post data is required." });

            var userResponseModel = await userService.CreateUserAsync(createUserDto);

            if (userResponseModel.MessageID == -2)
                return BadRequest(new { MessageId = userResponseModel.MessageID, MessageDescription = userResponseModel.MessageDescription });
            if (userResponseModel.MessageID == 0)
                return BadRequest(new { MessageId = userResponseModel.MessageID, MessageDescription = userResponseModel.MessageDescription });

            if (userResponseModel.MessageID == -99)
                return StatusCode(500, new { MessageId = -99, MessageDescription = userResponseModel.MessageDescription });

            if (userResponseModel.MessageID == -100)
                return StatusCode(500, new { MessageId = -100, MessageDescription = userResponseModel.MessageDescription });

            return Ok(new { MessageId = userResponseModel.MessageID, MessageDescription = userResponseModel.MessageDescription, User = userResponseModel.User });
        }

        [Authorize]
        [HttpPost]
        [Route("UserList")]
        
        public async Task<ActionResult<UserResponseModelList>> GetUsersAsync()
        {
            var userResponseModelList = await userService.GetUsersAsync();

            if (userResponseModelList.MessageID == -4)
                return NoContent();
            if (userResponseModelList.MessageID == -99)
                return StatusCode(500, new { MessageId = -99, MessageDescription = userResponseModelList.MessageDescription });
            if (userResponseModelList.MessageID == -100)
                return StatusCode(500, new { MessageId = -100, MessageDescription = userResponseModelList.MessageDescription });
            return Ok(new { MessageId = userResponseModelList.MessageID, MessageDescription = userResponseModelList.MessageDescription, Users = userResponseModelList.Users });
        }

        [Authorize]
        [HttpPost]
        [Route("UserDetail")]
        public async Task<ActionResult<UserResponseModelDetailed>> GetUserDetailedAsync([FromBody] UserParamModel userParam)
        {
            if (userParam == null || userParam.UserID == Guid.Empty)
                return BadRequest(new { MessageId = -6, MessageDescription = "User parameter cannot be null or empty." });
            var userResponseModelDetailed = await userService.GetUserDetailedAsync(userParam);
            if (userResponseModelDetailed.MessageID == -4)
                return Ok(new { MessageId = -4, MessageDescription = userResponseModelDetailed.MessageDescription });
            if (userResponseModelDetailed.MessageID == -99)
                return StatusCode(500, new { MessageId = -99, MessageDescription = userResponseModelDetailed.MessageDescription });
            if (userResponseModelDetailed.MessageID == -100)
                return StatusCode(500, new { MessageId = -100, MessageDescription = userResponseModelDetailed.MessageDescription });
            return Ok(new { MessageId = userResponseModelDetailed.MessageID, MessageDescription = userResponseModelDetailed.MessageDescription, User = userResponseModelDetailed.User });
        }

        [Authorize]
        [HttpPost]
        [Route("UpdateUser")]
        public async Task<ActionResult<UserResponseModelDetailed>> UpdateUserAsync([FromBody] UpdateUserRequestDto updateUserRequestDto)
        {
            if (updateUserRequestDto == null || updateUserRequestDto.UserID == Guid.Empty)
                return BadRequest(new { MessageId = -6, MessageDescription = "Update user request cannot be null or empty." });
            var userResponseModelDetailed = await userService.UpdateUserAsync(updateUserRequestDto);
            if (userResponseModelDetailed.MessageID == -2)
                return BadRequest(new { MessageId = userResponseModelDetailed.MessageID, MessageDescription = userResponseModelDetailed.MessageDescription });
            if (userResponseModelDetailed.MessageID == -1)
                return BadRequest(new { MessageId = userResponseModelDetailed.MessageID, MessageDescription = userResponseModelDetailed.MessageDescription });
            if (userResponseModelDetailed.MessageID == -99)
                return StatusCode(500, new { MessageId = userResponseModelDetailed.MessageID, MessageDescription = userResponseModelDetailed.MessageDescription });
            if (userResponseModelDetailed.MessageID == -100)
                return StatusCode(500, new { MessageId = -100, MessageDescription = userResponseModelDetailed.MessageDescription });
            return Ok(new { MessageId = userResponseModelDetailed.MessageID, MessageDescription = userResponseModelDetailed.MessageDescription, User = userResponseModelDetailed.User });
        }

        [Authorize]
        [HttpPost]
        [Route("SoftDeleteUser")]
        public async Task<ActionResult<bool>> SoftDeleteUserAsync([FromBody] UserParamModel userParam)
        {
            if (userParam == null || userParam.UserID == Guid.Empty)
                return BadRequest(new { MessageId = -6, MessageDescription = "User parameter cannot be null or empty." });
            var result = await userService.SoftDeleteUserAsync(userParam);

            return Ok(result);
        }

        [Authorize]
        [HttpPost]
        [Route("ReActivateUser")]
        public async Task<ActionResult<UserResponseModelDetailed>> ReActivateUserAsync([FromBody] UserParamModel userParam)
        {
            if (userParam == null || userParam.UserID == Guid.Empty)
                return BadRequest(new { MessageId = -6, MessageDescription = "User parameter cannot be null or empty." });
            var userResponseModelDetailed = await userService.ReActivateUserAsync(userParam);
            if (userResponseModelDetailed.MessageID == -4)
                return NoContent();
            if (userResponseModelDetailed.MessageID == -99)
                return StatusCode(500, new { MessageId = userResponseModelDetailed.MessageID, MessageDescription = userResponseModelDetailed.MessageDescription });
            if (userResponseModelDetailed.MessageID == -100)
                return StatusCode(500, new { MessageId = userResponseModelDetailed.MessageID, MessageDescription = userResponseModelDetailed.MessageDescription });
            return Ok(new { MessageId = userResponseModelDetailed.MessageID, MessageDescription = userResponseModelDetailed.MessageDescription, User = userResponseModelDetailed.User });
        }
    }
}
