using System;
using System.Data;
using System.Web.UI;
using lw.Utils;
using lw.WebTools;
using System.IO;
using System.Web;

namespace lw.Slider
{
	/// <summary>
	/// Slider manager class
	/// </summary>
	public class SliderManager
	{

		SliderDS ds;

		string config = cte.ConfigFile;

		/// <summary>
		/// 
		/// </summary>
        public SliderManager()
		{
		}

		#region Methods

        /* Start Slider */
		/// <summary>
		/// Adds a new slide
		/// </summary>
		/// <param name="Title">Title of the new slide that will also be used to rename the image</param>
		/// <param name="Description">Description of the slide</param>
		/// <param name="OrderNbr">The Order Number of the slide</param>
		/// <param name="ImageFile">The image file to be added for the slide</param>
		/// <param name="URL">A URL to be used if needed (links)</param>
		/// <returns>DataRow, the created slide as a row</returns>
		public DataRow AddSlide(string Title, string Description, int OrderNbr, HttpPostedFile ImageFile, string URL)
		{
            SliderDS.SlidesRow row = DS.Slides.NewSlidesRow();
			row.Title = Title;
            row.Description = Description;
            row.OrderNbr = OrderNbr;
			row.URL = URL;

			ds.Slides.Rows.Add(row);

			AcceptChanges();

			string fileName = StringUtils.ToURL(Title) + "-" + row.Id;

			string imagePath = WebContext.Server.MapPath("~/images/slides/" + fileName + ".jpg");
			ImageFile.SaveAs(imagePath);

			row.ImageUrl = fileName;
			AcceptChanges();

			return row;
		}
		//some comments
		/// <summary>
		/// Updates the existing slide
		/// </summary>
		/// <param name="Id">Id of the current slide</param>
		/// <param name="title">Title of the slide</param>
		/// <param name="description">Description of the slide</param>
		/// <param name="orderNbr">The order number of the current slide</param>
		/// <param name="imageUrl">The name of the newly uploaded image</param>
		/// <param name="ImageFile">The request file comming form the form</param>
		/// <param name="OriginalImagePath">The server mapped path of the existing image</param>
		/// <param name="URL">A URL to be used if needed (links)</param>
		/// <returns>False if not found and true if updated</returns>
        public bool UpdateSlide(int Id, string title, string description, int orderNbr, string imageUrl, HttpPostedFile ImageFile, string OriginalImagePath, string URL)
        {
			DataView dv = this.GetSlides(string.Format("Id = {0}", Id));

			if (dv.Count <= 0)
				return false;

			//if file input holds image file
			if (ImageFile != null)
			{
				if (OriginalImagePath != null && File.Exists(OriginalImagePath))
					File.Delete(OriginalImagePath);

				string imagePath = WebContext.Server.MapPath("~/images/slides/" + imageUrl + ".jpg");
				ImageFile.SaveAs(imagePath);
			}
			else 
			{
				FileInfo fi = new FileInfo(OriginalImagePath);
				fi.MoveTo(WebContext.Server.MapPath("~/images/slides/" + imageUrl + ".jpg"));
			}

            SliderDS.SlidesRow row = (SliderDS.SlidesRow)dv[0].Row;

			row.Title = title != null ? title : row.Title;
			row.Description = description != null ? description : row.Description; ;
			row.OrderNbr = orderNbr != -1 ? orderNbr : row.OrderNbr;
			row.ImageUrl = imageUrl != null ? imageUrl : row.ImageUrl;
			row.URL = URL != null ? URL : "";

			row.Table.AcceptChanges();
			AcceptChanges();

			return true;
		}

		/// <summary>
		/// Deletes the existing slide based on it's unique ID
		/// </summary>
		/// <param name="Id">Id of the slide to be deleted</param>
		/// <returns>If slide not found it returns false else true</returns>
		public bool DeleteSlide(int Id)
		{
            DataView dv = this.GetSlides(string.Format("Id = {0}", Id));

			if (dv.Count <= 0)
				return false;

			SliderDS.SlidesRow row = (SliderDS.SlidesRow)dv[0].Row;

			row.Delete();
			AcceptChanges();
			string path = WebContext.Server.MapPath("~/" + lw.CTE.Folders.MainSliderFolder);
			string ImageName = string.Format("{0}\\{1}", path, Id + ".jpg");

			if (File.Exists(ImageName))
				File.Delete(ImageName);

			return true;
		}

        public SliderDS.SlidesRow GetSlide(int Id)
		{
            DataView dv = GetSlides(string.Format("Id = {0}", Id));
			SliderDS.SlidesRow row = (SliderDS.SlidesRow)dv[0].Row;
			return row;
		}

        public DataView GetSlides()
		{
            return GetSlides("");
		}

		public DataView GetSlides(string cond)
		{
            return new DataView(DS.Slides, cond, "OrderNbr Desc", DataViewRowState.CurrentRows);
		}

		/* End Slider */
		
		void AcceptChanges()
		{
			DS.AcceptChanges();
			XmlManager.SetDataSet(config, DS);
		}

		#endregion

		#region variables
		public SliderDS DS
		{
			get
			{
				if (ds == null)
				{
					ds = new SliderDS();
					DataSet _ds = XmlManager.GetDataSet(config);
					if (_ds != null)
						ds.Merge(_ds);
				}
				return ds;
			}
		}
		#endregion
	}
}
