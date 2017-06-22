
using lw.Utils;
using lw.WebTools;
namespace lw.Members
{
	/// <summary>
	/// Class containing all the constants that are using in the <see cref="Members"/> library
	/// </summary>
	public class cte
	{
		public const string lib = "MembersManager";

		public const string MembersAdp = "MembersAdp";

		public const string UpdateMembersCmd = "UpdateMembersCmd";


		public const int UserAlreadyExists = -2;
		public const int EmailAlreadyExists = -3;


		public const string NetworkTable = "MemberNetworks";
		public const string NetworkRelateToField = "MemberId";


		public const string LoggedInUserContextKey = "LoggedInUser";

		public const string MemberEducationContext = "MemberEducationContext";

		public const string AlternatePictureSize = "AlternatePictureSize";
	}
}