using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using lw.WebTools;
using lw.Base;

namespace lw.SecurityControls
{
	/// <summary>
	/// Created the new NoCaptcha Re-Captcha from Google
	/// </summary>
	/// <example>
	/// <code>
	/// <![CDATA[
	/// <lw:NoCaptchaRecaptcha runat="server" theme="lite" />
	/// ]]>
	/// </code>
	/// </example>
	/// <remarks>
	/// Secret and Private keys must be added in Parameters.config
	/// ReCaptchaPublicKey and ReCaptchaPrivateKey.
	/// Get the keys from https://www.google.com/recaptcha/admin
	/// </remarks>
	[ToolboxData("<{0}:NoCaptchaRecaptcha runat=\"server\" />")]
	public class NoCaptchaRecaptcha: WebControl
	{
		#region Private Fields

		private const string RECAPTCHA_CHALLENGE_FIELD = "recaptcha_challenge_field";
		private const string RECAPTCHA_RESPONSE_FIELD = "g-recaptcha-response";

		private const string RECAPTCHA_HOST = "//www.google.com/recaptcha/api.js";

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


		public NoCaptchaRecaptcha()
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

			ThisPage.RegisterScriptFile("re-captcha", GenerateChallengeUrl());
		}
		protected override void Render(HtmlTextWriter writer)
		{
			this.RenderContents(writer);
		}
		protected override void RenderContents(HtmlTextWriter output)
		{
			output.Write(String.Format("<div class=\"g-recaptcha\" data-sitekey=\"{0}\"", this.PublicKey));
			if (!String.IsNullOrWhiteSpace(this.Theme))
			{
				output.Write(string.Format(" data-theme=\"{0}\"", this.Theme));
			}
			output.Write("></div>");
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
                var validator = new NoRecaptchaValidator();
                validator.Sercret = this.PrivateKey;
                validator.RemoteIP = Page.Request.UserHostAddress;
                validator.Response = Context.Request.Form[RECAPTCHA_RESPONSE_FIELD];
                validator.Proxy = this.proxy;

                if (validator.Response == null)
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
        private string GenerateChallengeUrl()
        {
            StringBuilder urlBuilder = new StringBuilder();
            urlBuilder.Append(RECAPTCHA_HOST);
			if (Language != null)
				urlBuilder.AppendFormat("hl={0}", this.Language);

			return urlBuilder.ToString();
        }


		CustomPage _thisPage;
		public CustomPage ThisPage
		{
			get
			{
				return this.Page as CustomPage;
			}
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
		[Description("UI language for the reCAPTCHA control. https://developers.google.com/recaptcha/docs/language for supported languages.")]
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
