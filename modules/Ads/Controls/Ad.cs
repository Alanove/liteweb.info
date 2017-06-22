using System;

using lw.Base;


namespace lw.Ads.Controls
{
	/// <summary>
	/// Gets a single AD from the database depending on provided zone, type and keywords
	/// An AD will not appear twice on the same page, Ids are inserte into the page context after each fetch
	/// The AD will render depending on the provided render type, Image, HTML or Name
	/// Impressions will automatically increment after render, It is put in the unload Event because it is not needed for the AD functionallity
	/// </summary>
	public class Ad : System.Web.UI.WebControls.Literal
	{
		bool _bound = false;
		AdRenderType _type = AdRenderType.Image;
		string _zone = "";
		string _keywords = "";

		string _render = "";

		AdsManager _aMgr = new AdsManager();
		AdsView _ad = null;

		public override void DataBind()
		{
			if (_bound)
				return;
			_bound = true;

			CustomPage page = this.Page as CustomPage;

			string ids = "";

			if (page != null)
			{
				if (page.PageContext[cte.AdsContext] != null)
					ids = page.PageContext[cte.AdsContext].ToString();
				else
					page.AddContext(cte.AdsContext, "", false);
			}

			_ad = _aMgr.GetAd(Zone, Keywords, ids);
			if (_ad == null)
				return;

			AdsView ad = _ad;

			if(page != null)
				page.AddContext(cte.AdsContext, string.Format("{0},{1}", page.PageContext[cte.AdsContext], ad.AdId), true);

			switch (_type)
			{
				case AdRenderType.Image:
					if (string.IsNullOrEmpty(ad.Image))
						goto default;
					if (string.IsNullOrEmpty(ad.URL))
					{
						_render = string.Format("<img src=\"{0}\" alt=\"{1}\" />",
						ad.Image,
						ad.Description
						);
					}
					else
					{
						_render = string.Format("<a href=\"{0}\" title=\"{1}\"{3}><img src=\"{2}\" alt=\"{1}\" /></a>",
							ad.URL,
							ad.Description,
							ad.Image,
							ad.NewWindow != null && ad.NewWindow.Value ? " target=\"_blank\"" : ""
							);
					}
					break;
				case AdRenderType.Html:
					if (string.IsNullOrEmpty(ad.HtmlCode))
						goto default;

					_render = ad.HtmlCode;
					break;
				case AdRenderType.Name:
				default:
					_render = string.Format("<a href=\"{0}\" title=\"{1}\">{1}</a>",
						ad.URL, ad.Description);
					break;
			}

			base.DataBind();
		}

		protected override void Render(System.Web.UI.HtmlTextWriter writer)
		{
			if(_ad != null)
			{
				writer.Write(_render);
			}
		}

		protected override void OnUnload(EventArgs e)
		{
			if(_ad != null)
				_aMgr.UpdateAdImpressions(_ad.AdId); 
			base.OnUnload(e);
		}

		#region Properties

		public AdRenderType Type
		{
			get { return _type; }
			set { _type = value; }
		}
		public string Zone
		{
			get { return _zone; }
			set { _zone = value; }
		}
		public string Keywords
		{
			get { return _keywords; }
			set { _keywords = value; }
		}

		#endregion
	}
}
