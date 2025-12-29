using ChatNest.Models.Common;
using ChatNest.Models.Domain;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ChatNest.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly IConfiguration configuration;

        public AdminRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public async Task<UserResponseModelDetailed> AdminChangeUserRoleAsync(Guid AdminID, Guid UserID, string NewRole)
        {
            var response = new UserResponseModelDetailed();
            try
            {

                using (var connection = new SqlConnection(configuration.GetConnectionString("ChatNestConnectionString")))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@adminID", AdminID);
                    parameters.Add("@userID", UserID);
                    parameters.Add("@newRole", NewRole);
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
        public async Task<BaseResponse> AdminToggleUserStatusAsync(Guid AdminID, Guid UserID, bool NewStatus)
        {
            var response = new BaseResponse
            {
                MessageID = 0,
                MessageDescription = string.Empty
            };
            try
            {
                using (var connection = new SqlConnection(configuration.GetConnectionString("ChatNestConnectionString")))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@adminID", AdminID);
                    parameters.Add("@userID", UserID);
                    parameters.Add("@newStatus", NewStatus);
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
        public async Task<UserResponseModelList> AdminGetUsersAsync(Guid AdminID)
        {
            var response = new UserResponseModelList();

            try
            {
                using (var connection = new SqlConnection(configuration.GetConnectionString("ChatNestConnectionString")))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@adminID", AdminID);
                    parameters.Add("@messageID", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    parameters.Add("@messagedescription", dbType: DbType.String, direction: ParameterDirection.Output, size: 255);
                    var usersData = await connection.QueryAsync<UserResponse>("usp_AdminGetAllUsers", parameters, commandType: CommandType.StoredProcedure);

                    response.MessageID = parameters.Get<int>("@messageID");
                    response.MessageDescription = parameters.Get<string>("@messagedescription");

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
