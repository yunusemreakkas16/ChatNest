using ChatNest.Models.Domain;
using Microsoft.Data.SqlClient;
using Dapper;
using System.Data;
using System.Reflection.Metadata;

namespace ChatNest.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly IConfiguration configuration;

        public ChatRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<AddUserToChatResponseModel> AddUserToChatAsync(Guid chatID, Guid adminID, List<Guid>? userIDs)
        {
            var response = new AddUserToChatResponseModel
            {
                MessageID = 0,
                MessageDescription = string.Empty
            };

            try
            {
                using(SqlConnection connection = new SqlConnection(configuration.GetConnectionString("ChatNestConnectionString")))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@chatID", chatID, DbType.Guid);
                    parameters.Add("@addedByID", adminID, DbType.Guid);
                    parameters.Add("@userIDsToAdd", userIDs != null ? string.Join(",", userIDs) : null, DbType.String);

                    parameters.Add("@messageID", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    parameters.Add("@messageDescription", dbType: DbType.String, size: 255, direction: ParameterDirection.Output);

                    await connection.ExecuteAsync("usp_AddUserToGroupChat", parameters, commandType: CommandType.StoredProcedure);

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

        public async Task<CreateChatResponseModel> CreateChatAsync(Chat chat, List<Guid>? participantIDs, Guid? targetUserID)
        {
            var response = new CreateChatResponseModel
            {
                MessageID = 0,
                MessageDescription = string.Empty
            };

            try
            {
                using(SqlConnection connection = new SqlConnection(configuration.GetConnectionString("ChatNestConnectionString")))
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@creatorID", chat.createdBy, DbType.Guid);
                    parameters.Add("@isGroup", chat.isGroup, DbType.Boolean);
                    parameters.Add("@name", chat.name, DbType.String);

                    parameters.Add("@participantIDs", participantIDs != null ? string.Join(",", participantIDs) : null, DbType.String);
                    parameters.Add("@targetUserID", targetUserID, DbType.Guid);

                    parameters.Add("@chatID", chat.id, DbType.Guid, ParameterDirection.Output);

                    parameters.Add("@messageID", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    parameters.Add("@messageDescription", dbType: DbType.String, size: 255, direction: ParameterDirection.Output);

                    await connection.ExecuteAsync("usp_CreateChat", parameters, commandType: CommandType.StoredProcedure);

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

        public async Task<DeleteChatResponseModel> DeleteChatAsync(Guid chatID, Guid requesterID)
        {
            var response = new DeleteChatResponseModel
            {
                MessageID = 0,
                MessageDescription = string.Empty
            };
            try
            {
                using (SqlConnection connection = new SqlConnection(configuration.GetConnectionString("ChatNestConnectionString")))
                {
                    var parameters = new DynamicParameters();

                    parameters.Add("@chatID", chatID, DbType.Guid);
                    parameters.Add("@requestingUserID", requesterID, DbType.Guid);

                    parameters.Add("@messageID", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    parameters.Add("@messageDescription", dbType: DbType.String, size: 255, direction: ParameterDirection.Output);

                    await connection.ExecuteAsync("usp_DeleteChat", parameters, commandType: CommandType.StoredProcedure);

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

        public async Task<ChatResponseModel> GetChatSummariesForUserAsync(Guid userID)
        {
            var response = new ChatResponseModel
            {
                MessageID = 0,
                MessageDescription = string.Empty,
                Chats = new List<ChatSummary>()
            };

            try
            {
                using(SqlConnection connection = new SqlConnection(configuration.GetConnectionString("ChatNestConnectionString")))
                {
                    var parameters = new DynamicParameters();

                    parameters.Add("@userID", userID, DbType.Guid);

                    parameters.Add("@messageID", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    parameters.Add("@messageDescription", dbType: DbType.String, size: 255, direction: ParameterDirection.Output);

                    var chatSummaries = await connection.QueryAsync<ChatSummary>("usp_GetChatSummariesForUser", parameters, commandType: CommandType.StoredProcedure);
                    response.Chats = chatSummaries.ToList();

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

        public async Task<LeaveChatResponseModel> LeaveChatAsync(Guid chatID, Guid userID)
        {
            var response = new LeaveChatResponseModel
            {
                MessageID = 0,
                MessageDescription = string.Empty
            };
            try
            {
                using (SqlConnection connection = new SqlConnection(configuration.GetConnectionString("ChatNestConnectionString")))
                {
                    var parameters = new DynamicParameters();

                    parameters.Add("@chatID", chatID, DbType.Guid);
                    parameters.Add("@userID", userID, DbType.Guid);

                    parameters.Add("@messageID", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    parameters.Add("@messageDescription", dbType: DbType.String, size: 255, direction: ParameterDirection.Output);

                    await connection.ExecuteAsync("usp_LeaveGroupChat", parameters, commandType: CommandType.StoredProcedure);

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

        public async Task<RemoveUserFromChatResponseModel> RemoveUserFromChatAsync(Guid chatID, Guid adminID, Guid userID)
        {
            var response = new RemoveUserFromChatResponseModel
            {
                MessageID = 0,
                MessageDescription = string.Empty
            };
            try
            {
                using (SqlConnection connection = new SqlConnection(configuration.GetConnectionString("ChatNestConnectionString")))
                {
                    var parameters = new DynamicParameters();

                    parameters.Add("@chatID", chatID, DbType.Guid);
                    parameters.Add("@removedByID", adminID, DbType.Guid);
                    parameters.Add("@userIDToRemove", userID, DbType.Guid);

                    parameters.Add("@messageID", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    parameters.Add("@messageDescription", dbType: DbType.String, size: 255, direction: ParameterDirection.Output);

                    await connection.ExecuteAsync("usp_RemoveUserFromGroupChat", parameters, commandType: CommandType.StoredProcedure);

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

        public async Task<ManageGroupAdmin> SetGroupAdminAsync(Guid chatID, Guid adminID, Guid userID, bool makeAdmin)
        {
            var response = new ManageGroupAdmin
            {
                MessageID = 0,
                MessageDescription = string.Empty
            };
            try
            {
                using (SqlConnection connection = new SqlConnection(configuration.GetConnectionString("ChatNestConnectionString")))
                {
                    var parameters = new DynamicParameters();

                    parameters.Add("@chatID", chatID, DbType.Guid);
                    parameters.Add("@adminUserID", adminID, DbType.Guid);
                    parameters.Add("@targetUserID", userID, DbType.Guid);
                    parameters.Add("@makeAdmin", makeAdmin, DbType.Boolean);

                    parameters.Add("@messageID", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    parameters.Add("@messageDescription", dbType: DbType.String, size: 255, direction: ParameterDirection.Output);

                    await connection.ExecuteAsync("usp_ManageGroupAdmin", parameters, commandType: CommandType.StoredProcedure);

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

        public async Task<UpdateGroupNameResponseModel> UpdateGroupNameAsync(Guid chatID, Guid userID, string newName)
        {
            var response = new UpdateGroupNameResponseModel
            {
                MessageID = 0,
                MessageDescription = string.Empty
            };
            try
            {
                using (SqlConnection connection = new SqlConnection(configuration.GetConnectionString("ChatNestConnectionString")))
                {
                    var parameters = new DynamicParameters();

                    parameters.Add("@chatID", chatID, DbType.Guid);
                    parameters.Add("@userID", userID, DbType.Guid);
                    parameters.Add("@newName", newName, DbType.String);

                    parameters.Add("@messageID", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    parameters.Add("@messageDescription", dbType: DbType.String, size: 255, direction: ParameterDirection.Output);

                    await connection.ExecuteAsync("usp_UpdateGroupName", parameters, commandType: CommandType.StoredProcedure);

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
