using System;
using System.Data;
using lw.CTE.Enum;
using lw.Data;
using lw.WebTools;
using System.Web;
using lw.CTE;
using System.IO;
using lw.Utils;
using lw.GraphicUtils;

namespace lw.Events
{
	public class CalendarManager : DirectorBase
	{
		public CalendarManager()
			: base(cte.lib)
		{ }


		/// <summary>
		/// Fetches and returns calendar events depending on the condition
		/// </summary>
		/// <param name="cond">Query condition ex: dateFrom = '1/1/2011' </param>
		/// <returns>CalendarDs.CalendarDataTable CalendarEvents</returns>
		public DataTable GetEvents(string cond)
		{
			CalendarAdp adp = new CalendarAdp();
			return adp.GetEvents(cond);
		}

		// Fetches and returns all Events in a DataView(to match CalendarNetwork functions) depending on the condition
		public DataView GetDataEvents(string cond)
		{
			string sql = "select * from CalendarView";

			if (!String.IsNullOrEmpty(cond))
				sql += " where " + cond;

			return new DataView(DBUtils.GetDataSet(sql, cte.lib).Tables[0], "", "sort asc, DateModified desc", DataViewRowState.CurrentRows);
		}


		/// <summary>
		/// Fetches and returns calendar events on the desired language
		/// </summary>
		/// <param name="lan">Language</param>
		/// <returns>CalendarDs.CalendarDataTable CalendarEvents</returns>
		public DataTable GetEvents(Languages lan)
		{
			return GetEvents(lan, "");
		}

		/// <summary>
		/// Fetches and returns calendar events on the desired language + condition
		/// </summary>
		/// <param name="lan">Language</param>
		/// <param name="cond">Query condition ex: dateFrom = '1/1/2011' </param>
		/// <returns>CalendarDs.CalendarDataTable CalendarEvents</returns>
		public DataTable GetEvents(Languages lan, string cond)
		{
			CalendarAdp adp = new CalendarAdp();

			if (string.IsNullOrWhiteSpace(cond))
				cond += string.Format(" and Language={0}", (int)lan);
			else
				cond = string.Format("Language={0}", (int)lan);


			return GetEvents(cond);
		}

		/// <summary>
		/// Returns all events
		/// </summary>
		/// <returns>CalendarDs.CalendarDataTable CalendarEvents</returns>
		public DataTable GetEvents()
		{
			return GetEvents("");
		}

		/// <summary>
		/// Returns event details
		/// </summary>
		/// <param name="EventId">Int if of the event</param>
		/// <returns><seealso cref="CalendarDs.CalendarRow"/></returns>
		public DataRow GetEventDetails(int EventId)
		{
			DataTable events = GetEvents(string.Format("Id={0}", EventId));

			if (events.Rows.Count > 0)
				return (CalendarDs.CalendarRow)events.Rows[0];

			return null;
		}


		/// <summary>
		/// Get Calendar Categories
		/// </summary>
		/// <param name="cond">Condition</param>
		/// <returns><seealso cref="CalendarDs.CalendarCategoriesDataTable"/>CalendarDs.CalendarCategoriesDataTable</returns>
		public CalendarDs.CalendarCategoriesDataTable GetCategories(string cond)
		{
			CalendarCategoriesAdp adp = new CalendarCategoriesAdp();
			return adp.GetCategories(cond);
		}

		/// <summary>
		/// Get Calendar Category Details
		/// </summary>
		/// <param name="CategoryId">Category Id</param>
		/// <returns><seealso cref="CalendarDs.CalendarCategoriesRow"/>CalendarDs.CalendarCategoriesRow</returns>
		public CalendarDs.CalendarCategoriesRow GetCategoryDetails(int CategoryId)
		{
			CalendarDs.CalendarCategoriesDataTable categories = GetCategories(string.Format("CategoryId={0}", CategoryId));

			if (categories.Rows.Count > 0)
				return (CalendarDs.CalendarCategoriesRow)categories.Rows[0];

			return null;
		}


		/// <summary>
		/// Adds an event to the calendar
		/// </summary>
		/// <param name="req">HttpRequest object containing the form fields</param>
		/// <param name="CategoryId">CategoryId</param>
		/// <returns>ID of the created event</returns>
		public int AddEvent(System.Web.HttpRequest req, int CategoryId)
		{
			Languages lan = Languages.Default;

			if (!String.IsNullOrWhiteSpace(req["Language"]))
			{
				lan = (Languages)Enum.Parse(typeof(Languages), req["Language"]);
			}

			DateTime? dateTo = null;

			if (!String.IsNullOrWhiteSpace(req.Form["DateTo"]))
				dateTo = DateTime.Parse(req.Form["DateTo"]);

			return AddEvent(DateTime.Parse(req.Form["Date"]), dateTo,
				"", "", "",
				(byte)(req.Form["Status"] == "on" ? 1 : 0),
				req.Form["Description"],
				CategoryId, 0, lan);

		}

		/// <summary>
		/// Adds an event to the calendar
		/// </summary>
		/// <param name="req">HttpRequest object containing the form fields</param>
		/// <param name="CategoryId">CategoryId</param>
		/// <param name="UserId">the User or Member Id that added this event</param>
		/// <returns>ID of the created event</returns>
		public int AddEvent(System.Web.HttpRequest req, int CategoryId, int UserId)
		{
			Languages lan = Languages.Default;

			if (!String.IsNullOrWhiteSpace(req["Language"]))
			{
				lan = (Languages)Enum.Parse(typeof(Languages), req["Language"]);
			}
			DateTime? dateTo = null;

			if (!String.IsNullOrWhiteSpace(req.Form["DateTo"]))
				dateTo = DateTime.Parse(req.Form["DateTo"]);

			return AddEvent(DateTime.Parse(req.Form["DateFrom"]),
				dateTo,
				req.Form["Time"],
				req.Form["Title"],
				req.Form["Location"],
				(byte)(req.Form["Status"] == "on" ? 1 : 0),
				WebContext.Server.HtmlDecode(req.Form["Description"]),
				CategoryId,
				UserId,
				lan);

		}

		/// <summary>
		/// Adds an event to the calendar
		/// </summary>
		/// <param name="DateFrom">DateFrom Field</param>
		/// <param name="DateTo">DateTo Field</param>
		/// <param name="Time">Time of the Event (just a string could be from 1:00 pm to 2:00 pm or simply 3:00 pm)</param>
		/// <param name="Title">Title of the event</param>
		/// <param name="Location">Location</param>
		/// <param name="Status">Status 1: enabled 0: disabled</param>
		/// <param name="Description">Description of the event</param>
		/// <param name="CategoryId">Category Id for the event</param>
		/// <param name="UserId">the User or Member Id that added this event</param>
		/// <param name="lan">Event Language <seealso cref="Languages"/></param>
		/// <returns>ID of the created event</returns>
		public int AddEvent(DateTime DateFrom, DateTime? DateTo,
			string Time, string Title, string Location,
			byte Status, string Description,
			int CategoryId, int UserId, Languages lan)
		{
			CalendarDs ds = new CalendarDs();
			CalendarDs.CalendarRow row = ds.Calendar.NewCalendarRow();
			CalendarDsTableAdapters.CalendarTableAdapter Adp = new CalendarDsTableAdapters.CalendarTableAdapter();

			row.DateFrom = DateFrom;
			row.DateTo = (System.DateTime)DateTo;
			row.Time = Time;
			row.Title = Title;
			row.Location = Location;
			row.Status = Status;
			row.Description = Description;
			row.CategoryId = CategoryId;
			row.UserId = UserId;
			row.Language = (short)lan;

			ds.Calendar.AddCalendarRow(row);
			Adp.Update(ds);

			return row.Id;
		}

		/// <summary>
		/// Updates an event to the calendar
		/// </summary>
		/// <param name="Id">ID of the edited event</param>
		/// <param name="req">HttpRequest object containing the form fields</param>
		/// <param name="CategoryId">Category Id for the event</param>
		/// <returns>true if saved false if not</returns>
		public bool UpdateEvent(int Id, System.Web.HttpRequest req, int CategoryId)
		{
			Languages lan = Languages.Default;

			if (!String.IsNullOrWhiteSpace(req["Language"]))
			{
				lan = (Languages)Enum.Parse(typeof(Languages), req["Language"]);
			}

			DateTime? dateTo = null;

			if (!String.IsNullOrWhiteSpace(req.Form["DateTo"]))
				dateTo = DateTime.Parse(req.Form["DateTo"]);

			return UpdateEvent(Id, DateTime.Parse(req.Form["Date"]), dateTo,
				"", "", "",
				(byte)(req.Form["Status"] == "on" ? 1 : 0),
				WebContext.Server.HtmlDecode(req.Form["Description"]),
				CategoryId, 0, lan);
		}

		/// <summary>
		/// Updates an event to the calendar
		/// </summary>
		/// <param name="Id">ID of the edited event</param>
		/// <param name="req">HttpRequest object containing the form fields</param>
		/// <param name="CategoryId">Category Id for the event</param>
		/// <param name="UserId">the User or Member Id that added this event</param>
		/// <returns>true if saved false if not</returns>
		public bool UpdateEvent(int Id, System.Web.HttpRequest req, int CategoryId, int UserId)
		{
			Languages lan = Languages.Default;

			if (!String.IsNullOrWhiteSpace(req["Language"]))
			{
				lan = (Languages)Enum.Parse(typeof(Languages), req["Language"]);
			}

			DateTime? dateTo = null;

			if (!String.IsNullOrWhiteSpace(req.Form["DateTo"]))
				dateTo = DateTime.Parse(req.Form["DateTo"]);

			return UpdateEvent(Id, DateTime.Parse(req.Form["DateFrom"]),
				dateTo,
				req.Form["Time"],
				req.Form["Title"],
				req.Form["Location"],
				(byte)(req.Form["Status"] == "on" ? 1 : 0),
				WebContext.Server.HtmlDecode(req.Form["Description"]),
				CategoryId,
				UserId, lan);
		}

		/// <summary>
		/// Updates an event to the calendar
		/// </summary>
		/// <param name="Id">ID of the edited event</param>
		/// <param name="DateFrom">DateFrom Field</param>
		/// <param name="DateTo">DateTo Field</param>
		/// <param name="Time">Time of the Event (just a string could be from 1:00 pm to 2:00 pm or simply 3:00 pm)</param>
		/// <param name="Title">Title of the event</param>
		/// <param name="Location">Location</param>
		/// <param name="Status">Status 1: enabled 0: disabled</param>
		/// <param name="Description">Description of the event</param>
		/// <param name="CategoryId">Category Id for the event</param>
		/// <param name="UserId">the User or Member Id that added this event</param>
		/// <param name="lan">Event Language <seealso cref="Languages"/></param>
		/// <returns>true if saved false if not</returns>
		/// 
		public bool UpdateEvent(int Id, DateTime DateFrom, DateTime? DateTo,
			string Time, string Title, string Location,
			byte Status, string Description,
			int CategoryId, int UserId, Languages lan)
		{
			CalendarAdp adp = new CalendarAdp();

			adp.UpdateEvent(DateFrom, DateTo, Time, Title, Location, (int)Status, Description, CategoryId,
				UserId, (short)lan, DateTime.Now, Id);

			return true;
		}


		public void UpdateEventStatus(int EventId, int Status)
		{
			string sql = string.Format("Update Calendar set Status={0} where Id={1}", Status, EventId);
			DBUtils.GetDataSet(sql, cte.lib);
		}



		public void UpdateDefaultImage(int EventId, HttpPostedFile OriginalImage, bool DeleteOld)
		{
			DataRow dr = GetEventDetails(EventId);

			string Image = dr["Image"].ToString();

			string imagePart = Path.GetFileNameWithoutExtension(Image);
			string imageExtension = Path.GetExtension(Image);

			string path = Folders.EventsFolder;
			path = Path.Combine(path, string.Format("Event_{0}", EventId));
			path = lw.WebTools.WebContext.Server.MapPath("~/" + path);

			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);

			string ImgPath = "";

			if (!string.IsNullOrEmpty(OriginalImage.FileName))
			{
				ImgPath = Path.Combine(path, StringUtils.ToURL(dr["Title"]) + StringUtils.GeneratePassword() + Path.GetExtension(OriginalImage.FileName));
				OriginalImage.SaveAs(ImgPath);
			}
			else
				ImgPath = "";

			string thumbName = Path.Combine(path, string.Format("{0}-t{1}", imagePart, imageExtension));
			string largeName = Path.Combine(path, string.Format("{0}-l{1}", imagePart, imageExtension));
			string mediumName = Path.Combine(path, string.Format("{0}-m{1}", imagePart, imageExtension));


			DeleteOld = (OriginalImage.ContentLength > 0 || DeleteOld) || String.IsNullOrWhiteSpace(path) && !String.IsNullOrEmpty(Image);

			if (DeleteOld && !String.IsNullOrWhiteSpace(Image))
			{
				try
				{
					if (File.Exists(Path.Combine(path, Image)))
						File.Delete(Path.Combine(path, Image));

					if (File.Exists(Path.Combine(path, thumbName)))
						File.Delete(Path.Combine(path, thumbName));

					if (File.Exists(Path.Combine(path, mediumName)))
						File.Delete(Path.Combine(path, mediumName));

					if (File.Exists(Path.Combine(path, largeName)))
						File.Delete(Path.Combine(path, largeName));
				}
				catch
				{

				}
			}


			if (File.Exists(ImgPath))
			{
				imagePart = Path.GetFileNameWithoutExtension(ImgPath);
				imageExtension = Path.GetExtension(ImgPath);

				thumbName = Path.Combine(path, string.Format("{0}-t{1}", imagePart, imageExtension));
				largeName = Path.Combine(path, string.Format("{0}-l{1}", imagePart, imageExtension));
				mediumName = Path.Combine(path, string.Format("{0}-m{1}", imagePart, imageExtension));

				Config cfg = new Config();

				Dimension
					thumbSize = new Dimension(cfg.GetKey(cte.Event_ThumbImageSize)),
					mediumSize = new Dimension(cfg.GetKey(cte.Event_MediumImageSize)),
					largeSize = new Dimension(cfg.GetKey(cte.Event_LargeImageSize));


				GraphicUtils.ImageUtils.CropImage(ImgPath, thumbName, thumbSize.IntWidth, thumbSize.IntHeight, ImageUtils.AnchorPosition.Default);
				GraphicUtils.ImageUtils.Resize(ImgPath, mediumName, mediumSize.IntWidth, mediumSize.IntHeight);
				GraphicUtils.ImageUtils.Resize(ImgPath, largeName, largeSize.IntWidth, largeSize.IntHeight);

				Image = imagePart + imageExtension;

				string sql = string.Format("Update Calendar set Image=N'{0}' where Id={1}", Image, EventId);
				DBUtils.GetDataSet(sql, cte.lib);
			}
		}


		public void DeleteImage(int EventId)
		{
			DataRow dr = GetEventDetails(EventId);

			string Image = dr["Image"].ToString();

			string imagePart = Path.GetFileNameWithoutExtension(Image);
			string imageExtension = Path.GetExtension(Image);


			string path = Folders.EventsFolder;
			path = Path.Combine(path, string.Format("Event_{0}", EventId));
			path = lw.WebTools.WebContext.Server.MapPath("~/" + path);

			if (!string.IsNullOrEmpty(dr["Image"].ToString()))
			{
				path = Path.Combine(path, dr["Image"].ToString());
			}

			string thumbName = Path.Combine(path, string.Format("{0}-t{1}", imagePart, imageExtension));
			string largeName = Path.Combine(path, string.Format("{0}-l{1}", imagePart, imageExtension));
			string mediumName = Path.Combine(path, string.Format("{0}-m{1}", imagePart, imageExtension));

			try
			{
				if (File.Exists(Path.Combine(path, Image)))
					File.Delete(Path.Combine(path, Image));

				if (File.Exists(Path.Combine(path, thumbName)))
					File.Delete(Path.Combine(path, thumbName));

				if (File.Exists(Path.Combine(path, mediumName)))
					File.Delete(Path.Combine(path, mediumName));

				if (File.Exists(Path.Combine(path, largeName)))
					File.Delete(Path.Combine(path, largeName));
			}
			catch
			{

			}

			string sql = string.Format("Update Calendar set Image=N'{0}' where Id={1}", "", EventId);
			DBUtils.GetDataSet(sql, cte.lib);
		}





		/// <summary>
		/// Deletes an event from the database
		/// </summary>
		/// <param name="Id">ID of the event</param>
		public void DeleteEvent(int Id)
		{
			CalendarAdp adp = new CalendarAdp();
			adp.DeleteEvent(Id);
		}

		/// <summary>
		/// Adds a calendar category
		/// </summary>
		/// <param name="CategoryName">Name</param>
		/// <returns>id of the added category</returns>
		public int AddCalendarCategory(string CategoryName)
		{
			CalendarDs ds = new CalendarDs();
			CalendarDsTableAdapters.CalendarCategoriesTableAdapter Adp = new CalendarDsTableAdapters.CalendarCategoriesTableAdapter();
			CalendarDs.CalendarCategoriesDataTable dt = new CalendarDs.CalendarCategoriesDataTable();
			CalendarDs.CalendarCategoriesRow row = dt.NewCalendarCategoriesRow();

			row.CategoryName = CategoryName;

			dt.AddCalendarCategoriesRow(row);
			Adp.Update(dt);

			return row.CategoryId;
		}


		/// <summary>
		/// Update calendar cateogory
		/// </summary>
		/// <param name="CategoryId">Category Id</param>
		/// <param name="CategoryName">New Name</param>
		/// <returns>true for success, false for fail</returns>
		public bool UpdateCalendarCategory(int CategoryId, string CategoryName)
		{
			CalendarDs.CalendarCategoriesRow row = GetCategoryDetails(CategoryId);
			CalendarAdp adp = new CalendarAdp();

			row.CategoryName = CategoryName;

			adp.Update(row);

			return true;
		}

		/// <summary>
		/// Deletes a calendar category
		/// </summary>
		/// <param name="CategoryId">Category Id</param>
		public void DeleteCategory(int CategoryId)
		{
			CalendarCategoriesAdp adp = new CalendarCategoriesAdp();
			adp.DeleteCategory(CategoryId);
		}



		public static string GetMediumImage(int PageId, string Image)
		{
			if (String.IsNullOrEmpty(Image))
				return "";

			string imageName = Path.GetFileNameWithoutExtension(Image);
			string extension = Path.GetExtension(Image);

			string path = Path.Combine(Folders.EventsFolder, "Event_" + PageId.ToString());
			return Path.Combine(path, imageName + "-m" + extension);
		}

		public static string GetThumbImage(int PageId, string Image)
		{
			if (String.IsNullOrEmpty(Image))
				return "";

			string imageName = Path.GetFileNameWithoutExtension(Image);
			string extension = Path.GetExtension(Image);

			string path = Path.Combine(Folders.EventsFolder, "Event_" + PageId.ToString());
			return Path.Combine(path, imageName + "-t" + extension);
		}

		public static string GetLargeImage(int PageId, string Image)
		{
			if (String.IsNullOrEmpty(Image))
				return "";

			string imageName = Path.GetFileNameWithoutExtension(Image);
			string extension = Path.GetExtension(Image);

			string path = Path.Combine(Folders.EventsFolder, "Event_" + PageId.ToString());
			return Path.Combine(path, imageName + "-l" + extension);
		}

		public static string GetImage(int PageId, string Image)
		{
			if (String.IsNullOrEmpty(Image))
				return "";

			string path = Path.Combine(Folders.EventsFolder, "Event_" + PageId.ToString());
			return Path.Combine(path, Image);
		}
	}
}
