using ChatNest.Models.Domain;
using ChatNest.Repositories;

namespace ChatNest.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository messageRepository;

        public MessageService(IMessageRepository messageRepository)
        {
            this.messageRepository = messageRepository;
        }
        public async Task<DeleteMessageResponseModel> DeleteMessageAsync(Guid messageID, Guid userID)
        {
             return await messageRepository.DeleteMessageAsync(messageID, userID);
        }

        public async Task<MessagesListResponseModel> GetMessagesListAsync(Guid chatID, Guid userID)
        {
            return await messageRepository.GetMessagesAsync(chatID, userID);
        }

        public async Task<SendMessageResponseModel> SendMessageAsync(Message message)
        {
            return await messageRepository.SendMessageAsync(message);
        }
    }
}
