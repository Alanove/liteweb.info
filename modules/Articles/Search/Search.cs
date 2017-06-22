using System;
using System.Linq;


namespace lw.Articles.Search
{
	public class NewsSearch
	{
		public static void CreateIndexes()
		{
			LINQ.NewsManager nMgr = new LINQ.NewsManager();

			IQueryable<LINQ.NewsDetail> newsList = nMgr.GetAllNews();

			foreach(LINQ.NewsDetail news in newsList)
			{
				AddToIndex(news.NewsId);
			}
		}


		public static void AddToIndex(object NewsId)
		{
			int newsId = (int)NewsId;

			LINQ.NewsManager nMgr = new LINQ.NewsManager();

			LINQ.NewsDetailView news = nMgr.GetNewsDetails(newsId);
		}



		public static void RemoveFromIndex(object NewsId)
		{
		}



		public static void UpdateIndex(object NewsId)
		{
			int newsId = (int)NewsId;

			LINQ.NewsManager nMgr = new LINQ.NewsManager();

			LINQ.NewsDetailView news = nMgr.GetNewsDetails(newsId);
		}
	}
}
