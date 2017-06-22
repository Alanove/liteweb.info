using System;

using lw.WebTools;

namespace lw.Utilities
{
	/// <summary>
	/// Creates a random number between the Min and Max Values (max and min are inclusive)
	/// </summary>
	public class RandomNumber: System.Web.UI.WebControls.Label
	{
		string _Format = "{0}";


		public override void RenderBeginTag(System.Web.UI.HtmlTextWriter writer)
		{
			//base.RenderBeginTag(writer);
		}
		public override void RenderEndTag(System.Web.UI.HtmlTextWriter writer)
		{
			//base.RenderEndTag(writer);
		}

		protected override void Render(System.Web.UI.HtmlTextWriter writer)
		{
			Random r = new Random();
			int v = r.Next(Min, Max + 1);

			this.Text = string.Format(Format, v);
			base.Render(writer);
		}

		/// <summary>
		/// Sets the format reference for this date
		/// </summary>
		public string Format
		{
			get
			{
				return _Format;
			}
			set
			{
				_Format = value;
			}
		}

		int _min;
		/// <summary>
		/// Set set minimum number that can be returned
		/// </summary>
		public int Min
		{
			get
			{
				return _min;
			}
			set
			{
				_min = value;
			}
		}

		int _max;
		/// <summary>
		/// Set set maximum number that can be returned
		/// </summary>
		public int Max
		{
			get
			{
				return _max;
			}
			set
			{
				_max = value;
			}
		}
	}
}
