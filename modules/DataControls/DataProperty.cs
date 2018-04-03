using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using lw.Utils;
using lw.Base;
using lw.WebTools;
using lw.HashTags;

namespace lw.DataControls
{
	/// <summary>
	/// Writes a data property to the client.
	/// Works with: <see cref="DataProvider"/>, <see cref="CustomDataSource"/>
	/// </summary>
	public class DataProperty : Literal, IDataProperty
	{
		string _property, _format = "{0}";
		object _dataObj;
		bool _bound = false;
		int _maxCharacters = -1;
		string _closingSentence = "...";
		string _class = "";
		bool _iVisible = true;

		/// <summary>
		/// DataProperty Constructor
		/// </summary>
		public DataProperty()
		{
			int i = 0;
			i++;
		}

		internal void bind()
		{
			if (_bound)
				return;

			_dataObj = ControlUtils.GetBoundedDataField(NamingContainer, _property, LoadFromDraft);

			_bound = _dataObj != null;

			if (!_bound)
				return;

			if (!String.IsNullOrWhiteSpace(Class))
			{
				var obj = this as IAttributeAccessor;

				if (obj != null)
					obj.SetAttribute("class", Class);
			}



			string str = "";
			if (_dataObj != DBNull.Value && _dataObj != null && _dataObj.ToString() != "")
				str = string.Format(Format, _dataObj);
			else
				str = nullValue;

			if (MaxCharacters > 0)
			{
				str = StringUtils.Trankate(StringUtils.StripOutHtmlTags(str), _maxCharacters, _closingSentence);
			}

			if (RenderAsHTML)
			{
				str = StringUtils.AddSup(str);
				str = StringUtils.ReplaceNbSup(str);
				str = StringUtils.ReplaceItalicSabis(str);
			}
			if (!String.IsNullOrWhiteSpace(str) || Editable)
			{
				Visible = true;
			}
			else
			{
				Visible = false;
			}
			_iVisible = Visible;
			//str = WebContext.Server.HtmlEncode(str);
			if (_iVisible)
			{
				if (PutBR)
				{
					if (str != null)
					{

						Text = StringUtils.PutBR(str);
					}
				}
				else
					Text = str;
			}

		}

		public override void DataBind()
		{
			bind();
			base.DataBind();
		}

		protected override void Render(HtmlTextWriter writer)
		{
			bind();

			if (LoadFromDraft && ImEditable)
			{
				writer.Write(string.Format(@"<div class=""hidden original-version {1}"" id=""For_Edit_{0}"">
", UniqueID.Replace(":", "_"), _class + " " + _property));

				writer.Write(ControlUtils.GetBoundedDataField(NamingContainer, _property, false));

				//writer.Write(string.Format("</div>",
				//	this.UniqueID.Replace(":", "_"), ""));
				writer.Write("</div>");
			}

			if (Editable && ImEditable)
			{
				//<div class=Eheader><a href=""javascript:lw_Editor.edit('{0}', '{1}')"">Edit</a></div>					

				writer.Write(string.Format(@"<div class=""editingarea {1}"" id=""Edit_{0}"">
", UniqueID.Replace(":", "_"), _class + " " + _property));

				base.Render(writer);

				//writer.Write(string.Format("</div>",
				//	this.UniqueID.Replace(":", "_"), ""));
				writer.Write("</div>");
			}
			else
			{
				if (InjectHashTagsLinks)
					Text = HashTagsManager.InjectTagLinks(Text);

				base.Render(writer);
			}
		}

		#region Properties
		#endregion

		bool _editable;
		/// <summary>
		/// Returns if the propety can be editable inside the Inline-CMS
		/// </summary>
		public bool Editable
		{
			get
			{
				var page = Page as CustomPage;
				_editable = page != null && page.Editable;
				return _editable;
			}
		}

		bool _imEditable = false;
		/// <summary>
		/// Tells the property to be editable.
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

		/// <summary>
		/// The css property of the tag
		/// </summary>
		public string Class
		{
			get
			{
				return _class;
			}
			set
			{
				_class = value;
			}
		}

		/// <summary>
		/// Defines the Data Property to be used with this tag
		/// The proerty is usually the column name from a datatablse or any other data source.
		/// </summary>
		public string Property
		{
			get
			{
				return _property;
			}
			set
			{
				_property = value;
			}
		}

		/// <summary>
		/// Defines the format of the returning string
		/// </summary>
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

		/// <summary>
		/// 
		/// </summary>
		public bool IVisible
		{
			get
			{
				return _iVisible;
			}
		}

		bool _putBR = true;
		/// <summary>
		/// Replace new lines with <br />
		/// </summary>
		//[Obsolete("PutBR is deprecated, please use RenderAsHTML instead.")]
		public bool PutBR
		{
			get
			{
				return _putBR;
			}
			set
			{
				_putBR = value;
			}
		}

		bool _renderAsHTML = true;
		/// <summary>
		/// Renders the property as html.
		/// Replaces new lines with <![CDATA[<BR />]]> 
		/// calls: <seealso cref="StringUtils.AddSup"/> 
		/// calls: <seealso cref="ReplaceItalicSabis"/>
		/// calls: <seealso cref="ReplaceNbSup"/>
		/// </summary>
		public bool RenderAsHTML
		{
			get
			{
				return _renderAsHTML;
			}
			set
			{
				_renderAsHTML = value;
			}
		}

		bool? _loadFromDraft = null;
		/// <summary>
		/// Defines if the data property should read from the draft of the related table
		/// </summary>
		public bool LoadFromDraft
		{
			get
			{
				if (_loadFromDraft == null)
				{
					_loadFromDraft = Editable;
					if (WebContext.Request["preview"] == "true")
						_loadFromDraft = true;
					if (WebContext.Request["preview"] == "true")
						_loadFromDraft = true;
				}
				return _loadFromDraft.Value;
			}
			set
			{
				_loadFromDraft = value;
			}
		}




		bool _injectHashTagsLinks = false;
		/// <summary>
		/// Tells the property to replace all words with # into a hash tag link.
		/// </summary>
		public bool InjectHashTagsLinks
		{
			get
			{
				return _injectHashTagsLinks;
			}
			set
			{
				_injectHashTagsLinks = value;
			}
		}


		string nullValue = "";
		/// <summary>
		/// Sets the property's value when the DB's actual value is NULL
		/// </summary>
		public string NullValue
		{
			get
			{
				return nullValue;
			}
			set
			{
				nullValue = value;
			}
		}
	}
}