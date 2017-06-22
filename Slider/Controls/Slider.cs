using System;
using System.Data;
using System.Web.UI;


namespace lw.Slider.Controls
{
	public class Slider : System.Web.UI.WebControls.Repeater
	{
		bool _bound = false;
	 
		
		SliderManager tMgr = new SliderManager();

		public Slider()
		{
		}

		public override void DataBind()
		{
			if (_bound)
				return;

			_bound = true;
			
			DataView SliderDV = new DataView();

            SliderDV = tMgr.GetSlides();
            this.DataSource = SliderDV;
			base.DataBind();
		}

		protected override void Render(HtmlTextWriter writer)
		{
			object obj = this.DataSource;
			if (obj != null && ((DataView)obj).Count > 0)
				base.Render(writer);
		}

	 
	}
}
