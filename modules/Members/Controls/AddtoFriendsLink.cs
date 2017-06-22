using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;


namespace lw.Members.Controls
{
	public class AddtoFriendsLink : HtmlAnchor
	{
		//<a href="javascript:" id='add-friends-<lw:MemberProfileProperty Property=MemberId runat=server />' onclick="lw.members.addFriend('<lw:MemberProfileProperty Property=GeuId runat=server />', this)">Add to Friends</a>

		bool _bound = false;
		int _memberId;
		string _format = "{0}";
		string _AddToFriendsText = "Add To Friends";
		string _PendingFriendText = "Friend Request Sent";
		string _AcceptFriendText = "Accept";
		string _UnFriendText = "Unfriend";

		string _tag = "a";

		public AddtoFriendsLink()
		{
		}

		public override void DataBind()
		{
			if (this._bound)
				return;
			_bound = true;

			lw.Base.CustomPage page = this.Page as lw.Base.CustomPage;

			DataTable MyFriends = FriendsManager.GetMyFriends(page);

			this.HRef = "javascript:";

			DataRow memberRow = DataBinder.Eval(this.NamingContainer, "DataItem") as DataRow;
			if (memberRow == null)
			{
				DataRowView memberRowView = DataBinder.Eval(this.NamingContainer, "DataItem") as DataRowView;
				if (memberRowView != null)
					memberRow = memberRowView.Row;
			}
			if (memberRow == null)
				return;

			_memberId = (int)memberRow["MemberId"];

			if (lw.WebTools.WebContext.Profile.UserId == _memberId)
			{
				this.Visible = false;
				return;
			}
			DataView friendView = new DataView(MyFriends,
									string.Format("MemberId={0}", _memberId),
									"", DataViewRowState.CurrentRows);

			DataView myFriendView = new DataView(MyFriends,
									string.Format("FriendId={0}", _memberId),
									"", DataViewRowState.CurrentRows);

			int privacy = (int)memberRow["Privacy"];

			if (friendView.Count > 0)
			{
				if ((Int16)friendView[0]["Status"] == (Int16)FriendStatus.Approved)
				{
					if ((Int16)myFriendView[0]["Status"] == (Int16)FriendStatus.Pending)
					{
						this.Attributes.Add("onclick", string.Format("lw.members.acceptFriend('{0}', this)", memberRow["GeuId"]));
						this.InnerHtml = string.Format(Format, AcceptFriendText);
					}
					else
					{
						this.Attributes.Add("onclick", string.Format("lw.members.unfriend('{0}', this)", memberRow["GeuId"]));
						this.InnerHtml = string.Format(Format, UnFriendText);
					}
				}
				else
				{
					if ((Int16)friendView[0]["Status"] == (Int16)FriendStatus.Pending)
					{
						_tag = "span";
						this.InnerHtml = string.Format(Format, PendingFriendText);
					}
					else
					{
						friendView = new DataView(MyFriends,
										string.Format("FriendId={0} and Status={1}", _memberId, (Int16)FriendStatus.Pending),
										"", DataViewRowState.CurrentRows);
						if (friendView.Count > 0)
						{
							this.InnerHtml = string.Format(Format, AcceptFriendText);
							this.Attributes.Add("onclick", string.Format("lw.members.acceptFriend('{0}', this)", memberRow["GeuId"]));

						}
					}
				}
			}
			else
			{
				this.Attributes.Add("onclick", string.Format("lw.members.addFriend('{0}', this)", memberRow["GeuId"]));
				this.InnerHtml = string.Format(Format, AddToFriendsText);
			}
			this.ID = this.UniqueID;
		}

		protected override void RenderBeginTag(HtmlTextWriter writer)
		{
			if (_tag == "span")
			{
				writer.Write("<span");
				if (this.Attributes["Class"] != null)
				{
					writer.Write(" class=\"" + this.Attributes["Class"] + "\"");
				}
				writer.Write(">");
			}
			else
				base.RenderBeginTag(writer);
		}
		protected override void RenderEndTag(HtmlTextWriter writer)
		{
			if (_tag == "span")
			{
				writer.Write("</span>");
			}
			else
				base.RenderEndTag(writer);
		}

		#region properties

		public override string UniqueID
		{
			get
			{
				return "add-friends-" + _memberId;
			}
		}

		public string AddToFriendsText
		{
			get
			{
				return _AddToFriendsText;
			}
			set
			{
				_AddToFriendsText = value;
			}
		}
		public string PendingFriendText
		{
			get
			{
				return _PendingFriendText;
			}
			set
			{
				_PendingFriendText = value;
			}
		}
		public string AcceptFriendText
		{
			get
			{
				return _AcceptFriendText;
			}
			set
			{
				_AcceptFriendText = value;
			}
		}

		public string UnFriendText
		{
			get
			{
				return _UnFriendText;
			}
			set
			{
				_UnFriendText = value;
			}
		}

		public String Format
		{
			get { return _format; }
			set { _format = value; }
		}

		#endregion
	}
}
