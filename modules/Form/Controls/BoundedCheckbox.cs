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
	/// Output a checkbox that bounds to any NamingContainer
	/// NamingFrom: The name of the ID field, it will be used to create the names and ids of the checkbox.
	/// NamingFormat the format to create the names, Default: "{0}"
	/// BoundTo the checkbox can be bounded to any boolean field, default: not bound
	/// </summary>
	public class BoundedCheckbox:HtmlInputCheckBox, IBoundedElement
	{
		object DataObj;
		bool _bound = false;


		void bind()
		{
			if (_bound)
				return;

			DataObj = DataBinder.Eval(this.NamingContainer, "DataItem");
			_bound = DataObj != null;

			if (!_bound)
				return;

			string name = _namingFormat;

			if (!String.IsNullOrWhiteSpace(_namingFrom))
			{
				name = String.Format(name,
					ControlUtils.GetBoundedDataField(this.NamingContainer, _namingFrom)
				);
				this.Value = ControlUtils.GetBoundedDataField(this.NamingContainer, _namingFrom).ToString();
				this.ID = name;
				this.Attributes["name"] = name;
			}

			if (!String.IsNullOrWhiteSpace(_boundTo))
			{
				try
				{
					object obj = ControlUtils.GetBoundedDataField(this.NamingContainer, _boundTo);
					if (obj.GetType() == true.GetType())
						this.Checked = (bool)obj;
					else
					{
						this.Checked = float.Parse(obj.ToString()) > 0;
					}
				}
				catch(Exception ex)
				{
					ErrorContext.Add("BoundedCheckbox: " + _boundTo + " " + _namingFrom, "Can only be bounded to a boolean field");
				}
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

		#endregion
	}
}
