using ChatNest.Models.Common;

namespace ChatNest.Models.Domain
{
    public class Chat
    {
        public Guid id { get; set; }
        public bool isGroup { get; set; }
        public string? name { get; set; }
        public Guid createdBy { get; set; }
        public DateTime? createdAt { get; set; }
        public bool? isDeleted { get; set; }
    }

    public class UserChat
    {
        public Guid userId { get; set; }
        public Guid chatId { get; set; }
        public bool? isAdmin { get; set; }
        public DateTime? joinedAt { get; set; }
        public bool? isDeleted { get; set; }
    }

    public class ChatSummary
    {
        public Guid ChatID { get; set; }
        public bool isGroup { get; set; }

        public string DisplayName { get; set; } // For group chats, this is the group name; for direct chats, it's the other user's name
        public DateTime LastMessageDate { get; set; } // The date of the last message in the chat
        public string LastMessageContent { get; set; } // The content of the last message in the chat

    }

    public class ChatResponseModel : BaseResponse
    {
        public List<ChatSummary> Chats { get; set; } = new(); // avoid null reference exceptions
    }

    public class CreateChatResponseModel : BaseResponse { }
    public class AddUserToChatResponseModel : BaseResponse { }
    public class RemoveUserFromChatResponseModel : BaseResponse { }
    public class LeaveChatResponseModel : BaseResponse { }
    public class DeleteChatResponseModel : BaseResponse { }
    public class ManageGroupAdmin : BaseResponse { }
    public class UpdateGroupNameResponseModel : BaseResponse { }
}
