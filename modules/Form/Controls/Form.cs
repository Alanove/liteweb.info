using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using lw.Utils;
using lw.WebTools;
using System.Linq;
using System.Web;
using System.IO;
using lw.Forms.Classes;
using lw.Forms.Data;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using lw.CTE;


namespace lw.Forms.Controls
{
    /// <summary>
    /// Creates an lw.Form element
    /// </summary>
    /// <example>
    /// Javascript for Ajax Callback:
    /// <code>
    /// function ContactUs_Callback(e) {
    ///	if (!e.valid) {
    ///		lw.alert(lw.validator.getHtmlMessage(e.data), null, "Ok");
    ///		return;
    ///	}
    /// 
    ///	if (isOk(e.error)) {
    ///		lw.alert(e.error, null, "Ok");
    ///		return;
    ///	}
    ///	if (e.data == "success") {
    ///		lw.alert("Your form was successfully sent.");
    ///		$("#_Form").each(function () {
    ///			this.reset();
    ///		});
    ///	}
    ///}
    /// </code>
    /// HTML Tag:
    /// <code>
    /// <![CDATA[
    /// <lw:Form ID="_Form" class="Form" runat="server" AjaxCallback="ContactUs_Callback" AjaxSubmit="true">
    ///		<lw:ValidationGroup runat="server" Bubble="false" />
    ///			<lw:Validator runat="server" For="Name" Required="true" DisplayName="Name" EmptyText="" />
    ///		</lw:ValidationGroup>
    ///		<lw:BoundedTextField BoundTo="Name" NamingFrom="MemberId" Id="Name" runat="server" />
    ///	</lw:Form>]]></code>
    /// 
    /// Server Side Code:
    /// <code>AjaxResponse resp = new AjaxResponse();
    /// if (_Form.IsPostBack)
    /// {
    ///    resp.callBack = _Form.AjaxCallback;
    ///    _Form.Validate();
    ///    resp.valid = _Form.IsValid.Value;
    ///    if (resp.valid)
    ///    {
    ///         lw.Network.Mail m = new lw.Network.Mail("Contact Us Form");
    ///           m.Data = _Form.GetValues();
    ///
    ///         Config cfg = new Config();
    ///
    ///            m.Send();
    ///           resp.data = "success";
    ///       }
    ///       else
    ///            {
    ///             resp.data = _Form.Messages;
    ///      }
    ///   resp.WriteJson();
    ///}</code>
    /// </example>
    /// <remarks>
    /// When used with <see cref="ValidationGroup"/> an ID must be added.
    /// </remarks>
    [System.ComponentModel.DefaultProperty("method"),
        ToolboxData("<{0}:Form runat=server></{0}:Form>")]
    [ParseChildren(ChildrenAsProperties = false)]
    public class Form : WebControl
    {
        FormMethod _method = FormMethod.Post;
        FormEncType _encType = FormEncType.None;
        bool _isPostBack;
        string _validationGroup = "";
        bool _valuesSet = false;
        bool? _validated;
        string _namePart = "";
        bool _validateServerRequest = true;
        string _dataType = "json";

        bool _dataForm = false;
        string formType = "";
        DataFormsManager _formsMgr = new DataFormsManager();

        FormRequest _request = null;


        /// <summary>
        /// Constructor for Form element
        /// </summary>
        public Form()
            : base("form")
        {
            this.Attributes["action"] = WebContext.Request.RawUrl.Split('?')[0];
            if (WebContext.Request.QueryString.Count > 0)
                this.Attributes["action"] += "?" + WebContext.Request.QueryString.ToString();

            string[] parts = WebContext.Request.RawUrl.Split('#');

            if (parts.Length > 1)
                this.Attributes["action"] += "#" + parts[1];


            ClientIDMode = System.Web.UI.ClientIDMode.Static;
        }

        #region Override
        protected override void OnInit(EventArgs e)
        {
            if (!ValidateServerRequest || WebTools.Security.IsValidRequest())
            {
				_namePart = Request.Get(this.ID);
                _isPostBack = !String.IsNullOrWhiteSpace(_namePart) && _namePart == this.UniqueID;

                if (!String.IsNullOrWhiteSpace(_namePart) && !_isPostBack && this.Attributes["Action"] != WebContext.Request.RawUrl.Split('?')[0])
                {
                    string[] temp = _namePart.Split(':');
                    _isPostBack = !String.IsNullOrWhiteSpace(_namePart) && temp[temp.Length - 1] == this.ID;
                }
            }
            base.OnInit(e);
        }



        public override void RenderBeginTag(HtmlTextWriter writer)
        {
            base.RenderBeginTag(writer);
            string clientId = this.UniqueID.Replace(this.ID, "");
            if (String.IsNullOrEmpty(clientId))
                clientId = this.ClientID;

            writer.Write(string.Format("<input type=\"hidden\" name=\"{0}\" value=\"{1}\" />", this.ID, this.UniqueID));
        }


        private bool _bound = false;
        public override void DataBind()
        {
            if (_bound)
                return;
            _bound = true;

            lw.Base.CustomPage page = this.Page as lw.Base.CustomPage;

            string action = Attributes["action"];
            if (action != "")
            {
                try
                {
                    action = Page.ResolveClientUrl(action);
                    Attributes["action"] = action;
                }
                catch
                {

                }
            }
            if (!_valuesSet)
            {
                this.SetValues(this);
                _valuesSet = true;
            }


            if (_ajaxSubmit)
            {
                page.RegisterLoadScript("init-forms", "lw.ajaxForms.init();");
            }

            base.DataBind();
        }
        protected override void OnLoad(EventArgs e)
        {
            this.Attributes["method"] = Method.ToString();
            if (EncType != FormEncType.None)
                this.Attributes["enctype"] = EnumHelper.GetDescription(EncType);
            this.Attributes["name"] = this.UniqueID;
            //	this.Attributes["id"] = this.UniqueID.Replace(":", "_");

            if (this.Attributes["onsubmit"] != null)
                this.Attributes.Add("onsubmit", this.Attributes["onsubmit"]);

            if (_ajaxSubmit)
            {
                this.Attributes.Add("ajax", "true");
                this.Attributes.Add("LoaderArea", _loaderArea);
                this.Attributes.Add("AjaxCallback", AjaxCallback);
                this.Attributes.Add("DataType", DataType);
            }

            this.Attributes["action"] = this.Attributes["action"].Replace("/{root}", WebContext.Root);

            base.OnLoad(e);
        }
        void SetValues(Control ParentControl)
        {
            if (ParentControl.Controls.Count == 0)
                return;

            foreach (Control ctrl in ParentControl.Controls)
            {
                SetValues(ctrl);
                HtmlControl _ctrl = ctrl as HtmlControl;
                if (_ctrl == null)
                    continue;

                if ((ctrl as HtmlInputFile) != null)
                {
                    EncType = FormEncType.Data;
                    continue;
                }

                if (IsPostBack)
                    SetValue(_ctrl, GetValue(_ctrl));
            }
        }



        #endregion
        #region Methods
        /// <summary>
        /// Represents the request of this form
        /// </summary>
        public FormRequest Request
        {
            get
            {
                if (_request == null)
                {
                    _request = new FormRequest();
                    if (this.Method == FormMethod.Get)
                    {
                        foreach (string key in WebContext.Request.QueryString.Keys)
                        {
                            string[] temp = key.Split(this.IdSeparator);
                            _request.Add(temp[temp.Length - 1], WebContext.Request.QueryString[key]);
                        }
                    }
                    else
                    {
                        foreach (string key in WebContext.Request.Form.Keys)
                        {
                            string[] temp = key.Split(this.IdSeparator);
                            _request.Add(temp[temp.Length - 1], WebContext.Request.Form[key]);
                        }
                    }
                }
                return _request;
            }
            set
            {
                _request = value;
            }
        }



        /// <summary>
        /// Re-set the post back value of the control
        /// Used when the form is submitted using normal browser submit to re-fill the fields.
        /// </summary>
        /// <param name="ctrl"></param>
        public void SetPostBackValue(HtmlControl ctrl)
        {
            SetValue(ctrl, this.GetValue(ctrl));
        }
        public void SetPostBackValue(HtmlControl ctrl, string v)
        {
            if (!_isPostBack)
                SetValue(ctrl, v);
            else
                SetValue(ctrl, this.GetValue(ctrl));
        }
        public void SetValue(HtmlControl ctrl, string v)
        {
            switch (ctrl.GetType().Name)
            {
                case "HtmlInputHidden":
                case "HtmlInputText":
                case "HtmlInputPassword":
                case "BoundedHiddenElement":
                case "BoundedTextField":
                case "HtmlInputGenericControl":
                    HtmlInputControl _input = ctrl as HtmlInputControl;
                    _input.Value = v;
                    break;
                case "HtmlTextArea":
                case "BoundedRichTextEditor":
                case "BoundedTextArea":
                    HtmlTextArea textArea = ctrl as HtmlTextArea;
                    textArea.Value = v;
                    break;
                case "HtmlSelect":
                case "BoundedSelectList":
                    HtmlSelect select = ctrl as HtmlSelect;
                    select.Value = v;
                    break;
                case "HtmlInputCheckBox":
                case "BoundedCheckbox":
                    HtmlInputCheckBox checkbox = ctrl as HtmlInputCheckBox;
                    checkbox.Checked = !string.IsNullOrWhiteSpace(v) && (checkbox.Value == v || v == "on");
                    break;
                case "HtmlInputRadioButton":
                    HtmlInputRadioButton radio = ctrl as HtmlInputRadioButton;
                    Regex r = new Regex(":[a-z_0-9]*$", RegexOptions.IgnoreCase);
                    v = Request.Get(radio.Name);
                    radio.Checked = radio.Value == v;
                    break;
            }
        }

        /// <summary>
        /// Will get the value of any HtmlControl within the form
        /// </summary>
        /// <param name="ctrl">Any HtmlControl <seealso cref="HtmlControl"/></param>
        /// <returns></returns>
        public string GetValue(HtmlControl ctrl)
        {
            switch (ctrl.GetType().Name)
            {
                case "HtmlInputHidden":
                case "HtmlInputText":
                case "HtmlInputPassword":
                case "BoundedHiddenElement":
                case "BoundedTextField":
                case "HtmlInputGenericControl":
                    HtmlInputControl _input = ctrl as HtmlInputControl;
                    return Request.Get(_input.ID);
                case "HtmlTextArea":
                case "BoundedRichTextEditor":
                case "BoundedTextArea":
                    HtmlTextArea textArea = ctrl as HtmlTextArea;
                    return Request.Get(textArea.ID);
                case "HtmlSelect":
                case "BoundedSelectList":
                    HtmlSelect select = ctrl as HtmlSelect;
                    return Request.Get(select.ID);
                case "HtmlInputCheckBox":
                case "BoundedCheckbox":
                    HtmlInputCheckBox checkbox = ctrl as HtmlInputCheckBox;
                    return !String.IsNullOrEmpty(Request.Get(checkbox.ID)) ?
                        Request.Get(checkbox.ID) :
                        "";
                case "HtmlInputRadioButton":
                    HtmlInputRadioButton radio = ctrl as HtmlInputRadioButton;
                    return Request.Get(radio.Attributes["name"]);
                default:
                    return "";
            }
        }

        /// <summary>
        /// NameValueCollection pairs representing all the form element ids with their values
        /// ex: if the form contains a Name control ret["Name"] will return the value of this control
        /// </summary>
        /// <returns></returns>
        public NameValueCollection GetValues()
        {
            NameValueCollection ret = new NameValueCollection();
            GetAllValues(ret, this);
            return ret;
        }

        /// <summary>
        /// Clear form, Sets all values to ""
        /// </summary>
        public void Clear()
        {
            Clear(this);
        }

        /// <summary>
        /// Clear ctrl, Sets all values to ""
        /// </summary>
        public void Clear(Control ctrl)
        {
            if (ctrl.Controls.Count == 0)
                return;
            foreach (Control _ctrl in ctrl.Controls)
            {
                Clear(_ctrl);
                HtmlControl __ctrl = _ctrl as HtmlControl;
                if (__ctrl == null || __ctrl.ID == null)
                    continue;

                SetValue(__ctrl, "");
            }
        }

        /// <summary>
        /// Internal: recursive through all form elements to get all the values and will set them
        /// in the NamevalueCollection ret
        /// </summary>
        /// <param name="ret"><seealso cref="NamevalueCollection"/></param>
        /// <param name="ParentControl"></param>
        void GetAllValues(NameValueCollection ret, Control ParentControl)
        {
            if (ParentControl.Controls.Count == 0)
                return;
            foreach (Control ctrl in ParentControl.Controls)
            {
                GetAllValues(ret, ctrl);
                HtmlControl _ctrl = ctrl as HtmlControl;
                if (_ctrl == null || _ctrl.ID == null)
                    continue;

                string v = GetValue(_ctrl);
                if (v != null)
                    ret.Add(_ctrl.ID, v);
            }
        }

        /// <summary>
        /// Returns a NameValueCollection using a given Dictionary<string,string> 
        /// </summary>
        /// <param name="ret"></param>
        /// <param name="ParentControl"></param>
        public NameValueCollection GetAllValuesInDictionnary(Dictionary<string, string> dic)
        {
            NameValueCollection ret = new NameValueCollection();
            foreach (var kvp in dic)
            {
                string value = null;
                if (kvp.Value != null)
                    value = kvp.Value.ToString();

                ret.Add(kvp.Key.ToString(), value);
            }
            return ret;
        }


        /// <summary>
        /// Sets back all form values after PostBack
        /// </summary>
        /// <param name="ret"></param>
        /// <param name="ParentControl"></param>
        public void SetAllValues(NameValueCollection ret, Control ParentControl)
        {
            if (ParentControl.Controls.Count == 0)
                return;
            foreach (Control ctrl in ParentControl.Controls)
            {
                SetAllValues(ret, ctrl);
                HtmlControl _ctrl = ctrl as HtmlControl;
                if (_ctrl == null || _ctrl.ID == null)
                    continue;

                SetValue(_ctrl, GetValue(_ctrl));
            }
        }

        /// <summary>
        /// Saves the form in the visitor's profile.
        /// Must be called after post back (Ajax or normal)
        /// </summary>
		public void Save()
		{

			var formData = JsonConvert.SerializeObject(
							Request,
							Formatting.None,
							new JsonSerializerSettings
							{
								ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
								Converters = new List<JsonConverter>
					{
						new JavaScriptDateTimeConverter()
					}
							}
						);
			var thisForm = WebContext.Profile.FormsData.Where(f => f.FormId == this.ID);
			if (thisForm.Count() > 0)
			{
				thisForm.Single().JSonData = formData;
			}
			else
			{
				var obj = new FormsData();
				obj.FormId = this.ID;
				obj.JSonData = formData;
				WebContext.Profile.FormsData.Add(obj);
			}
			WebContext.Profile.Save();

		}


		/// <summary>
		/// Saves the form in the DataForm Table.
		/// </summary>
		public string SaveDataForm(string dataFormType)
		{
			if (_dataForm)
			{
				IDictionary<string, string> dict = new Dictionary<string, string>();
				NameValueCollection allValues = GetValues();
				foreach (var k in allValues.AllKeys)
				{
					dict.Add(k, allValues[k]);
				}

				string serializedData = _formsMgr.SerializeFormValues(dict);

				//string serializedData = _formsMgr.SerializeForm(Request);

				string guid = "";
				string email = "";
				string name = "";
				if (Request.Data.Keys.Contains("Email"))
					email = Request.Data["Email"];
				if (Request.Data.Keys.Contains("Name"))
					name = Request.Data["Name"];

				if (Request.Data.Keys.Contains("FirstName") && Request.Data.Keys.Contains("LastName"))
					name = Request.Data["FirstName"] + " " + Request.Data["LastName"];



				Dictionary<string, string> filesList = new Dictionary<string, string>();
				

				string pathMail = Path.Combine("~", ConfigCte.EmailsFolder);
				string emailName = this.formType + ".htm";
				pathMail = WebContext.Server.MapPath(Path.Combine(pathMail, emailName)); //get the entire path to retrieve file and save it in destination
				filesList.Add("HtmlFile", emailName);

				string serializedFiles = "";
			


				if (WebContext.Request.Files.Count > 0)
				{

					//Get files and serialize
					HttpFileCollection files = WebContext.Request.Files;


					foreach (string key in files.Keys)
					{
						HttpPostedFile f = files[key];
						filesList.Add(key, StringUtils.FileNameToURL(f));
					}


					FormRequest filesFormRequest = new FormRequest();
					filesFormRequest.Data = filesList;
					serializedFiles = _formsMgr.SerializeForm(filesFormRequest);

					guid = _formsMgr.SaveFormData(dataFormType, email, name, serializedData, serializedFiles);

					string path = lw.CTE.Folders.FormsAttachementFolder + "/" + guid;
					path = WebContext.Server.MapPath("~/" + path);

					if (!Directory.Exists(path))
						Directory.CreateDirectory(path);

					foreach (string key in files.Keys)
					{
						string filePath = "";

						HttpPostedFile f = files[key];

						//string fileName = f.FileName;

						filePath = Path.Combine(path, StringUtils.FileNameToURL(f));

						f.SaveAs(filePath);
					}

					//Myriam start
					System.IO.File.Copy(pathMail, Path.Combine(path, emailName), true); //Copy = retrieve file(source) and save it in destination
				}
				else
				{
					
					FormRequest filesFormRequest = new FormRequest();
					filesFormRequest.Data = filesList;
					serializedFiles = _formsMgr.SerializeForm(filesFormRequest);
					

					guid = _formsMgr.SaveFormData(dataFormType, email, name, serializedData, serializedFiles);
					
					string path = lw.CTE.Folders.FormsAttachementFolder + "/" + guid;
					path = WebContext.Server.MapPath("~/" + path);
					if (!Directory.Exists(path))
						Directory.CreateDirectory(path);

					System.IO.File.Copy(pathMail, Path.Combine(path, emailName), true); //Copy = retrieve file(source) and save it in destination
					
				}

				return _formsMgr.GetFormByGuid(guid).ToString();

			}
			return null;
		}


        /// <summary>
        /// Loads the data saved in the form profile and fills all the fields with related info
        /// </summary>
        public void LoadSavedData()
        {
            Request = GetSavedFormData(this.ID);
            LoadSavedData(this, Request);
        }
        /// <summary>
        /// Loads the data saved in the form profile and fills all the fields with related info
        /// </summary>
        public void LoadSavedData(Control ParentControl, FormRequest SavedData)
        {
            if (ParentControl.Controls.Count == 0)
                return;

            foreach (Control ctrl in ParentControl.Controls)
            {
                LoadSavedData(ctrl, SavedData);
                HtmlControl _ctrl = ctrl as HtmlControl;
                if (_ctrl == null || _ctrl.ID == null)
                    continue;

                SetValue(_ctrl, SavedData.Get(_ctrl.ID));
            }
        }

        /// <summary>
        /// Returns the saved data for this form
        /// </summary>
        /// <param name="FormId"></param>
        /// <returns></returns>
        public static FormRequest GetSavedFormData(string FormId)
        {
            var ret = new FormRequest();
            var thisForm = WebContext.Profile.FormsData.Where(f => f.FormId == FormId);
            if (thisForm.Count() > 0)
            {
                var json = thisForm.Single().JSonData;
                ret = (FormRequest)JsonConvert.DeserializeObject(json, typeof(FormRequest));
            }
            return ret;
        }
        /// <summary>
        /// Deletes the saved data for this form
        /// </summary>
        /// <param name="FormId"></param>
        /// <returns></returns>
        public static void DeleteSavedData(string FormId)
        {
            var thisForm = WebContext.Profile.FormsData.Where(f => f.FormId == FormId);
            if (thisForm.Count() > 0)
            {
                WebContext.Profile.FormsData.Remove(thisForm.Single());
            }
        }

        /// <summary>
        /// Validates the group by running through all the ValidationGroup children
        /// </summary>
        public void Validate()
        {
            foreach (Control ctrl in this.Controls)
            {
                ValidationGroup _validator = ctrl as ValidationGroup;
                if (_validator != null)
                {
                    _validator.Validate();
                    if (!_validator.Validated.Value)
                    {
                        _validated = false;

                        if (_Messages.Count == 0)
                            _Messages.Add(Resources.Validation.PleaseFix);

                        foreach (string message in _validator.Messages)
                            _Messages.Add(message);
                    }
                }
            }
        }

        /// <summary>
        /// Validates the specified validation group 
        /// </summary>
        /// <param name="ValidationGroupId">ValidationGroupId</param>
        public void Validate(string ValidationGroupId)
        {
            ValidationGroup _validator = this.FindControlRecursive(ValidationGroupId) as ValidationGroup;
            if (_validator != null)
            {
                _validator.Validate();
                if (!_validator.Validated.Value)
                {
                    _validated = false;

                    if (_Messages.Count == 0)
                        _Messages.Add(Resources.Validation.PleaseFix);

                    foreach (string message in _validator.Messages)
                        _Messages.Add(message);
                }
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Used to determine if the form was submitted
        /// </summary>
        public bool IsPostBack
        {
            get
            {
                if (_isPostBack)
                {
                    SetAllValues(GetValues(), this);
                }
                return _isPostBack;
            }
        }

        bool? _isAjax = null;
        /// <summary>
        /// Used to determine if the form was submitted via ajax
        /// </summary>
        public bool IsAjax
        {
            get
            {
                if (_isAjax == null)
                {
                    _isAjax = WebContext.Request["ajax"] == "true";
                }
                return _isAjax.Value;
            }
        }


        NameValueCollection _data = null;
        /// <summary>
        /// Returns the data submited to this form
        /// </summary>
        public NameValueCollection Data
        {
            get { return _data ?? (_data = GetValues()); }
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
        /// <summary>
        /// Container for all the validation messages
        /// </summary>
        public List<string> ValidationMessages
        {
            get
            {
                return _Messages;
            }
        }

        #endregion

        #region Attributes
        /// <summary>
        /// The form Method Get or Post
        /// </summary>
        public FormMethod Method
        {
            get { return _method; }
            set { _method = value; }
        }

        /// <summary>
        /// The enctype of the form 
        /// if the form has any control with type file the enctype will automatically be set to "Data"
        /// </summary>
        public FormEncType EncType
        {
            get { return _encType; }
            set { _encType = value; }
        }

        /// <summary>
        /// Tells the form to which <seealso cref="ValidationGroup">ValidationGroup</seealso> Should follow
        /// If this is not set the form will follow all Children ValidationGroup
        /// This is usefull if more than one form share the same ValidationGroup
        /// </summary>
        public string ValidationGroup
        {
            get { return _validationGroup; }
            set { _validationGroup = value; }
        }

        /// <summary>
        /// flag if the form is validated if you want to directly check the form please use IsValid
        /// </summary>
        public bool? Validated
        {
            get { return _validated; }
            set { _validated = value; }
        }

        /// <summary>
        /// Returns if the form is valid or not
        /// </summary>
        public bool? IsValid
        {
            get { return _validated == null || _validated.Value; }
        }

        /// <summary>
        /// Flag used to validate if the submittion is coming from the same server
        /// Default: true, can be set to false if you want interractions between 2 or more servers.
        /// TODO: specify wich servers are permitted to post
        /// </summary>
        public bool ValidateServerRequest
        {
            get { return _validateServerRequest; }
            set { _validateServerRequest = value; }
        }

        bool _ajaxSubmit = false;
        /// <summary>
        /// Tells the form to submit via ajax
        /// Default: false
        /// </summary>
        public bool AjaxSubmit
        {
            get { return _ajaxSubmit; }
            set { _ajaxSubmit = value; }
        }

        string _successMessage = "";
        /// <summary>
        /// This message will be displayed after successfull PostBack
        /// The message can be set from code behind or from the tag itself
        /// </summary>
        public string SuccessMessage
        {
            get { return _successMessage; }
            set { _successMessage = value; }
        }

        string _ajaxCallback = "";
        /// <summary>
        /// Javascript Function that will be called on success
        /// </summary>
        /// <remarks>This is not a C# function if you want to call a C# function after postback simply call it from the code behind.</remarks>
        public string AjaxCallback
        {
            get { return _ajaxCallback; }
            set { _ajaxCallback = value; }
        }

        string _loaderArea = "";
        /// <summary>
        /// jQuery selector that defines where the loader should appear, 
        /// if not defined the loader will appear on top of the body
        /// </summary>
        public string LoaderArea
        {
            get { return _loaderArea; }
            set { _loaderArea = value; }
        }


        string _loaderClass = "";
        /// <summary>
        /// Sets the css class for the loader (here the loader image can be changed).
        /// </summary>
        public string LoaderClass
        {
            get { return _loaderClass; }
            set { _loaderClass = value; }
        }

        /// <summary>
        /// Ajax DataType (xml, json, script, or html)
        /// </summary>
        public string DataType
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

        /// <summary>
        /// Default: false
        /// </summary>
        public bool DataForm
        {
            get
            {
                return _dataForm;
            }
            set
            {
                _dataForm = value;
            }
        }

        /// <summary>
        /// formType (Contact Us, Application Form ...)
        /// </summary>
        public string FormType
        {
            get
            {
                return formType;
            }
            set
            {
                formType = value;
            }
        }

        //Myriam end



        CTE.Enum.Languages _validationLanguage = CTE.Enum.Languages.English;
        /// <summary>
        /// Specifies the validation language to be used with this form validation.
        /// If this value is not set the validation language is automatically set following the culture info present in web.config
        /// </summary>
        public lw.CTE.Enum.Languages ValidationLanguage
        {
            get
            {
                return _validationLanguage;
            }
            set
            {
                _validationLanguage = value;
            }
        }

        #endregion
    }
}
