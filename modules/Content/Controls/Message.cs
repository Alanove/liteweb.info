using System;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using lw.Base;

namespace lw.Content.Controls
{
	public class Message : HtmlControl
	{
		string _format = "{0}", _seperator = "&nbsp;";
		string keyFilter = "";
		string text = "";
		string tag = "";
		bool autoHide = false;
		int hideAfter = 10;
		bool closable = false;
		bool forceDisplay = false;
		HideEffect closeEffect = HideEffect.Fade;
		string addClass = "";

		string closeId;
		public Message()
		{
			
		}

		public override void DataBind()
		{
			CustomPage page = this.Page as CustomPage;
			closeId = string.Format("{0}-close", this.ClientID);

			if (Closable)
			{
				if (page != null)
				{
					page.RegisterLoadScript(closeId,
						string.Format("$('#{0}').click(function(){{lw.close('{1}', '{2}');}});",
							closeId, ClientID, CloseEffect));
				}
			}
			if (AutoHide)
			{/*
				page.RegisterLoadScript(ClientID,
						string.Format(@"window.setTimeout(function(){{
try{{lw.AutoHide('{0}', '{1}');}}catch(e){{}}
}}, {2});", ClientID, CloseEffect, HideAfter));
			  * */
			}

			base.DataBind();
		}
		protected override void Render(HtmlTextWriter writer)
		{
			if (Attributes["class"] != null)
				Attributes["class"] += " " + addClass;
			else
				Attributes["class"] = addClass;

			StringBuilder sb = new StringBuilder();
			if (tag != "")
			{
				sb.Append(string.Format("<{0} id=\"{1}\"", tag, this.ClientID));

				foreach (string attr in this.Attributes.Keys)
					sb.Append(string.Format(" {0}=\"{1}\"", attr, Attributes[attr]));

				sb.Append(">");
			}
			if (Closable)
			{
				if (String.IsNullOrEmpty(tag))
					tag = "div";

				sb.Append(string.Format("<div id=\"{0}\" class=\"close\"></div>", closeId));
			}
			sb.Append(Text);
			if(Tag != "")
				sb.Append("</" + tag + ">");

			this.Visible = this.Text.Trim() != "" || this.forceDisplay;

			this.text = sb.ToString();

			if (this.Visible)
				writer.Write(text);
		}

		public void AddClass(string className)
		{
			addClass = className;
		}

		#region Properties
		
		public HideEffect CloseEffect
		{
			get { return closeEffect; }
			set { closeEffect = value; }
		}
		public bool Closable
		{
			get { return closable; }
			set { closable = value; }
		}
		public bool AutoHide
		{
			get { return autoHide; }
			set { autoHide = value; }
		}
		public int HideAfter
		{
			get { return hideAfter; }
			set { hideAfter = value; }
		}
		public string Tag
		{
			get { return tag; }
			set { tag = value; }
		}
		public string Text
		{
			get { return text; }
			set { text = value; }
		}
		public string Format
		{
			get { return _format; }
			set { _format = value; }
		}
		public string Seperator
		{
			get { return _seperator; }
			set { _seperator = value; }
		}
		public string KeyFilter
		{
			get { return keyFilter; }
			set { keyFilter = value; }
		}
		public bool ForceDisplay
		{
			get { return forceDisplay; }
			set { forceDisplay = value; }
		}
		#endregion

		bool _editable = false;
		public bool Editable
		{
			get
			{
				CustomPage page = this.Page as CustomPage;
				this._editable = page != null ? page.Editable : false;
				return _editable;
			}
		}
	}
}