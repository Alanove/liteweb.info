using System;

using lw.Utils;

namespace lw.WebTools
{
	/// <summary>
	/// Used to hold Ext Ajax Response object 
	/// All variables used here are directly reflected to a client side object returned for the ext ajax request.
	/// </summary>
	public class AjaxResponse
	{
		/// <summary>
		/// flag to point if the submit proccess was successfull
		/// </summary>
		public bool success = true;

		/// <summary>
		/// flag to point if the form was validated
		/// </summary>
		public bool valid = true;
		
		/// <summary>
		/// Not implemented, but can used to store a callback action
		/// </summary>
		public string action;

		/// <summary>
		/// callback function
		/// </summary>
		public string callBack;

		/// <summary>
		/// Error message
		/// </summary>
		public string error;

		/// <summary>
		/// Response message
		/// </summary>
		public string message;

		/// <summary>
		/// Can store exceptions that can be passed to the client
		/// </summary>
		public Exception exception = null;

		/// <summary>
		/// Returned data can be json or any other form of data
		/// data must be serializable object
		/// </summary>
		public object data;

		/// <summary>
		/// Can store the total number of records
		/// </summary>
		public long total = 0;


		/// <summary>
		/// Writes the object to the response using Json serialization
		/// Invokes: WriteJson(true);
		/// </summary>
		public void WriteJson()
		{
			WriteJson(true);
		}

		/// <summary>
		/// Writes the object to the response using Json serialization
		/// </summary>
		/// <param name="StopPage">Invoke Response.End() after writing the json object, default: true</param>
		public void WriteJson(bool StopPage)
		{
			//Testing if the form submit is coming from iframe and not ajax
			//bool xhr = WebContext.Request.Headers["HTTP_X_REQUESTED_WITH"] == "XMLHttpRequest";
			//if (!xhr)
				//WebContext.Response.Write("<textarea>");

			WebUtils.JSonSerialize(this);
			
			//if (!xhr)
			//	WebContext.Response.Write("</textarea>");

			try
			{

				if (StopPage)
					WebContext.Response.End();
			}
			catch (Exception ex)
			{

			}
		}

		/// <summary>
		/// Returns the Json encoded string of the object
		/// </summary>
		public string GetJsonString()
		{
			return StringUtils.JSonSerialize(this);
		}
	}
}
