using System;
using System.Data;

using lw.Utils;
using lw.WebTools;

namespace lw.Testimonials
{
	public class TestimonialsManager
	{

		TestimonialsDS ds;

		string config = cte.ConfigFile;

		public TestimonialsManager()
		{
		}

		#region Methods

		/* Start Testimonials */

		public DataRow AddTestimonial(string Text, string From, DateTime? Date, int Approved, int Display, int GroupId)
		{
			TestimonialsDS.TestimonialsRow row = DS.Testimonials.NewTestimonialsRow();

			row.Text = Text;
			row.From = From;
			if (Date != null)
				row.Date = Date.Value;

			row.Approved = Approved;
			row.Display = Display;
			row.GroupId = GroupId;
			row.DateModified = DateTime.Now;

			ds.Testimonials.Rows.Add(row);

			AcceptChanges();

			return row;
		}


		//// Removed and Replaced by the above one that returns the row since it is needed //////

		//public bool AddTestimonial(string Text, string From, DateTime? Date, int Approved, int Display, int GroupId)
		//{
		//	TestimonialsDS.TestimonialsRow row = DS.Testimonials.NewTestimonialsRow();

		//	row.Text = Text;
		//	row.From = From;
		//	if(Date != null)
		//		row.Date = Date.Value;
			
		//	row.Approved = Approved;
		//	row.Display = Display;
		//	row.GroupId = GroupId;
		//	row.DateModified = DateTime.Now;

		//	ds.Testimonials.Rows.Add(row);

		//	AcceptChanges();

		//	return true;
		//}

		public bool UpdateTestimonial(int Id, string Text, string From, DateTime? Date, int Approved, int Display, int GroupId)
		{
			DataView dv = this.GetTestimonials(string.Format("Id = {0}", Id));

			if (dv.Count <= 0)
				return false;

			TestimonialsDS.TestimonialsRow row = (TestimonialsDS.TestimonialsRow)dv[0].Row;

			row.Text = Text;
			row.From = From;
			if (Date != null)
				row.Date = Date.Value;

			row.Approved = Approved;
			row.Display = Display;
			row.GroupId = GroupId;

			row.Table.AcceptChanges();
			AcceptChanges();

			return true;
		}

		public bool DeleteTestimonial(int Id)
		{
			DataView dv = this.GetTestimonials(string.Format("Id = {0}", Id));

			if (dv.Count <= 0)
				return false;

			TestimonialsDS.TestimonialsRow row = (TestimonialsDS.TestimonialsRow)dv[0].Row;

			row.Delete();
			AcceptChanges();

			return true;
		}

		public TestimonialsDS.TestimonialsRow GetTestimonial(int Id)
		{
			DataView dv = GetTestimonials(string.Format("Id = {0}", Id));
			TestimonialsDS.TestimonialsRow row = (TestimonialsDS.TestimonialsRow)dv[0].Row;
			return row;
		}
		public DataView GetTestimonials()
		{
			return GetTestimonials("");
		}
		public DataView GetTestimonials(string cond)
		{
			return new DataView(DS.Testimonials, cond, "Id", DataViewRowState.CurrentRows);
		}

		/* End Testimonials */

		/* Start Testimonials Groups */

		public bool AddTestimonialGroup(string GroupName)
		{
			if (this.GetTestimonialGroups("GroupName='" + StringUtils.SQLEncode(GroupName) + "'").Count > 0)
			{
				return false;
			}

			TestimonialsDS.GroupsRow row = DS.Groups.NewGroupsRow();

			row.GroupName = GroupName;

			ds.Groups.Rows.Add(row);

			AcceptChanges();

			return true;
		}
		public bool UpdateTestimonialGroup(int GroupId, string GroupName)
		{

			DataView dv = GetTestimonialGroups(string.Format("GroupId={0}", GroupId));

			if (dv.Count <= 0)
				return false;

			TestimonialsDS.GroupsRow row = (TestimonialsDS.GroupsRow)dv[0].Row;


			row.GroupName = GroupName;

			row.Table.AcceptChanges();
			AcceptChanges();

			return true;
		}
		public bool DeleteTestimonialGroup(int GroupId)
		{
			DataView dv = GetTestimonialGroups(string.Format("GroupId={0}", GroupId));

			if (dv.Count <= 0)
				return false;

			DataView testimonials = this.GetTestimonials(string.Format("GroupId = {0}", GroupId));

			foreach (DataRowView drv in testimonials)
			{
				DeleteTestimonial((int)(drv["TestimonialId"]));
			}

			TestimonialsDS.GroupsRow row = (TestimonialsDS.GroupsRow)dv[0].Row;

			row.Delete();
			AcceptChanges();

			return true;
		}

		public TestimonialsDS.GroupsRow GetTestimonialGroup(int GroupId)
		{
			DataView dv = GetTestimonialGroups(string.Format("{0}", GroupId));
			TestimonialsDS.GroupsRow row = (TestimonialsDS.GroupsRow)dv[0].Row;
			return row;
		}
		public DataView GetTestimonialGroups()
		{
			return GetTestimonialGroups("");
		}
		public DataView GetTestimonialGroups(string cond)
		{
			return new DataView(DS.Groups, cond, "GroupId", DataViewRowState.CurrentRows);
		}

		/* End Testimonial Groups */

		void AcceptChanges()
		{
			DS.AcceptChanges();
			XmlManager.SetDataSet(config, DS);
		}

		#endregion

		#region variables
		public TestimonialsDS DS
		{
			get
			{
				if (ds == null)
				{
					ds = new TestimonialsDS();
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
