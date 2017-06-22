using System.Collections.Generic;
using System.Xml;

namespace lw.Weather
{
	class Weather
	{
		/// <summary>
		/// The function that returns the current conditions for the specified location.
		/// </summary>
		/// <param name="location">City or ZIP code</param>
		/// <returns></returns>
		public static Conditions GetCurrentConditions(string location)
		{
			Conditions conditions = new Conditions();

			XmlDocument xmlConditions = new XmlDocument();
			xmlConditions.Load(string.Format("http://www.google.com/ig/api?weather={0}", location));

			if (xmlConditions.SelectSingleNode("xml_api_reply/weather/problem_cause") != null)
			{
				conditions = null;
			}
			else
			{
				conditions.City = xmlConditions.SelectSingleNode("/xml_api_reply/weather/forecast_information/city").Attributes["data"].InnerText;
				conditions.Condition = xmlConditions.SelectSingleNode("/xml_api_reply/weather/current_conditions/condition").Attributes["data"].InnerText;
				conditions.TempC = xmlConditions.SelectSingleNode("/xml_api_reply/weather/current_conditions/temp_c").Attributes["data"].InnerText;
				conditions.TempF = xmlConditions.SelectSingleNode("/xml_api_reply/weather/current_conditions/temp_f").Attributes["data"].InnerText;
				conditions.Humidity = xmlConditions.SelectSingleNode("/xml_api_reply/weather/current_conditions/humidity").Attributes["data"].InnerText;
				conditions.Wind = xmlConditions.SelectSingleNode("/xml_api_reply/weather/current_conditions/wind_condition").Attributes["data"].InnerText;
			}

			return conditions;
		}

		/// <summary>
		/// The function that gets the forecast for the next four days.
		/// </summary>
		/// <param name="location">City or ZIP code</param>
		/// <returns></returns>
		public static List<Conditions> GetForecast(string location)
		{
			List<Conditions> conditions = new List<Conditions>();

			XmlDocument xmlConditions = new XmlDocument();
			xmlConditions.Load(string.Format("http://www.google.com/ig/api?weather={0}", location));

			if (xmlConditions.SelectSingleNode("xml_api_reply/weather/problem_cause") != null)
			{
				conditions = null;
			}
			else
			{
				foreach (XmlNode node in xmlConditions.SelectNodes("/xml_api_reply/weather/forecast_conditions"))
				{
					Conditions condition = new Conditions();
					condition.City = xmlConditions.SelectSingleNode("/xml_api_reply/weather/forecast_information/city").Attributes["data"].InnerText;
					condition.Condition = node.SelectSingleNode("condition").Attributes["data"].InnerText;
					condition.High = node.SelectSingleNode("high").Attributes["data"].InnerText;
					condition.Low = node.SelectSingleNode("low").Attributes["data"].InnerText;
					condition.DayOfWeek = node.SelectSingleNode("day_of_week").Attributes["data"].InnerText;
					conditions.Add(condition);
				}
			}

			return conditions;
		}
	}

}
