using System;
using System.Data;
using System.Text;
using System.Web.UI;
using lw.Base;
using lw.Data;
using lw.DataControls;

namespace lw.Members.Controls
{
	public class MemberEducationList : CustomDataSource
	{
		bool _bound = false;
		EmptyDataSrc _dataSrc = null;

		
		public override void DataBind()
		{
			if (_bound)
				return;
			_bound = true;

			CustomPage page = this.Page as CustomPage;
			DataTable cachedData = null;
			MemberList memberListParent = null;

			Object obj = page.PageContext[cte.MemberEducationContext];

			_dataSrc = new EmptyDataSrc();

			if (obj != null)
			{
				cachedData = obj as DataTable;
			}
			else
			{
				Control parent = this.Parent;

				while (parent != null)
				{
					memberListParent = parent as MemberList;

					if (memberListParent != null)
						break;

					parent = parent.Parent;
				}

				if (memberListParent != null)
				{
					DataTable existingList = memberListParent.Data as DataTable;

					if (existingList != null)
					{
						StringBuilder filter = new StringBuilder();

						string sep = "";
						foreach (DataRow member in existingList.Rows)
						{
							filter.Append(sep);
							filter.Append(member["MemberId"]);
							sep = ",";
						}

						cachedData = DBUtils.GetDataSet(String.Format("Select * from MemberEducationView where MemberId in ({0})",
							filter.ToString()), cte.lib).Tables[0];
					}
				}
			}

			_dataSrc = new EmptyDataSrc();

			int currentUser = (int)DataBinder.Eval(this.NamingContainer, "DataItem.MemberId");


			if (cachedData != null)
			{
				this.Data = new DataView(cachedData, string.Format("MemberId={0}", currentUser), "UniversityName", DataViewRowState.CurrentRows);

				_dataSrc.RowsCount = cachedData.Rows.Count;
			}
			else
			{
				DataTable temp = DBUtils.GetDataSet(String.Format("Select * from MemberEducationView where MemberId = {0}",
							currentUser), cte.lib).Tables[0];
				this.Data = temp;

				_dataSrc.RowsCount = temp.Rows.Count;
			}

			_dataSrc.Data = this.Data;
			_dataSrc.HasData = _dataSrc.RowsCount > 0;
			base.DataBind();
		}

		public override Data.IDataSource DataSrc
		{
			get
			{
				return _dataSrc;
			}
			set
			{
				base.DataSrc = value;
			}
		}
	}
}
