using ChatNest.Models.Domain;

namespace ChatNest.Services
{
    public interface IMessageService
    {
        Task<SendMessageResponseModel> SendMessageAsync(Message message);
        Task<MessagesListResponseModel> GetMessagesListAsync(Guid chatID, Guid userID);
        Task<DeleteMessageResponseModel> DeleteMessageAsync(Guid messageID, Guid userID);
    }
}
