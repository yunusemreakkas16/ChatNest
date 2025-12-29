using Microsoft.Data.SqlClient;
using Dapper;
using System.Data;
using ChatNest.Models.Domain;
using Azure;
using ChatNest.Models.DTO;
using ChatNest.Models.Common;

namespace ChatNest.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IConfiguration configuration;

        public UserRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<UserResponseModel> CreateUserAsync(User user)
        {
            var response = new UserResponseModel();
            try
            {
                using (var connection = new SqlConnection(configuration.GetConnectionString("ChatNestConnectionString")))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@username", user.username);
                    parameters.Add("@email", user.email);
                    parameters.Add("@passwordHash", user.passwordHash);

                    parameters.Add("@messageID", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
                    parameters.Add("@messagedescription", dbType: System.Data.DbType.String, direction: System.Data.ParameterDirection.Output, size: 255);

                    var userData = await connection.QuerySingleOrDefaultAsync<UserResponse>("usp_CreateUser", parameters, commandType: CommandType.StoredProcedure);

                    response.User = userData;
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

        public async Task<BaseResponse> SoftDeleteUserAsync(UserParamModel userParam)
        {
            var response = new BaseResponse();
            try
            {
                using (var connection = new SqlConnection(configuration.GetConnectionString("ChatNestConnectionString")))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@userID", userParam.UserID);
                    parameters.Add("@messageID", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    parameters.Add("@messagedescription", dbType: DbType.String, direction: ParameterDirection.Output, size: 255);
                    await connection.ExecuteAsync("usp_SoftDeleteUser", parameters, commandType: CommandType.StoredProcedure);
                    var messageID = parameters.Get<int>("@messageID");
                    var messageDescription = parameters.Get<string>("@messagedescription");

                    response.MessageID = messageID;
                    response.MessageDescription = messageDescription;

                }
            }
            catch (SqlException sqlEx)
            {
                throw new Exception($"Database error: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Unexpected error: {ex.Message}");
            }
            return response;
        }

        public async Task<UserResponseModelDetailed> ReActivateUserAsync(UserParamModel userParam)
        {
            var response = new UserResponseModelDetailed();
            try
            {
                using (var connection = new SqlConnection(configuration.GetConnectionString("ChatNestConnectionString")))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@userID", userParam.UserID);
                    parameters.Add("@messageID", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    parameters.Add("@messagedescription", dbType: DbType.String, direction: ParameterDirection.Output, size: 255);

                    var userData = await connection.QuerySingleOrDefaultAsync<UserResponseDetailed>("usp_ReActivateUser", parameters, commandType: CommandType.StoredProcedure);

                    response.User = userData;
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

        public async Task<UserResponseModelDetailed> GetUserDetailedAsync(UserParamModel userParam)
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
                    parameters.Add("@userID", userParam.UserID);
                    parameters.Add("@messageID", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    parameters.Add("@messagedescription", dbType: DbType.String, direction: ParameterDirection.Output, size: 255);
                    var userData = await connection.QuerySingleOrDefaultAsync<UserResponseDetailed>("usp_GetUserbyID", parameters, commandType: CommandType.StoredProcedure);

                    response.User = userData;
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

        public async Task<UserResponseModelDetailed> UpdateUserAsync(User user)
        {
            var response = new UserResponseModelDetailed();

            try
            {
                using (var connection = new SqlConnection(configuration.GetConnectionString("ChatNestConnectionString")))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@userID", user.userID);

                    // Nullable parameters
                    parameters.Add("@username", string.IsNullOrEmpty(user.username) ? null : user.username);
                    parameters.Add("@email", string.IsNullOrEmpty(user.email) ? null : user.email);
                    parameters.Add("@passwordHash", string.IsNullOrEmpty(user.passwordHash) ? null : user.passwordHash);

                    parameters.Add("@messageID", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    parameters.Add("@messagedescription", dbType: DbType.String, direction: ParameterDirection.Output, size: 255);

                    var userData = await connection.QuerySingleOrDefaultAsync<UserResponseDetailed>("usp_UpdateUser", parameters, commandType: CommandType.StoredProcedure);

                    response.User = userData;
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

        public async Task<UserIDResponseModel> GetUserIDsByMailsAsync(GetIDsByEmailRequestsDto Emails)
        {
            var response = new UserIDResponseModel();
            try
            {
                using (var connection = new SqlConnection(configuration.GetConnectionString("ChatNestConnectionString")))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@Emails", string.Join(",", Emails.Email));
                    parameters.Add("@messageID", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    parameters.Add("@messageDescription", dbType: DbType.String, direction: ParameterDirection.Output, size: 255);

                    var userIDs = await connection.QueryAsync<UserIDResponse>("usp_FindUserIDsByEmails", parameters, commandType: CommandType.StoredProcedure);

                    response.UserIDResponse = userIDs.ToList();

                    response.MessageID = parameters.Get<int>("@messageID");
                    response.MessageDescription = parameters.Get<string>("@messageDescription");
                }
            }
            catch (SqlException sqlEx)
            {
                throw new Exception($"Database error: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Unexpected error: {ex.Message}");
            }

            return response;
        }

        public async Task<UserIDResponseModel> GetUserByEmailFailedAsync(string email)
        {
            var response = new UserIDResponseModel();
            try
            {
                using (var connection = new SqlConnection(configuration.GetConnectionString("ChatNestConnectionString")))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@Email", email);
                    parameters.Add("@messageID", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    parameters.Add("@messageDescription", dbType: DbType.String, direction: ParameterDirection.Output, size: 255);

                    var userIDs = await connection.QueryAsync<UserIDResponse>("usp_GetUserByEmailFailed", parameters, commandType: CommandType.StoredProcedure);

                    response.UserIDResponse = userIDs.ToList();
                    response.MessageID = parameters.Get<int>("@messageID");
                    response.MessageDescription = parameters.Get<string>("@messageDescription");
                }
            }
            catch (SqlException sqlEx)
            {
                throw new Exception($"Database error: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Unexpected error: {ex.Message}");
            }
            return response;
        }   
    }
}
