using System;
using System.IO;
using System.Web;
using System.Web.UI.HtmlControls;
using lw.CTE;
using lw.CTE.Enum;
using lw.Utils;
using lw.WebTools;

namespace lw.Downloads.Handler
{
	public class DownloadFile : HtmlAnchor, IHttpHandler
	{
		string _file = "";
		DownloadType _Type = DownloadType.Products;

		public override void DataBind()
		{
			this.HRef = string.Format("download-file.axd?file={0}&type={1}", File, Type);
			base.DataBind();
		}
		public void ProcessRequest(HttpContext context)
		{
			System.Web.HttpRequest Request = context.Request;
			System.Web.HttpServerUtility Server = context.Server;
			System.Web.HttpResponse Response = context.Response;

			_file = Request.QueryString["file"];

			try
			{
				Type = (DownloadType)Enum.Parse(typeof(DownloadType), Request.QueryString["Type"]);
			}
			catch (Exception Ex)
			{
				Response.Redirect("~/");
			}


			string path = "";

			switch (_Type)
			{
				case DownloadType.News:
					path = Folders.NewsFile + "/";
					_file = string.Format("{0}/{1}{2}", WebContext.StartDir, path, _file);
					break;
				case DownloadType.Downloads:
					//_file = Server.MapPath(_file);
					break;
				default:
					_file = string.Format("{0}/{1}{2}", WebContext.StartDir, path, _file);
					break;
			}

			

			FileInfo fi = new FileInfo(Server.MapPath(_file));

			string mimeType = IO.GetMimeType(_file);
			if (mimeType == "")
				mimeType = "application/force-download";

			//Response.AddHeader("Content-Transfer-Encoding", "binary");


			Response.Clear();
			Response.AddHeader("Content-Disposition", "attachment; filename=" + fi.Name);
			Response.AddHeader("Content-Type", mimeType);
			Response.AddHeader("Content-Length", fi.Length.ToString());
			Response.WriteFile(fi.FullName);
			Response.Flush();
			Response.End();
		}
		#region properties
		public string File
		{
			get { return _file; }
			set { _file = value; }
		}
		public DownloadType Type
		{
			get { return _Type; }
			set { _Type = value; }
		}
		#endregion
		public bool IsReusable
		{
			get
			{
				return true;
			}
		}
	}
}
