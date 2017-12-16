using System;
using System.Data;
using System.Drawing;
using System.Web.UI;
using lw.CTE;
using lw.ImageControls;
using lw.WebTools;
using lw.Utils;

namespace lw.Members.Controls
{
	public class ProfilePicture : System.Web.UI.HtmlControls.HtmlImage
	{
		bool _bound = false;
		string _Src = "";
		object _Image, _Title;
		int _width = -1;
		int _height = -1;
		Color fillColor = Color.Transparent;
		ImageType imageType = ImageType.Resize;
		string noPicture = "", noMPicture = "", noFPicture = "";
		bool _useGUID = false;

		DataRow memberRow;

		public ProfilePicture()
		{
		}

		public ProfilePicture(object image, object title)
		{
			_Image = image;
			_Title = title;
		}


		public override void DataBind()
		{
			if (this._bound)
				return;
			_bound = true;

			if (this.NamingContainer == null)
				return;

			memberRow = DataBinder.Eval(this.NamingContainer, "DataItem") as DataRow;
			if (memberRow == null)
			{
				DataRowView drv = DataBinder.Eval(this.NamingContainer, "DataItem") as DataRowView;
				if (drv == null)
				{
					ErrorContext.Add("invalid-dataitem", "Invalid DataItem for profile picture, DataItem must be DataRow or DataRowView (other wise needs programming to work)");
					this.Visible = false;
					return;
				}
				memberRow = drv.Row;
			}

			base.DataBind();
		}

		protected override void Render(System.Web.UI.HtmlTextWriter writer)
		{
			if (!_bound)
				this.DataBind();




			if (memberRow == null)
				return;


			MembersManager mMgr = new MembersManager();


			//why we have to get the member row again??? i couldn't find a reason.
			//memberRow = mMgr.GetMemberProfile((int)memberRow["MemberId"]);


			string picture = "";
			if (memberRow["Picture"] != System.DBNull.Value &&
				!String.IsNullOrEmpty(memberRow["Picture"].ToString()))
			{
				picture = memberRow["Picture"].ToString();
			}
			int MemberId = (int)memberRow["MemberId"];
			int privacy = 0;

			if (memberRow["Privacy"] != DBNull.Value)
				privacy = (int)memberRow["Privacy"];

			string image = "";

			PrivacySettingsManager psMgr = new PrivacySettingsManager();
			if (MemberId != WebContext.Profile.UserId)
			{
				switch (psMgr.CheckMemberProperty("ProfilePicture", privacy))
				{
					case PrivacyOptions.OnlyMe:
						picture = "";
						break;
					case PrivacyOptions.Friends:
						lw.Base.CustomPage page = this.Page as lw.Base.CustomPage;

						DataTable MyFriends = FriendsManager.GetMyFriends(page);

						if (MyFriends.Select(string.Format("FriendId={0} and Status=1", memberRow["MemberId"])).Length == 0)
						{
							picture = "";
						}
						break;
					default:
						break;
				}
				if (!Visible)
					return;
			}

			if (picture != "")
			{
				if (useGUID)
					image = string.Format("{0}/{2}/{1}",
						MembersSettings.MemberPicturesFolder, picture, StringUtils.ToURL(memberRow["Geuid"]).Replace("-", ""));
				else
					image = string.Format("{0}/{2}/{1}",
					MembersSettings.MemberPicturesFolder, picture, StringUtils.ToURL(memberRow["UserName"]));
			}
			else
			{
				if (NoPicture != "")
				{
					image = NoPicture;
					this._Src = NoPicture;
				}

				else
				{
					if (NoFPicture != "" && NoMPicture != "")
					{
						Gender? _gender = Gender.Male;
						try
						{
							_gender = (lw.Members.Gender)Enum.Parse(typeof(lw.Members.Gender), memberRow["Gender"].ToString());
						}
						catch
						{

						}


						if (_gender == lw.Members.Gender.Male)
						{
							image = NoMPicture;
							this._Src = NoMPicture;
						}
						else
						{
							image = NoFPicture;
							this._Src = NoFPicture;
						}
					}
				}
			}

			//this.Alt = memberRow["Name"].ToString();
			this.Alt = memberRow["FirstName"].ToString() + " " + memberRow["LastName"].ToString();

			switch (MemberImageType)
			{
				case ImageType.Crop:
				case ImageType.Resize:
					if (_width > 0)
					{
						if (MemberImageType == ImageType.Resize)
						{
							this._Src = string.Format("{4}/prv/handlers/ImageResizer.ashx?src={0}&width={1}&height={2}&fillColor={3}",
								image, _Width, _Height, FillColor.ToArgb(), WebContext.Root);
						}
						else
						{
							this._Src = string.Format("{4}/prv/handlers/ImageCropper.ashx?src={0}&width={1}&height={2}&fillColor={3}",
								image, _Width, _Height, FillColor.ToArgb());
						}
					}
					else
						this._Src = string.Format("{0}/{1}", WebContext.Root, image);
					break;
				case ImageType.NewResize:
					this._Src = string.Format("{3}prv/handlers/ResizeImage.ashx?img={0}&w={1}&h={2}",
							image, _Width, _Height, WebContext.Root);
					break;
				case ImageType.Large:
					this._Src = image;
					break;
				case ImageType.Medium:
					if (picture != "")
						this._Src = image.ToLower().Replace(".jpg", "-m.jpg");
					else
						this._Src = NoPicture;
					break;
				case ImageType.Thumb:
					if (picture != "")
						this._Src = image.ToLower().Replace(".jpg", "-s.jpg?" + DateTime.Now.ToFileTime()); // Datetime added to prevent cashing
					else
						this._Src = NoPicture;
					break;
				default:
					break;
			}
			if (this._Src != "")
			{
				this.Src = WebContext.Root + "/" + this._Src;
				base.Render(writer);
			}
		}

		public int _Width
		{
			get { return _width; }
			set { _width = value; }
		}
		public int _Height
		{
			get { return _height; }
			set { _height = value; }
		}
		public Color FillColor
		{
			get { return fillColor; }
			set { fillColor = value; }
		}
		public ImageType MemberImageType
		{
			get { return imageType; }
			set { imageType = value; }
		}

		public string NoPicture
		{
			get
			{
				return noPicture;
			}
			set
			{
				noPicture = value;
			}
		}

		public string NoMPicture
		{
			get
			{
				return noMPicture;
			}
			set
			{
				noMPicture = value;
			}
		}


		public string NoFPicture
		{
			get
			{
				return noFPicture;
			}
			set
			{
				noFPicture = value;
			}
		}

		public bool useGUID
		{
			get
			{
				return _useGUID;
			}
			set
			{
				_useGUID = value;
			}
		}
	}
}
