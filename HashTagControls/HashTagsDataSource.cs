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
	/// Gets all the tags depending on different search criterias.
	/// </summary>
	public class HashTagsDataSource : CustomDataSource
	{
		bool _bound = false;
		string type = null;


		public HashTagsDataSource()
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


		/// <summary>
		/// Builds the select command for the data source
		/// </summary>
		public void BuildQuery()
		{
			StringBuilder cond = new StringBuilder();

			if (!String.IsNullOrWhiteSpace(ParentURL))
			{
				if (Recursive)
				{
					cond.AppendFormat(@"TagId in ( 
	select TagId from HashTags_Relations where RelateTo in (
		select PageId from dbo.Pages_GetDescendantsByPageName('{0}')
	) and 
	RelationType=1)", StringUtils.SQLEncode(ParentURL));
				}
				else
				{
					cond.AppendFormat(@"TagId in ( 
	select TagId from HashTags_Relations where RelateTo in (
		select PageId from Pages where URL='{0}' or Title='{0}'
	) and 
	RelationType=1)", StringUtils.SQLEncode(ParentURL));
				}
			}

			if (!String.IsNullOrWhiteSpace(CustomCondition))
			{
				cond.Append(CustomCondition);
			}

			this.SelectCommand = "Select * from HashTagsView";
			if (cond.Length > 0)
			{
				this.SelectCommand += " where " + cond.ToString();
			}

			if (EnablePaging)
			{
				this.OrderBy = "Tag";
			}
			else
			{
				this.SelectCommand += " order by Tag";
			}


			//WebContext.Response.Write(this.SelectCommand);
		}

		#region Properties


		string _customCondition = "";
		/// <summary>
		/// Ads a customized condition to the selection query.
		/// </summary>
		public string CustomCondition
		{
			get { return _customCondition; }
			set { _customCondition = value; }
		}

		/// <summary>
		/// Returns or sets NewsType (Name) 
		/// Priority: Tag Value, Route, QueryString
		/// </summary>
		/// 
		public string ParentURL
		{
			get
			{
				if (type == null)
				{
					type = MyPage.GetQueryValue("ParentURL");
				}
				return type;
			}
			set 
			{
				type = value; 
			}
		}



		bool _recursive = true;
		/// <summary>
		/// If true the data source will fetch the data from the category and all child categories
		/// Default: false
		/// </summary>
		public bool Recursive
		{
			get
			{
				return _recursive;
			}
			set
			{
				_recursive = value;
			}
		}

		int? _recursiveLevel = null;
		public int? RecursiveLEvel
		{
			get
			{
				return _recursiveLevel;
			}
			set
			{
				_recursiveLevel = value;
			}
		}

		
		#endregion
	}
}