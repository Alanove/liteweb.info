using System;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using lw.CTE;
using lw.WebTools;
using lw.Base;
using lw.DataControls;
using lw.Utils;

namespace lw.HashTags.Controls
{

	/// <summary>
	/// Created a link to the associated tag
	/// Must be included inside a DataContainer having HashTagsDataSource
	/// </summary>
	[PersistChildren(false)]
	public class HashTagLink : HtmlAnchor
	{
		#region internal variables
		bool _bound = false;
		string _format = "{0}";
		string _path = "";

		string activeClass = "";

		#endregion

		public override void DataBind()
		{
			if (this._bound)
				return;
			_bound = true;

			this.Title = Title;
			if (!HashStart)
				this.HRef = lw.WebTools.WebUtils.GetFromWebConfig(lw.CTE.parameters.HashTagsAbsPath) + "/" + this.Title;
			else this.HRef = "#" + this.Title;

			if(!String.IsNullOrWhiteSpace(Path))
				this.HRef = Path + "/" + this.HRef;

			this.HRef = WebContext.Root + this.HRef;

			if (this.Controls.Count == 0)
				this.InnerText = string.Format(Format, this.Title);

			base.DataBind();

			this.Attributes["class"] = this.Attributes["class"];
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

		

		#region Properties

		bool _hashStart = false;
		/// <summary>
		/// allows the hashlink to start with a hash instead of the actual link
		/// <example>#example instead of ~/example-path/example</example>
		/// </summary>
		public bool HashStart
		{
			get
			{
				return _hashStart;
			}
			set
			{
				_hashStart = value;
			}
		}
		
		string _title = null;
		public string Title
		{
			get
			{
				if (_title == null)
					_title = ControlUtils.GetBoundedDataField(this.NamingContainer, "Tag").ToString();
				return _title;
			}
			set
			{
				_title = value;
			}
		}




		/// <summary>
		/// Text format inside the tag
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
		/// static string to be added before the generated dynamic link
		/// </summary>
		public string Path
		{
			get { return _path; }
			set { _path = value; }
		}


		/// <summary>
		/// this class is added to the tag if the news item is currently displayed in the same page.
		/// </summary>
		public string ActiveClass
		{
			get { return activeClass; }
			set { activeClass = value; }
		}

		#endregion

	}
}
