using Microsoft.Data.SqlClient;
using Dapper;
using System.Data;
using ChatNest.Models.Domain;
using Azure;
using ChatNest.Models.DTO;

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
            var response = new UserResponseModel
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
                    parameters.Add("@username", user.username);
                    parameters.Add("@email", user.email);
                    parameters.Add("@passwordHash", user.passwordHash);

                    parameters.Add("@messageID", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);
                    parameters.Add("@messagedescription", dbType: System.Data.DbType.String, direction: System.Data.ParameterDirection.Output, size: 255);

                    var userData = await connection.QuerySingleOrDefaultAsync<UserResponse>("usp_CreateUser", parameters, commandType: CommandType.StoredProcedure);

                    response.MessageID = parameters.Get<int>("@messageID");
                    response.MessageDescription = parameters.Get<string>("@messagedescription");

                    if (response.MessageID == -2 || response.MessageID == 0 || response.MessageID == -99)
                    {
                        response.User = null;
                    }
                    else
                    {
                        response.User = userData;
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

        public async Task<object> SoftDeleteUserAsync(UserParamModel userParam)
        {
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
                    if (messageID == 1 || messageID == -1 || messageID == -99)
                    {
                        return new { MessageID = messageID, MessageDescription = messageDescription };
                    }
                    else
                    {
                        throw new Exception($"Unexpected MessageID ({messageID}) from stored procedure.");
                    }
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

        }

        public async Task<UserResponseModelDetailed> ReActivateUserAsync(UserParamModel userParam)
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

                    var userData = await connection.QuerySingleOrDefaultAsync<UserResponseDetailed>("usp_ReActivateUser", parameters, commandType: CommandType.StoredProcedure);

                    if (userData == null)
                    {
                        response.MessageID = -4;
                        response.MessageDescription = "Stored procedure success but no data found.";
                    }
                    else
                    {
                        response.User = userData;
                        response.MessageID = parameters.Get<int>("@messageID");
                        response.MessageDescription = parameters.Get<string>("@messagedescription");
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

                    if (userData == null)
                    {
                        response.MessageID = -4;
                        response.MessageDescription = "Stored procedure success but no data found.";
                    }

                    else
                    {
                        response.User = userData;
                        response.MessageID = parameters.Get<int>("@messageID");
                        response.MessageDescription = parameters.Get<string>("@messagedescription");
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

        public async Task<UserResponseModelList> GetUsersAsync()
        {
            var response = new UserResponseModelList
            {
                Users = new List<UserResponse>(),
                MessageID = 0,
                MessageDescription = string.Empty
            };

            try
            { 
                using (var connection = new SqlConnection(configuration.GetConnectionString("ChatNestConnectionString")))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@messageID", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    parameters.Add("@messagedescription", dbType: DbType.String, direction: ParameterDirection.Output, size: 255);
                    var usersData = await connection.QueryAsync<UserResponse>("usp_GetAllUser", parameters, commandType: CommandType.StoredProcedure);

                    if (usersData == null)
                    {
                        response.MessageID = -4;
                        response.MessageDescription = "Stored procedure success but no data found.";

                    }
                    else
                    {
                        response.Users = usersData.ToList();
                        response.MessageID = parameters.Get<int>("@messageID");
                        response.MessageDescription = parameters.Get<string>("@messagedescription");
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

        public async Task<UserResponseModelDetailed> UpdateUserAsync(User user)
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
                    parameters.Add("@userID", user.userID);
                    parameters.Add("@username", user.username);
                    parameters.Add("@email", user.email);
                    parameters.Add("@passwordHash", user.passwordHash);
                    parameters.Add("@messageID", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    parameters.Add("@messagedescription", dbType: DbType.String, direction: ParameterDirection.Output, size: 255);
                    var userData = await connection.QuerySingleOrDefaultAsync<UserResponseDetailed>("usp_UpdateUser", parameters, commandType: CommandType.StoredProcedure);

                    response.MessageID = parameters.Get<int>("@messageID");
                    response.MessageDescription = parameters.Get<string>("@messagedescription");

                    if (response.MessageID == -2 || response.MessageID == -1 || response.MessageID == -99)
                    {
                        response.User = null;
                    }
                    else
                    {
                        response.User = userData;
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
    }
}
