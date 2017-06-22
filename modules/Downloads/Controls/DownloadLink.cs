using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using lw.Utils;
using lw.WebTools;

using lw.Base;

namespace lw.Downloads.Controls
{
	public class DownloadLink : HtmlAnchor
	{
		bool _bound = false;
		DownloadLinktype _type = DownloadLinktype.Regular;
		string category = "";
        string titleformat = "{0}";
		string format = "{0}";
		DataRow _downloadItem = null;
		int _maxCharacters = -1;
		string _closingSentence = "...";

		public DownloadLink()
		{
		}

		public override void DataBind()
		{
			if (this._bound)
				return;
			_bound = true;



			if (DownloadItem != null)
			{
				if (Int32.Parse(DownloadItem["Status"].ToString()) == (int)CTE.Enum.DownloadStatus.Disabled && MyPage.Editable)
				{
					this.Attributes.Add("class", "disabled");
				}

				if (Int32.Parse(DownloadItem["Status"].ToString()) != (int)CTE.Enum.DownloadStatus.Disabled || MyPage.Editable)
				{
					if (this.Controls.Count == 0)
					{
						if (MaxCharacters > 0)
						{
							this.InnerHtml = this.Title = StringUtils.Trankate(StringUtils.StripOutHtmlTags(string.Format(Format, DownloadItem["Title"])), _maxCharacters, _closingSentence);
						}
						else
						{
							this.InnerHtml = this.Title = string.Format(Format, DownloadItem["Title"]);
						}
					}

					this.HRef = Type == DownloadLinktype.Regular ? DownloadItem["DownloadLink"].ToString() :
							string.Format("{2}/download-file.axd?file={0}&Type={1}",
								DownloadItem["DownloadLink"],
								Type.ToString(),
								WebContext.Root
							);
				}
				else
				{
					this.Visible = false;
				}

                if (TitleFormat != null)
                    this.Attributes.Add("title", string.Format(TitleFormat, DownloadItem["Title"]));
			}
			else
			{
				this.Visible = false;
			}

			base.DataBind();

			if (MyPage.Editable)
			{
				this.Attributes.Add("data-editable", "true");
				this.Attributes.Add("data-category", Category);
				this.Attributes.Add("data-type", "downloads");
				if (DownloadItem != null)
				{
					this.Attributes.Add("data-id", DownloadItem["DownloadId"].ToString());
				}
			}

          
		}

		protected override void Render(HtmlTextWriter writer)
		{
			base.Render(writer);
		}


		CustomPage myPage = null;
		CustomPage MyPage
		{
			get
			{
				if (myPage == null)
				{
					myPage = this.Page as CustomPage;
				}
				return myPage;
			}

		}


		public DataRow DownloadItem
		{
			get
			{
				if (_downloadItem == null)
				{
					try
					{
						object obj = DataBinder.Eval(this.NamingContainer, "DataItem");

						if (obj != null)
						{
							DataRowView row = obj as DataRowView;
							if (row != null)
								_downloadItem = row.Row;
							else
								_downloadItem = obj as DataRow;
						}
					}
					catch
					{
					}
					if (_downloadItem == null)
					{
						Downloads downloads = new Downloads();
						DataTable downloadsDT;
						if (!StringUtils.IsNullOrWhiteSpace(category))
							downloadsDT = downloads.GetDownloadsByType(category);
						else
							downloadsDT = downloads.GetDownloads("");

						DataView downloadsView = downloadsDT.DefaultView;
						downloadsView.RowFilter = "DateAdded <= '" + DateTime.Now + "'";
						downloadsView.Sort = "DateAdded Desc";
						if (downloadsView.Count > 0)
							_downloadItem = downloadsView[0].Row;
					}
				}
				return _downloadItem;
			}
			set
			{
				_downloadItem = value;
			}
		}
		public string Format
		{
			get { return format; }
			set { format = value; }
		}
		public string Category
		{
			get { return category; }
			set { category = value; }
		}
        public string TitleFormat
        {
            get { return titleformat; }
            set { titleformat = value; }
        }
		public DownloadLinktype Type
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
		/// <summary>
		/// The max number of characters that should be returned.
		/// </summary>
		public int MaxCharacters
		{
			get
			{
				return _maxCharacters;
			}
			set
			{
				_maxCharacters = value;
			}
		}
		/// <summary>
		/// If the letters were more than <see cref="MaxCharacters"/>, this is the closing sentence.
		/// Default: ...
		/// </summary>
		public string ClosingSentence
		{
			get
			{
				return _closingSentence;
			}
			set
			{
				_closingSentence = value;
			}
		}
	}
}
