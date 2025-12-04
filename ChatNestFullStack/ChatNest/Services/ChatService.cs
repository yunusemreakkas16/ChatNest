using ChatNest.Models.Domain;
using ChatNest.Models.DTO;
using ChatNest.Repositories;

namespace ChatNest.Services
{
    public class ChatService : IChatService
    {
        private readonly IChatRepository chatRepository;
        private readonly IUserService userService;

        public ChatService(IChatRepository chatRepository, IUserService userService)
        {
            this.chatRepository = chatRepository;
            this.userService = userService;
        }

        public async Task<CreateChatResponseModel> CreateChatAsync(CreateChatRequestDTO createChatRequestDTO)
        {
            if (createChatRequestDTO == null)
                throw new ArgumentNullException(nameof(createChatRequestDTO));

            // Email to UserID conversion
            if (createChatRequestDTO.TargetEmails != null && createChatRequestDTO.TargetEmails.Any())
            {
                var emailRequest = new GetIDsByEmailRequestsDto { Email = createChatRequestDTO.TargetEmails };
                var userIdsResponse = await userService.GetUsersByEmailsAsync(emailRequest);

                if (createChatRequestDTO.IsGroup)
                {
                    createChatRequestDTO.ParticipantIDs = (createChatRequestDTO.ParticipantIDs ?? new List<Guid>())
                        .Concat(userIdsResponse.UserIDResponse.Select(u => u.UserID))
                        .Distinct()
                        .ToList();
                }
                else
                {
                    createChatRequestDTO.TargetUserID = userIdsResponse.UserIDResponse.First().UserID;
                }
            }

            if (!createChatRequestDTO.IsGroup)
            {
                if (createChatRequestDTO.TargetUserID == null)
                    return new CreateChatResponseModel { MessageID = -1, MessageDescription = "Target user must be provided for 1-on-1 chat." };

                if (createChatRequestDTO.ParticipantIDs != null && createChatRequestDTO.ParticipantIDs.Any())
                    return new CreateChatResponseModel { MessageID = -11, MessageDescription = "1-on-1 chat cannot have participantIDs." };
            }
            else
            {
                if (createChatRequestDTO.ParticipantIDs == null || !createChatRequestDTO.ParticipantIDs.Any())
                    return new CreateChatResponseModel { MessageID = -12, MessageDescription = "Group chat must have at least one participant." };
            }



            var chat = new Chat
            {
                isGroup = createChatRequestDTO.IsGroup,
                name = createChatRequestDTO.Name,
                createdBy = createChatRequestDTO.CreatedBy,
            };

            return await chatRepository.CreateChatAsync(chat, createChatRequestDTO.ParticipantIDs, createChatRequestDTO.TargetUserID);
        }

        public async Task<AddUserToChatResponseModel> AddUserToChatAsync(AddUserRequestDTO addUserRequestDTO)
        {
            if (addUserRequestDTO == null)
            {
                throw new ArgumentNullException(nameof(addUserRequestDTO), "AddUserRequestDTO cannot be null");
            }

            return await chatRepository.AddUserToChatAsync(addUserRequestDTO.ChatID, addUserRequestDTO.AdminID, addUserRequestDTO.UserIDs);
        }


        public async Task<DeleteChatResponseModel> DeleteChatAsync(Guid chatID, Guid requesterID)
        {
            return await chatRepository.DeleteChatAsync(chatID, requesterID);
        }

        public async Task<ChatResponseModel> GetChatSummariesForUserAsync(Guid userID)
        {
            return await chatRepository.GetChatSummariesForUserAsync(userID);
        }

        public async Task<LeaveChatResponseModel> LeaveChatAsync(Guid chatID, Guid userID)
        {
            return await chatRepository.LeaveChatAsync(chatID, userID);
        }

        public async Task<RemoveUserFromChatResponseModel> RemoveUserFromChatAsync(RemoveUserRequestDTO removeUserRequestDTO)
        {
            if (removeUserRequestDTO == null)
            {
                throw new ArgumentNullException(nameof(removeUserRequestDTO), "RemoveUserRequestDTO cannot be null");
            }

            return await chatRepository.RemoveUserFromChatAsync(removeUserRequestDTO.ChatID, removeUserRequestDTO.AdminID, removeUserRequestDTO.UserID);
        }

        public async Task<ManageGroupAdmin> SetGroupAdminAsync(SetGroupAdminRequestDTO setGroupAdminRequestDTO)
        {
            if (setGroupAdminRequestDTO == null)
            {
                throw new ArgumentNullException(nameof(setGroupAdminRequestDTO), "SetGroupAdminRequestDTO cannot be null");
            }

            return await chatRepository.SetGroupAdminAsync(setGroupAdminRequestDTO.ChatID, setGroupAdminRequestDTO.AdminID, setGroupAdminRequestDTO.UserID, setGroupAdminRequestDTO.MakeAdmin);
        }

        public async Task<UpdateGroupNameResponseModel> UpdateGroupNameAsync(UpdateGroupNameRequestDTO updateGroupNameRequestDTO)
        {
            if (updateGroupNameRequestDTO == null)
            {
                throw new ArgumentNullException(nameof(updateGroupNameRequestDTO), "UpdateGroupNameRequestDTO cannot be null");
            }

            return await chatRepository.UpdateGroupNameAsync(updateGroupNameRequestDTO.ChatID, updateGroupNameRequestDTO.UserID, updateGroupNameRequestDTO.NewName );
        }

        public Task<GetChatMembersResponseModel> GetChatMembersAsync(Guid chatID)
        {
            return chatRepository.GetChatMembers(chatID);
        }
    }
}
