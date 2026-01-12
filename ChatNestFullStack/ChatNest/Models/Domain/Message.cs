using ChatNest.Models.Common;

namespace ChatNest.Models.Domain
{
    public class Message
    {
        public Guid? id { get; set; }
        public Guid chatId { get; set; }
        public Guid senderId { get; set; }
        public string content { get; set; }
        public bool? isDeleted { get; set; }
    }

    public class SendMessageResponseModel : BaseResponse
    {
        public Guid NewMessageID { get; set; }
    }
    public class DeleteMessageResponseModel : BaseResponse { }

    public class GroupMessageResponse
    {
        public Guid MessageID { get; set; }
        public Guid ChatID { get; set; }
        public Guid SenderID { get; set; }
        public string Content { get; set; }
        public DateTime SentAt { get; set; }
        public string DisplayName { get; set; } // Name of the sender
    }

    public class OneToOneMessageResponse
    {
        public Guid MessageID { get; set; }
        public Guid ChatID { get; set; }
        public string Content { get; set; }
        public DateTime SentAt { get; set; }

        public Guid SenderID { get; set; }
        public string SenderName { get; set; }

        public Guid ReceiverID { get; set; }
        public string ReceiverName { get; set; }
    }

    public class MessagesListResponseModel : BaseResponse
    {
        public List<GroupMessageResponse>? GroupMessages { get; set; } // Only filled when isGroup = true
        public List<OneToOneMessageResponse>? OneToOneMessages { get; set; } // Only filled when isGroup = false
    }


}
