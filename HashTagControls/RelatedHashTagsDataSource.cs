using System;
using System.Data;
using System.Text;
using System.Web.UI;
using lw.Base;
using lw.CTE.Enum;
using lw.DataControls;
using lw.Utils;
using lw.WebTools;
using lw.Data;

namespace lw.HashTags.Controls
{
	/// <summary>
	/// Gets the related tags to a specific page
	/// </summary>
	public class RelatedHashTagsDataSource : CustomDataSource
	{
		bool _bound = false;


		public RelatedHashTagsDataSource()
		{
			this.DataLibrary = cte.lib;
		}

		public override void DataBind()
		{
			if (_bound)
				return;
			_bound = true;

			BuildQuery();


			base.DataBind();
		}



		int? _pageId = null;
		/// <summary>
		/// Returns or sets PageId of which the class will fetch the related pages
		/// </summary>
		public int? PageId
		{
			get
			{
				if (_pageId == null)
				{
					object obj = DataBinder.Eval(this.NamingContainer, "DataItem.PageId");
					if (obj != null)
						_pageId = (int)obj;
					return _pageId;
				}
				if (_pageId == null)
				{
					object obj = MyPage.GetQueryValue("PageId");
					if (obj != null && !String.IsNullOrEmpty(obj.ToString()))
					{
						string _temp = obj.ToString().Trim();
						if (lw.Utils.Validation.IsInteger(_temp))
							_pageId = Int32.Parse(_temp);
					}
					else
					{
						obj = MyPage.GetQueryValue("Id");
						if (obj != null && !String.IsNullOrEmpty(obj.ToString()))
						{
							string _temp = obj.ToString().Trim();
							if (lw.Utils.Validation.IsInteger(_temp))
								_pageId = Int32.Parse(_temp);
						}
					}
				}
				return _pageId;
			}
			set { _pageId = value; }
		}

		/// <summary>
		/// Builds the select command for the data source
		/// </summary>
		public void BuildQuery()
		{
			StringBuilder cond = new StringBuilder();


			this.SelectCommand = @"Select * from HashTags where TagId in (Select TagId from HashTags_Relations where RelateTo={0} and RelationType=1)";

			if (EnablePaging)
			{
				this.OrderBy = "Tag";
			}
			else
			{
				this.SelectCommand += " order by Tag";
			}

			this.SelectCommand = string.Format(this.SelectCommand, PageId);


			///WebContext.Response.Write(this.SelectCommand);
		}

	}
}