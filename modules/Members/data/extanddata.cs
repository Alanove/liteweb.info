
namespace lw.Members
{
	public class MembersAdp : lw.Members.MembersDsTableAdapters.MembersTableAdapter
	{
		public MembersDs.MembersDataTable GetDataCondition(string condition)
		{
			if (condition != "")
				condition = " where " + condition;

			base.CommandCollection[0].CommandText += condition;
			return base.GetData();
		}
	}
}