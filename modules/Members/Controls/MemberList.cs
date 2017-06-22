using System;
using System.Data;
using System.Text;
using System.Web.UI;
using lw.CTE.Enum;
using lw.DataControls;
using lw.Utils;
using lw.WebTools;

namespace lw.Members.Controls
{
	public class MemberList : CustomDataSource
	{
		bool _bound = false;
		bool dataBound = false;
		MemberListBy _ListBy = MemberListBy.All;
		int? _max = null;
        string _relatedChapter = null;

		public MemberList()
		{
			this.DataLibrary = cte.lib;
			this.OrderBy = "LastName";
		}
		public override object Data
		{
			get
			{
				if (dataBound)
					return base.Data;
				dataBound = true;

				return base.Data;
			}
			set
			{
			}
		}
		public override void DataBind()
		{
			if (_bound)
				return;
			_bound = true;

			StringBuilder cond = new StringBuilder();

			IDataItemContainer namingContainer = this.NamingContainer as IDataItemContainer;
			DataRow parentRow = null;


			try
			{
				parentRow = DataBinder.Eval(this.NamingContainer, "DataItem") as DataRow;
				if (parentRow == null)
				{
					DataRowView memberRowView = DataBinder.Eval(this.NamingContainer, "DataItem") as DataRowView;
					if (memberRowView != null)
						parentRow = memberRowView.Row;
				}
			}
			catch
			{
			}

			switch (ListBy)
			{
				case MemberListBy.All:
					break;
				case MemberListBy.Smart:
					bool isSearch = false;


					if (!isSearch)
					{
						this.SelectCommand = String.Format(@"select {1} * from memberview
where 
(	memberid 
	in ( select MemberId from MemberEducation 
		where UniversityId 
		in ( select UniversityId from MemberEducation
			where MemberId = {0})
		)
	or MemberId 
	in	( select MemberId from MemberLocation 
			where RegionId 
			in ( select RegionId from MemberLocation 
				where MemberId = {0})
			or CountryId 
			in ( select CountryId from MemberLocation
				where MemberId = {0})
			)
	or SABISSchoolAttended 
	in ( select SABISSchoolAttended from MemberProfile 
		Where MemberId = {0})
)", WebContext.Profile.UserId, getMax());
					}
					break;
				case MemberListBy.SameRegion:
					this.SelectCommand = String.Format(@"select {1} * from memberview
where 
(	MemberId 
	in	( select MemberId from MemberLocation 
			where RegionId 
			in ( select RegionId from MemberLocation 
				where MemberId = {0})
			or CountryId 
			in ( select CountryId from MemberLocation
				where MemberId = {0})
			)
)", WebContext.Profile.UserId, getMax());

					break;
				case MemberListBy.Search:

					this.Visible = WebContext.Request.QueryString["s"] == "1";

					break;
				case MemberListBy.Friends:

					int _memberId = WebContext.Profile.UserId;

					if (parentRow != null)
					{
						_memberId = (int)parentRow["MemberId"];
					}
					cond.Append(string.Format(" and MemberId in (select FriendId from Friends where Status=1 and MemberId={0}) and MemberId in (select MemberId from Friends where Status=1 and FriendId={0})", _memberId));
					break;
				case MemberListBy.PendingRequests:
					cond.Append(string.Format(" and MemberId in (select FriendId from Friends where Status=0 and MemberId={0})", WebContext.Profile.UserId));
					break;
				case MemberListBy.SameUniversity:
					this.SelectCommand = String.Format(@"select {1} * from memberview
where 
(	memberid 
	in ( select MemberId from MemberEducation 
		where UniversityId 
		in ( select UniversityId from MemberEducation
			where MemberId = {0})
		)
)", WebContext.Profile.UserId, getMax());
					break;
				case MemberListBy.SameNetwork:
					this.SelectCommand = String.Format(@"select {1} * from memberview
where 
(	SABISSchoolAttended 
	in ( select SABISSchoolAttended from MemberProfile 
		Where MemberId = {0})
)", WebContext.Profile.UserId, getMax());

					//cond.Append(string.Format(" and MemberId in (select MemberId from MemberNetworks where NetworkId in (Select NetworkId from MemberNetworks where MemberId={0}))", WebContext.Profile.UserId));
					break;

                case MemberListBy.ChapterMembers:
                    this.SelectCommand = String.Format(@"select {0} * from MemberView where memberid in (select memberid from groupsmembers 
	where groupid in (select id from groups where uniquename like '{1}'))", getMax(), RelatedChapter);

                    //cond.Append(string.Format(" and MemberId in (select MemberId from MemberNetworks where NetworkId in (Select NetworkId from MemberNetworks where MemberId={0}))", WebContext.Profile.UserId));
                    break;
				default:
					break;
			}


			if (String.IsNullOrWhiteSpace(this.SelectCommand))
			{
				this.SelectCommand = string.Format("select {0} * from MemberView where 1=1", getMax());
			}

            cond.Append(String.Format(" and Status&{0}<>0", ((int)UserStatus.Enabled) | ((int)UserStatus.Modified)));

			if (ListBy != MemberListBy.Friends)
				cond.Append(string.Format(" And MemberId<>{0}", WebContext.Profile.UserId));

            // Remove Admin Admin userId = 1 from data feed
            if (WebContext.Profile.UserId != 1)
                cond.Append(string.Format(" And MemberId<>{0}", 1));

			string temp = WebContext.Request.QueryString["MemberKeyword"];
			if (string.IsNullOrWhiteSpace(temp))
				temp = WebContext.Request.QueryString["q"];
			if (!String.IsNullOrWhiteSpace(temp))
			{
				cond.Append(" and (");
				cond.Append(StringUtils.SplitSearchKeyword(temp, "Name"));
				cond.Append(" or ");
				cond.Append(StringUtils.SplitSearchKeyword(temp, "UserName"));
				cond.Append(" or ");
				cond.Append("Email = '" + StringUtils.SQLEncode(temp) + "'");
				cond.Append(")");
			}

			temp = WebContext.Request.QueryString["MemberUniversity"];
			if (!String.IsNullOrWhiteSpace(temp))
			{
				cond.Append(" and MemberId in (select MemberId from MemberEducationView where ");
				cond.Append(StringUtils.SplitSearchKeyword(temp, "Name"));
				cond.Append(" or ");
				cond.Append(StringUtils.SplitSearchKeyword(temp, "UniversityName"));
				cond.Append(")");
			}

			temp = WebContext.Request.QueryString["_ctl0:Content:SABISSchoolAttended"];
			if (!String.IsNullOrWhiteSpace(temp))
			{
				cond.Append(" and SABISSchoolAttended=" + Int32.Parse(temp).ToString());
			}
			temp = WebContext.Request.QueryString["_ctl0:Content:GraduationYear"];
			if (!String.IsNullOrWhiteSpace(temp))
			{
				cond.Append(" and GraduationYear='" + StringUtils.SQLEncode(temp) + "'");
			}

			temp = WebContext.Request.QueryString["_ctl0:Content:Country"];
			if (!String.IsNullOrWhiteSpace(temp))
			{
				cond.Append(" and MemberId in (select MemberId from MemberLocationView where CountryId='" + StringUtils.SQLEncode(temp) + "')");
			}

			temp = WebContext.Request.QueryString["City"];
			if (!String.IsNullOrWhiteSpace(temp))
			{
				cond.Append(" and MemberId in (select MemberId from MemberLocationView where ");
				cond.Append(StringUtils.SplitSearchKeyword(temp, "City"));
				cond.Append(" or ");
				cond.Append(StringUtils.SplitSearchKeyword(temp, "Region"));
				cond.Append(" or ");
				cond.Append(StringUtils.SplitSearchKeyword(temp, "Address"));
				cond.Append(")");
			}

			temp = WebContext.Request.QueryString["Company"];
			if (!String.IsNullOrWhiteSpace(temp))
			{
				cond.Append(" and MemberId in (select MemberId from MemberCareer where ");
				cond.Append(StringUtils.SplitSearchKeyword(temp, "Company"));
				cond.Append(")");
			}

			if (!string.IsNullOrWhiteSpace(Filter))
				cond.Append(" and " + Filter);

			this.SelectCommand = this.SelectCommand + cond.ToString();

			//WebContext.Response.Write(this.SelectCommand);
			//WebContext.Response.End();

			this.OrderBy = "LastName";

			base.DataBind();
		}


		#region properties
		string _filter = "";
		/// <summary>
		/// Adds an additional sql filter to the list.
		/// </summary>
		public string Filter
		{
			get
			{
				return _filter;
			}
			set
			{
				_filter = value;
			}
		}

		public MemberListBy ListBy
		{
			get { return _ListBy; }
			set { _ListBy = value; }
		}

        public String RelatedChapter
        {
            get { return _relatedChapter; }
            set { _relatedChapter = value; }
        }

		public int? Max
		{
			get { return _max; }
			set { _max = value; }
		}

		public string getMax()
		{
			return (Max != null ? "Top " + Max : "");
		}
		#endregion
	}
}
