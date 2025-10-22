using ChatNest.Models.Domain;
using ChatNest.Models.DTO;

namespace ChatNest.Repositories
{
    public interface IAdminRepository
    {
        Task<UserResponseModelDetailed> AdminChangeUserRoleAsync(AdminChangeUserRoleDTO adminChangeUserRoleDTO);
        Task<AdminResponseModelDTO> AdminToggleUserStatusAsync(AdminToggleUserStatusDTO adminToggleUserStatusDTO);
    }
}
