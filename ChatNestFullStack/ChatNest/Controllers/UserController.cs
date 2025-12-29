using Azure;
using ChatNest.Models.Common;
using ChatNest.Models.Domain;
using ChatNest.Models.DTO;
using ChatNest.Services;
using Microsoft.AspNetCore.Authorization;
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

            var response = await userService.CreateUserAsync(createUserDto);

            return response.MessageID switch
            {
                1 => Ok(response),
                -2 => BadRequest(response),
                0 => BadRequest(response),
                -99 => StatusCode(500, response),
                -100 => StatusCode(500, response),
                _ => BadRequest(response)
            };

        }

        [Authorize]
        [HttpPost]
        [Route("UserDetail")]
        public async Task<ActionResult<UserResponseModelDetailed>> GetUserDetailedAsync([FromBody] UserParamModel userParam)
        {
            if (userParam == null || userParam.UserID == Guid.Empty)
                return BadRequest(new { MessageId = -6, MessageDescription = "User parameter cannot be null or empty." });

            var response = await userService.GetUserDetailedAsync(userParam);

            return response.MessageID switch
            {
                1 => Ok(response),
                -99 => StatusCode(500, response),
                -100 => StatusCode(500, response),
                _ => BadRequest(response)
            };

        }

        [Authorize]
        [HttpPost]
        [Route("UpdateUser")]
        public async Task<ActionResult<UserResponseModelDetailed>> UpdateUserAsync([FromBody] UpdateUserRequestDto updateUserRequestDto)
        {
            if (updateUserRequestDto == null || updateUserRequestDto.UserID == Guid.Empty)
                return BadRequest(new { MessageId = -6, MessageDescription = "Update user request cannot be null or empty." });

            var response = await userService.UpdateUserAsync(updateUserRequestDto);

            return response.MessageID switch
            {
                1 => Ok(response),
                -2 => BadRequest(response),
                -1 => BadRequest(response),
                -99 => StatusCode(500, response),
                -100 => StatusCode(500, response),
                _ => BadRequest(response)
            };

        }

        [Authorize]
        [HttpPost]
        [Route("SoftDeleteUser")]
        public async Task<ActionResult<BaseResponse>> SoftDeleteUserAsync([FromBody] UserParamModel userParam)
        {
            if (userParam == null || userParam.UserID == Guid.Empty)
                return BadRequest(new { MessageId = -6, MessageDescription = "User parameter cannot be null or empty." });
            var result = await userService.SoftDeleteUserAsync(userParam);

            return result.MessageID switch
            {
                1 => Ok(result),
                -1 => BadRequest(result),
                -99 => StatusCode(500, result),
                -100 => StatusCode(500, result),
                _ => BadRequest(result)
            };
        }

        [Authorize]
        [HttpPost]
        [Route("ReActivateUser")]
        public async Task<ActionResult<UserResponseModelDetailed>> ReActivateUserAsync([FromBody] UserParamModel userParam)
        {
            if (userParam == null || userParam.UserID == Guid.Empty)
                return BadRequest(new { MessageId = -6, MessageDescription = "User parameter cannot be null or empty." });

            var response = await userService.ReActivateUserAsync(userParam);

            return response.MessageID switch
            {
                1 => Ok(response),
                -99 => StatusCode(500, response),
                -100 => StatusCode(500, response),
                _ => BadRequest(response)
            };
        }
    }
}
