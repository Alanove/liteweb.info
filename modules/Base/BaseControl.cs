using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

using lw.WebTools;
using lw.CTE;

namespace lw.Base
{

	/// <summary>
	/// Inherit BaseControl instead of Control to gain direct access to 
	/// MyPage	lw.Base.CustomPage
	/// CMSMode	if working inside the CMS
	/// </summary>
	[ParseChildren(false)]
	[PersistChildren(true)]
	public class BaseControl : WebControl
	{
		/// <summary>
		/// Creates the base control
		/// </summary>
		public BaseControl()
		{
		}

		/// <summary>
		/// Creates the base control with an html tag
		/// </summary>
		/// <param name="tag">The html tag. ex: div, table, etc..</param>
		public BaseControl(string tag):base(tag)
		{
		}


		#region override
		public override void RenderBeginTag(HtmlTextWriter writer)
		{
			if(_renderContainerTag)
				base.RenderBeginTag(writer);
		}
		public override void RenderEndTag(HtmlTextWriter writer)
		{
			if(_renderContainerTag)
				base.RenderEndTag(writer);
		}
		#endregion


		#region Properties
		CustomPage _page;
		/// <summary>
		/// Returns the reffered lw.Base.CustomPage where the control belongs
		/// </summary>
		public CustomPage MyPage
		{
			get
			{
				if(_page == null)
				{
					_page = this.Page as CustomPage;
				}
				return _page;
			}
		}

		bool _renderContainerTag = false;
		/// <summary>
		/// Flag if the tag surrounding the container should be rendered.
		/// Default: false, only children are rendered.
		/// </summary>
		public bool RenderContainerTag
		{
			get
			{
				return _renderContainerTag;
			}
			set
			{
				_renderContainerTag = value;
			}
		}

		string _imageNotFound;
		/// <summary>
		/// Can be used in the control to display empty images.
		/// </summary>
		public string ImageNotFound
		{
			get
			{
				if (String.IsNullOrWhiteSpace(_imageNotFound))
				{
					_imageNotFound = Config.GetFromWebConfig(parameters.ImageNotFound);
				}
				return _imageNotFound;
			}
			set
			{
				_imageNotFound = value;
			}
		}


		bool? _cmsMode = null;
		/// <summary>
		/// Indicates if the tag is used inside the CMS
		/// Can also be set in web.config.
		/// </summary>
		public bool? CMSMode
		{
			get
			{
				if (_cmsMode == null)
				{
					string val = Config.GetFromWebConfig(lw.CTE.parameters.CMSMode);
					_cmsMode = !String.IsNullOrEmpty(val) && bool.Parse(val.Trim());
				}
				return _cmsMode;
			}
			set
			{
				_cmsMode = value;
			}
		}


		string _masterPageFile = null;
		/// <summary>
		/// 
		/// </summary>
		public virtual string MasterPageFile
		{
			get
			{
				return _masterPageFile;
			}
			set
			{
				_masterPageFile = value;
			}
		}

		#endregion
	}
}
