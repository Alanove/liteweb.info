
namespace lw.Articles.Controls
{
	public class MostRatedArticles : NewsDataSrc
	{
		public override void DataBind()
		{
			this.Sort = ArticlesSort.Custom;
			this.CustomSort = "(Ranking + UserRating) Desc";

			base.DataBind();
		}
	}
}
