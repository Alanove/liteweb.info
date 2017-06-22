using System.ComponentModel;

using lw.Data; 

namespace lw.Articles
{
	public partial class NewsData : ToCastToComponenet
	{
		public NewsData()
		{
			InitializeComponent();
			initData();
		}

		public NewsData(IContainer container)
		{
			container.Add(this);

			InitializeComponent();
			initData();
		}

		protected override void initData()
		{
			this.AddDataComponent(cte.NewsAdp, this._sqlNewsAdp);

			this.AddDataComponent(cte.NewsleterUsersAdp, this._sqlNewsletterUsersAdp);
			this.AddDataComponent(cte.NewsletterViewAdp, this._sqlNewsViewAdp);
			this.AddDataComponent(cte.NewsletterGroupsAdp, this._sqlNewsltetterGroupsAdp);
			this.AddDataComponent(cte.NewsViewAdp, this._sqlNewsViewAdp);
			this.AddDataComponent(cte.NewsTypesAdp, this._sqlNewsTypes);

			this.AddDataComponent(cte.EmptyCommand, this._EmptyCommand);

			this.AddDataComponent(cte.CalendarAdp, this._sqlCalendarAdp);
			this.AddDataComponent(cte.CalendarCategoriesAdp, this._sqlCalendarCategories);

			this.AddDataComponent(cte.NewsDateViewAdp, this._sqlNewsDateView);
		}
	}
}
