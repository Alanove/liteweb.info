
namespace lw.Articles.Controls
{
	public class MostReadArticles : NewsDataSrc
	{
		public override void DataBind()
		{
			this.Sort = ArticlesSort.Custom;
			this.CustomSort = "(Ranking + [Views]) Desc";

			base.DataBind();
		}
	}
}
