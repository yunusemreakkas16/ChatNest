using ChatNest.Repositories;
using static ChatNest.Models.Domain.Friendship;
using static ChatNest.Services.IFriendshipService;
namespace ChatNest.Services
{
    public class FriendshipService : IFriendshipService
    {
        public FriendshipService(IFriendshipRepository friendshipRepository)
        {
            FriendshipRepository = friendshipRepository;
        }

        public IFriendshipRepository FriendshipRepository { get; }

        public async Task<FriendRequestListResponseModel> GetFriendRequestsAsync(Guid userId, FriendRequestDirection direction)
        {
            var directionString = direction.ToString().ToLower(); // sent or received

            var result = await FriendshipRepository.GetFriendRequestsAsync(userId, directionString);
            return result;
        }

        public async Task<FriendListResponseModel> GetFriendsAsync(Guid userId)
        {
            var response = await FriendshipRepository.GetFriendListAsync(userId);
            return response;
        }

        public async Task<FriendshipStatusResponseModel> GetFriendshipStatusAsync(Guid userId, Guid targetUserId)
        {
            return await FriendshipRepository.CheckFriendshipStatusAsync(userId, targetUserId);
        }

        public async Task<RemoveFriendResponseModel> RemoveFriendAsync(Guid userId, Guid friendId)
        {
            return await FriendshipRepository.RemoveFriendAsync(userId, friendId);
        }

        public async Task<ManageFriendRequestResponseModel> ManageFriendRequestAsync(Guid clientUserID, Guid otherUserID, IFriendshipService.FriendRequestAction action)
        {
            var actionString = action.ToString().ToLower();
            var response = await FriendshipRepository.ManageFriendRequestAsync(clientUserID, otherUserID, actionString);
            return response;
        }

        public async Task<SendFriendRequestResponseModel> SendFriendRequestAsync(Guid requesterId, string receiverEmail)
        {
            var response = await FriendshipRepository.SendFriendRequestAsync(requesterId, receiverEmail);
            return response;
        }
    }
}
