using ChatNest.Models.Domain;

namespace ChatNest.Repositories
{
    public interface IMessageRepository
    {
        Task<SendMessageResponseModel> SendMessageAsync(Message message);
        Task<MessagesListResponseModel> GetMessagesAsync(Guid chatID, Guid userID);
        Task<MessagesListResponseModel> GetMessagesByIDAsync(Guid requestedMessageID, Guid userID);
        Task<DeleteMessageResponseModel> DeleteMessageAsync(Guid messageID, Guid userID);
    }
}
