using System.Text.Json.Serialization;
using static ChatNest.Models.Domain.Friendship;

namespace ChatNest.Services
{
    public interface IFriendshipService
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum FriendRequestAction
        {
            Accept,
            Reject
        }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public enum FriendRequestDirection
        {
            Sent,
            Received
        }

        Task<SendFriendRequestResponseModel> SendFriendRequestAsync(Guid requesterId, string receiverEmail);
        Task<FriendRequestListResponseModel> GetFriendRequestsAsync(Guid userId, FriendRequestDirection direction);
        Task<ManageFriendRequestResponseModel> RespondToFriendRequestAsync(Guid userId, Guid requesterId, FriendRequestAction action);
        Task<FriendListResponseModel> GetFriendsAsync(Guid userId);
        Task<RemoveFriendResponseModel> RemoveFriendAsync(Guid userId, Guid friendId);
        Task<FriendshipStatusResponseModel> GetFriendshipStatusAsync(Guid userId, Guid targetUserId);
    }
}
