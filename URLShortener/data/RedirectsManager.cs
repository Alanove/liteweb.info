using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using lw.CTE;
using lw.Data;
using lw.Utils;
using lw.WebTools;
using lw.Base;
using lw.URLShortener.data;

namespace lw.URLShortener
{
	public class RedirectsManager : LINQManager
    {
		public RedirectsManager():base(cte.lib)
		{
		}

		public string AddRedirection (string url)
		{
			if (!String.IsNullOrWhiteSpace (url))
			{
				if (!url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) && !url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
					url = "http://" + url;

				sabis_urlshortener u = GetUrl(url);
				if (u != null)
					return u.Id.ToString();
				else
				{
					string shortUrl = StringUtils.GenerateRandomText(6);
					sabis_urlshortener sU = GetShortUrl(shortUrl);
					if (sU != null)
						return AddRedirection(url);
					else
					{
						sabis_urlshortener sR = new sabis_urlshortener
						{
							Id = shortUrl,
							Url = url,
							DateCreated = DateTime.Now,
							Clicks = 0
						};

						RedirectsData.sabis_urlshorteners.InsertOnSubmit(sR);
						RedirectsData.SubmitChanges ();

						return shortUrl;
					}
				}
			}
			else return null;
		}

		public void UpdateClick (string ShortUrl)
		{
			sabis_urlshortener sR = RedirectsData.sabis_urlshorteners.Single(temp => temp.Id == ShortUrl);
			int clicks = (int)sR.Clicks;

			sR.Clicks = clicks + 1;

			RedirectsData.SubmitChanges ();
		}

		public sabis_urlshortener GetUrl(string URL)
		{
			var q = RedirectsData.sabis_urlshorteners.Where(u => u.Url == URL).FirstOrDefault();
			if (q != null)
				return q;
			return null;
		}

		public IQueryable<sabis_urlshortener> GetUrls()
		{
			var query = from url in RedirectsData.sabis_urlshorteners
						select url;
			return query;
		}

		public sabis_urlshortener GetShortUrl (string ShortUrl)
		{
			var q = RedirectsData.sabis_urlshorteners.Where(u => u.Id == ShortUrl).FirstOrDefault();
			if (q != null)
				return q;
			return null;
		}

		#region Variables


		public URLShortenerDataContext RedirectsData
		{
			get
			{
				if (_dataContext == null)
					_dataContext = new URLShortenerDataContext(Connection);
				return (URLShortenerDataContext)_dataContext;
			}
		}

		#endregion
    }
}
