using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;

using lw.WebTools;
using lw.Base;


namespace lw.VideoGallery.Controls
{
	public class VideoLink : HtmlAnchor
	{
		bool _bound = false;
		VideoLinkType _type = VideoLinkType.ArchiveByType;
		bool _IncludeVideoCategoryPath = false;
		string _innerText = "";
		string _format = "{0}";
		bool _hideText = false;
		string _path = "";
		string _extension = "";

		public VideoLink()
		{
		}

		public override void DataBind()
		{
			if (this._bound)
				return;
			_bound = true;
			
			int _videoId = -1;
			object obj = DataBinder.Eval(this.NamingContainer, "DataItem.VideoId");

			if (!String.IsNullOrEmpty(obj.ToString()))
				_videoId = (int)obj;
			else
				return;
			
			MediaGalleryManager mMgr = new MediaGalleryManager();
			VideosView _videoView = mMgr.GetVideoView(_videoId);

			string text = _videoView.Title;

			switch (Type)
			{
				case VideoLinkType.ArchiveByType:
					text = _videoView.VideoCategoryTitle;
					this.HRef = string.Format("{2}/{0}/{1}.aspx",
						Path == "" ? "video-categories" : Path,
						_videoView.VideoCategoryUniqueName,
						WebContext.Root);
					break;
				case VideoLinkType.VideoID:
					if (IncludeVideoCategoryPath)
					{
						this.HRef = string.Format("{3}/{0}{1}/{2}{4}",
							Path == "" ? "" : Path + "/",
							_videoView.VideoCategoryUniqueName,
							_videoView.VideoId,
							WebContext.Root,
							Extension
						);
					}
					else
					{
						this.HRef = string.Format("{2}/{0}{1}{3}",
							Path == "" ? "" : Path + "/",
							_videoView.VideoId,
							WebContext.Root,
							Extension
						);
					}
					break;
				case VideoLinkType.Video:
				default:
					this.HRef = string.Format("{2}/{0}/{1}{3}",
						Path == "" ? "videos" : Path,
						_videoView.UniqueName,
						WebContext.Root,
						Extension
						);
					if (IncludeVideoCategoryPath)
					{
						this.HRef = string.Format("{3}/{0}/{1}/{2}{4}",
						Path == "" ? "videos" : Path,
						_videoView.VideoCategoryUniqueName,
						_videoView.UniqueName,
						WebContext.Root,
						Extension
						);
					}
					else
					{
						this.HRef = string.Format("{2}/{0}/{1}{3}",
						Path == "" ? "videos" : Path,
						_videoView.UniqueName,
						WebContext.Root,
						Extension
						);
					}
					break;

			}
			if (this.Controls.Count == 0)
				this.InnerHtml = string.Format(Format, text);

			base.DataBind();

			if (MyPage.Editable && ImEditable)
			{
				this.Attributes.Add("data-editable", "true");
				this.Attributes.Add("data-id", _videoId.ToString());
				this.Attributes.Add("data-type", "videos");
			}
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

		public VideoLinkType Type
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
		public string Format
		{
			get
			{
				return _format;
			}
			set
			{
				_format = value;
			}
		}
		public string Path
		{
			get
			{
				return _path;
			}
			set
			{
				_path = value;
			}
		}
		public bool IncludeVideoCategoryPath
		{
			get { return _IncludeVideoCategoryPath; }
			set { _IncludeVideoCategoryPath = value; }
		}
		public bool HideText
		{
			get { return _hideText; }
			set { _hideText = value; }
		}

		public string Extension
		{
			get { return _extension; }
			set { _extension = value; }
		}



		bool _imEditable = false;
		/// <summary>
		/// Indicates if this link should contain the editing parameters
		/// </summary>
		public bool ImEditable
		{
			get
			{
				return _imEditable;
			}
			set
			{
				_imEditable = value;
			}
		}
	}
}