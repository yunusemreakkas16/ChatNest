using static ChatNest.Models.Domain.Friendship;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
namespace ChatNest.Repositories
{
    public class FriendshipRepository : IFriendshipRepository
    {
        private readonly IConfiguration configuration;

        public FriendshipRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<FriendshipStatusResponseModel> CheckFriendshipStatusAsync(Guid userId, Guid targetUserId)
        {
            var response = new FriendshipStatusResponseModel
            {
                status = string.Empty,
                MessageID = 0,
                MessageDescription = string.Empty
            };

            try
            {
                using(var connection = new SqlConnection(configuration.GetConnectionString("ChatNestConnectionString")))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@userID", userId, DbType.Guid);
                    parameters.Add("@targetUserID", targetUserId, DbType.Guid);
                    parameters.Add("@status", dbType: DbType.String, size: 20, direction: ParameterDirection.Output);
                    parameters.Add("@messageID", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    parameters.Add("@messageDescription", dbType: DbType.String, size: 255, direction: ParameterDirection.Output);

                    await connection.ExecuteAsync("usp_CheckFriendshipStatus", parameters, commandType: CommandType.StoredProcedure);

                    response.status = parameters.Get<string>("@status") ?? "none";
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

        public async Task<FriendListResponseModel> GetFriendListAsync(Guid userId)
        {
            var response = new FriendListResponseModel();

            try
            {
                using (var connection = new SqlConnection(configuration.GetConnectionString("ChatNestConnectionString")))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@UserId", userId, DbType.Guid);
                    parameters.Add("@messageID", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    parameters.Add("@messageDescription", dbType: DbType.String, size: 255, direction: ParameterDirection.Output);
                    var friendlistResponse = await connection.QueryAsync<FriendResponse>("usp_GetFriendList", parameters, commandType: CommandType.StoredProcedure);

                    response.Friends = friendlistResponse?.ToList() ?? new List<FriendResponse>();
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

        public async Task<FriendRequestListResponseModel> GetFriendRequestsAsync(Guid userId, string direction)
        {
            var response = new FriendRequestListResponseModel
            {
                FriendRequests = new List<FriendRequestResponse>(),
                MessageID = 0,
                MessageDescription = string.Empty
            };
            try
            {
                using (var connection = new SqlConnection(configuration.GetConnectionString("ChatNestConnectionString")))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@userId", userId, DbType.Guid);
                    parameters.Add("@direction", direction, DbType.String);

                    parameters.Add("@messageID", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    parameters.Add("@messageDescription", dbType: DbType.String, size: 255, direction: ParameterDirection.Output);

                    var friendRequestResponse = await connection.QueryAsync<FriendRequestResponse>("usp_GetFriendRequests", parameters, commandType: CommandType.StoredProcedure);

                    response.FriendRequests = friendRequestResponse?.ToList() ?? new List<FriendRequestResponse>();
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

        public async Task<ManageFriendRequestResponseModel> ManageFriendRequestAsync(Guid clientUserID, Guid otherUserID, string action)
        {
            var response = new ManageFriendRequestResponseModel
            {
                MessageID = 0,
                MessageDescription = string.Empty
            };
            try
            {
                using(var connection = new SqlConnection(configuration.GetConnectionString("ChatNestConnectionString")))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@clientUserID", clientUserID, DbType.Guid);
                    parameters.Add("@otherUserID", otherUserID, DbType.Guid);
                    parameters.Add("@action", action, DbType.String);
                    parameters.Add("@messageID", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    parameters.Add("@messageDescription", dbType: DbType.String, size: 255, direction: ParameterDirection.Output);
                    await connection.ExecuteAsync("usp_ManageFriendRequest", parameters, commandType: CommandType.StoredProcedure);

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

        public async Task<RemoveFriendResponseModel> RemoveFriendAsync(Guid userId, Guid friendId)
        {
            var response = new RemoveFriendResponseModel
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
                    parameters.Add("@friendID", friendId, DbType.Guid);
                    parameters.Add("@messageID", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    parameters.Add("@messageDescription", dbType: DbType.String, size: 255, direction: ParameterDirection.Output);

                    await connection.ExecuteAsync("usp_RemoveFriend", parameters, commandType: CommandType.StoredProcedure);

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

        public async Task<SendFriendRequestResponseModel> SendFriendRequestAsync(Guid requesterId, string receiverEmail)
        {
            var response = new SendFriendRequestResponseModel
            {
                MessageID = 0,
                MessageDescription = string.Empty
            };
            try
            {
                using (var connection = new SqlConnection(configuration.GetConnectionString("ChatNestConnectionString")))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@requesterId", requesterId, DbType.Guid);
                    parameters.Add("@receiverEmail", receiverEmail, DbType.String);
                    parameters.Add("@messageID", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    parameters.Add("@messageDescription", dbType: DbType.String, size: 255, direction: ParameterDirection.Output);

                    await connection.ExecuteAsync("usp_SendFriendRequest", parameters, commandType: CommandType.StoredProcedure);

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
