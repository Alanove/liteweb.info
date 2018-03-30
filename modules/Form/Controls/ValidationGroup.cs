using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Resources;
using System.Web.UI;
using lw.Base;

namespace lw.Forms.Controls
{
	public class ValidationGroup: System.Web.UI.Control
	{

		#region functions

		/// <summary>
		/// Validates the group by running through all the Validator children
		/// </summary>
		public void Validate()
		{
			_Validated = true;
			foreach (Control ctrl in this.Controls)
			{
				Validator _validator = ctrl as Validator;
				if (_validator != null)
				{
					_validator.Validate();
					if (!_validator.Validated)
					{
						_Validated = false;

						_Messages.Add(_validator.Message);
					}
				}
			}
		}

		#endregion

		#region override

		protected override void OnInit(EventArgs e)
		{
			_CustomPage = this.Page as lw.Base.CustomPage;
			base.OnInit(e);
		}

		public override void DataBind()
		{
			
            _Form = this.Parent as Form;
			if (_Form == null)
			{
				throw new Exception("Validation Group can only be located inside a Form");
			}

			if (_ClientSideValidation)
			{
				ResourceSet resourceSet = Resources.Validation.ResourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true, true);

				Dictionary<string, string> resources = new Dictionary<string, string>();

				foreach (DictionaryEntry entry in resourceSet)
				{
					resources[entry.Key.ToString()] = entry.Value.ToString();
				}

				string scriptString = lw.Utils.StringUtils.JSonSerialize(resources);

                //_CustomPage.RegisterHeaderScript("validation-entries", "lw.validator.resources = " + scriptString + ";", true);
                _CustomPage.RegisterLoadScript("validation-entries", "lw.validator.resources = " + scriptString + ";", true);

				Dictionary<string, object> validationOptions = new Dictionary<string, object>();
				validationOptions["id"] = this.UniqueID;
				validationOptions["Form"] = _Form.ClientID;
				validationOptions["AlertErrorSummary"] = _AlertErrorSummary;
				validationOptions["SummaryMessageID"] = _SummaryMessageID;
				if (!String.IsNullOrEmpty(_SummaryMessageID))
					validationOptions["SummaryMessageClientID"] = _CustomPage.FindControlRecursive(_CustomPage, _SummaryMessageID).ClientID;
				validationOptions["NotValidClass"] = _NotValidClass;
				validationOptions["ValidClass"] = _ValidClass;
				validationOptions["Bubble"] = _Bubble;

                scriptString = lw.Utils.StringUtils.JSonSerialize(validationOptions);
                //_CustomPage.RegisterHeaderScript("validation-group-" + UniqueID, "lw.validator.groups['" + this.UniqueID + "'] = " + scriptString + ";", true);
                _CustomPage.RegisterLoadScript("validation-group-" + UniqueID, "lw.validator.groups['" + this.UniqueID + "'] = " + scriptString + ";", true);

				_CustomPage.RegisterLoadScript("val-init", "lw.validator.init();", true);

				//Filling the Validators Dictionary
				_Validators = new List<Validator>();
				foreach (Control child in this.Controls)
				{
					Validator validator = child as Validator;
					if (validator != null)
					{
						_Validators.Add(validator);
					}
				}
			}
			base.DataBind();
		}

		protected override void Render(System.Web.UI.HtmlTextWriter writer)
		{
			//base.Render(writer);
		}

		#endregion

		#region Readonly Properties

		CustomPage _CustomPage;
		/// <summary>
		/// Points to the Page containing this group
		/// </summary>
		CustomPage CustomPage
		{
			get { return _CustomPage; }
		}

		Form _Form;
		/// <summary>
		/// Points to the related form for this validation.
		/// Not defined before databind.
		/// </summary>
		public Form Form
		{
			get { return _Form; }
		}

		List<Validator> _Validators;
		/// <summary>
		/// Contains the list of validators inside this group
		/// </summary>
		public List<Validator> Validators
		{
			get { return _Validators; }
		}


		bool? _Validated = null;
		/// <summary>
		/// Returns true if the group is validated
		/// If null it means that the Group [Validate()] is not yet invoked
		/// </summary>
		public bool? Validated
		{
			get
			{
				return _Validated;
			}
		}

		List<string> _Messages = new List<string>();
		/// <summary>
		/// Container for all the validation messages
		/// </summary>
		public List<string> Messages
		{
			get
			{
				return _Messages;
			}
		}

		#endregion

		#region Attributes

		bool _AlertErrorSummary = true;
		/// <summary>
		/// Displays a modal alert of error summary.
		/// </summary>
		public bool AlertErrorSummary
		{
			get { return _AlertErrorSummary; }
			set { _AlertErrorSummary = value; }
		}

		string _SummaryMessageID;
		/// <summary>
		/// Points to where the validation messages should appear
		/// </summary>
		public string SummaryMessageID
		{
			get { return _SummaryMessageID; }
			set { _SummaryMessageID = value; }
		}

		string _NotValidClass = "val-error";
		/// <summary>
		/// This class will be added to the fields that are not validated.
		/// If set to nothing this feature will be disabled.
		/// Default value: "val-error"
		/// </summary>
		public string NotValidClass
		{
			get { return _NotValidClass; }
			set { _NotValidClass = value; }
		}

		string _ValidClass = "";
		/// <summary>
		/// This class will be added to the fields that are validated.
		/// If set to nothing this feature will be disabled.
		/// Default value: nothing
		/// </summary>
		public string ValidClass
		{
			get { return _ValidClass; }
			set { _ValidClass = value; }
		}

		bool _ClientSideValidation = true;
		/// <summary>
		/// If set to false the group will only be validated on the server
		/// </summary>
		public bool ClientSideValidation
		{
			get { return _ClientSideValidation; }
			set { _ClientSideValidation = value; }
		}


		bool _Bubble = true;
		/// <summary>
		/// Tells the form if it should display a validation buble next to each field.
		/// Default value: true
		/// </summary>
		public bool Bubble
		{
			get { return _Bubble; }
			set { _Bubble = value; }
		}
		#endregion
	}
}
