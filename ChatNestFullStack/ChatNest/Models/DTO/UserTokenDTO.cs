using ChatNest.Models.Common;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ChatNest.Models.DTO
{
    public class UserTokenDTO
    {
    }

    namespace ChatNest.Models.DTO
    {
        public class LoginRequestDTO
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [MinLength(8, ErrorMessage = "Password must be at least 6 characters long.")]
            public string Password { get; set; }
        }
        public class LogoutRequestDTO
        {
            public string RefreshToken { get; set; }
        }

        public class TokenValidationRequestDTO
        {
            [Required]
            public string Token { get; set; }
            [Required]
            public string UserAgent { get; set; }
            public string IpAddress { get; set; }
        }


        public class RefreshTokenRequest
        {
            [Required]
            public string RefreshToken { get; set; }
        }

        public class RefresherRequestDTO
        {
            [Required]
            public string RefreshToken { get; set; }
            public string UserAgent { get; set; }
            public string IpAddress { get; set; }

        }

        public class RefresherResponseDTO
        {
            public string AccessToken { get; set; }
            public string RefreshToken { get; set; }

        }

    }
}
