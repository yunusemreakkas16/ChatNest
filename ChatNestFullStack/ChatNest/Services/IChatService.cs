using ChatNest.Models.Domain;
using ChatNest.Models.DTO;

namespace ChatNest.Services
{
    public interface IChatService
    {
        // think about service approach maybe toggle?
        Task<CreateChatResponseModel> CreateChatAsync(CreateChatRequestDTO createChatRequestDTO);
        // Get last messages for all chats of a user
        Task<ChatResponseModel> GetChatSummariesForUserAsync(Guid userID);
        // update chat name (for group chats)
        Task<UpdateGroupNameResponseModel> UpdateGroupNameAsync(UpdateGroupNameRequestDTO updateGroupNameRequestDTO);
        // add user to chat (for group chats)
        Task<AddUserToChatResponseModel> AddUserToChatAsync(AddUserRequestDTO addUserRequestDTO) ;
        // Remove user from chat (for group chats)
        Task<RemoveUserFromChatResponseModel> RemoveUserFromChatAsync(RemoveUserRequestDTO removeUserRequestDTO);
        // leave chat (for group chats)
        Task<LeaveChatResponseModel> LeaveChatAsync(Guid chatID, Guid userID);
        // Delete chat (for group chats)
        Task<DeleteChatResponseModel> DeleteChatAsync(Guid chatID, Guid requesterID);
        // Manage group admin (for group chats)
        Task<ManageGroupAdmin> SetGroupAdminAsync(SetGroupAdminRequestDTO setGroupAdminRequestDTO);
        // Get chat members
        Task<GetChatMembersResponseModel> GetChatMembersAsync(Guid chatID);
    }
}
