using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Data;

using lw.Utils;
using lw.WebTools;
using lw.Forms.Interface;

namespace lw.Forms.Controls
{
	/// <summary>
	/// Output an input text that bounds to any NamingContainer
	/// NamingFrom: (For Lists) The name of the ID field, it will be used to create the names and ids of the textfields.
	/// NamingFormat the format to create the names, Default: "{0}"
	/// BoundTo the input text can be bounded to any data field, default: not bound
	/// </summary>
	public class BoundedTextField : HtmlInputText, IBoundedElement
	{
		object DataObj;
		bool _bound = false;
		string _format = "{0}";
		bool _readonly = false;


		void bind()
		{
			if (_bound)
				return;

			DataObj = DataBinder.Eval(this.NamingContainer, "DataItem");
			_bound = DataObj != null;

			if (!_bound)
				return;

			

			if (!String.IsNullOrWhiteSpace(_namingFrom))
			{
				string name = _namingFormat;
				name = String.Format(name,
					ControlUtils.GetBoundedDataField(this.NamingContainer, _namingFrom)
				);
				this.ID = name;
				this.Attributes["name"] = name;
			}

			if (!String.IsNullOrWhiteSpace(_boundTo))
			{
				object obj = ControlUtils.GetBoundedDataField(this.NamingContainer, _boundTo);
				if (obj != null)
					this.Value = String.Format(_format, obj);
			}

			if (_readonly)
			{
				this.Attributes["readonly"] = "true";
			}
		}


		public override void DataBind()
		{
			bind();
			base.DataBind();
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


		public bool ReadOnly
		{
			get
			{
				return _readonly;
			}
			set
			{
				_readonly = value;
			}
		}

		#endregion
	}
}
