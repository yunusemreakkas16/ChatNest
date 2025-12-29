using ChatNest.Models.Common;
using ChatNest.Models.Domain;
using ChatNest.Models.DTO.ChatNest.Models.DTO;

namespace ChatNest.Services
{
    public interface IAuthService
    {
        Task<(string AccessToken, string RefreshToken)> GenerateTokensAsync(Guid userId, string userAgent, string ipAddress);
        Task<(BaseResponse baseResponse,string AccessToken, string RefreshToken)> LogInAsync(LoginRequestDTO loginRequestDTO, string userAgent, string ipAddress);

        Task<(BaseResponse baseResponse, string AccessToken, string RefreshToken)> ContinueLoginAsync(Guid userId, string password, string userAgent, string ipAddress);
        Task<BaseResponse> LogOutAsync(string refreshToken);
        Task<BaseResponse> LogOutAllDevicesAsync(Guid userId);
        Task<BaseResponse> ValidateTokenAsync(TokenValidationRequestDTO tokenValidationRequestDTO, HttpContext context);
        Task<RefresherResponseDTO> RefreshTokensAsync(RefresherRequestDTO refresherRequestDTO);
    }

}
