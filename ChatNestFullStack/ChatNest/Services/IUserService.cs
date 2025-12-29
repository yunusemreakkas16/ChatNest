using ChatNest.Models.Common;
using ChatNest.Models.Domain;
using ChatNest.Models.DTO;

namespace ChatNest.Services
{
    public interface IUserService
    {
        Task<UserResponseModel> CreateUserAsync(CreateUserRequestDto createUserDto);
        Task<UserResponseModelDetailed> GetUserDetailedAsync(UserParamModel userParam);
        Task<UserResponseModelDetailed> UpdateUserAsync(UpdateUserRequestDto updateUserRequestDto);
        Task<BaseResponse> SoftDeleteUserAsync(UserParamModel userParam);
        Task<UserResponseModelDetailed> ReActivateUserAsync(UserParamModel userParam);
        Task<UserIDResponseModel> GetUsersByEmailsAsync(GetIDsByEmailRequestsDto getIDByEmailRequestDto);
        Task<UserIDResponseModel> GetUserByEmailFailedAsync(string email);

    }
}
