using System;
using System.Data;
using System.Web.UI;


namespace lw.Testimonials.Controls
{
	public enum TestimonialsType
	{
		Parent, Testimonials
	}

	public class Testimonials : System.Web.UI.WebControls.Repeater
	{
		bool _bound = false;
		TestimonialsType _type;
		
		TestimonialsManager tMgr = new TestimonialsManager();

		public Testimonials()
		{
		}

		public override void DataBind()
		{
			if (_bound)
				return;

			_bound = true;
			
			object obj = null;
			DataView parentDV = new DataView();
			DataView testimonialsDV = new DataView();

			switch (this.Type)
			{
				case TestimonialsType.Parent:
					parentDV = tMgr.GetTestimonialGroups();
					this.DataSource = parentDV;
					break;

				case TestimonialsType.Testimonials:
					obj = DataBinder.Eval(this.NamingContainer, "DataItem.GroupId");
					if (!String.IsNullOrEmpty(obj.ToString()))
					{
						testimonialsDV = tMgr.GetTestimonials("GroupId=" + obj.ToString() + " and Approved=1");
						this.DataSource = testimonialsDV;
					}
					break;
			}
			base.DataBind();
		}

		protected override void Render(HtmlTextWriter writer)
		{
			object obj = this.DataSource;
			if (obj != null && ((DataView)obj).Count > 0)
				base.Render(writer);
		}

		public TestimonialsType Type
		{
			get
			{
				return _type;
			}
			set
			{
				_type = value;
			}
		}
	}
}
