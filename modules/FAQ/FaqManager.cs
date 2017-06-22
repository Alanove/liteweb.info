using System.Data;
using lw.Utils;
using lw.WebTools;


namespace lw.FAQ
{
	public class FaqManager
	{
		FaqDS ds;

		string config = cte.ConfigFile;

		public FaqManager()
		{
		}

		#region Methods

		/* Start Faqs */

		public bool AddFaq(string Question, string Answer, int GroupId, int Sort)
		{
			FaqDS.FaqRow row = DS.Faq.NewFaqRow();

			row.Question = Question;
			row.Answer = Answer;
			row.GroupId = GroupId;
			row.Sort = Sort;

			ds.Faq.Rows.Add(row);

			AcceptChanges();

			return true;
		}
		public bool UpdateFaq(int FaqId, string Question, string Answer, int GroupId, int Sort)
		{
			DataView dv = this.GetFaqs(string.Format("FaqId = {0}", FaqId));

			if (dv.Count <= 0)
				return false;

			FaqDS.FaqRow row = (FaqDS.FaqRow)dv[0].Row;

			row.Question = Question;
			row.Answer = Answer;
			row.GroupId = GroupId;
			row.Sort = Sort;

			row.Table.AcceptChanges();
			AcceptChanges();

			return true;
		}
		public bool DeleteFaq(int FaqId)
		{
			DataView dv = this.GetFaqs(string.Format("FaqId = {0}", FaqId));

			if (dv.Count <= 0)
				return false;

			FaqDS.FaqRow row = (FaqDS.FaqRow)dv[0].Row;

			row.Delete();
			AcceptChanges();

			return true;
		}

		public FaqDS.FaqRow GetFaq(int FaqId)
		{
			DataView dv = GetFaqs(string.Format("{0}", FaqId));
			FaqDS.FaqRow row = (FaqDS.FaqRow)dv[0].Row;
			return row;
		}
		public DataView GetFaqs()
		{
			return GetFaqs("");
		}
		public DataView GetFaqs(string cond)
		{
			return new DataView(DS.Faq, cond, "FaqId", DataViewRowState.CurrentRows);
		}

		/* End Faqs */

		/* Start Faqs Groups */

		public bool AddFaqGroup(string GroupName)
		{
			if (this.GetFaqGroups("GroupName='" + StringUtils.SQLEncode(GroupName) + "'").Count > 0)
			{
				return false;
			}

			FaqDS.GroupsRow row = DS.Groups.NewGroupsRow();

			row.GroupName = GroupName;

			ds.Groups.Rows.Add(row);

			AcceptChanges();

			return true;
		}
		public bool UpdateFaqGroup(int GroupId, string GroupName)
		{

			DataView dv = GetFaqGroups(string.Format("GroupId={0}", GroupId));

			if (dv.Count <= 0)
				return false;

			FaqDS.GroupsRow row = (FaqDS.GroupsRow)dv[0].Row;


			row.GroupName = GroupName;

			row.Table.AcceptChanges();
			AcceptChanges();

			return true;
		}
		public bool DeleteFaqGroup(int GroupId)
		{
			DataView dv = GetFaqGroups(string.Format("GroupId={0}", GroupId));

			if (dv.Count <= 0)
				return false;

			DataView faqs = this.GetFaqs(string.Format("GroupId = {0}", GroupId));

			foreach (DataRowView drv in faqs)
			{
				DeleteFaq((int)(drv["FaqId"]));
			}

			FaqDS.GroupsRow row = (FaqDS.GroupsRow)dv[0].Row;

			row.Delete();
			AcceptChanges();

			return true;
		}

		public FaqDS.GroupsRow GetFaqGroup(int GroupId)
		{
			DataView dv = GetFaqGroups(string.Format("{0}", GroupId));
			FaqDS.GroupsRow row = (FaqDS.GroupsRow)dv[0].Row;
			return row;
		}
		public DataView GetFaqGroups()
		{
			return GetFaqGroups("");
		}
		public DataView GetFaqGroups(string cond)
		{
			return new DataView(DS.Groups, cond, "GroupId", DataViewRowState.CurrentRows);
		}

		/* End Faqs Groups */

		void AcceptChanges()
		{
			DS.AcceptChanges();
			XmlManager.SetDataSet(config, DS);
		}

		#endregion

		#region variables
		public FaqDS DS
		{
			get
			{
				if (ds == null)
				{
					ds = new FaqDS();
					DataSet _ds = XmlManager.GetDataSet(config);
					if (_ds != null)
						ds.Merge(_ds);
				}
				return ds;
			}
		}
		#endregion
	}
}
