using System;
using System.Data;
using System.Web.UI;
using lw.CTE.Enum;
using lw.DataControls;
using lw.Networking;
using lw.Utils;
using lw.WebTools;
using lw.CTE;

namespace lw.PhotoAlbums.Controls
{
	public class PhotoAlbumsCategoriesDS : CustomDataSource
	{
		bool _bound = false;
		string _top = "100 PERCENT";
		bool _randomise = false;
		Languages _language = Languages.Default;
		
		PhotoAlbumsManager pMgr;
		
		public PhotoAlbumsCategoriesDS()
		{
			this.DataLibrary = cte.lib;
			OrderBy = "CategoryId Desc";
			pMgr = new PhotoAlbumsManager();
		}

		public override void DataBind()
		{
			if (_bound)
				return;
			_bound = true;

			if (Randomise)
			{
				this.SelectCommand = string.Format("Select top {0} *, newid() as ___Ran from PhotoAlbumsCategories", Top);
				this.OrderBy = "___Ran";
			}
			else
				this.SelectCommand = string.Format("Select top {0} * from PhotoAlbumsCategories", Top);

			
			this.SelectCommand += " Where 1=1";
			
			
			if(_language != Languages.Default)
				this.SelectCommand += string.Format(" And (Language={0} or Language={1})", (int)_language, (int)Languages.Default);

			if (CMSMode != null && CMSMode.Value)
			{
				string q = WebContext.Request["q"];
				string cat = WebContext.Request["CategoryId"];

				if (!string.IsNullOrWhiteSpace(q))
					this.SelectCommand += string.Format(" and (CategoryName like '%{0}%' or UniqueName like '%{0}%')", StringUtils.SQLEncode(q));

				if (!string.IsNullOrWhiteSpace(cat))
					this.SelectCommand += string.Format(" and (CategoryId = {0}", StringUtils.SQLEncode(cat));
			}


			if (!EnablePaging)
			{
				this.SelectCommand += " Order By " + this.OrderBy;
			}

			base.DataBind();
		}


		public string Top
		{
			get { return _top; }
			set { _top = value; }
		}

		public bool Randomise
		{
			get { return _randomise; }
			set { _randomise = true; }
		}

		Languages Language
		{
			get { return _language; }
			set { _language = value; }
		}
	}
}