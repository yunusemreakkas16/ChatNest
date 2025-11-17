using ChatNest.Models.Common;


namespace ChatNest.Models.Domain
{
    public class Friendship
    {
        public class FriendshipCreateModel
        {
            public Guid requesterId { get; set; }
            public Guid receiverId { get; set; }
        }

        public class FriendResponse
        {
            public Guid FriendID { get; set; }
            public string FriendName { get; set; }
            public string FriendMail { get; set; }
            public DateTime createdAt { get; set; }
        }
        public class FriendListResponseModel : BaseResponse
        {
            public List<FriendResponse> Friends { get; set; } = new(); // avoid null reference exceptions
        }
        public class FriendRequestResponse
        {
            public Guid? RequesterID { get; set; } // Populated only when direction = 'received'; null when direction = 'sent'
            public Guid? ReceiverID { get; set; } // Populated only when direction = 'sent'; null when direction = 'received'
            public string Name { get; set; }
            public string Email { get; set; }
            public string FriendShipStatus { get; set; }
            public DateTime createdAt { get; set; }

        }
        public class FriendRequestListResponseModel : BaseResponse
        {
            public List<FriendRequestResponse> FriendRequests { get; set; } = new(); // avoid null reference exceptions
        }
        public class FriendshipStatusResponseModel : BaseResponse
        {
            public string status { get; set; } // "Accepted", "Pending", "Rejected", etc.
        }

        public class RemoveFriendResponseModel : BaseResponse { }
        public class SendFriendRequestResponseModel : BaseResponse { }
        public class ManageFriendRequestResponseModel : BaseResponse { }

    }
}
