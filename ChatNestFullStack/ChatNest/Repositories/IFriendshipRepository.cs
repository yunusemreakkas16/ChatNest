using ChatNest.Models.Domain;
using static ChatNest.Models.Domain.Friendship;

namespace ChatNest.Repositories
{
    public interface IFriendshipRepository
    {
        Task<FriendListResponseModel> GetFriendListAsync(Guid userId);
        Task<FriendRequestListResponseModel> GetFriendRequestsAsync(Guid userId, string direction);
        Task<FriendshipStatusResponseModel> CheckFriendshipStatusAsync(Guid userId, Guid targetUserId);
        Task<SendFriendRequestResponseModel> SendFriendRequestAsync(Guid requesterId, string receiverEmail);
        Task<RemoveFriendResponseModel> RemoveFriendAsync(Guid userId, Guid friendId);
        Task<ManageFriendRequestResponseModel> ManageFriendRequestAsync(Guid userId, Guid requesterId, string action);
    }
}
