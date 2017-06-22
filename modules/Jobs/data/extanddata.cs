
namespace lw.Jobs
{
	public class JobOffersAdp : JobsDSTableAdapters.JobOffersTableAdapter
	{
		public JobsDS.JobOffersDataTable GetJobOffers (string condition)
		{
			if (condition != "")
				condition = " where " + condition;

			base.CommandCollection[0].CommandText += condition;
			return base.GetData();
		}
	}
	public class JobApplicationsAdp : JobsDSTableAdapters.JobApplicationsTableAdapter
	{
		public JobsDS.JobApplicationsDataTable GetJobApplications(string condition)
		{
			if (condition != "")
				condition = " where " + condition;

			base.CommandCollection[0].CommandText += condition;
			return base.GetData();
		}
	}
	public class JobCategoriesAdp : JobsDSTableAdapters.JobCategoriesTableAdapter
	{
		public JobsDS.JobCategoriesDataTable GetJobCategories(string condition)
		{
			if (condition != "")
				condition = " where " + condition;

			base.CommandCollection[0].CommandText += condition;
			return base.GetData();
		}
	}
	public class JobPositionsAdp : JobsDSTableAdapters.JobPositionsTableAdapter
	{
		public JobsDS.JobPositionsDataTable GetJobPositions(string condition)
		{
			if (condition != "")
				condition = " where " + condition;

			base.CommandCollection[0].CommandText += condition;
			return base.GetData();
		}
	}
	public class JobOffersViewAdp : JobsDSTableAdapters.JobOffersViewTableAdapter
	{
		public JobsDS.JobOffersViewDataTable GetJobOffersView(string condition)
		{
			if (condition != "")
				condition = " where " + condition;

			base.CommandCollection[0].CommandText += condition;
			return base.GetData();
		}
	}
	public class JobApplicationsViewAdp : JobsDSTableAdapters.JobApplicationsViewTableAdapter
	{
		public JobsDS.JobApplicationsViewDataTable GetJobApplicationsView(string condition)
		{
			if (condition != "")
				condition = " where " + condition;

			base.CommandCollection[0].CommandText += condition;
			return base.GetData();
		}
	}
}