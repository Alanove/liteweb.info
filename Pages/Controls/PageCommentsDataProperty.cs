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

namespace lw.Pages.Controls
{
	/// <summary>
	/// Writes a data property to the client.
	/// Works with: <see cref="DataProvider"/>, <see cref="CustomDataSource"/>
	/// </summary>
	public class PageCommentsDataProperty : Literal
	{
		string _property, _format = "{0}";
		object _dataObj;
		bool _bound = false;
		int _maxCharacters = -1;
		string _closingSentence = "...";
		string _class = "";
		bool _iVisible = true;
		bool _putBr = true;

		/// <summary>
		/// DataProperty Constructor
		/// </summary>
		public PageCommentsDataProperty()
		{
			int i = 0;
			i++;
		}

		internal void bind()
		{
			if (_bound)
				return;

			PagesCommentsDataItem pagesDataItem = null;

			if (String.IsNullOrWhiteSpace(SourceId))
			{
				Control parent = this.Parent;
				while (parent != null)
				{
					pagesDataItem = parent as PagesCommentsDataItem;
					if (pagesDataItem != null)
						break;
					parent = parent.Parent;
				}
			}
			var page = Page as CustomPage;
			DataTable properties = null;
			if (pagesDataItem == null)
			{
				properties = page.PageContext[cte.PageProperties + "-" + SourceId] as DataTable;
			}
			else
			{
				properties = pagesDataItem.PageProperties;
			}

			if (properties != null)
			{
				try
				{ 
					int pageId = (int)ControlUtils.GetBoundedDataField(NamingContainer, "PageId", false);

					var dv = new DataView(properties, "PageId=" + pageId.ToString() + " and DataPropertyName='" + StringUtils.SQLEncode(Property) + "'", "", DataViewRowState.CurrentRows);
					if (dv.Count > 0)
						_dataObj = dv[0]["DataPropertyValue"];
				}
				catch
				{

				}
			}
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

			if (MaxCharacters > 0)
			{
				str = StringUtils.Trankate(StringUtils.StripOutHtmlTags(str), _maxCharacters, _closingSentence);
			}

			str = StringUtils.AddSup(str);
			str = StringUtils.ReplaceNbSup(str);
			str = StringUtils.ReplaceItalicSabis(str);

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
				if (_putBr)
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

			if (false && Editable && ImEditable)
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
		/// <summary>
		/// Replace new lines with <br />
		/// </summary>
		public bool PutBR
		{
			get
			{
				return _putBr;
			}
			set
			{
				_putBr = value;
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

		string _sourceId = "";
		public string SourceId
		{
			get
			{
				return _sourceId;
			}
			set
			{
				_sourceId = value;
			}
		}
	}
}