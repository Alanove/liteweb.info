using System;
using System.Data;

using lw.WebTools;

namespace lw.Slider
{
    public class Slider
    {
		DataSet _ds;
		DataTable _dt;

		public DataView GetSlides(string cond)
		{
			return new DataView(SliderTable, cond, "", DataViewRowState.CurrentRows);
		}

		public int AddSlide(string from, string text)
		{
			DataRow dr = SliderTable.NewRow();
			dr["from"] = from;
			dr["Text"] = text;
			dr["DateModified"] = DateTime.Now;

			SliderTable.Rows.Add(dr);
			AcceptChanges();

			return (int)dr["Id"];
		}

		public void UpdateSlide(int id, string from, string text)
		{
			DataView dv = GetSlides(string.Format("id={0}", id));
			if (dv.Count == 0)
				return;

			DataRow dr = SliderTable.NewRow();
			dr["from"] = from;
			dr["Text"] = text;
			dr["DateModified"] = DateTime.Now;

			SliderTable.Rows.Add(dr);
			AcceptChanges();

		}

		void AcceptChanges()
		{
			DS.AcceptChanges();
			XmlManager.SetDataSet(cte.ConfigFile, DS);
		}

		DataSet DS
		{
			get
			{
				if(_ds == null)
					_ds = XmlManager.GetDataSet(cte.ConfigFile);

				return _ds;
			}
		}
		DataTable SliderTable
		{
			get
			{
				if (_dt == null)
					_dt = DS.Tables[0];

				return _dt;
			}
		}
	}
}
