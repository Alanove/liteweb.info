using System.Linq;

using lw.Data;

namespace lw.Articles.LINQ
{
	public class NewsManager:LINQManager
	{
		public NewsManager()
			: base(cte.lib){
		}

		public IQueryable<NewsDetail> GetAllNews(){
			return from n in NewsData.NewsDetails
					select n;	
		}

		public NewsDetail GetNews(int newsId)
		{
			var q = from n in NewsData.NewsDetails
					where n.NewsId == newsId 
					select n;
			if (q.Count() > 0)
				return q.First();
			return null;
		}
		public NewsDetailView GetNewsDetails(int NewsId)
		{
			var q = from n in NewsData.NewsDetailViews
					where n.NewsId == NewsId
					select n;
			if (q.Count() > 0)
				return q.First();
			return null;
		}
		public NewsDetailView GetNewsDetails(string UniqueName)
		{
			var q = from n in NewsData.NewsDetailViews
					where n.UniqueName == UniqueName
					|| n.Title == UniqueName
					select n;
			if (q.Count() > 0)
				return q.First();
			return null;
		}

		public NewsDateView GetNewsDate(int TypeId)
		{
			var q = from n in NewsData.NewsDateViews
					where n.NewsType == TypeId
					select n;
			if (q.Count() > 0)
				return q.First();
			return null;
		}
		public void UpdateNews(NewsDetail news)
		{
			NewsData.SubmitChanges();
		}


		#region return newslist
		/// <summary>
		/// Returns news for the specified type id
		/// </summary>
		/// <param name="typeId">New Type Id</param>
		/// <returns>NewsDetailView</returns>
		public IQueryable<NewsDetailView> GetNewsList(int typeId)
		{
			var q = from n in NewsData.NewsDetailViews
					where n.NewsType == typeId
					select n;
			return q;
		}

		/// <summary>
		/// Returns news for the specified type id
		/// </summary>
		/// <param name="typeId">New Type Id</param>
		/// <returns>NewsDetailView</returns>
		public IQueryable<NewsDetailView> GetNewsList(int typeId, NewsStatus status)
		{
			var q = from n in NewsData.NewsDetailViews
					where 
						n.NewsType == typeId && 
						n.Status == (int)status
					select n;
			return q;
		}

		/// <summary>
		/// Returns news for the specified type id
		/// </summary>
		/// <param name="typeId">New Type Name</param>
		/// <returns>NewsDetailView</returns>
		public IQueryable<NewsDetailView> GetNewsList(string typeName)
		{
			var q = from n in NewsData.NewsDetailViews
					where n.TypeUniqueName == typeName
					select n;
			return q;
		}

		/// <summary>
		/// Returns news for the specified type id
		/// </summary>
		/// <param name="typeId">New Type Name</param>
		/// <returns>NewsDetailView</returns>
		public IQueryable<NewsDetailView> GetNewsList(string typeName, NewsStatus status)
		{
			var q = from n in NewsData.NewsDetailViews
					where 
						n.TypeUniqueName == typeName &&
						n.Status == (int)status
					select n;
			return q;
		}
		#endregion


		#region Variables

		public NewsDataContext NewsData
		{
			get
			{
				if (_dataContext == null)
					_dataContext = new NewsDataContext(this.Connection);
				return (NewsDataContext)_dataContext;
			}
		}

		#endregion
	}
}
