using System;
using System.Text;
using System.Web.UI;
using lw.CTE.Enum;
using lw.DataControls;
using lw.Utils;
using lw.WebTools;

namespace lw.VideoGallery.Controls
{
	public class VideosDataSource : CustomDataSource
	{
		bool _bound = false;
		string _top = "100 PERCENT";
		VideoTagStatus status = VideoTagStatus.Archive;
		string category = "";
		MediaGalleryManager mMgr;
		Languages _language = Languages.Default;
		string _condition = "";

		public VideosDataSource()
		{
			this.DataLibrary = cte.lib;
			mMgr = new MediaGalleryManager();
		}

		public override void DataBind()
		{
			if (_bound)
				return;
			_bound = true;

			StringBuilder cond = new StringBuilder();			 

			if (Category != "")
			{
				cond.Append(string.Format(" And VideoCategoryTitle='{0}'", StringUtils.SQLEncode(Category)));
			}



			if (status != VideoTagStatus.All)
			{
				if (status == VideoTagStatus.Inherit)
				{
					try
					{
						int _status = -1;
						object obj = DataBinder.Eval(this.NamingContainer, "DataItem.Status");
						if (obj != null)
							_status = Int32.Parse(obj.ToString());
						else
							_status = (int)VideoStatus.Archive;

						cond.Append(string.Format(" And Status={0}", _status));
					}
					catch
					{
					}
				}
				else
				{
					cond.Append(string.Format(" And Status={0}", (int)Status));
				}
			}
			else
				if (CMSMode == null || !CMSMode.Value)
					cond.Append(string.Format(" And Status<>{0}", (int)VideoStatus.Disabled));


			if (_language != Languages.Default)
			{
				cond.Append(string.Format(" and Language={0}", (short)_language));
			}

			if (!string.IsNullOrWhiteSpace(Condition))
			{
				cond.Append(string.Format(" And " + Condition));
			}


			string q = WebContext.Request["q"];
			if (!String.IsNullOrWhiteSpace(q))
			{
				cond.Append(string.Format(" And (Title like '%{0}%' or VideoCategoryTitle like '%{0}%')", StringUtils.SQLEncode(q)));
			}

			this.SelectCommand = string.Format("select top {0} * from VideosView where 1=1 {1}", 
				Top, 
				cond.ToString());

			if (!EnablePaging && OrderBy!=null)
			{
				this.SelectCommand += " Order By " + this.OrderBy;
			}

			base.DataBind();
		}

		public string Top
		{
			get { return _top; }
			set { _top = value; }
		}

		public VideoTagStatus Status
		{
			get
			{
				return status;
			}
			set
			{
				status = value;
			}
		}
		public string Condition
		{
			get { return _condition; }
			set { _condition = value; }
		}
		public string Category
		{
			get
			{
				return category;
			}
			set
			{
				category = value;
			}
		}


		public Languages Language
		{
			get { return _language; }
			set { _language = value; }
		}
	}
}