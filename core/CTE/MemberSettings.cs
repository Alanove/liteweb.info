
namespace lw.CTE
{
	public class MembersSettings
	{
		public const string SecretQuestionsFile = "SecretQuestions.config";
		public const string AwareFile = "Aware.config";
		public const string PrivacyFile = "member-privacy.config";

		public const string SignOutFlag = "SignOutFlag";



		public const string MemberPicturesFolder = "Prv/Images/member-pictures";
		public const int ProfilePictureWidth = 1920; //used only for original resize
		//height is not usually considered, this value is only put as maximum) but the resizing will be proportional
		public const int ProfilePictureHeight = 1080;


        public const int MProfilePictureWidth = 800;
        public const int MProfilePictureHeight = 800;

        public const int SProfilePictureWidth = 250;
        public const int SProfilePictureHeight = 250;

		public const string ProfilesDirectory = "ProfilesDirectory";
	}
}