using System;
using System.Data;
using System.Data.SqlClient;
using lw.CTE;
using lw.Data;
using lw.Members;

namespace lw.Chat
{
	public class ChatManager
	{
		private string lib = "";

		public ChatManager()
		{
			//some tests
			lib = cte.lib;
			//test

		}

		public int OpenConversation(int userId1, int userId2)
		{
			SqlCommand command = DBUtils.StoredProcedure("Chat_OpenConversation", cte.lib);
			DBUtils.AddProcedureParameter(command, "@userFrom", SqlDbType.Int, userId1, ParameterDirection.Input);
			DBUtils.AddProcedureParameter(command, "@userTo", SqlDbType.Int, userId2, ParameterDirection.Input);
			DBUtils.AddProcedureParameter(command, "@ConversationId", SqlDbType.Int, -1, ParameterDirection.Output);

			command.Connection.Open();
			command.ExecuteNonQuery();
			int conversationId = (int) command.Parameters["@ConversationId"].Value;
			command.Connection.Close();

			return conversationId;
		}

		public int OpenGroupConversation(int userId1, int userId2, int convId)
		{
			SqlCommand command = DBUtils.StoredProcedure("Chat_OpenGroupConversation", cte.lib);
			DBUtils.AddProcedureParameter(command, "@convId", SqlDbType.Int, convId, ParameterDirection.Input);
			DBUtils.AddProcedureParameter(command, "@userFrom", SqlDbType.Int, userId1, ParameterDirection.Input);
			DBUtils.AddProcedureParameter(command, "@userTo", SqlDbType.Int, userId2, ParameterDirection.Input);
			DBUtils.AddProcedureParameter(command, "@ConversationId", SqlDbType.Int, -1, ParameterDirection.Output);

			command.Connection.Open();
			command.ExecuteNonQuery();
			int conversationId = (int) command.Parameters["@ConversationId"].Value;
			command.Connection.Close();

			return conversationId;
		}

		public int JoinUser(int userId2, int convId)
		{
			SqlCommand command = DBUtils.StoredProcedure("Chat_JoinUser", cte.lib);
			DBUtils.AddProcedureParameter(command, "@convId", SqlDbType.Int, convId, ParameterDirection.Input);
			DBUtils.AddProcedureParameter(command, "@userTo", SqlDbType.Int, userId2, ParameterDirection.Input);
			DBUtils.AddProcedureParameter(command, "@ConversationId", SqlDbType.Int, -1, ParameterDirection.Output);

			command.Connection.Open();
			command.ExecuteNonQuery();
			int conversationId = (int) command.Parameters["@ConversationId"].Value;
			command.Connection.Close();

			return conversationId;
		}

		public int LeaveUser(int userId2, int convId)
		{
			SqlCommand command = DBUtils.StoredProcedure("Chat_LeaveUser", cte.lib);
			DBUtils.AddProcedureParameter(command, "@convId", SqlDbType.Int, convId, ParameterDirection.Input);
			DBUtils.AddProcedureParameter(command, "@userTo", SqlDbType.Int, userId2, ParameterDirection.Input);
			DBUtils.AddProcedureParameter(command, "@ConversationId", SqlDbType.Int, -1, ParameterDirection.Output);

			command.Connection.Open();
			command.ExecuteNonQuery();
			int conversationId = (int) command.Parameters["@ConversationId"].Value;
			command.Connection.Close();

			return conversationId;
		}

		public void UpdateConversationStatus(int ConversationId, int UserId, int Status)
		{
			UpdateConversationStatus(ConversationId, UserId,
				(ConversationStatus) Enum.Parse(typeof (ConversationStatus), Status.ToString()));
		}

		public void UpdateConversationStatus(int ConversationId, int UserId, ConversationStatus Status)
		{
			SqlCommand command = DBUtils.StoredProcedure("Chat_UpdateConversationStatus", cte.lib);
			DBUtils.AddProcedureParameter(command, "@ConversationId", SqlDbType.Int, ConversationId, ParameterDirection.Input);
			DBUtils.AddProcedureParameter(command, "@UserId", SqlDbType.Int, UserId, ParameterDirection.Input);
			DBUtils.AddProcedureParameter(command, "@Status", SqlDbType.Int, (int) Status, ParameterDirection.Input);

			command.Connection.Open();
			command.ExecuteNonQuery();
			command.Connection.Close();
		}

		/// <summary>
		/// Sends a message to the conversation opening all pending conversation for receiptions
		/// </summary>
		/// <param name="UserFrom">User Id</param>
		/// <param name="ConversationId">ConversationId</param>
		/// <param name="Message">Chat Message</param>
		/// <returns>MessageId can be used to communicate with client messageid</returns>
		public int SendMessage(int UserFrom, int ConversationId, string Message)
		{
			SqlCommand command = DBUtils.StoredProcedure("Chat_SendMessage", cte.lib);
			DBUtils.AddProcedureParameter(command, "@UserFrom", SqlDbType.Int, UserFrom, ParameterDirection.Input);
			DBUtils.AddProcedureParameter(command, "@ConversationId", SqlDbType.Int, ConversationId, ParameterDirection.Input);
			DBUtils.AddProcedureParameter(command, "@Message", SqlDbType.NText, Message, ParameterDirection.Input);

			DBUtils.AddProcedureParameter(command, "@MessageId", SqlDbType.Int, -1, ParameterDirection.Output);

			command.Connection.Open();
			command.ExecuteNonQuery();
			int MessageId = (int) command.Parameters["@MessageId"].Value;
			command.Connection.Close();

			return MessageId;
		}
	}
}