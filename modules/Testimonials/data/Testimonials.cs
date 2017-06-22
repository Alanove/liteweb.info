using System;
using System.Data;

using lw.WebTools;

namespace lw.Testimonials
{
	public class Testimonials
	{
		DataSet _ds;
		DataTable _dt;

		public DataView GetTestimonials(string cond)
		{
			return new DataView(TestimonialsTable, cond, "", DataViewRowState.CurrentRows);
		}

		public int AddTestimonial(string from, string text)
		{
			DataRow dr = TestimonialsTable.NewRow();
			dr["from"] = from;
			dr["Text"] = text;
			dr["DateModified"] = DateTime.Now;

			TestimonialsTable.Rows.Add(dr);
			AcceptChanges();

			return (int)dr["Id"];
		}

		public void UpdateTestimonial(int id, string from, string text)
		{
			DataView dv = GetTestimonials(string.Format("id={0}", id));
			if (dv.Count == 0)
				return;

			DataRow dr = TestimonialsTable.NewRow();
			dr["from"] = from;
			dr["Text"] = text;
			dr["DateModified"] = DateTime.Now;

			TestimonialsTable.Rows.Add(dr);
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
		DataTable TestimonialsTable
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
