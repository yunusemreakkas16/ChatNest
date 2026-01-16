using ChatNest.Models.Domain;
using ChatNest.Repositories;
using ChatNest.SignalR;
using Microsoft.AspNetCore.SignalR;

namespace ChatNest.Services
{
    public class MessageService : IMessageService
    {
        private readonly IMessageRepository messageRepository;
        private readonly IHubContext<ChatHub> hubContext;

        public MessageService(IMessageRepository messageRepository, IHubContext<ChatHub> hubContext)
        {
            this.messageRepository = messageRepository;
            this.hubContext = hubContext;
        }
        public async Task<DeleteMessageResponseModel> DeleteMessageAsync(Guid messageID, Guid userID)
        {
            return await messageRepository.DeleteMessageAsync(messageID, userID);
        }

        public async Task<MessagesListResponseModel> GetMessagesListAsync(Guid chatID, Guid userID)
        {
            return await messageRepository.GetMessagesAsync(chatID, userID);
        }

        public async Task<MessagesListResponseModel> GetMessagesByIDAsync(Guid requestedMessageID, Guid userID)
        {
            return await messageRepository.GetMessagesByIDAsync(requestedMessageID, userID);
        }

        public async Task<SendMessageResponseModel> SendMessageAsync(Message message)
        {
            var response = await messageRepository.SendMessageAsync(message);

            if (response.MessageID == 1) // success
            {
                // Get the newly created message with enriched data
                var getMessageResponse = await this.GetMessagesByIDAsync(response.NewMessageID, message.senderId);

                var enrichedGroupMessage = getMessageResponse.GroupMessages?.FirstOrDefault();
                var enrichedOneToOneMessage = getMessageResponse.OneToOneMessages?.FirstOrDefault();

                if (enrichedGroupMessage != null)
                {
                    await hubContext.Clients.Group(message.chatId.ToString())
                        .SendAsync("ReceiveMessage", enrichedGroupMessage);
                    Console.WriteLine("It Worked!");
                }
                else if (enrichedOneToOneMessage != null)
                {
                    await hubContext.Clients.Group(message.chatId.ToString())
                        .SendAsync("ReceiveMessage", enrichedOneToOneMessage);
                    Console.WriteLine("It Worked!");
                }
                else
                {
                    // fallback: broadcast minimal payload
                    var broadcast = new GroupMessageResponse
                    {
                        MessageID = response.NewMessageID,
                        ChatID = message.chatId,
                        SenderID = message.senderId,
                        Content = message.content,
                        SentAt = DateTime.UtcNow,
                        DisplayName = "Unknown"
                    };

                    await hubContext.Clients.Group(message.chatId.ToString())
                        .SendAsync("ReceiveMessage", broadcast);
                }
            }

            return response;
        }
    }
}
