
using lw.Base;
using lw.WebTools;

namespace lw.Maps
{
	public class GoogleMap : System.Web.UI.WebControls.WebControl
	{
		string _id = "Google_Map";
		string _key = "";
		string _scriptId = "Google_Maps_Script";

		string _s = "";
		bool bound = false;

		public GoogleMap()
			: base("div")
		{
			Config cfg = new Config();
			Key = cfg.GetKey("MapsKey");
		}

		public override void DataBind()
		{
			if (bound)
				return;
			bound = true;

			//System.Web.UI.WebControls.Panel panel = new System.Web.UI.WebControls.Panel();
			//panel.ID = this.ID;
			//this.Controls.Add(panel);

			CustomPage p = this.Page as CustomPage;
			if (p != null)
			{
				p.RegisterScriptFile(this._scriptId, "http://maps.google.com/maps?file=api&amp;v=2&amp;key=" + Key);
				p.RegisterScriptFile(this._scriptId + "_Local", string.Format("{0}/js/Maps.js", WebContext.Root));

				p.RegisterLoadScript("load map", string.Format("lw_map.init(\"{0}\");", this._id), true);
			}
			base.DataBind();
		}

		protected override void Render(System.Web.UI.HtmlTextWriter writer)
		{
			base.Render(writer);
		}
		public override string UniqueID
		{
			get
			{
				return _id;
			}
		}
		public override string  ID
		{
			get
			{
				return _id;
			}
			set
			{
				_id = value;
			}
		}
		protected string Key
		{
			get
			{
				return this._key;
			}
			set
			{
				this._key = value;
			}
		}
	}
}
