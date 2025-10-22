using ChatNest.Models.Domain;
using ChatNest.Models.DTO;
using Dapper;
using Microsoft.Data.SqlClient;

namespace ChatNest.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly IConfiguration configuration;

        public AdminRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public async Task<UserResponseModelDetailed> AdminChangeUserRoleAsync(AdminChangeUserRoleDTO adminChangeUserRoleDTO)
        {
            var response = new UserResponseModelDetailed
            {
                User = null,
                MessageID = 0,
                MessageDescription = string.Empty
            };
            try
            {

                using (var connection = new SqlConnection(configuration.GetConnectionString("ChatNestConnectionString")))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@adminID", adminChangeUserRoleDTO.AdminID);
                    parameters.Add("@userID", adminChangeUserRoleDTO.UserID);
                    parameters.Add("@newRole", adminChangeUserRoleDTO.NewRole);
                    parameters.Add("@messageID", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
                    parameters.Add("@messageDescription", dbType: System.Data.DbType.String, size: 255, direction: System.Data.ParameterDirection.Output);

                    var userData = await connection.QueryFirstOrDefaultAsync<UserResponseDetailed>("usp_AdminChangeUserRole", parameters, commandType: System.Data.CommandType.StoredProcedure);

                    response.MessageID = parameters.Get<int>("@messageID");
                    response.MessageDescription = parameters.Get<string>("@messageDescription");

                    if (response.MessageID == 1)
                    {
                        response.User = userData;
                    }
                    else
                    {
                        response.User = null;
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                response.MessageID = -99;
                response.MessageDescription = $"Database error: {sqlEx.Message}";
            }
            catch (Exception ex)
            {
                response.MessageID = -100;
                response.MessageDescription = $"Unexpected error: {ex.Message}";
            }
            return response;
        }


        public async Task<AdminResponseModelDTO> AdminToggleUserStatusAsync(AdminToggleUserStatusDTO adminToggleUserStatusDTO)
        {
            var response = new AdminResponseModelDTO
            {
                MessageID = 0,
                MessageDescription = string.Empty
            };
            try
            {
                using (var connection = new SqlConnection(configuration.GetConnectionString("ChatNestConnectionString")))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@adminID", adminToggleUserStatusDTO.AdminID);
                    parameters.Add("@userID", adminToggleUserStatusDTO.UserID);
                    parameters.Add("@newStatus", adminToggleUserStatusDTO.NewStatus);
                    parameters.Add("@messageID", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
                    parameters.Add("@messageDescription", dbType: System.Data.DbType.String, size: 255, direction: System.Data.ParameterDirection.Output);
                    await connection.ExecuteAsync("usp_AdminToggleUserStatus", parameters, commandType: System.Data.CommandType.StoredProcedure);
                    response.MessageID = parameters.Get<int>("@messageID");
                    response.MessageDescription = parameters.Get<string>("@messageDescription");
                }
            }
            catch (SqlException sqlEx)
            {
                response.MessageID = -99;
                response.MessageDescription = $"Database error: {sqlEx.Message}";
            }
            catch (Exception ex)
            {
                response.MessageID = -100;
                response.MessageDescription = $"Unexpected error: {ex.Message}";
            }
            return response;

        }
    }
}
