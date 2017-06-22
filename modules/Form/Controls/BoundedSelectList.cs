using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Data;

using lw.Utils;
using lw.WebTools;
using lw.Forms.Interface;
using lw.DataControls;

namespace lw.Forms.Controls
{
	/// <summary>
	/// Output an input text that bounds to any NamingContainer
	/// NamingFrom: (For Lists) The name of the ID field, it will be used to create the names and ids of the textfields.
	/// NamingFormat the format to create the names, Default: "{0}"
	/// BoundTo the input text can be bounded to any data field, default: not bound
	/// </summary>
	public class BoundedSelectList:HtmlSelect, IBoundedElement
	{
		object DataObj;
		bool _bound = false;

		/// <summary>
		/// Constructor fod Bounded Select List
		/// </summary>
		public BoundedSelectList()
		{
			DataTextField = "Name";
			DataValueField = "Value";
		}

		void bind()
		{
			if (_bound)
				return;

			if (!String.IsNullOrWhiteSpace(_boundTo))
			{

				DataObj = DataBinder.Eval(this.NamingContainer, "DataItem");
				_bound = DataObj != null;

				if (!String.IsNullOrWhiteSpace(_namingFrom))
				{
					string name = _namingFormat;
					name = String.Format(name,
						ControlUtils.GetBoundedDataField(this.NamingContainer, _namingFrom)
					);
					this.ID = name;
					this.Attributes["name"] = name;
				}
			}
			else
			{
				_bound = true;
			}

			if (!_bound)
				return;

			lw.DataControls.CustomDataSource dataSrc = null;
			
			if (!String.IsNullOrWhiteSpace(source))
			{
				Control ctrl = Page.FindControl(source);
				if (ctrl != null)
					dataSrc = ctrl as CustomDataSource;
			}

			if (dataSrc != null)
			{
				DataSource = dataSrc.Data;
			}

			base.DataBind();

			if (!String.IsNullOrWhiteSpace(_boundTo))
			{
				object obj = ControlUtils.GetBoundedDataField(this.NamingContainer, _boundTo);
				if (obj != null)
					this.Value = String.Format(_format, obj).Trim();
			}

			lw.Base.CustomPage page = this.Page as lw.Base.CustomPage;

			if (ValueFromQuery)
			{
				string val = page.GetQueryValue(this.ID);
				if (!String.IsNullOrWhiteSpace(val))
					this.Value = val;
			}

		}

		
		public override void DataBind()
		{
			bind();
		}


		#region properties
		string _namingFrom = "";
		/// <summary>
		/// NamingFrom: The name of the ID field, it will be used to create the names and ids of the checkbox.
		/// </summary>
		public string NamingFrom
		{
			get { return _namingFrom; }
			set { _namingFrom = value; }
		}

		string _namingFormat = "{0}";
		/// <summary>
		/// The name of the ID field, it will be used to create the names and ids of the checkbox.
		/// </summary>
		public string NamingFormat
		{
			get { return _namingFormat; }
			set { _namingFormat = value; }
		}

		string _boundTo;
		/// <summary>
		/// BoundTo the checkbox can be bounded to any boolean field, default: not bound
		/// </summary>
		public string BoundTo
		{
			get { return _boundTo; }
			set { _boundTo = value; }
		}


		string source;
		/// <summary>
		/// Points to the datasource location (lw.DataControls.CustomDataSource)
		/// </summary>
		public string Source
		{
			get
			{
				return source;
			}
			set
			{
				source = value;
			}
		}

		string _format = "{0}";

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


		bool valueFromQuery = true;
		/// <summary>
		/// If true the list will automatically set its value to the value coming from the page query.
		/// </summary>
		public bool ValueFromQuery
		{
			get
			{
				return valueFromQuery;
			}
			set
			{
				valueFromQuery = value;
			}
		}
		#endregion
	}
}
