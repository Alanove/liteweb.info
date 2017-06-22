using System;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;

using lw.Utils;
using lw.WebTools;

namespace lw.Forms.Controls
{
	/// <summary>
	/// Creates a form Validator
	/// <example>
	/// <code>
	/// <![CDATA[
	/// <lw:Validator runat="server" For="Name" Required="true" DisplayName="Name" />
	/// <lw:Validator runat="server" For="TestRadioField" Required="true" DisplayName="Message" EmptyText="" ValidateWith="TestRadio" ValidateWithValue="yes" />
	/// ]-->
	/// </code>
	/// </example>
	/// </summary>
	public class Validator: Control
	{

		#region Validate Functions


		/// <summary>
		/// Checks if the Field is valid
		/// Runs through all validation routes...
		/// </summary>
		public void Validate()
		{
			if (ValidateWith != null)
			{
				string thisval = _ValidationGroup.Form.GetValue(_validateWithControl);
				switch (ValidateWithCondition)
				{
					case lw.Forms.CompareCondition.HaveValue:
						if (String.IsNullOrWhiteSpace(thisval))
						{
							_Validated = true;
							return;
						}
						break;
					case lw.Forms.CompareCondition.Different:
						if (thisval == ValidateWithValue)
						{
							_Validated = true;
							return;
						}
						break;
					default:
						if (Compare(_ValidateWithValue, thisval, _DataType) != CompareCondition)
						{
							_Validated = true;
							return;
						}
						break;
				}
			}

			if (_Required)
			{
				_Validated = !String.IsNullOrWhiteSpace(_Value);

				if (!_Validated)
				{
					_Message = !String.IsNullOrWhiteSpace(_CustomMessage) ?
									_CustomMessage :
									String.Format(Resources.Validation.Required, _DisplayName);
					return;
				}
			}
			else
				_Validated = true;

			if (!String.IsNullOrWhiteSpace(Value) || !String.IsNullOrWhiteSpace(_Min) || !String.IsNullOrWhiteSpace(_Max))
			{
				switch (_DataType)
				{
					case DataType.Integer:
						_Validated = Validation.IsInteger(_Value);
						if (_Validated)
							goto case DataType.Decimal;
						else
						{
							_Message = !String.IsNullOrWhiteSpace(_CustomMessage) ?
								_CustomMessage :
								String.Format(Resources.Validation.Integer, _DisplayName);
						}
						break;
					case DataType.Number:
					case DataType.Decimal:
						_Validated = Validation.IsDecimal(_Value);
						if (_Validated)
						{
							if ((!String.IsNullOrWhiteSpace(_Min) && Decimal.Parse(_Value) < Decimal.Parse(_Min))
								||
								(!String.IsNullOrWhiteSpace(_Max) && Decimal.Parse(_Value) > Decimal.Parse(_Max))
							)
							{
								_Validated = false;
								_Message = !String.IsNullOrWhiteSpace(_CustomMessage) ?
									_CustomMessage :
									String.Format(Resources.Validation.Range_Number, _DisplayName, _Min, _Max);
							}
						}
						else
						{
							_Message = !String.IsNullOrWhiteSpace(_CustomMessage) ?
								_CustomMessage :
								String.Format(Resources.Validation.Number, _DisplayName);
						}
						break;
					case DataType.Date:
						DateTime val;
						_Validated = DateTime.TryParse(_Value, out val);
						if (!_Validated)
						{
							_Message = !String.IsNullOrWhiteSpace(_CustomMessage) ?
								_CustomMessage :
								String.Format(Resources.Validation.Date, _DisplayName);
						}
						else
						{
							DateTime min, max;
							if ((DateTime.TryParse(_Min, out min) && val < min) || (DateTime.TryParse(_Max, out max) && val > max))
							{
								_Validated = false;
								_Message = !String.IsNullOrWhiteSpace(_CustomMessage) ?
									_CustomMessage :
									String.Format(Resources.Validation.Range_Date, _DisplayName, _Min, _Max);
							}
						}
						break;
					case DataType.Email:
						_Validated = Validation.IsEmail(_Value);
						if (_Validated)
							goto default;
						else
						{
							_Message = !String.IsNullOrWhiteSpace(_CustomMessage) ?
								_CustomMessage :
								String.Format(Resources.Validation.Email, _DisplayName);
						}
						break;
					case DataType.Image:
						_Validated = Validation.IsImage(_Value);
						if (_Validated)
						{
							goto case DataType.File;
						}
						else
						{
							_Message = !String.IsNullOrWhiteSpace(_CustomMessage) ?
								_CustomMessage :
								String.Format(Resources.Validation.Image, _DisplayName);
						}
						break;
					case DataType.File:
							HttpPostedFile file = WebContext.Request.Files[this.ControlToValidateClientId];
							if ((!String.IsNullOrWhiteSpace(_Min) && file.ContentLength < Int32.Parse(_Min) * 1024)
								||
								(!String.IsNullOrWhiteSpace(_Max) && file.ContentLength > Int32.Parse(_Max) * 1024)
							)
							{
								_Validated = false;
								_Message = !String.IsNullOrWhiteSpace(_CustomMessage) ?
									_CustomMessage :
									String.Format(Resources.Validation.Range_File, _DisplayName, _Min, _Max);
							}
						break;
					default:
						if ((!String.IsNullOrWhiteSpace(_Min) && _Value.Length < Int32.Parse(_Min))
								||
								(!String.IsNullOrWhiteSpace(_Max) && _Value.Length > Int32.Parse(_Max))
							)
						{
							_Validated = false;
							_Message = !String.IsNullOrWhiteSpace(_CustomMessage) ?
								_CustomMessage :
								String.Format(Resources.Validation.Range_String, _DisplayName, _Min, _Max);
						}
						break;
				}
			}
			if (!_Validated)
				return;

			if (CompareToControl != null)
			{
				if (Compare(_Value, _CompareToValue, _DataType) != CompareCondition)
				{
					_Validated = false;
					_Message = _CustomMessage;
					if(String.IsNullOrWhiteSpace(_Message))
					{
						switch (CompareCondition)
						{
							case Forms.CompareCondition.GreaterThan:
								_Message = String.Format(Resources.Validation.Compare_GreaterThan, _DisplayName, _CompareToDisplayName);
								break;
							case Forms.CompareCondition.LessThan:
								_Message = String.Format(Resources.Validation.Compare_LessThan, _DisplayName, _CompareToDisplayName);
								break;
							default:
								_Message = String.Format(Resources.Validation.Compare_Equal, _DisplayName, _CompareToDisplayName);
								break;
						}
					}
				}
			}

			//exit
		}
		
		#endregion


		#region override

		bool _bound = false;
		public override void DataBind()
		{
			if (_bound)
				return;
			_bound = true;

			_ValidationGroup = this.Parent as ValidationGroup;
			if (_ValidationGroup == null)
			{
				throw new Exception("Validator can only be located inside a Validation Group");
			}


			_ControlToValidate = _ValidationGroup.Form.FindControlRecursive(_For) as HtmlControl;

			switch (_DataType)
			{
				case DataType.File:
					if(WebContext.Request.Files[_ControlToValidate.UniqueID]!= null)
						_Value = WebContext.Request.Files[_ControlToValidate.UniqueID].FileName;
					_ControlToValidateClientId = _ControlToValidate.UniqueID;
					break;
				default:
					_Value = _ValidationGroup.Form.GetValue(_ControlToValidate);
					_ControlToValidateClientId = _ControlToValidate.ClientID;
					break;

			}

			if (_Value == _EmptyText)
			{
				_Value = "";

				_ValidationGroup.Form.SetValue(_ControlToValidate, "");
			}



			if (!String.IsNullOrWhiteSpace(_CompareTo))
			{
				_CompareToControl = _ValidationGroup.Form.FindControlRecursive(_CompareTo) as HtmlControl;
				_CompareToValue = _ValidationGroup.Form.GetValue(_CompareToControl);
				_CompareToCliendId = _CompareToControl.ClientID;
				

				//if _CompareToDisplayName is null find the displayname from other validators within the same group
				if (String.IsNullOrWhiteSpace(_CompareToDisplayName))
				{
					var q = ValidationGroup.Validators.Find(c => c.For == _CompareToControl.ID);
					if (q != null)
						_CompareToDisplayName = q.DisplayName;
					else
						_CompareToDisplayName = _CompareTo;
				}
			}

			if (!String.IsNullOrWhiteSpace(_ValidateWith))
			{
				_validateWithControl = _ValidationGroup.Form.FindControlRecursive(_ValidateWith) as HtmlControl;
			}


			if (_ValidationGroup.Form.IsPostBack)
			{
				Validate();
			}

			// The below are definitions for client side validation
			if (_ClientSideValidation)
			{
				//if (_Required)
				//{
					_ControlToValidate.Attributes["class"] = _ControlToValidate.Attributes["class"] + " lw-validate";
				//}

				StringBuilder validationQuery = new StringBuilder();

				validationQuery.Append(string.Format("DataType={0}&", _DataType));
				validationQuery.Append(string.Format("Required={0}&", Required));
				validationQuery.Append(string.Format("Min={0}&", _Min));
				validationQuery.Append(string.Format("Max={0}&", _Max));
				validationQuery.Append(string.Format("CompareTo={0}&", _CompareToCliendId));
				validationQuery.Append(string.Format("CompareCondition={0}&", EnumHelper.GetDescription(_CompareCondition)));
				validationQuery.Append(string.Format("ValidateExpression={0}&", _ValidateExpression));
				validationQuery.Append(string.Format("CustomMessage={0}&", _CustomMessage));
				validationQuery.Append(string.Format("DisplayName={0}&", _DisplayName));
				validationQuery.Append(string.Format("Group={0}&", ValidationGroup.UniqueID));
				validationQuery.Append(string.Format("CompareToDisplayName={0}&", _CompareToDisplayName));
				validationQuery.Append(string.Format("EmptyText={0}&", _EmptyText));
				validationQuery.Append(string.Format("EmptyClass={0}&", _EmptyClass));
				if (_validateWithControl != null)
				{
					validationQuery.Append(string.Format("ValidateWith={0}&", _validateWithControl.ClientID));
					validationQuery.Append(string.Format("ValidateWithValue={0}&", ValidateWithValue));
					validationQuery.Append(string.Format("ValidateWithCondition={0}", EnumHelper.GetDescription(ValidateWithCondition)));
				}

				_ControlToValidate.Attributes["data-lw-validate"] = validationQuery.ToString();


				if (!String.IsNullOrWhiteSpace(_EmptyText))
				{
					_ControlToValidate.Attributes["placeholder"] = _EmptyText;
				}
			}

			

			base.DataBind();
		} 

		#endregion

		#region Readonly Properties

		string _Message = "";
		/// <summary>
		/// Contains the validation message.
		/// </summary>
		public string Message
		{
			get { return _Message; }
		}

		ValidationGroup _ValidationGroup;
		/// <summary>
		/// Points to the related Validation Group.
		/// </summary>
		public ValidationGroup ValidationGroup
		{
			get { return _ValidationGroup; }
		}

		string _Value = null;
		/// <summary>
		/// The value of the associated field.
		/// </summary>
		public string Value
		{
			get { return _Value; }
		}

		string _CompareToValue;
		/// <summary>
		/// The value of the associated field.
		/// </summary>
		public string CompareToValue
		{
			get { return _CompareToValue; }
		}

		string _ControlToValidateClientId;
		/// <summary>
		/// Returns the Validated Field Client Id
		/// </summary>
		string ControlToValidateClientId
		{
			get { return _ControlToValidateClientId; }
		}

		string _CompareToCliendId;
		/// <summary>
		/// Returns the Compare To Field Client Id
		/// </summary>
		string CompareToCliendId
		{
			get { return _CompareToCliendId; }
		}

		HtmlControl _ControlToValidate;
		/// <summary>
		/// Points to the validated control object
		/// </summary>
		public HtmlControl ControlToValidate
		{
			get { return _ControlToValidate; }
		}

		HtmlControl _CompareToControl;
		/// <summary>
		/// Points to the compared control
		/// </summary>
		public HtmlControl CompareToControl
		{
			get { return _CompareToControl; }
		}

		bool _Validated = false;
		/// <summary>
		/// Returns true if validated, false is not
		/// This value is calculated after calling the Validate() Function.
		/// </summary>
		public bool Validated
		{
			get { return _Validated; }
		}
		#endregion

		#region Properties and Attributes

		bool _Required = true;
		/// <summary>
		/// Checks if the field is mandatory
		/// Default value for required is true
		/// </summary>
		public bool Required
		{
			get
			{
				return _Required;
			}
			set
			{
				_Required = value;
			}
		}

		string _For;
		/// <summary>
		/// Points to the first control to be validated
		/// </summary>
		public string For
		{
			get { return _For; }
			set { _For = value; }
		}

		DataType _DataType = DataType.String;
		/// <summary>
		/// Points to the correct datatype
		/// </summary>
		public DataType DataType
		{
			get { return _DataType; }
			set { _DataType = value; }
		}

		string _Min = null;
		/// <summary>
		/// Sets the minimum value of a field depending on datatype.
		/// ex: for string it's the length of the string, for number min will check the number
		/// This value will be converted to a number or date depending on datatype
		/// </summary>
		public string Min
		{
			get { return _Min; }
			set { _Min = value; }
		}

		string _Max = null;
		/// <summary>
		/// Sets the minimum value of a field depending on datatype.
		/// ex: for string it's the length of the string, for number min will check the number
		/// This value will be converted to a number or date depending on datatype
		/// </summary>
		public string Max
		{
			get { return _Max; }
			set { _Max = value; }
		}

		string _CompareTo;
		/// <summary>
		/// If we want to compare between 2 fields we should set the value in compare to
		/// </summary>
		public string CompareTo
		{
			get { return _CompareTo; }
			set { _CompareTo = value; }
		}

		CompareCondition? _CompareCondition = lw.Forms.CompareCondition.Equal;
		/// <summary>
		/// Sets the compare condition (equal, greater or less than)
		/// </summary>
		public CompareCondition? CompareCondition
		{
			get { return _CompareCondition; }
			set { _CompareCondition = value; }
		}

		string _ValidateExpression;
		/// <summary>
		/// Sets the validation regular expression
		/// </summary>
		public string ValidateExpression
		{
			get { return _ValidateExpression; }
			set { _ValidateExpression = value; }
		}

		string _CustomMessage;
		/// <summary>
		/// Overrrides any auto message associated with this field.
		/// for ex if the field is required this messasge will show instead of the generic required message.
		/// </summary>
		public string CustomMessage
		{
			get { return _CustomMessage; }
			set { _CustomMessage = value; }
		}

		bool _ClientSideValidation = true;
		/// <summary>
		/// If set to true this field will not be included in the client side validation
		/// </summary>
		public bool ClientSideValidation
		{
			get { return _ClientSideValidation; }
			set { _ClientSideValidation = value; }
		}

		string _DisplayName;
		/// <summary>
		/// The Display Name of the validated field.
		/// </summary>
		public string DisplayName
		{
			get { return _DisplayName; }
			set { _DisplayName = value; }
		}

		string _CompareToDisplayName;
		/// <summary>
		/// This display name of the compare to field
		/// If not set, will try to automatically fetch it from other validators, if still not found the id will be used
		/// </summary>
		public string CompareToDisplayName
		{
			get { return _CompareToDisplayName; }
			set { _CompareToDisplayName = value; }
		}

		string _EmptyText;
		/// <summary>
		/// This text will be the value of the field if empty
		/// </summary>
		public string EmptyText
		{
			get { return _EmptyText; }
			set { _EmptyText = value; }
		}

		string _EmptyClass = "lw-empty";
		/// <summary>
		/// This CSS class will be added when the field is empty
		/// </summary>
		public string EmptyClass
		{
			get { return _EmptyClass; }
			set { _EmptyClass = value; }
		}


		string _ValidateWith;
		/// <summary>
		/// Defines if this validation group should be evaluated when the ValidateWith field have a certain condition.
		/// </summary>
		public string ValidateWith
		{
			get
			{
				return _ValidateWith;
			}
			set
			{
				_ValidateWith = value;
			}
		}

		HtmlControl _validateWithControl;
		/// <summary>
		/// Returns the control representing the validate with
		/// </summary>
		public HtmlControl ValidateWithControl
		{
			get
			{
				return _validateWithControl;
			}
		}

		lw.Forms.CompareCondition _ValidateWithCondition = lw.Forms.CompareCondition.HaveValue;
		/// <summary>
		/// Defines the condition on which the ValidateWith should be evaluated. Default: HaveValue
		/// </summary>
		public lw.Forms.CompareCondition ValidateWithCondition
		{
			get
			{
				return _ValidateWithCondition;
			}
			set
			{
				_ValidateWithCondition = value;
			}
		}

		string _ValidateWithValue;
		/// <summary>
		/// Defines the value of the related field on which the field must be validated, again depending on ValidateWithCondition Value.
		/// If this field is left empty, the validation will just look if the field have a value or not.
		/// </summary>
		public string ValidateWithValue
		{
			get
			{
				return _ValidateWithValue;
			}
			set
			{
				_ValidateWithValue = value;
			}
		}




		#endregion


		#region Static

		/// <summary>
		/// Compares 2 objects the type of the objects must be defined or the compare will consider them as a String
		/// </summary>
		/// <param name="obj1">Object 1</param>
		/// <param name="obj2">Object 2</param>
		/// <param name="type">the DataType<seealso cref="Forms.DataType"/></param>
		/// <returns>The Condition <seealso cref="lw.Forms.CompareCondiction"/></returns>
		public static Forms.CompareCondition Compare(string obj1, string obj2, DataType type)
		{
			switch (type)
			{
				case DataType.Number:
				case DataType.Decimal:
				case DataType.Integer:
					double val1 = double.Parse(obj1);
					double val2 = double.Parse(obj2);
					if (val1 > val2)
						return Forms.CompareCondition.GreaterThan;
					if (val1 < val2)
						return Forms.CompareCondition.LessThan;
					return Forms.CompareCondition.Equal;
				case DataType.Date:
					DateTime date1 = DateTime.Parse(obj1);
					DateTime date2 = DateTime.Parse(obj2);
					if (date1 > date2)
						return Forms.CompareCondition.GreaterThan;
					if (date1 < date2)
						return Forms.CompareCondition.LessThan;
					return Forms.CompareCondition.Equal;
				case DataType.String:
				case DataType.HTMLSelect:
					if (string.Compare(obj1, obj2) > 0)
						return Forms.CompareCondition.GreaterThan;
					if (string.Compare(obj1, obj2) < 0)
						return Forms.CompareCondition.LessThan;
					return Forms.CompareCondition.Equal;
			default:
				if (obj1.Length > obj2.Length)
					return Forms.CompareCondition.GreaterThan;
				if (obj1.Length > obj2.Length)
					return Forms.CompareCondition.LessThan;
				return Forms.CompareCondition.Equal;
			}
		}
		#endregion
	}
}
