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
    public class ChatController : ControllerBase
    {
        private readonly IChatService chatService;

        public ChatController(IChatService chatService)
        {
            this.chatService = chatService;
        }

        [Authorize]
        [HttpPost]
        [Route("CreateChat")]
        public async Task<ActionResult<CreateChatResponseModel>> CreateChatAsync([FromBody] CreateChatRequestDTO createChatRequestDTO)
        {
            var response = await chatService.CreateChatAsync(createChatRequestDTO);

            return response.MessageID switch
            {
                1 => Ok(response),
                -1 => NotFound(response),
                -2 or -3 or -10 or -11 => BadRequest(response),
                -4 or -6 => Conflict(response),
                -99 => StatusCode(500, response),
                _ => BadRequest(response)
            };
        }

        [Authorize]
        [HttpPost]
        [Route("AddUserToChat")]
        public async Task<ActionResult<AddUserToChatResponseModel>> AddUserToChatAsync([FromBody] AddUserRequestDTO addUserRequestDTO)
        {
            var response = await chatService.AddUserToChatAsync(addUserRequestDTO);
            return response.MessageID switch
            {
                1 => Ok(response),
                -1 => NotFound(response),
                -2 or -3 => BadRequest(response),
                -99 => StatusCode(500, response),
                _ => BadRequest(response)
            };
        }

        [Authorize]
        [HttpPost]
        [Route("DeleteChat")]
        public async Task<ActionResult<DeleteChatResponseModel>> DeleteChatAsync(Guid chatID, Guid requesterID)
        {
            var response = await chatService.DeleteChatAsync(chatID, requesterID);
            return response.MessageID switch
            {
                1 => Ok(response),
                -1 => NotFound(response),
                -2 => BadRequest(response),
                -99 => StatusCode(500, response),
                _ => BadRequest(response)
            };
        }

        [Authorize]
        [HttpPost]
        [Route("GetChatSummariesForUser")]
        public async Task<ActionResult<ChatSummary>> GetChatSummariesForUserAsync(Guid userID)
        {
            var response = await chatService.GetChatSummariesForUserAsync(userID);
            return response.MessageID switch
            {
                1 => Ok(response),
                -1 or -2 => NotFound(response),
                -99 => StatusCode(500, response),
                _ => BadRequest(response)
            };
        }

        [Authorize]
        [HttpPost]
        [Route("LeaveChat")]
        public async Task<ActionResult<LeaveChatResponseModel>> LeaveChatAsync(Guid chatID, Guid userID)
        {
            var response = await chatService.LeaveChatAsync(chatID, userID);
            return response.MessageID switch
            {
                1 => Ok(response),
                -1 => NotFound(response),
                -2 or -3 or -4 => BadRequest(response),
                -99 => StatusCode(500, response),
                _ => BadRequest(response)
            };
        }

        [Authorize]
        [HttpPost]
        [Route("RemoveUserFromChat")]
        public async Task<ActionResult<RemoveUserFromChatResponseModel>> RemoveUserFromChatAsync(RemoveUserRequestDTO removeUserRequestDTO)
        {
            var response = await chatService.RemoveUserFromChatAsync(removeUserRequestDTO);
            return response.MessageID switch
            {
                1 => Ok(response),
                -1 => NotFound(response),
                -2 or -3 or -4 => BadRequest(response),
                -99 => StatusCode(500, response),
                _ => BadRequest(response)
            };
        }

        [Authorize]
        [HttpPost]
        [Route("SetGroupAdmin")]
        public async Task<ActionResult<ManageGroupAdmin>> SetGroupAdminAsync(SetGroupAdminRequestDTO setGroupAdminRequestDTO)
        {
            var response = await chatService.SetGroupAdminAsync(setGroupAdminRequestDTO);
            return response.MessageID switch
            {
                1 => Ok(response),
                -1 or -4 or -5 or -6 => BadRequest(response),
                -2 or -3 => NotFound(response),
                -99 => StatusCode(500, response),
                _ => BadRequest(response)
            };
        }

        [Authorize]
        [HttpPost]
        [Route("UpdateGroupName")]
        public async Task<ActionResult<UpdateGroupNameResponseModel>> UpdateGroupNameAsync(UpdateGroupNameRequestDTO updateGroupNameRequestDTO)
        {
            var response = await chatService.UpdateGroupNameAsync(updateGroupNameRequestDTO);
            return response.MessageID switch
            {
                1 => Ok(response),
               -1 or -2  => BadRequest(response),
                -99 => StatusCode(500, response),
                _ => BadRequest(response)
            };
        }

        [Authorize]
        [HttpPost]
        [Route("GetChatMembers")]
        public async Task<ActionResult<GetChatMembersResponseModel>> GetChatMembersAsync(Guid chatID)
        {
            var response = await chatService.GetChatMembersAsync(chatID);
            return response.MessageID switch
            {
                1 => Ok(response),
                -1 => NotFound(response),
                -99 => StatusCode(500, response),
                _ => BadRequest(response)
            };
        }
    }
}
