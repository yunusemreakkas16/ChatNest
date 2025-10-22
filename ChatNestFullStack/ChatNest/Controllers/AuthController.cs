using Azure;
using ChatNest.Models.Common;
using ChatNest.Models.Domain;
using ChatNest.Models.DTO.ChatNest.Models.DTO;
using ChatNest.Services;
using ChatNest.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ChatNest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService authService;
        private readonly IUserService userService;

        public AuthController(IAuthService authService, IUserService userService)
        {
            this.authService = authService;
            this.userService = userService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthTokensResponseModel>> Login([FromBody] LoginRequestDTO loginRequestDTO)
        {
            if (loginRequestDTO == null || string.IsNullOrEmpty(loginRequestDTO.Email) || string.IsNullOrEmpty(loginRequestDTO.Password))
            {
                return BadRequest("Email and Password are required.");
            }
            var userAgent = Request.Headers["User-Agent"].ToString();

            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            ipAddress = IpHelper.NormalizeIp(ipAddress);


            var loginResponse = await authService.LogInAsync(loginRequestDTO, userAgent, ipAddress);

            var AuthResponse = new AuthTokensResponseModel
            {
                MessageID = loginResponse.baseResponse.MessageID,
                MessageDescription = loginResponse.baseResponse.MessageDescription,
                AccessToken = loginResponse.AccessToken,
                RefreshToken = loginResponse.RefreshToken

            };

            return AuthResponse.MessageID switch
            {
                1 => Ok(AuthResponse),
                -1 => NotFound(AuthResponse)
            };
        }

        [HttpPost("logout")]
        public async Task<ActionResult<BaseResponse>> Logout([FromBody] LogoutRequestDTO logoutRequestDTO)
        {
            var logoutResponse = await authService.LogOutAsync(logoutRequestDTO.RefreshToken);
            return logoutResponse.MessageID switch
            {
                1 => Ok(logoutResponse),
                0 => NotFound(logoutResponse)
            };
        }

        [HttpPost("logout_allDevices")]
        public async Task<ActionResult<BaseResponse>> LogoutAllDevices([FromQuery] Guid userId)
        {
            if (userId == Guid.Empty)
            {
                return BadRequest("User ID is required.");
            }
            var logoutAllResponse = await authService.LogOutAllDevicesAsync(userId);
            return logoutAllResponse.MessageID switch
            {
                1 => Ok(logoutAllResponse),
                0 => NotFound(logoutAllResponse)
            };
        }

        [HttpPost("ValidateToken")]
        public async Task<ActionResult<BaseResponse>> ValidateToken([FromBody] TokenValidationRequestDTO tokenValidationRequestDTO)
        {
            if (tokenValidationRequestDTO == null || string.IsNullOrEmpty(tokenValidationRequestDTO.Token))
            {
                return BadRequest("Token is required.");
            }
            var validationResponse = await authService.ValidateTokenAsync(tokenValidationRequestDTO, HttpContext);
            return validationResponse.MessageID switch
            {
                1 => Ok(validationResponse),
                0 => Unauthorized(validationResponse),
                _ => StatusCode(500, validationResponse)

            };
        }

        [AllowAnonymous]
        [HttpPost("RefreshToken")]
        public async Task<ActionResult<RefresherResponseDTO>> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            // 🟢 Check Model State if valid
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // 🟢 Convert to RefresherRequestDTO
            var refresherRequest = new RefresherRequestDTO
            {
                RefreshToken = request.RefreshToken,
                UserAgent = Request.Headers["User-Agent"].ToString(),
                IpAddress = IpHelper.NormalizeIp(HttpContext.Connection.RemoteIpAddress?.ToString())
            };

            var response = await authService.RefreshTokensAsync(refresherRequest);
            return Ok(response);
        }
    }
}