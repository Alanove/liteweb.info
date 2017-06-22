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
	public class MediaHashTagsDataSource : CustomDataSource
	{
		bool _bound = false;
		HashTagTypes _mediaType = HashTagTypes.Media;


		public MediaHashTagsDataSource()
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

			if (1==1)
			{
				cond.AppendFormat(@"TagId in ( 
	select TagId from HashTags_Relations where RelateTo in (
		select Id from Media) and 
	RelationType=" + (int)MediaType + ")");
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

		public HashTagTypes MediaType
		{
			get
			{ return _mediaType; }
			set
			{ _mediaType = value; }
		}
		
		#endregion
	}
}