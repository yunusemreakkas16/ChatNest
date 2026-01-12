using ChatNest.Models.Domain;
using Microsoft.Data.SqlClient;
using Dapper;
using System.Data;

namespace ChatNest.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly IConfiguration configuration;

        public MessageRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public async Task<DeleteMessageResponseModel> DeleteMessageAsync(Guid messageID, Guid userID)
        {
            var response = new DeleteMessageResponseModel
            {
                MessageID = 0,
                MessageDescription = string.Empty
            };

            try
            {
                using(SqlConnection connection = new SqlConnection(configuration.GetConnectionString("ChatNestConnectionString")))
                {
                    var parameters = new DynamicParameters();

                    parameters.Add("@messageID", messageID);
                    parameters.Add("@userID", userID);

                    parameters.Add("@messageStatus", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    parameters.Add("@messageDescription", dbType: DbType.String, size: 255, direction: ParameterDirection.Output);

                    await connection.ExecuteAsync("usp_DeleteMessage", parameters, commandType: CommandType.StoredProcedure);

                    response.MessageID = parameters.Get<int>("@messageStatus");
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

        public async Task<MessagesListResponseModel> GetMessagesAsync(Guid chatID, Guid userID)
        {
            var response = new MessagesListResponseModel
            {
                GroupMessages = new List<GroupMessageResponse>(),
                OneToOneMessages = new List<OneToOneMessageResponse>(),
                MessageID = 0,
                MessageDescription = string.Empty
            };
            try
            {
                using (SqlConnection connection = new SqlConnection(configuration.GetConnectionString("ChatNestConnectionString")))
                {
                    var parameters = new DynamicParameters();

                    parameters.Add("@chatID", chatID);
                    parameters.Add("@userID", userID);

                    parameters.Add("@isGroup", dbType: DbType.Boolean, direction: ParameterDirection.Output);
                    parameters.Add("@messageID", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    parameters.Add("@messageDescription", dbType: DbType.String, size: 255, direction: ParameterDirection.Output);

                    if (await connection.ExecuteScalarAsync("usp_GetMessagesForChat", parameters, commandType: CommandType.StoredProcedure) == null)
                    {
                        // If no results, check the output parameters
                        response.MessageID = parameters.Get<int>("@messageID");
                        response.MessageDescription = parameters.Get<string>("@messageDescription");
                        return response;
                    }

                    // Get the output parameters
                    bool isGroup = parameters.Get<bool>("@isGroup");
                    response.MessageID = parameters.Get<int>("@messageID");
                    response.MessageDescription = parameters.Get<string>("@messageDescription");

                    if (isGroup)
                    {
                        // For group chat
                        var groupMessages = await connection.QueryAsync<GroupMessageResponse>(
                            "usp_GetMessagesForChat",
                            parameters,
                            commandType: CommandType.StoredProcedure);

                        response.GroupMessages = groupMessages.ToList();
                    }
                    else
                    {
                        // For 1-1 chat
                        var oneToOneMessages = await connection.QueryAsync<OneToOneMessageResponse>(
                            "usp_GetMessagesForChat",
                            parameters,
                            commandType: CommandType.StoredProcedure);

                        response.OneToOneMessages = oneToOneMessages.ToList();
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

        public async Task<SendMessageResponseModel> SendMessageAsync(Message message)
        {
            var response = new SendMessageResponseModel();

            try
            {
                using (SqlConnection connection = new SqlConnection(configuration.GetConnectionString("ChatNestConnectionString")))
                {
                    var parameters = new DynamicParameters();

                    parameters.Add("@chatID", message.chatId);
                    parameters.Add("@senderID", message.senderId);
                    parameters.Add("@content", message.content);

                    parameters.Add("@newMessageID", dbType: DbType.Guid, direction: ParameterDirection.Output);
                    parameters.Add("@messageID", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    parameters.Add("@messageDescription", dbType: DbType.String, size: 255, direction: ParameterDirection.Output);

                    await connection.ExecuteAsync("usp_SendMessage", parameters, commandType: CommandType.StoredProcedure);

                    response.MessageID = parameters.Get<int>("@messageID");
                    response.MessageDescription = parameters.Get<string>("@messageDescription");
                    response.NewMessageID = parameters.Get<Guid>("@newMessageID");
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
