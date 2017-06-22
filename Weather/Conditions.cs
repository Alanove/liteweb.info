using System;

namespace lw.Weather
{
	public class Conditions
	{
		string city = "No Data";
		string dayOfWeek = DateTime.Now.DayOfWeek.ToString();
		string condition = "No Data";
		string tempF = "No Data";
		string tempC = "No Data";
		string humidity = "No Data";
		string wind = "No Data";
		string high = "No Data";
		string low = "No Data";

		public string City
		{
			get { return city; }
			set { city = value; }
		}

		public string Condition
		{
			get { return condition; }
			set { condition = value; }
		}

		public string TempF
		{
			get { return tempF; }
			set { tempF = value; }
		}

		public string TempC
		{
			get { return tempC; }
			set { tempC = value; }
		}

		public string Humidity
		{
			get { return humidity; }
			set { humidity = value; }
		}

		public string Wind
		{
			get { return wind; }
			set { wind = value; }
		}

		public string DayOfWeek
		{
			get { return dayOfWeek; }
			set { dayOfWeek = value; }
		}

		public string High
		{
			get { return high; }
			set { high = value; }
		}

		public string Low
		{
			get { return low; }
			set { low = value; }
		}
	}

}
