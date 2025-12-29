using ChatNest.Models.Common;
using ChatNest.Models.Domain;

namespace ChatNest.Repositories
{
    public interface IAdminRepository
    {
        Task<UserResponseModelDetailed> AdminChangeUserRoleAsync(Guid AdminID, Guid UserID, string NewRole);
        Task<BaseResponse> AdminToggleUserStatusAsync(Guid AdminID, Guid UserID, bool NewStatus);
        Task<UserResponseModelList> AdminGetUsersAsync(Guid AdminID);
    }
}
