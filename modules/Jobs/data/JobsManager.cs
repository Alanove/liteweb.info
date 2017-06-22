using System;
using System.IO;
using System.Web;
using lw.GraphicUtils;
using lw.Utils;
using lw.WebTools;

namespace lw.Jobs
{
	public class JobsManager
	{
		public JobsManager()
		{ 
		}

		void UpdateError(string str)
		{
			lw.WebTools.ErrorContext.Add(cte.lib, str);
		}

		public JobsDS.JobOffersDataTable GetJobOffers(string cond)
		{
			JobOffersAdp adp = new JobOffersAdp();
			return adp.GetJobOffers(cond);
		}

		public JobsDS.JobApplicationsDataTable GetJobApplications(string cond)
		{
			JobApplicationsAdp adp = new JobApplicationsAdp();
			return adp.GetJobApplications(cond);
		}

		public JobsDS.JobCategoriesDataTable GetJobCategories(string cond)
		{
			JobCategoriesAdp adp = new JobCategoriesAdp();
			return adp.GetJobCategories(cond);
		}

		public JobsDS.JobPositionsDataTable GetJobPositions(string cond)
		{
			JobPositionsAdp adp = new JobPositionsAdp();
			return adp.GetJobPositions(cond);
		}

		public JobsDS.JobOffersViewDataTable GetJobOffersView(string cond)
		{
			JobOffersViewAdp adp = new JobOffersViewAdp();
			return adp.GetJobOffersView(cond);
		}

		public JobsDS.JobApplicationsViewDataTable GetJobApplicationsView(string cond)
		{
			JobApplicationsViewAdp adp = new JobApplicationsViewAdp();
			return adp.GetJobApplicationsView(cond);
		}

		/* Job Offers */

		public int AddJobOffer(System.Web.HttpRequest req)
		{
			decimal _salary = 0;

			if (!String.IsNullOrEmpty(req.Form["Salary"]))
				_salary = decimal.Parse(req.Form["Salary"]);

			return AddJobOffer(req.Form["Title"], req.Form["CompanyName"], _salary, req.Form["Description"], 
				req.Form["ContactName"], req.Form["PhoneNumber"], req.Form["Email"], req.Files["RelatedFile"],
				req.Form["Url"], req.Form["Address"], Int32.Parse(req.Form["Category"]), req.Form["Country"],
				Int32.Parse(req.Form["Position"]), req.Form["Status"] == "1"? true:false, req.Form["Date"]);
		}

		public int AddJobOffer(string Title, string CompanyName, decimal Salary, string Description, string ContactName,
			string PhoneNumber, string Email, HttpPostedFile Logo, string Url, string Address, int CategoryId,
			string CountryCode, int PositionId, bool Status, string ExpiryDate)
		{
			try
			{
				JobsDS ds = new JobsDS();
				lw.Jobs.JobsDSTableAdapters.JobOffersTableAdapter Adp = new lw.Jobs.JobsDSTableAdapters.JobOffersTableAdapter();
				JobsDS.JobOffersDataTable dt = new JobsDS.JobOffersDataTable();
				JobsDS.JobOffersRow row = dt.NewJobOffersRow();

				DateTime date;

				row.Title = Title;
				row.CompanyName = CompanyName;
				row.Salary = Salary;
				row.Description = Description;
				row.ContactName = ContactName;
				row.PhoneNumber = PhoneNumber;
				row.Email = Email;
				row.Url = Url;
				row.Address = Address;
				row.CategoryId = CategoryId;
				row.CountryCode = CountryCode;
				row.PositionId = PositionId;
				row.Status = Status;
				row.CreationDate = DateTime.Now;
				row.ModifiedDate = DateTime.Now;
				if (DateTime.TryParse(ExpiryDate, out date))
					row.ExpiryDate = date;

				dt.AddJobOffersRow(row);
				Adp.Update(dt);

				int jobOfferId = row.Id;


				JobOffersAdp adp = new JobOffersAdp();
				if (Logo != null && Logo.ContentLength > 0)
				{
					Config cfg = new Config();

					string path = WebContext.Server.MapPath("~/" + lw.CTE.Folders.JobLogos);
					string ext = StringUtils.GetFileExtension(Logo.FileName);
					string fileName = StringUtils.GetFriendlyFileName(Logo.FileName);

					string _imageName = string.Format("{0}_{1}.{2}",
						fileName, jobOfferId, ext);

					string imageName = string.Format("{0}\\{1}", path, _imageName);

					Logo.SaveAs(imageName);

					if (cfg.GetKey("JobLogo") == "on")
					{
						try
						{
							int _Width = Int32.Parse(cfg.GetKey("JobLogoWidth"));
							int _Height = Int32.Parse(cfg.GetKey("JobLogoHeight"));

							ImageUtils.Resize(imageName, imageName, _Width, _Height);
							adp.UpdateLogo(_imageName, jobOfferId);

						}
						catch (IOException Ex)
						{
							this.UpdateError("IO ERROR, Could not save job offer logo: <br />" + Ex.Message);
						}
						catch (Exception ex1)
						{
							throw ex1;
						}
					}
					else
					{
						try
						{
							adp.UpdateLogo(_imageName, jobOfferId);
							Logo.SaveAs(Path.Combine(path, _imageName));
						}
						catch (IOException Ex)
						{
							this.UpdateError("IO ERROR, Could not save job offer logo: <br />" + Ex.Message);
						}
						catch (Exception ex1)
						{
							throw ex1;
						}
					}
				}
				return jobOfferId;
			}
			catch (Exception Ex)
			{
				this.UpdateError("Fail to add job: <br />" + Ex.Message);
				return -1;
			}
		}

		public void UpdateJobOffer(int JobOfferId, System.Web.HttpRequest req)
		{
			decimal _salary = 0;

			if (!String.IsNullOrEmpty(req.Form["Salary"]))
				_salary = decimal.Parse(req.Form["Salary"]);

			UpdateJobOffer(JobOfferId, req.Form["Title"], req.Form["CompanyName"], _salary, req.Form["Description"],
				req.Form["ContactName"], req.Form["PhoneNumber"], req.Form["Email"], req.Files["Logo"],
				req.Form["Url"], req.Form["Address"], Int32.Parse(req.Form["Category"]), req.Form["Country"],
				Int32.Parse(req.Form["Position"]), req.Form["Status"] == "1" ? true : false, req.Form["Date"],(req.Form["DeleteLogo"] == "on"));
		}

		public void UpdateJobOffer(int JobOfferId, string Title, string CompanyName, decimal Salary, string Description,
			string ContactName, string PhoneNumber, string Email, HttpPostedFile Logo, string Url, string Address,
			int CategoryId, string CountryCode, int PositionId, bool Status, string ExpiryDate, bool DeleteLogo)
		{
			try
			{
				JobOffersAdp adp = new JobOffersAdp();
				JobsDS.JobOffersRow row = (JobsDS.JobOffersRow)GetJobOffers(string.Format("Id={0}", JobOfferId)).Rows[0];

				DateTime date;

				row.Title = Title;
				row.CompanyName = CompanyName;
				row.Salary = Salary;
				row.Description = Description;
				row.ContactName = ContactName;
				row.PhoneNumber = PhoneNumber;
				row.Email = Email;
				row.Url = Url;
				row.Address = Address;
				row.CategoryId = CategoryId;
				row.CountryCode = CountryCode;
				row.PositionId = PositionId;
				row.Status = Status;
				row.ModifiedDate = DateTime.Now;
				if (DateTime.TryParse(ExpiryDate, out date))
					row.ExpiryDate = date;
				else
					row.ExpiryDate = DateTime.MaxValue;

				adp.Update(row);
				
				string path = WebContext.Server.MapPath("~/" + lw.CTE.Folders.JobLogos);
				
				string fileName = "";

				DeleteLogo = (DeleteLogo || (Logo != null && Logo.ContentLength > 0))
							&& row["Logo"].ToString() != "";

				if (DeleteLogo)
				{
					fileName = Path.Combine(path, row["Logo"].ToString());
					if (File.Exists(fileName))
						File.Delete(fileName);

					adp.UpdateLogo("", JobOfferId);
				}

				if (Logo != null && Logo.ContentLength > 0)
				{
					Config cfg = new Config();

					string ext = StringUtils.GetFileExtension(Logo.FileName);
					fileName = StringUtils.GetFriendlyFileName(Logo.FileName);

					string _imageName = string.Format("{0}_{1}.{2}",
						fileName, JobOfferId, ext);

					string imageName = string.Format("{0}\\{1}", path, _imageName);

					Logo.SaveAs(imageName);

					if (cfg.GetKey("JobLogo") == "on")
					{
						try
						{
							int _Width = Int32.Parse(cfg.GetKey("JobLogoWidth"));
							int _Height = Int32.Parse(cfg.GetKey("JobLogoHeight"));

							ImageUtils.Resize(imageName, imageName, _Width, _Height);
							adp.UpdateLogo(_imageName, JobOfferId);

						}
						catch (IOException Ex)
						{
							this.UpdateError("IO ERROR, Could not save job offer logo: <br />" + Ex.Message);
						}
						catch (Exception ex1)
						{
							throw ex1;
						}
					}
					else
					{
						try
						{
							adp.UpdateLogo(_imageName, JobOfferId);
							Logo.SaveAs(Path.Combine(path, _imageName));
						}
						catch (IOException Ex)
						{
							this.UpdateError("IO ERROR, Could not save job offer logo: <br />" + Ex.Message);
						}
						catch (Exception ex1)
						{
							throw ex1;
						}
					}
				}
			}
			catch (Exception Ex)
			{
				this.UpdateError("Fail to update job offer: <br />" + Ex.Message);
			}
		}

		public void DeleteJobOffer(int JobOfferId)
		{
			try
			{
				JobOffersAdp adp = new JobOffersAdp();
				adp.DeleteJobOffer(JobOfferId);
			}
			catch (Exception Ex)
			{
				this.UpdateError("Fail to delete job offer: <br />" + Ex.Message);
			}
		}

		public void UpdateJobOfferStatus(int JobOfferId, bool Status)
		{
			try
			{
				JobOffersAdp adp = new JobOffersAdp();
				adp.UpdateJobOfferStatus(Status, JobOfferId);
			}
			catch (Exception Ex)
			{
				this.UpdateError("Fail to update job offer status: <br />" + Ex.Message);
			}
		}

		/* Job Applications */

		public int AddJobApplication(System.Web.HttpRequest req)
		{
			decimal _salary = 0;

			if (!String.IsNullOrEmpty(req.Form["Salary"]))
				_salary = decimal.Parse(req.Form["Salary"]);

			return AddJobApplication(req.Form["Title"], req.Form["CompanyName"], _salary, req.Form["Description"], 
				req.Form["ContactName"], req.Form["PhoneNumber"], req.Form["Email"], Int32.Parse(req.Form["Category"]), 
				req.Form["Country"], Int32.Parse(req.Form["Position"]), req.Form["Status"] == "1"? true:false);
		}

		public int AddJobApplication(string Title, string CompanyName, decimal Salary, string Description, string ContactName,
			string PhoneNumber, string Email, int CategoryId, string CountryCode, int PositionId, bool Status)
		{
			try
			{

				JobsDS ds = new JobsDS();
				lw.Jobs.JobsDSTableAdapters.JobApplicationsTableAdapter Adp = new lw.Jobs.JobsDSTableAdapters.JobApplicationsTableAdapter();
				JobsDS.JobApplicationsDataTable dt = new JobsDS.JobApplicationsDataTable();
				JobsDS.JobApplicationsRow row = dt.NewJobApplicationsRow();

				row.Title = Title;
				row.CompanyName = CompanyName;
				row.Salary = Salary;
				row.Description = Description;
				row.ContactName = ContactName;
				row.PhoneNumber = PhoneNumber;
				row.Email = Email;
				row.CategoryId = CategoryId;
				row.CountryCode = CountryCode;
				row.PositionId = PositionId;
				row.Status = Status;
				row.CreationDate = DateTime.Now;
				row.ModifiedDate = DateTime.Now;

				dt.AddJobApplicationsRow(row);
				Adp.Update(dt);

				return row.Id;
			}
			catch (Exception Ex)
			{
				this.UpdateError("Fail to add job application: <br />" + Ex.Message);
				return -1;
			}
		}

		public void UpdateJobApplication(int JobApplicationId, System.Web.HttpRequest req)
		{
			decimal _salary = 0;

			if (!String.IsNullOrEmpty(req.Form["Salary"]))
				_salary = decimal.Parse(req.Form["Salary"]);

			UpdateJobApplication(JobApplicationId, req.Form["Title"], req.Form["CompanyName"], _salary, req.Form["Description"],
				req.Form["ContactName"], req.Form["PhoneNumber"], req.Form["Email"], Int32.Parse(req.Form["Category"]), 
				req.Form["Country"], Int32.Parse(req.Form["Position"]), req.Form["Status"] == "1" ? true : false);
		

		}
		public void UpdateJobApplication(int JobApplicationId, string Title, string CompanyName, decimal Salary,
			string Description, string ContactName, string PhoneNumber, string Email, int CategoryId, string CountryCode,
			int PositionId, bool Status)
		{
			try
			{
				JobApplicationsAdp adp = new JobApplicationsAdp();
				adp.UpdateJobApplication(Title, CompanyName, Salary, Description, ContactName, PhoneNumber, Email, CategoryId,
					CountryCode, PositionId, DateTime.Now, Status, JobApplicationId);
			}
			catch (Exception Ex)
			{
				this.UpdateError("Fail to update job application: <br />" + Ex.Message);
			}
		}

		public void DeleteJobApplication(int JobApplicationId)
		{
			try
			{
				JobApplicationsAdp adp = new JobApplicationsAdp();
				adp.DeleteJobApplication(JobApplicationId);
			}
			catch (Exception Ex)
			{
				this.UpdateError("Fail to delete job application: <br />" + Ex.Message);
			}
		}

		public void UpdateJobApplicationStatus(int JobOfferId, bool Status)
		{
			try
			{
				JobApplicationsAdp adp = new JobApplicationsAdp();
				adp.UpdateJobApplicationStatus(Status, JobOfferId);
			}
			catch (Exception Ex)
			{
				this.UpdateError("Fail to update job application status: <br />" + Ex.Message);
			}
		}

		/* Job Categories */

		public void AddJobCategory(string CategoryName)
		{
			try
			{
				JobCategoriesAdp adp = new JobCategoriesAdp();
				adp.Insert(CategoryName, StringUtils.ToURL(CategoryName));
			}
			catch (Exception Ex)
			{
				this.UpdateError("Fail to add job category: <br />" + Ex.Message);
			}
		}

		public void UpdateJobCategory(string CategoryName, int CategoryId)
		{
			try
			{
				JobsDS.JobCategoriesRow row = (JobsDS.JobCategoriesRow)GetJobCategories(string.Format("CategoryId={0}", CategoryId)).Rows[0];

				row.CategoryName = CategoryName;
				row.UniqueName = StringUtils.ToURL(CategoryName);

				JobCategoriesAdp adp = new JobCategoriesAdp();
				adp.Update(row);
			}
			catch (Exception Ex)
			{
				this.UpdateError("Fail to update job category: <br />" + Ex.Message);
			}
		}

		public void DeleteJobCategory(int CategoryId)
		{
			try
			{
				JobCategoriesAdp adp = new JobCategoriesAdp();
				adp.DeleteJobCategory(CategoryId);
			}
			catch (Exception Ex)
			{
				this.UpdateError("Fail to delete job category: <br />" + Ex.Message);
			}
		}

		/* Job Positions */

		public void AddJobPosition(string PositionName)
		{
			try
			{
				JobCategoriesAdp adp = new JobCategoriesAdp();
				adp.Insert(PositionName, StringUtils.ToURL(PositionName));
			}
			catch (Exception Ex)
			{
				this.UpdateError("Fail to add job position: <br />" + Ex.Message);
			}
		}

		public void UpdateJobPosition(string PositionName, int PositionId)
		{
			try
			{
				JobsDS.JobPositionsRow row = (JobsDS.JobPositionsRow)GetJobPositions(string.Format("PositionId={0}", PositionId)).Rows[0];

				row.PositionName = PositionName;

				JobPositionsAdp adp = new JobPositionsAdp();
				adp.Update(row);
			}
			catch (Exception Ex)
			{
				this.UpdateError("Fail to update job position: <br />" + Ex.Message);
			}
		}

		public void DeleteJobPosition(int PositionId)
		{
			try
			{
				JobPositionsAdp adp = new JobPositionsAdp();
				adp.DeleteJobPosition(PositionId);
			}
			catch (Exception Ex)
			{
				this.UpdateError("Fail to delete job position: <br />" + Ex.Message);
			}
		}
	}
}
