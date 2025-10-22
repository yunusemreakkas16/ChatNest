using ChatNest.Models.Common;

namespace ChatNest.Models.Domain
{
    public class User
    {
        public Guid? userID { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string passwordHash { get; set; }
        public string? role { get; set; }
        public DateTime? createdAt { get; set; }
        public bool? isDeleted { get; set; }
    }

    public class UserParamModel
    {
        public Guid UserID { get; set; }
    }

    public class UserResponse
    {
        public Guid UserID { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserRole { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class UserResponseDetailed
    {
        public Guid UserID { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserPasswordHash { get; set; }
        public string UserRole { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class UserResponseModel
    {
        public UserResponse? User { get; set; }
        public int MessageID { get; set; }
        public string MessageDescription { get; set; }
    }
    public class UserResponseModelDetailed
    {
        public UserResponseDetailed? User { get; set; }
        public int MessageID { get; set; }
        public string MessageDescription { get; set; }
    }

    public class UserResponseModelList
    {
        public List<UserResponse>? Users { get; set; }
        public int MessageID { get; set; }
        public string MessageDescription { get; set; }
    }


    public class UserIDResponse
    {
        public Guid UserID { get; set; }
    }

    public class UserIDResponseModel : BaseResponse
    {
        public UserIDResponse? UserIDResponse { get; set; }
    }


}
