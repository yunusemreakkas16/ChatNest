using ChatNest.Models.Common;
using Microsoft.AspNetCore.Http.HttpResults;
using Newtonsoft.Json.Linq;
using System.Net;

namespace ChatNest.Models.Domain
{
    public class UserToken
    {
        public Guid? id { get; set; }
        public Guid userId { get; set; }
        public string token { get; set; }
        public string refreshToken { get; set; }
        public string userAgent { get; set; }
        public string ipAddress { get; set; }
        public bool isActive { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime expiresAt { get; set; }
        public DateTime refreshExpiresAt { get; set; }

    }

    public class UserTokenResponseModel : BaseResponse
    {
    }

    public class UserTokenResponseModelByRefreshToken : BaseResponse
    {
        public UserToken? Data { get; set; }
    }

    public class AuthTokensResponseModel : BaseResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }

    }
}
