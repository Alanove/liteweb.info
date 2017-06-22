using System;

using lw.WebTools;

namespace lw.Utilities
{
	public class LocalDate: System.Web.UI.WebControls.Label
	{
		string _Format = "{0}";

		protected override void Render(System.Web.UI.HtmlTextWriter writer)
		{
			DateTime date = DateTime.UtcNow;


			string timeDifference = Config.GetFromWebConfig("TimeDifference");
			if (timeDifference != null && timeDifference != "")
				date = date.AddHours(double.Parse(timeDifference));

			this.Text = string.Format(new System.Globalization.CultureInfo(_cultureInfo), Format, date);
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

		string _cultureInfo = "en-US";
		/// <summary>
		/// Sets or gets the culture info of this tag
		/// </summary>
		public string CultureInfo
		{
			get
			{
				return _cultureInfo;
			}
			set
			{
				_cultureInfo = value;
			}
		}
	}
}
