using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

using lw.CTE;
using lw.CTE.Enum;
using lw.Data;
using lw.Utils;
using lw.WebTools;

namespace lw.SocialMedia
{
	public class SocialMediaManager : LINQManager
	{

		public SocialMediaManager()
			: base(cte.lib)
		{ }


		public IQueryable<SocialMediaPost> GetSocialMedia(int PageId)
		{
			var query = from s in SocialMediaData.SocialMediaPosts
						where s.PageId == PageId
						select s;
			if (query.Count() > 0)
				return query;
			return null;
		}


		public int GetSocialMedia(int PageId, int Type)
		{
			return (from r in SocialMediaData.SocialMediaPosts
					where r.PageId == PageId && r.Type == Type
					select r).Count();
		}

		#region Variables

		public SocialMediaDataContext SocialMediaData
		{
			get
			{
				if (_dataContext == null)
					_dataContext = new SocialMediaDataContext(this.Connection);
				return (SocialMediaDataContext)_dataContext;
			}
		}

		#endregion
	}
}
