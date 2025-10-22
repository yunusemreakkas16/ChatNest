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
    public class MessageController : ControllerBase
    {
        private readonly IMessageService messageService;

        public MessageController(IMessageService messageService)
        {
            this.messageService = messageService;
        }

        [Authorize]
        [HttpPost]
        [Route("SendMessage")]
        public async Task<ActionResult<SendMessageResponseModel>> SendMessageAsync(Message message)
        {
            var response = await messageService.SendMessageAsync(message);

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
        [Route("GetMessagesList")]
        public async Task<ActionResult<MessagesListResponseModel>> GetMessagesListAsync(Guid chatID, Guid userID)
        {
            var response = await messageService.GetMessagesListAsync(chatID, userID);
            return response.MessageID switch
            {
                1 => Ok(response),
                -1 or -2 => BadRequest(response),
                -99 => StatusCode(500, response),
                _ => BadRequest(response)
            };
        }

        [Authorize]
        [HttpPost]
        [Route("DeleteMessage")]
        public async Task<ActionResult<DeleteMessageResponseModel>> DeleteMessageAsync(Guid messageID, Guid userID)
        {
            var response = await messageService.DeleteMessageAsync(messageID, userID);
            return response.MessageID switch
            {
                1 => Ok(response),
                -1 or -2 => BadRequest(response),
                -99 => StatusCode(500, response),
                _ => BadRequest(response)
            };
        }
    }
}
