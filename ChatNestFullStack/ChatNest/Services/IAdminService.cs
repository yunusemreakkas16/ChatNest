using ChatNest.Models.Common;
using ChatNest.Models.Domain;
using ChatNest.Models.DTO;

namespace ChatNest.Services
{
    public interface IAdminService
    {
        Task<UserResponseModelDetailed> AdminChangeUserRoleAsync(AdminChangeUserRoleDTO adminChangeUserRoleDTO);
        Task<BaseResponse> AdminToggleUserStatusAsync(AdminToggleUserStatusDTO adminToggleUserStatusDTO);
        Task<UserResponseModelList> AdminGetUsersAsync(Guid AdminID);

    }
}
