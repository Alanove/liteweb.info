using System;
using System.Collections.Generic;

namespace lw.CTE
{
	/// <summary>
	/// Holds data for chat information that needs to be saved in the profile.
	/// </summary>
	public class ChatProfile
	{
		public List<Conversation> ClosedConversations = new List<Conversation>();


		/// <summary>
		/// Friend List Status
		/// 2 = Open
		/// 2|16 = 18 Minimized (just like any chat box)
		/// </summary>
		public ConversationStatus fls = ConversationStatus.Open;

		public ChatProfile()
		{
		}
	}

	[Serializable]
	public class Conversation
	{
		public int ConversationId;
		public int LastMessage;

		public Conversation()
		{
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="cId">Conversation ID</param>
		/// <param name="lM">Last Message related to this conversation</param>
		public Conversation(int cId, int lM)
		{
			ConversationId = cId;
			LastMessage = lM;
		}
	}

	public enum ConversationStatus
	{
		Open = 2, //base for open conversations or maximized by default
		Pending = 8,  //base for closed conversations
		Minimized = 18, //2|16,
		Closed = 40 //32|8
	}
}
