using ChatNest.Models.Domain;
using ChatNest.Models.DTO;

namespace ChatNest.Repositories
{
    public interface IUserTokenRepository
    {
        Task<UserTokenResponseModel> CreateUserTokenAsync(UserToken userToken);
        Task<UserTokenResponseModel> ValidateUserTokenAsync(string Token, string UserAgent, string IpAddress);
        Task<UserTokenResponseModel> DeactivateTokenAsync(string token);
        Task<UserTokenResponseModel> DeactivateAllUserTokensAsync(Guid userId);
        Task<UserTokenResponseModelByRefreshToken> GetUserTokenByRefreshTokenAsync(string RefreshToken, string UserAgent, string IpAddress);
        Task<UserTokenResponseModel> DeactivateByRefreshTokenAsync(string refreshToken);
    }
}
