using ChatNest.Models.Domain;

namespace ChatNest.Models.DTO
{
    public class ChatDTO
    {

    }

    public class CreateChatRequestDTO
    {
        public bool IsGroup { get; set; }
        public string? Name { get; set; }
        public Guid CreatedBy { get; set; }
        public List<Guid>? ParticipantIDs { get; set; } // For group chat, this is a list of user IDs to add to the chat
        public Guid? TargetUserID { get; set; } // for 1 to 1 chat
    }
    public class AddUserRequestDTO
    {
        public Guid ChatID { get; set; }
        public Guid AdminID { get; set; } // The user who is adding other users to the chat
        public List<Guid>? UserIDs { get; set; } // List of user IDs to be added to the chat
    }

    public class RemoveUserRequestDTO
    {
        public Guid ChatID { get; set; }
        public Guid AdminID { get; set; } // The user who is removing other users from the chat
        public Guid UserID { get; set; } // List of user IDs to be removed from the chat
    }

    public class SetGroupAdminRequestDTO
    {
        public Guid ChatID { get; set; }
        public Guid AdminID { get; set; } // The user who is setting or removing the admin
        public Guid UserID { get; set; } // The user to be made admin or removed as admin
        public bool MakeAdmin { get; set; } // true to make admin, false to remove admin
    }

    public class UpdateGroupNameRequestDTO
    {
        public Guid ChatID { get; set; }
        public Guid UserID { get; set; } // The user who is updating the group name
        public string NewName { get; set; } // The new name for the group chat
    }

}
