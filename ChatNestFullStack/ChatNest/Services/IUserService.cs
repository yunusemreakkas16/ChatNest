using ChatNest.Models.Domain;
using ChatNest.Models.DTO;

namespace ChatNest.Services
{
    public interface IUserService
    {
        public Task<UserResponseModel> CreateUserAsync(CreateUserRequestDto createUserDto);
        public Task<UserResponseModelList> GetUsersAsync();

        public Task<UserResponseModelDetailed> GetUserDetailedAsync(UserParamModel userParam);
        public Task<UserResponseModelDetailed> UpdateUserAsync(UpdateUserRequestDto updateUserRequestDto);
        public Task<object> SoftDeleteUserAsync(UserParamModel userParam);
        public Task<UserResponseModelDetailed> ReActivateUserAsync(UserParamModel userParam);
        public Task<UserIDResponseModel> GetUsersByEmailsAsync(GetIDsByEmailRequestsDto getIDByEmailRequestDto);

    }
}
