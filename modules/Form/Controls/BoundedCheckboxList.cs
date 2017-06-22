using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Data;

using lw.Utils;
using lw.WebTools;
using lw.Forms.Interface;
using lw.DataControls;

namespace lw.Forms.Controls
{
	/// <summary>
	/// Output a checkbox list that bounds to any NamingContainer
	/// NamingFrom: The name of the ID field, it will be used to create the names and ids of the checkbox.
	/// NamingFormat the format to create the names, Default: "{0}"
	/// BoundTo the checkbox can be bounded to any boolean field, default: not bound
	/// </summary>
	public class BoundedCheckboxList: CheckBoxList, IBoundedElement 
	{
		object DataObj;
		bool _bound = false;
		object _boundToValues = null;
		DataType _dataType = DataType.Array;

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

			if (!String.IsNullOrWhiteSpace(_boundTo))
			{
				object obj = ControlUtils.GetBoundedDataField(this.NamingContainer, _boundTo);
				if (obj != null)
				{
					_boundToValues = obj;
				}
			}

			base.DataBind();
		}

		protected override void Render(HtmlTextWriter writer)
		{
			if (_boundToValues != null)
			{
				var len = this.Items.Count;
				switch (this.DataType)
				{
					case Forms.DataType.Array:
						string[] valuesArray = _boundToValues.ToString().Split(valueSeperator);
						for (int i = 0; i < len; i++)
						{
							this.Items[i].Selected = Array.IndexOf(valuesArray, this.Items[i].Value) >= 0;
						}
						break;
					case Forms.DataType.BitwiseNumber:
						Int32 bitwiseValue = Int32.Parse(_boundToValues.ToString());
						for (int i = 0; i < len; i++)
						{
							this.Items[i].Selected = (bitwiseValue & Int32.Parse(this.Items[i].Value)) != 0;
						}
						break;
				}
				
			}

			base.Render(writer);
		}

		public override void DataBind()
		{
			bind();
			base.DataBind();
		}

		/// <summary>
		/// Returns the selected values of this control separated by ","
		/// </summary>
		/// <returns>SelectedValues</returns>
		public string GetSelectedValues()
		{
			return GetSelectedValues(valueSeperator);
		}
		/// <summary>
		/// Returns the selected values of this control separated by Seperator
		/// </summary>
		/// <param name="Seperator">Seperator default: ","</param>
		/// <returns>SelectedValues</returns>
		public string GetSelectedValues(char Seperator)
		{
			var len = this.Items.Count;
			string ret = "";
			string sep = "";


			for (int i = 0; i < len; i++)
			{
				string key = this.UniqueID + this.IdSeparator + this.ID + "_" + i.ToString();
				this.Items[i].Selected = WebContext.Request[key] == "on";
				if (this.Items[i].Selected)
				{
					ret += sep + this.Items[i].Value;
					sep = Seperator.ToString();
				}
			}

			return ret;
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

		char valueSeperator = ',';
		public char ValueSeperator
		{
			get
			{
				return valueSeperator;
			}
			set
			{
				valueSeperator = value;
			}
		}

		/// <summary>
		/// Sets the data type for this element
		/// Array: string array seperated by ValueSeperator
		/// BitwiseNumber: Any number that uses the bitwise operation
		/// </summary>
		public DataType DataType
		{
			get
			{
				return _dataType;
			}
			set
			{
				_dataType = value;
			}
		}

		#endregion
	}
}
