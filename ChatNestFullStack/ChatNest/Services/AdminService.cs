using ChatNest.Models.Common;
using ChatNest.Models.Domain;
using ChatNest.Models.DTO;
using ChatNest.Repositories;

namespace ChatNest.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository adminRepository;

        public AdminService(IAdminRepository adminRepository)
        {
            this.adminRepository = adminRepository;
        }

        public async Task<UserResponseModelDetailed> AdminChangeUserRoleAsync(AdminChangeUserRoleDTO adminChangeUserRoleDTO)
        {
            return await adminRepository.AdminChangeUserRoleAsync(
                adminChangeUserRoleDTO.AdminID,
                adminChangeUserRoleDTO.UserID,
                adminChangeUserRoleDTO.NewRole
            );
        }

        public async Task<BaseResponse> AdminToggleUserStatusAsync(AdminToggleUserStatusDTO adminToggleUserStatusDTO)
        {
            return await adminRepository.AdminToggleUserStatusAsync(
                adminToggleUserStatusDTO.AdminID,
                adminToggleUserStatusDTO.UserID,
                adminToggleUserStatusDTO.NewStatus
            );
        }

        public async Task<UserResponseModelList> AdminGetUsersAsync(Guid AdminID)
        {
            return await adminRepository.AdminGetUsersAsync(AdminID);
        }
    }
}