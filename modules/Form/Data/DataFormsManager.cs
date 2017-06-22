using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using lw.CTE;
using lw.Data;
using lw.Utils;
using lw.WebTools;
using lw.GraphicUtils;

using lw.Forms.Classes;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using lw.CTE.Enum;
using System.Xml.Linq;
using lw.Network;

namespace lw.Forms.Data
{
    /// <summary>
    /// A class to manage the forms in the system inherited from LINQManager
    /// </summary>
    public class DataFormsManager : LINQManager
    {
        /// <summary>
        /// DataFormsManager constructor inherited from LINQManager
        /// cte.lib: the name of the Database library associated with this DLL
        /// </summary>
        public DataFormsManager()
            : base(cte.lib)
        {
        }


        public String SerializeForm(FormRequest Request)
        {
            var formData = JsonConvert.SerializeObject(
                Request,
                Formatting.None,
                new JsonSerializerSettings
                {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    Converters = new List<JsonConverter>
					{
						new JavaScriptDateTimeConverter()
					}
                }
            );
            return formData;
        }



        public FormRequest DeserializeForm(String formData)
        {
            var ret = new FormRequest();
            ret = (FormRequest)JsonConvert.DeserializeObject(formData, typeof(FormRequest));

            return ret;
        }


		public String SerializeFormValues(IDictionary<string, string> collection)
		{
			var formData = JsonConvert.SerializeObject(
				collection,
				Formatting.None,
				new JsonSerializerSettings
				{
					ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
					Converters = new List<JsonConverter>
					{
						new JavaScriptDateTimeConverter()
					}
				}
			);
			return formData;
		}

		public IDictionary<string, string> DeserializeFormValues(String formData)
		{
			var ret = new Dictionary<string, string>();
			ret = (Dictionary<string, string>)JsonConvert.DeserializeObject(formData, typeof(Dictionary<string, string>));

			return ret;
		}

        public String SaveFormData(string type, string email, string name, string data, string attachements)
        {

            string ipAddress = getUserIP();

            Dataform dtForm = new Dataform
            {
                Guid=System.Guid.NewGuid(),
                FormType = type,
                IP = ipAddress,
                Email = email,
                Name = name,
                DateCreated=DateTime.Now,
                Data = data,
                Attachements = attachements,
				Status = (int)FormStatus.Pending,
				History = new XElement("xml")
            };

            FormData.Dataforms.InsertOnSubmit(dtForm);
            FormData.SubmitChanges();

            return dtForm.Guid.ToString();
        }


		public void UpdateForm(int FormId, FormStatus Status, string Note)
		{
			Dataform form = GetForm(FormId);
			form.Status = (int)Status;

			XElement published = new XElement("Action");

			published.Add(new XElement("Status", Status));
			published.Add(new XElement("User", WebContext.Profile.dbUserName));
			published.Add(new XElement("Date", DateTime.Now));
			published.Add(new XElement("Note", Note));
			if (form.History == null)
			{
				//create a cml node
				form.History = new XElement("xml");
			}
			form.History.Add(published);
			FormData.SubmitChanges();

			UpdateXML(FormId, form.History.ToString());
		}

        public void UpdateFormStatus(int FormId, FormStatus status)
        {
            Dataform form = GetForm(FormId);
            form.Status = (int)status;
            FormData.SubmitChanges();
        }

        public Dataform GetForm(int FormId)
        {
            var query = from p in FormData.Dataforms
                        where
                        p.FormId == FormId
                        select p;
            if (query.Count() > 0)
                return query.Single();
            return null;
        }

        //Get the FormID from GUID of the form
        public int GetFormByGuid(string id)
        {
            int formId = (from p in FormData.Dataforms
                        where
                        p.Guid == new Guid(id)
                        select p.FormId).FirstOrDefault();
      
            return formId;
        }

        public IQueryable<Dataform> GetAllForms()
        {
            var query = from p in FormData.Dataforms
                        select p;
            return query;
        }

        public string getUserIP()
        {
            string IPAddress = string.Empty;

            string ipadd = WebTools.WebContext.IPAddress;

            String strHostName = HttpContext.Current.Request.UserHostAddress.ToString();

            IPAddress = System.Net.Dns.GetHostAddresses(strHostName).GetValue(0).ToString();
            return IPAddress;
        }

        public void UpdateHistory(int FormId, FormStatus Status, string Note)
        {
            Dataform thisForm = GetForm(FormId);

            XElement published = new XElement("Action");

            published.Add(new XElement("Status", Status));
            published.Add(new XElement("User", WebContext.Profile.dbUserName));
            published.Add(new XElement("Date", DateTime.Now));
            published.Add(new XElement("Note", Note));
            if (thisForm.History== null)
            {
                //create a cml node
                thisForm.History = new XElement("xml");
            }
            thisForm.History.Add(published);
            FormData.SubmitChanges();

            UpdateXML(FormId, thisForm.History.ToString());
        }

        public void UpdateXML(int FormId, string Xml)
        {
            string sql = string.Format("Update Dataform set History=N'{1}' where FormId={0}", FormId, StringUtils.SQLEncode(Xml));
            DBUtils.ExecuteQuery(sql, cte.lib);
        }

		public string GetHistory(int FormId)
		{
			Dataform form = GetForm(FormId);
			string retVal = "";
			XElement formHistory = form.History;
			var actions = from ms in formHistory.Descendants("Action")
						  orderby ms.Element("Date").Value descending
						  select new
						  {
							  Status = ms.Element("Status").Value,
							  User = ms.Element("User").Value,
							  Date = ms.Element("Date").Value,
							  Note = ms.Element("Note").Value
						  };

			string head = "", bottom = "";

			if (actions.Count() > 0)
			{
				head = "<div class=\"table-responsive\"><table class=\"table table-bordered table-striped\"><thead><th>Status</th><th>Contact Name</th><th>Action Date</th><th>Notes</th></thead><tbody>";
				bottom = "</tbody></table></div>";
			}
			StringBuilder sb = new StringBuilder();
			sb.Append(head);

			foreach (var action in actions)
			{
				sb.Append("<tr>");
				sb.Append(string.Format("<td><span class=\"label label-{0}\">{0}</span></td>", action.Status));
				sb.Append(string.Format("<td class=\"user\">{0}</td>", action.User));
				sb.Append(string.Format("<td class=\"date\">{0:f}</td>", Convert.ToDateTime(action.Date)));
				sb.Append(string.Format("<td class=\"note\">{0}</td>", action.Note));
				sb.Append("</tr>");


				//retVal = retVal + "<div><span class=\"status\">Status: " + action.Status + " </span> by: <span class=\"user\">" + action.User + " </span> Date: <span class=\"date\">" + string.Format("{0:f}", Convert.ToDateTime(action.Date)) + "</span> <div class=\"note\">Note:" + action.Note + "</div></div>";
			}
			sb.Append(bottom);
			return sb.ToString();
		}

        public string GetOpenFormsHistory()
        {
            string retVal = "";
            string historyValue = "";
            int counter = 0;
            IQueryable<Dataform> forms = GetAllForms().Where(x => x.Status==(int)FormStatus.Pending);
            retVal = retVal + "<table class='table'>" +
               "<thead><th>Form Type</th><th>Email</th><th>Form Name</th><th>Date Created</th><th>History</th>" +
               "</thead><tbody>";
            foreach (Dataform form in forms)
            {
                counter++;
                historyValue = "<ul>";
                XElement formHistory = form.History;
                if (formHistory != null)
                {
                    var actions = from ms in formHistory.Descendants("Action")
                                  select new
                                  {
                                      Status = ms.Element("Status").Value,
                                      User = ms.Element("User").Value,
                                      Date = ms.Element("Date").Value,
                                      Note = ms.Element("Note").Value
                                  };
                    foreach (var action in actions)
                    {
                        historyValue = historyValue + "<li style='font-family:Roboto'>" + action.User + " changed status to " + action.Status + " on " + string.Format("{0:M/dd/yyyy H:mm}", Convert.ToDateTime(action.Date)) + "<br /> Note:" + action.Note + "</li>";
                    }
                    historyValue = historyValue + "</ul>";
                    if (counter % 2 == 0)
                        retVal = retVal + "<tr><td>" + form.FormType + "</td><td>" + form.Email + "</td><td>" + form.Name + "</td><td>" + form.DateCreated + "</td><td>" + historyValue + "</td></tr>";
                    else
                        retVal = retVal + "<tr style='background:#f7f4f2'><td>" + form.FormType + "</td><td>" + form.Email + "</td><td>" + form.Name + "</td><td>" + form.DateCreated + "</td><td>" + historyValue + "</td></tr>";
                }

            }
            retVal = retVal + "</tbody></table>";
            Network.Mail m = new Network.Mail();
            m.From = "kabdelmassih@sabis.net";
            m.Body = retVal;
            m.To = "kabdelmassih@sabis.net";

            m.Send();
            return retVal;
        }

        #region Variables

        public DataFormsDataContext FormData
        {
            get
            {
                if (_dataContext == null)
                    _dataContext = new DataFormsDataContext(Connection);
                return (DataFormsDataContext)_dataContext;
            }
        }

        #endregion
    }
}
