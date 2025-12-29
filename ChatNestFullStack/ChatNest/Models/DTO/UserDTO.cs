using ChatNest.Models.Common;

namespace ChatNest.Models.DTO
{
    public class CreateUserRequestDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }

    public class UpdateUserRequestDto
    {
        public Guid UserID { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }   
    }

    public class GetIDsByEmailRequestsDto
    {
        public List<string> Email { get; set; }
    }




}
