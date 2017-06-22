using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using lw.WebTools;

namespace lw.SecurityControls
{
	[ToolboxData("<{0}:Recaptcha runat=\"server\" />")]
	public class Recaptcha: WebControl
	{
		#region Private Fields

		private const string RECAPTCHA_CHALLENGE_FIELD = "recaptcha_challenge_field";
		private const string RECAPTCHA_RESPONSE_FIELD = "recaptcha_response_field";

		private const string RECAPTCHA_SECURE_HOST = "https://www.google.com/recaptcha/api";
		private const string RECAPTCHA_HOST = "http://www.google.com/recaptcha/api";

		private RecaptchaResponse recaptchaResponse;

		private string publicKey;
		private string privateKey;
		private string theme;
		private string language;
		private Dictionary<string, string> customTranslations;
		private string customThemeWidget;
		private bool skipRecaptcha;
		private bool allowMultipleInstances;
		private bool overrideSecureMode;
		private IWebProxy proxy;

		#endregion


		public Recaptcha()
		{
			Config cfg = new Config();

			this.PublicKey = cfg.GetKey(cte.PublicKey);
			this.PrivateKey = cfg.GetKey(cte.PrivateKey);

		}

		#region Overriden Methods

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);

			if (string.IsNullOrWhiteSpace(this.PublicKey) || string.IsNullOrWhiteSpace(this.PrivateKey))
			{
				throw new ApplicationException("reCAPTCHA needs to be configured with a public & private key.");
			}
		}
		protected override void Render(HtmlTextWriter writer)
		{
			this.RenderContents(writer);
		}
		protected override void RenderContents(HtmlTextWriter output)
		{
			// <script> setting
			output.AddAttribute(HtmlTextWriterAttribute.Type, "text/javascript");
			output.RenderBeginTag(HtmlTextWriterTag.Script);
			output.Indent++;
			output.WriteLine("var RecaptchaOptions = {");
			output.Indent++;
			output.WriteLine("theme : '{0}',", this.theme ?? string.Empty);
			if (!string.IsNullOrEmpty(this.language))
				output.WriteLine("lang : '{0}',", this.language);
			if (this.customTranslations != null && this.customTranslations.Count > 0)
			{
				var i = 0;
				output.WriteLine("custom_translations : {");
				foreach (var customTranslation in this.customTranslations)
				{
					i++;
					output.WriteLine(
						i != this.customTranslations.Count ?
							"{0} : '{1}'," :
							"{0} : '{1}'",
						customTranslation.Key,
						customTranslation.Value);
				}
				output.WriteLine("},");
			}
			if (!string.IsNullOrEmpty(this.customThemeWidget))
				output.WriteLine("custom_theme_widget : '{0}',", this.customThemeWidget);
			output.WriteLine("tabindex : {0}", base.TabIndex);
			output.Indent--;
			output.WriteLine("};");
			output.Indent--;
			output.RenderEndTag();

			// <script> display
			output.AddAttribute(HtmlTextWriterAttribute.Type, "text/javascript");
			output.AddAttribute(HtmlTextWriterAttribute.Src, this.GenerateChallengeUrl(false), false);
			output.RenderBeginTag(HtmlTextWriterTag.Script);
			output.RenderEndTag();

			// <noscript> display
			output.RenderBeginTag(HtmlTextWriterTag.Noscript);
			output.Indent++;
			output.AddAttribute(HtmlTextWriterAttribute.Src, this.GenerateChallengeUrl(true), false);
			output.AddAttribute(HtmlTextWriterAttribute.Width, "500");
			output.AddAttribute(HtmlTextWriterAttribute.Height, "300");
			output.AddAttribute("frameborder", "0");
			output.RenderBeginTag(HtmlTextWriterTag.Iframe);
			output.RenderEndTag();
			output.WriteBreak(); // modified to make XHTML-compliant. Patch by xitch13@gmail.com.
			output.AddAttribute(HtmlTextWriterAttribute.Name, "recaptcha_challenge_field");
			output.AddAttribute(HtmlTextWriterAttribute.Rows, "3");
			output.AddAttribute(HtmlTextWriterAttribute.Cols, "40");
			output.RenderBeginTag(HtmlTextWriterTag.Textarea);
			output.RenderEndTag();
			output.AddAttribute(HtmlTextWriterAttribute.Name, "recaptcha_response_field");
			output.AddAttribute(HtmlTextWriterAttribute.Value, "manual_challenge");
			output.AddAttribute(HtmlTextWriterAttribute.Type, "hidden");
			output.RenderBeginTag(HtmlTextWriterTag.Input);
			output.RenderEndTag();
			output.Indent--;
			output.RenderEndTag();
		}

		#endregion


		#region Validate
		/// <summary>
        /// Perform validation of reCAPTCHA.
        /// </summary>
        public void Validate()
        {
            if (Visible && Enabled)
            {
                RecaptchaValidator validator = new RecaptchaValidator();
                validator.PrivateKey = this.PrivateKey;
                validator.RemoteIP = Page.Request.UserHostAddress;
                validator.Challenge = Context.Request.Form[RECAPTCHA_CHALLENGE_FIELD];
                validator.Response = Context.Request.Form[RECAPTCHA_RESPONSE_FIELD];
                validator.Proxy = this.proxy;

                if (validator.Challenge == null)
                {
                    this.recaptchaResponse = RecaptchaResponse.InvalidChallenge;
                }
                else if (validator.Response == null)
                {
                    this.recaptchaResponse = RecaptchaResponse.InvalidResponse;
                }
                else
                {
                    this.recaptchaResponse = validator.Validate();
                }
            }
        }

        #endregion

        /// <summary>
        /// This function generates challenge URL.
        /// </summary>
        private string GenerateChallengeUrl(bool noScript)
        {
            StringBuilder urlBuilder = new StringBuilder();
            urlBuilder.Append(Context.Request.IsSecureConnection || this.overrideSecureMode ? RECAPTCHA_SECURE_HOST : RECAPTCHA_HOST);
            urlBuilder.Append(noScript ? "/noscript?" : "/challenge?");
            urlBuilder.AppendFormat("k={0}", this.PublicKey);
            return urlBuilder.ToString();
        }


		#region Public Properties

		bool? _validated = null;
		[Category("validation")]
		public bool IsValid
		{
			get
			{
				if (_validated == null)
				{
					this.Validate();
					_validated = this.recaptchaResponse.IsValid;
				}
				return _validated.Value;
			}
		}

		[Category("Settings")]
		[Description("The public key from https://www.google.com/recaptcha/admin/create. Can also be set using RecaptchaPublicKey in AppSettings.")]
		public string PublicKey
		{
			get { return this.publicKey; }
			set { this.publicKey = value; }
		}

		[Category("Settings")]
		[Description("The private key from https://www.google.com/recaptcha/admin/create. Can also be set using RecaptchaPrivateKey in AppSettings.")]
		public string PrivateKey
		{
			get { return this.privateKey; }
			set { this.privateKey = value; }
		}

		[Category("Appearance")]
		[DefaultValue("red")]
		[Description("The theme for the reCAPTCHA control. Currently supported values are 'red', 'white', 'blackglass', 'clean', and 'custom'.")]
		public string Theme
		{
			get { return this.theme; }
			set { this.theme = value; }
		}

		[Category("Appearance")]
		[DefaultValue(null)]
		[Description("UI language for the reCAPTCHA control. Currently supported values are 'en', 'nl', 'fr', 'de', 'pt', 'ru', 'es', and 'tr'.")]
		public string Language
		{
			get { return this.language; }
			set { this.language = value; }
		}

		[Category("Appearance")]
		[DefaultValue(null)]
		public Dictionary<string, string> CustomTranslations
		{
			get { return this.customTranslations; }
			set { this.customTranslations = value; }
		}

		[Category("Appearance")]
		[DefaultValue(null)]
		[Description("When using custom theming, this is a div element which contains the widget. ")]
		public string CustomThemeWidget
		{
			get { return this.customThemeWidget; }
			set { this.customThemeWidget = value; }
		}

		[Category("Settings")]
		[DefaultValue(false)]
		[Description("Set this to true to stop reCAPTCHA validation. Useful for testing platform. Can also be set using RecaptchaSkipValidation in AppSettings.")]
		public bool SkipRecaptcha
		{
			get { return this.skipRecaptcha; }
			set { this.skipRecaptcha = value; }
		}

		[Category("Settings")]
		[DefaultValue(false)]
		[Description("Set this to true to enable multiple reCAPTCHA on a single page. There may be complication between controls when this is enabled.")]
		public bool AllowMultipleInstances
		{
			get { return this.allowMultipleInstances; }
			set { this.allowMultipleInstances = value; }
		}

		[Category("Settings")]
		[DefaultValue(false)]
		[Description("Set this to true to override reCAPTCHA usage of Secure API.")]
		public bool OverrideSecureMode
		{
			get { return this.overrideSecureMode; }
			set { this.overrideSecureMode = value; }
		}

		[Category("Settings")]
		[Description("Set this to override proxy used to validate reCAPTCHA.")]
		public IWebProxy Proxy
		{
			get { return this.proxy; }
			set { this.proxy = value; }
		}


		public string ErrorMsg
		{
			get
			{
				return recaptchaResponse.ErrorMessage;
			}
		}
		#endregion
	}
}
