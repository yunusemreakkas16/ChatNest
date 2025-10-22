using ChatNest.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ChatNest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FriendshipController : ControllerBase
    {
        private readonly IFriendshipService friendshipService;

        public FriendshipController(IFriendshipService friendshipService)
        {
            this.friendshipService = friendshipService;
        }

        [Authorize]
        [HttpPost]
        [Route("SendFriendRequest")]
        public async Task<ActionResult> SendFriendRequestAsync(Guid requesterId, string recieverEmail)
        {
            var response = await friendshipService.SendFriendRequestAsync(requesterId, recieverEmail);

            return response.MessageID switch
            {
                1 or 2 or 3 => Ok(response),
                -2 => NotFound(response),
                -3 or -5 => BadRequest(response),
                -4 or -6 => Conflict(response),
                -99 => StatusCode(500, response),
                _ => BadRequest(response)
            };

        }

        [Authorize]
        [HttpPost]
        [Route("GetFriendRequests")]
        public async Task<ActionResult> GetFriendRequestsAsync(Guid userId, IFriendshipService.FriendRequestDirection direction)
        {
            var response = await friendshipService.GetFriendRequestsAsync(userId, direction);
            return response.MessageID switch
            {
                1 => Ok(response),              
                0 => Ok(response),              
                -1 => BadRequest(response),     
                -99 => StatusCode(500, response),
                _ => BadRequest(response)       
            };
        }

        [Authorize]
        [HttpPost]
        [Route("GetFriendList")]
        public async Task<ActionResult> GetFriendsAsync(Guid userId)
        {
            var response = await friendshipService.GetFriendsAsync(userId);
            return response.MessageID switch
            {
                1 => Ok(response),
                0 => Ok(response),
                -99 => StatusCode(500, response),
                _ => BadRequest(response)
            };
        }

        [Authorize]
        [HttpPost]
        [Route("RespondToFriendRequest")]
        public async Task<ActionResult> RespondToFriendRequestAsync(Guid userId, Guid requesterId, IFriendshipService.FriendRequestAction action)
        {
            var response = await friendshipService.RespondToFriendRequestAsync(userId, requesterId, action);
            return response.MessageID switch
            {
                1 => Ok(response),
                0 => NotFound(response),
                -1 => BadRequest(response),
                -99 => StatusCode(500, response),
                _ => BadRequest(response)
            };
        }

        [Authorize]
        [HttpPost]
        [Route("FriendShipStatus")]
        public async Task<ActionResult> GetFriendshipStatusAsync(Guid userId, Guid targetUserId)
        {
            var response = await friendshipService.GetFriendshipStatusAsync(userId, targetUserId);
            return response.MessageID switch
            {
                1 => Ok(response),
                -1 => BadRequest(response),
                -99 => StatusCode(500, response),
                _ => BadRequest(response)
            };
        }

        [Authorize]
        [HttpPost]
        [Route("RemoveFriend")]
        public async Task<ActionResult> RemoveFriendAsync(Guid userId, Guid friendId)
        {
            var response = await friendshipService.RemoveFriendAsync(userId, friendId);
            return response.MessageID switch
            {
                1 => Ok(response),
                0 => NotFound(response),
                -99 => StatusCode(500, response),
                _ => BadRequest(response)
            };
        }


    }
}
