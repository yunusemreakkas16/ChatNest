using ChatNest.Models.Domain;

namespace ChatNest.Repositories
{
    public interface IChatRepository
    {
        // think about service approach maybe toggle?
        Task<CreateChatResponseModel> CreateChatAsync(Chat chat, List<Guid>? participantIDs, Guid? targetUserID);

        // Get last messages for all chats of a user
        Task<ChatResponseModel> GetChatSummariesForUserAsync(Guid userID);

        // update chat name (for group chats)
        Task<UpdateGroupNameResponseModel> UpdateGroupNameAsync(Guid chatID, Guid userID, string newName);

        // add user to chat (for group chats)
        Task<AddUserToChatResponseModel> AddUserToChatAsync(Guid chatID, Guid adminID, List<Guid>? userIDs);

        // Remove user from chat (for group chats)
        Task<RemoveUserFromChatResponseModel> RemoveUserFromChatAsync(Guid chatID, Guid adminID, Guid userID);

        // leave chat (for group chats)
        Task<LeaveChatResponseModel> LeaveChatAsync(Guid chatID, Guid userID);

        // Delete chat (for group chats)
        Task<DeleteChatResponseModel> DeleteChatAsync(Guid chatID, Guid requesterID);

        // Manage group admin (for group chats)
        Task<ManageGroupAdmin> SetGroupAdminAsync(Guid chatID, Guid adminID, Guid userID, bool makeAdmin);
    }
}
