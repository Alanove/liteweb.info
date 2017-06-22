using System.ComponentModel;

using lw.Data; 

namespace lw.Newsletter
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
			this.AddDataComponent(cte.NewsleterUsersAdp, this._sqlNewsletterUsersAdp);
			this.AddDataComponent(cte.NewsletterGroupsAdp, this._sqlNewsltetterGroupsAdp);
			this.AddDataComponent(cte.EmptyCommand, this._EmptyCommand);

		}
	}
}
