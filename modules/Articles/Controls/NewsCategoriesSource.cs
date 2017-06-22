using System;
using lw.DataControls;
using lw.Utils;

namespace lw.Articles.Controls
{
	public class NewsCategoriesSource : CustomDataSource
	{
		bool bound = false;

		string _parentCategory = null;

		public NewsCategoriesSource()
		{
			this.SelectCommand = "select *,NewsTypes.TypeId as NewsType from NewsTypes";
			this.DataLibrary = cte.lib;
		}

		public override void DataBind()
		{
			if (bound)
				return;
			bound = true;

			BuildQuery();


			base.DataBind();
		}


		public void BuildQuery()
		{
			string cond = "";
			if (!String.IsNullOrEmpty(Category))
			{
				cond += string.Format(" where Parent='{0}' or ParentUniqueName='{0}'", StringUtils.SQLEncode(Category));
			}
			this.SelectCommand += cond;
		}

		public string Category
		{
			get
			{
				return _parentCategory;
			}
			set
			{
				_parentCategory = value;
			}
		}

	}

}