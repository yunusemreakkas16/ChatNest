
using ChatNest.Models.Common;
using ChatNest.Models.Domain;
using ChatNest.Models.DTO;
using ChatNest.Models.DTO.ChatNest.Models.DTO;
using ChatNest.Repositories;
using ChatNest.Utils;
using Newtonsoft.Json.Linq;

namespace ChatNest.Services
{
    public class AuthService : IAuthService
    {
        private readonly IJwtService jwtService;
        private readonly IUserTokenRepository userTokenRepository;
        private readonly IConfiguration configuration;
        private readonly IUserService userService;

        public AuthService(IJwtService jwtService, IUserTokenRepository userTokenRepository, IConfiguration configuration, IUserService userService)
        {
            this.jwtService = jwtService;
            this.userTokenRepository = userTokenRepository;
            this.configuration = configuration;
            this.userService = userService;
        }



        public async Task<(string AccessToken, string RefreshToken)> GenerateTokensAsync(Guid userId, string userAgent, string ipAddress)
        {
            var accessToken = await jwtService.GenerateAccessTokenAsync(userId);
            var refreshToken = await jwtService.GenerateRefreshTokenAsync();

            var accessExpires = DateTime.UtcNow.AddMinutes(
                Convert.ToDouble(configuration["Jwt:AccessTokenExpiration"]));
            var refreshExpires = DateTime.UtcNow.AddDays(
                Convert.ToDouble(configuration["Jwt:RefreshTokenExpirationDays"]));

            var userToken = new UserToken
            {
                userId = userId,
                token = accessToken,
                refreshToken = refreshToken,
                userAgent = userAgent,
                ipAddress = ipAddress,
                expiresAt = accessExpires,
                refreshExpiresAt = refreshExpires
            };

            await userTokenRepository.CreateUserTokenAsync(userToken);

            return (accessToken, refreshToken);
        }

        public async Task<(BaseResponse baseResponse, string AccessToken, string RefreshToken)> LogInAsync(LoginRequestDTO loginRequestDTO, string userAgent, string ipAddress)
        {
            var baseResponse = new BaseResponse();

            var EmailResponse = new GetIDsByEmailRequestsDto
            {
                Email = new List<string> { loginRequestDTO.Email }
            };

            var UserIDResponse = await userService.GetUsersByEmailsAsync(EmailResponse);

            if (UserIDResponse.MessageID != 1 || UserIDResponse.UserIDResponse == null || !UserIDResponse.UserIDResponse.Any())
            {
                baseResponse.MessageID = -1;
                baseResponse.MessageDescription = "User not found with given email.";
                return (baseResponse, null, null);
            }

            var UserID = UserIDResponse.UserIDResponse.First().UserID;


            var userParam = new UserParamModel
            {
                UserID = UserID
            };

            var UserDetailedResponseModel = await userService.GetUserDetailedAsync(userParam);

            var isPasswordValid = PasswordHasher.VerifyPassword(loginRequestDTO.Password, UserDetailedResponseModel.User.UserPasswordHash);

            if (isPasswordValid)
            {
                // Generate tokens
                var (accessToken, refreshToken) = await GenerateTokensAsync(UserID, userAgent, ipAddress);

                baseResponse.MessageID = 1;
                baseResponse.MessageDescription = "Login successful.";

                return (baseResponse, accessToken, refreshToken);
            }
            else
            {
                baseResponse.MessageID = -1;
                baseResponse.MessageDescription = "Invalid password.";
                return (baseResponse, null, null);
            }
        }

        public async Task<BaseResponse> LogOutAsync(string refreshToken)
        {
           var response =  await userTokenRepository.DeactivateByRefreshTokenAsync(refreshToken);
            return response;
        }
        public async Task<BaseResponse> LogOutAllDevicesAsync(Guid userId)
        {
            var response = await userTokenRepository.DeactivateAllUserTokensAsync(userId);
            return response;
        }

        public async Task<RefresherResponseDTO> RefreshTokensAsync(RefresherRequestDTO refresherRequestDTO)
        {
            var refresherResponseDTO = new RefresherResponseDTO();
            // Get User token which matchs the refresh token
            var existingToken = await userTokenRepository.GetUserTokenByRefreshTokenAsync(refresherRequestDTO.RefreshToken, refresherRequestDTO.UserAgent, refresherRequestDTO.IpAddress);

            if (existingToken == null || existingToken.MessageID != 1 || existingToken.Data == null)
            {
                throw new UnauthorizedAccessException("Refresh token not associated with a valid user");
            }
            // Generate new tokens
            var newAccessToken = await jwtService.GenerateAccessTokenAsync(existingToken.Data.userId); 
            var newRefreshToken = await jwtService.GenerateRefreshTokenAsync();

            var accessExpires = DateTime.UtcNow.AddMinutes(
                Convert.ToDouble(configuration["Jwt:AccessTokenExpiration"]));
            var refreshExpires = DateTime.UtcNow.AddDays(
                Convert.ToDouble(configuration["Jwt:RefreshTokenExpirationDays"]));

            var userToken = new UserToken
            {
                userId = existingToken.Data.userId,
                token = newAccessToken,
                refreshToken = newRefreshToken,
                userAgent = refresherRequestDTO.UserAgent,
                ipAddress = refresherRequestDTO.IpAddress,
                expiresAt = accessExpires,
                refreshExpiresAt = refreshExpires
            };

            await userTokenRepository.CreateUserTokenAsync(userToken);

            // Deactivate the old refresh token
            await userTokenRepository.DeactivateByRefreshTokenAsync(refresherRequestDTO.RefreshToken);

            refresherResponseDTO.AccessToken = newAccessToken;
            refresherResponseDTO.RefreshToken = newRefreshToken;

            return (refresherResponseDTO);
        }

        public async Task<BaseResponse> ValidateTokenAsync(TokenValidationRequestDTO tokenValidationRequestDTO, HttpContext c)
        {

            if(string.IsNullOrEmpty(tokenValidationRequestDTO.IpAddress))
            {
                tokenValidationRequestDTO.IpAddress = c.Connection.RemoteIpAddress?.ToString();
            }

            var response = await userTokenRepository.ValidateUserTokenAsync(tokenValidationRequestDTO.Token, tokenValidationRequestDTO.UserAgent, tokenValidationRequestDTO.IpAddress);
            return response;
        }
    }
}
