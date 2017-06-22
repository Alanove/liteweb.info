using System.Data;
using System.Text;
using lw.Base;
using lw.CTE.Enum;
using lw.DataControls;
using lw.Utils;

namespace lw.Articles.Controls
{
	public class NewsArchiveDS : CustomDataSource
	{
		bool bound = false;
		string type = "";
		string path = "";
		string setLanguage = "";
		Languages? _Language;
		CustomPage _page;

		public NewsArchiveDS()
		{
			this.DataLibrary = cte.lib;
			this.OrderBy = "Month Desc";
		}

		public override void DataBind()
		{
			if (bound)
				return;
			bound = true;

			_page = this.Page as CustomPage;

			StringBuilder cond = new StringBuilder();
			NewsManager nMgr = new NewsManager();

			
			if (Type == "")
			{ 
				Type= _page.GetQueryValue("NewsType");
			}

			if (!StringUtils.IsNullOrWhiteSpace(Type))
			{
				DataView types = nMgr.GetNewsTypes(string.Format("UniqueName='{0}'", StringUtils.SQLEncode(Type)));
				if (types.Count > 0)
					cond.Append(string.Format(" And NewsType={0}", types[0]["TypeId"]));
			}

			CustomPage cPage = this.Page as CustomPage;

			if (_Language == null)
			{
				if (cPage != null)
					Language = cPage.Language;
			}

			switch (Language)
			{
				case Languages.French:
					setLanguage = "SET LANGUAGE French;";
					break;
				default:
					break;
			}
			
			if(Language != Languages.Default)
				cond.Append(string.Format(" And NewsLanguage={0}", (int)Language));
			
			string sql = "";
			if (cond.Length > 0)
				sql = cond.ToString().Substring(5);

			this.SelectCommand = string.Format("{0} SELECT * FROM NewsDateView WHERE {1}", setLanguage, sql);


			if (!EnablePaging)
			{
				this.SelectCommand += " Order By " + this.OrderBy;
			}

			base.DataBind();
		}

		public string Type
		{
			get { return type; }
			set { type = value; }
		}
		public string Path
		{
			get { return path; }
			set { path = value; }
		}
		public Languages Language
		{
			get { return _Language.Value; }
			set { _Language = value; }
		}
	}
}
