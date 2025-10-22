using ChatNest.Models.Domain;
using ChatNest.Models.DTO;
using Microsoft.Data.SqlClient;
using Dapper;
using System.Data;
using ChatNest.Models.Common;
namespace ChatNest.Repositories
{
    public class UserTokenRepository : IUserTokenRepository
    {
        private readonly IConfiguration configuration;

        public UserTokenRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public async Task<UserTokenResponseModel> CreateUserTokenAsync(UserToken userToken)
        {
            var response = new UserTokenResponseModel
            {
                MessageID = 0,
                MessageDescription = string.Empty
            };

            try
            {
                using (var connection = new SqlConnection(configuration.GetConnectionString("ChatNestConnectionString")))
                {
                    var parameters = new DynamicParameters();

                    parameters.Add("@userID", userToken.userId, DbType.Guid);
                    parameters.Add("@token", userToken.token, DbType.String);
                    parameters.Add("@refreshToken", userToken.refreshToken, DbType.String);
                    parameters.Add("@userAgent", userToken.userAgent, DbType.String);
                    parameters.Add("@ipAddress", userToken.ipAddress, DbType.String);
                    // Don't forget to asssign a value on expiresAt value on service layer
                    parameters.Add("@expiresAt", userToken.expiresAt, DbType.DateTime2);
                    parameters.Add("@refreshExpiresAt", userToken.refreshExpiresAt, DbType.DateTime2);

                    parameters.Add("@messageID", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    parameters.Add("@messageDescription", dbType: DbType.String, direction: ParameterDirection.Output, size: 255);

                    await connection.ExecuteAsync("InsertUserToken", parameters, commandType: CommandType.StoredProcedure);

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

        public async Task<UserTokenResponseModel> DeactivateAllUserTokensAsync(Guid userId)
        {
            var response = new UserTokenResponseModel
            {
                MessageID = 0,
                MessageDescription = string.Empty
            };

            try
            {
                using (var connection = new SqlConnection(configuration.GetConnectionString("ChatNestConnectionString")))
                {
                    var parameters = new DynamicParameters();

                    parameters.Add("@userID", userId, DbType.Guid);

                    parameters.Add("@messageID", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    parameters.Add("@messageDescription", dbType: DbType.String, direction: ParameterDirection.Output, size: 255);

                    await connection.ExecuteAsync("DeactivateAllUserTokens", parameters, commandType: CommandType.StoredProcedure);

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


        public async Task<UserTokenResponseModel> DeactivateTokenAsync(string token)
        {
            var response = new UserTokenResponseModel
            {
                MessageID = 0,
                MessageDescription = string.Empty
            };

            try
            {
                using (var connection = new SqlConnection(configuration.GetConnectionString("ChatNestConnectionString")))
                {
                    var parameters = new DynamicParameters();

                    parameters.Add("@token", token, DbType.String);
                    parameters.Add("@messageID", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    parameters.Add("@messageDescription", dbType: DbType.String, direction: ParameterDirection.Output, size: 255);

                    await connection.ExecuteAsync("DeactivateToken", parameters, commandType: CommandType.StoredProcedure);

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

        public async Task<UserTokenResponseModel> ValidateUserTokenAsync(string Token, string UserAgent, string IpAddress)
        {
            var response = new UserTokenResponseModel
            {
                MessageID = 0,
                MessageDescription = string.Empty
            };

            try
            {
                using (var connection = new SqlConnection(configuration.GetConnectionString("ChatNestConnectionString")))
                {
                    var parameters = new DynamicParameters();

                    parameters.Add("@token", Token, DbType.String);
                    parameters.Add("@userAgent", UserAgent, DbType.String);
                    parameters.Add("@ipAddress", IpAddress, DbType.String);

                    parameters.Add("@messageID", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    parameters.Add("@messageDescription", dbType: DbType.String, direction: ParameterDirection.Output, size: 255);

                    await connection.ExecuteAsync("ValidateUserToken", parameters, commandType: CommandType.StoredProcedure);

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

        public async Task<UserTokenResponseModelByRefreshToken> GetUserTokenByRefreshTokenAsync(string RefreshToken, string UserAgent, string IpAddress)
        {
            var response = new UserTokenResponseModelByRefreshToken();

            try
            {

                using (var connection = new SqlConnection(configuration.GetConnectionString("ChatNestConnectionString")))
                {
                    var dynamicParameters = new DynamicParameters();

                    dynamicParameters.Add("@refreshToken", RefreshToken, DbType.String);
                    dynamicParameters.Add("@userAgent", UserAgent, DbType.String);
                    dynamicParameters.Add("@ipAddress", IpAddress, DbType.String);

                    dynamicParameters.Add("@messageID", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    dynamicParameters.Add("@messageDescription", dbType: DbType.String, direction: ParameterDirection.Output, size: 255);

                    var userToken = await connection.QueryFirstOrDefaultAsync<UserToken>("GetUserTokenByRefreshToken", dynamicParameters, commandType: CommandType.StoredProcedure);

                    response.MessageID = dynamicParameters.Get<int>("@messageID");
                    response.MessageDescription = dynamicParameters.Get<string>("@messageDescription");

                    if (userToken != null)
                    {
                        response.Data = userToken;
                    }
                    else
                    {
                        response.MessageID = -1; // Not found
                        response.MessageDescription = "Refresh token not found or invalid.";
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

        public async Task<UserTokenResponseModel> DeactivateByRefreshTokenAsync(string refreshToken)
        {
            var response = new UserTokenResponseModel
            {
                MessageID = 0,
                MessageDescription = string.Empty
            };
            try
            {
                using (var connection = new SqlConnection(configuration.GetConnectionString("ChatNestConnectionString")))
                {
                    var parameters = new DynamicParameters();

                    parameters.Add("@refreshToken", refreshToken, DbType.String);

                    parameters.Add("@messageID", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    parameters.Add("@messageDescription", dbType: DbType.String, direction: ParameterDirection.Output, size: 255);

                    await connection.ExecuteAsync("DeactivateByRefreshToken", parameters, commandType: CommandType.StoredProcedure);

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
