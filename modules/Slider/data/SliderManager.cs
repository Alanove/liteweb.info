using System;
using System.Data;

using lw.Utils;
using lw.WebTools;

namespace lw.Slider
{
	public class SliderManager
	{

		SliderDS ds;

		string config = cte.ConfigFile;

        public SliderManager()
		{
		}

		#region Methods

        /* Start Slider */

        public DataRow AddSlide(string title, string description, int orderNbr, string imageUrl)
		{
			 
            SliderDS.SlidesRow row = DS.Slides.NewSlidesRow();
            row.Title = title;
            row.Description = description;
            row.OrderNbr = orderNbr;
            row.ImageUrl = imageUrl;

			ds.Slides.Rows.Add(row);

			AcceptChanges();

			return row;
		}

        public bool UpdateSlide(int Id, string title, string description, int orderNbr, string imageUrl)
        {
			DataView dv = this.GetSlides(string.Format("Id = {0}", Id));

			if (dv.Count <= 0)
				return false;

            SliderDS.SlidesRow row = (SliderDS.SlidesRow)dv[0].Row;

            row.Title = title;
            row.Description = description;
            row.OrderNbr = orderNbr;
            row.ImageUrl = imageUrl;

			row.Table.AcceptChanges();
			AcceptChanges();

			return true;
		}

		public bool DeleteSlide(int Id)
		{
            DataView dv = this.GetSlides(string.Format("Id = {0}", Id));

			if (dv.Count <= 0)
				return false;

			SliderDS.SlidesRow row = (SliderDS.SlidesRow)dv[0].Row;

			row.Delete();
			AcceptChanges();

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
