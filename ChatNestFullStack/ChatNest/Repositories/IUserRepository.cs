using ChatNest.Models.Domain;
using ChatNest.Models.DTO;

namespace ChatNest.Repositories
{
    public interface IUserRepository
    {
        Task<UserResponseModelDetailed> GetUserDetailedAsync(UserParamModel userParam);
        Task<UserResponseModelList> GetUsersAsync();
        Task<UserResponseModel> CreateUserAsync(User user);
        Task<UserResponseModelDetailed> UpdateUserAsync(User user);
        Task<object> SoftDeleteUserAsync(UserParamModel userParam);
        Task<UserResponseModelDetailed> ReActivateUserAsync(UserParamModel userParam);
        Task<UserIDResponseModel> GetUserIDsByMailsAsync(GetIDsByEmailRequestsDto emails);
    }
}
