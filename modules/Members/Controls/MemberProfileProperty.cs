using System;
using System.Data;
using System.Web.UI;
using lw.DataControls;
using lw.WebTools;
using lw.Utils;

namespace lw.Members.Controls
{
	public class MemberProfileProperty : System.Web.UI.WebControls.Literal, IDataProperty
	{
		bool _bound = false;
		string _property = "";
		string _format = "{0}";
		bool _iVisible = true;


		public override void DataBind()
		{
			if (_bound)
				return;
			_bound = true;
			DataRow memberRow = DataBinder.Eval(this.NamingContainer, "DataItem") as DataRow;
			if (memberRow == null)
			{
				DataRowView memberRowView = DataBinder.Eval(this.NamingContainer, "DataItem") as DataRowView;
				if (memberRowView != null)
					memberRow = memberRowView.Row;
			}
			if (memberRow == null)
				return;

			int privacy = (int)memberRow["Privacy"];
		
			if (memberRow == null)
			{
				DataRowView drv = DataBinder.Eval(this.NamingContainer, "DataItem") as DataRowView;
				if (drv == null)
				{
					ErrorContext.Add("invalid-dataitem", "Invalid Container for: " + Property);
					this.Visible = false;
					return;
				}
				memberRow = drv.Row;
			}
			object obj = null;
			if(memberRow.Table.Columns.Contains(Property))
				obj = memberRow[Property];

			PrivacySettingsManager psMgr = new PrivacySettingsManager();

			this.Visible = psMgr.CanAccess(memberRow, Property, this);

			if (!this.Visible)
				return;
			
			int MemberId = (int)memberRow["MemberId"];

			switch (Property)
			{
				/*case "Network":
					MemberNetworksManager nMgr = new MemberNetworksManager();
					IQueryable<MemberNetworksView> networks = nMgr.GetMemberNetworksView((int)memberRow["MemberId"]);
					if (networks.Count() > 0)
					{
						obj = networks.First().Name;
					}
					break;
				 */
				case "Gender":
					if (memberRow["Gender"] != DBNull.Value && memberRow["Gender"] != null)
					{
						Gender gender = (Gender)Enum.Parse(typeof(Gender), memberRow["Gender"].ToString());
						obj = gender.ToString();
					}
					break;
				case "Email":
					break;
				case "StudentID":
					if(MemberId != WebContext.Profile.UserId)
						this.Visible = false;
					break;
				default:
					break;
			}

			if (obj != null && obj.ToString().Trim() != "")
			{

				obj = string.Format(Format, obj);

				this.Text = StringUtils.AddSup(obj.ToString());
			}
			base.DataBind();

			_iVisible = this.Visible && !String.IsNullOrWhiteSpace(this.Text);
		}
		protected override void Render(HtmlTextWriter writer)
		{
	//		this.Text = string.Format(_format, this.Text);


			base.Render(writer);
		}

		public string Property
		{
			get
			{
				return _property;
			}
			set
			{
				_property = value;
			}
		}
		public string Format
		{
			set
			{
				_format = value;
			}
			get
			{
				return _format;
			}
		}

		public bool IVisible
		{
			get
			{
				return _iVisible;
			}
		}
	}
}
