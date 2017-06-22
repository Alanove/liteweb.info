using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using lw.Utils;
using lw.WebTools;

namespace lw.Menus
{
	public enum MenuTypeEnum
	{
		Regular,
		Dynamic,
		Seperator
	}
	public enum MenuDirection
	{
		RTL, LTR
	}
	public class lwMenu
	{
        string _defaultDomain;
        string _secureDomain;

		#region Constructors
		public lwMenu(string name, string url)
			: this(name, url, name)
		{
            _defaultDomain = WebTools.Config.GetFromWebConfig(lw.CTE.parameters.Domain);
            _secureDomain = WebTools.Config.GetFromWebConfig(lw.CTE.parameters.SecureDomain);
		}
		public lwMenu(string name, string url, bool isRoot)
			: this(name, url)
		{
			this.IsRoot = isRoot;
		}

        //Myriam
        public lwMenu(string name, string url, bool isRoot, bool secure)
            : this(name, url)
        {
            this.IsRoot = isRoot;
            this.IsSecure = secure;
        }
        //Myriam end
		public lwMenu(string name, string url, string DisplayName, bool isRoot)
			: this(name, url, DisplayName)
		{
			IsRoot = isRoot;
		}
		public lwMenu(string name, string url, string DisplayName)
		{
			this.Name = name;
			Url = url;
			this.DisplayName = DisplayName;
		}
		public lwMenu(string name, string url, string DisplayName, string ClassName)
		{
			this.Name = name;
			Url = url;
			this.DisplayName = DisplayName;
			this.ClassName = ClassName;
		}
		public lwMenu(string name, string url, string DisplayName, string ClassName, int id, int? parentId)
		{
			this.Name = name;
			Url = url;
			this.DisplayName = DisplayName;
			this.ClassName = ClassName;
			this.id = id;
			this.parentId = parentId;
		}
		public lwMenu(string name, string url, MenuTypeEnum menuType)
			: this(name, url)
		{
			this.MenuType = menuType;
		}
		#endregion

		//public static ReaderWriterLock _lock = new ReaderWriterLock();

		public string Name = "";
		string __url = "";
		public string DisplayName = "";
		public int id = -1;
		public int? parentId = null;
		string _pathUrl = null;

		public bool IsRoot = false;
		public string target;
		public string PageName = "";
		public bool GetUrlFromFirstChild = true;
		public MenuTypeEnum MenuType = MenuTypeEnum.Regular;
		public bool AddExtensions = false;
		public string PagesExtension = ".aspx";
		public string ClassName = "";
		public string SubPlaceHolder = "";
		bool _includeParentLink = true;
		bool? _canRender;
        public bool IsSecure = false;

		public List<lwMenu> Children = new List<lwMenu>();
		public List<lwMenu> Parents = new List<lwMenu>();
		public lw.CTE.Enum.UserStatus? UserStatus;
		public lw.CTE.Enum.UserType? UserType;

		public Dictionary<string, int> IndexFromParent = new Dictionary<string, int>();

		#region internal variables

		int? _breakAt = null;
		string _containerTag = "ul";
		bool? _current = null;
		string _url = "";
		bool _currentFromChildren = false;
		bool? _renderChildren = null;

		#endregion

		#region render
		/// <summary>
		/// Generates the html that will render the menu
		/// </summary>
		/// <param name="tag">the tag used to render each menu item, can be li, span, div...</param>
		/// <returns>HTML String</returns>
		public string ToString(string tag)
		{
			return ToString(tag, true, false, "");
		}

		/// <summary>
		/// Generates the html that will render the menu
		/// </summary>
		/// <param name="tag">the tag used to render each menu item, can be li, span, div...</param>
		/// <param name="includeParentLink">Include the link of the parent when generating the link hierarchy
		///		this will not override the original property IncludeParentLink
		/// </param>
		/// <param name="renderChildren">flag to render children or not</param>
		/// <param name="childrenParentTag">the tag used to render each menu item, can be li, span, div...</param>
		/// <returns>HTML String</returns>
		public string ToString(string tag, bool includeParentLink, bool renderChildren, string childrenParentTag)
		{
			return ToString(tag, includeParentLink, renderChildren, childrenParentTag, tag);
		}

		/// <summary>
		/// Generates the html that will render the menu
		/// </summary>
		/// <param name="tag">the tag used to render each menu item, can be li, span, div...</param>
		/// <param name="includeParentLink">Include the link of the parent when generating the link hierarchy
		/// this will not override the original property IncludeParentLink
		/// </param>
		/// <param name="renderChildren">flag to render children or not</param>
		/// <param name="childrenParentTag">The tag container for the children can be ul or div or ol...</param>
		/// <param name="childrenTag">the tag used to render each menu item, can be li, span, div...</param>
		/// <returns>HTML String</returns>
		public string ToString(string tag, bool includeParentLink, bool renderChildren, string childrenContainerTag, string childrenTag)
		{
			//TODO: fix caching by creating a unique key
			//Testing unique key from url
			//TODO: destroy cach when any related item is changed
			//It can be done by running through the cach's and destroying any related news or category
			//Another problem is to not cach the non-direct calls

			//string key = GetKey(renderChildren, tag);
			//if (WebContext.Cache[CachKey] != null)
			//	return WebContext.Cache[CachKey].ToString();

			bool hasOneItem = false;
			string _ret = "";

			try
			{
				//_lock.AcquireWriterLock(-1);

				if (!CanRender)
					return "";

				if (_renderChildren != null)
					renderChildren = _renderChildren.Value;

				StringBuilder ret = new StringBuilder();

				int? col = null;
				if (BreakAt != null)
					col = 0;


				if (renderChildren)
				{
					if (!String.IsNullOrWhiteSpace(SubPlaceHolder))
						ret.Append("<" + SubPlaceHolder + ">");

					if (!String.IsNullOrWhiteSpace(ContainerTag))
					{
						if (ContainerTag.ToLower() == "ol") ClassName = "dd-list";
						ret.Append("<" + ContainerTag + " start=\"1\" class=\"first" + (!String.IsNullOrEmpty(ClassName) ? " " + ClassName : "") + "\">");
					}
					for (int i = 0; i < Children.Count; i++)
					{
						lwMenu m = Children[i];
						if (!m.CanRender)
							continue;

						hasOneItem = true;


						//TODO: check first and last depending on visible items and not only on count.

						string _class = m.Current ? " current" : "";
						if (i == 0)
							_class += " first";
						if (i == Children.Count - 1)
							_class += " last";

						_class += " " + StringUtils.ToURL(m.url.Replace(".aspx", ""));

						if (!String.IsNullOrWhiteSpace(m.ClassName))
							_class = _class.Trim() + " " + m.ClassName;

						if (_class != "")
						{
							_class = " class=\"" + _class.Trim() +
								(tag.ToLower().IndexOf("li") >= 0 && _class.Trim().IndexOf("dd-item") < 0 ? " dd-item" : "") + "\"";
						}

						string _children = "";
						if (renderChildren && m.Children.Count > 0)
						{
							_children = m.ToString(childrenTag, includeParentLink, renderChildren, childrenContainerTag);
							if (!String.IsNullOrEmpty(childrenContainerTag))
								_children = _children != "" ? string.Format("<{0}>{1}</{0}>", childrenContainerTag, _children) : "";
						}
						ret.Append(String.Format(
							"<{0}{3}{4}>{1}{2}</{0}>",
							tag,
							m.GetLink(),
							_children,
							_class,
							(tag.ToLower().IndexOf("li") >= 0 ? " data-root=\"" + m.IsRoot + (m.id > 0 ? "\" data-id=\"" + m.id : "")
							+ "\" data-parent=\"" + (m.parentId.HasValue && m.parentId.Value > 0 ? m.parentId.Value : -1) + "\"" : ""))
						);

						if (BreakAt != null)
						{
							if (i > 0 && (i + 1) % BreakAt == 0 && i < Children.Count - 1 && !String.IsNullOrWhiteSpace(ContainerTag))
								ret.Append(string.Format("</{0}><{0} start=\"{1}\"{2}>",
									ContainerTag,
									this.BreakAt + i,
									i + BreakAt >= Children.Count - 1 ? " class=\"last\"" : ""));
						}
					}
					ret.Append("</" + ContainerTag + ">");
					if (!String.IsNullOrWhiteSpace(SubPlaceHolder))
						ret.Append("</" + SubPlaceHolder + ">");
				}




				_ret = ret.ToString();

				//TODO: fix this after fixing the key
				/*
				WebContext.Cache.Insert(
					CachKey,
					_ret,
					new System.Web.Caching.CacheDependency(WebContext.Server.MapPath("~/App_Code/Menu.cs"))
				);
				 * */

			}
			finally
			{
				//_lock.ReleaseWriterLock();
			}
			return hasOneItem ? _ret : "";
		}

		public bool CanRender
		{
			get
			{
				if (_canRender == null)
				{
					_canRender = true;
					//Checks the current user type and status
					if (UserType != null)
					{
						if ((int)(UserType & WebContext.Profile.CurrentUserType) == 0)
							_canRender = false;
					}
					if (UserStatus != null)
					{
						if ((int)((int)UserStatus & WebContext.Profile.CurrentUserStatus) == 0)
							_canRender = false;
					}
				}
				return _canRender.Value;
			}
			set
			{
				_canRender = value;
			}
		}

		#endregion

		public string GetKey(bool renderChildren, string tag)
		{
			string ret = this.url + this.Name + renderChildren.ToString() + tag;
			lwMenu m = this;
			while (m.Parents.Count > 0)
			{
				if (ret.IndexOf("~") == 0)
					break;
				ret = m.Parents[0].url + "/" + ret;
				m = m.Parents[0];
			}
			return ret;
		}

		public string GetLink()
		{
			Regex re = new Regex("&reg;|®", RegexOptions.IgnoreCase);
            string test = WebContext.Request.RawUrl;
			string url = string.Format("<a " + (StringUtils.IsNullOrWhiteSpace(this.DisplayName) ? "" : "class=\"dd-handle\"") + " href=\"{0}\" title=\"{1}\"{3}>{2}</a>",
						this.GetUrl(),
						this.DisplayName,
						re.Replace(this.DisplayName, "<sup>&reg;</sup>"),
						StringUtils.IsNullOrWhiteSpace(target) ? "" : string.Format("target=\"{0}\"", target)
						);
			return url;
		}

		public string GetUrl()
		{
			if (_url == "")
			{
				Regex urlRegEx = new Regex("((mailto\\:|(news|(ht|f)tp(s?))\\://){1}\\S+)", RegexOptions.IgnoreCase);
				if (urlRegEx.IsMatch(this.url))
				{
					_url = this.url;
					return _url;
				}

				string ret = this.PathUrl;


				Regex extensionTest = new Regex("[.]\\w+$", RegexOptions.IgnoreCase);
				if (!extensionTest.IsMatch(ret))
				{
					if (GetUrlFromFirstChild && this.Children.Count > 0)
						return this.Children[0].GetUrl();

					if (!string.IsNullOrEmpty(this.PageName))
						ret = Path.Combine(ret, PageName);

					if (!String.IsNullOrEmpty(PagesExtension) && AddExtensions)
					{
						ret = Path.Combine(ret, "/default" + PagesExtension);
					}
				}

				ret = ret.Replace("\\", "/");

				/*
				lwMenu m = this;
				while (m.Parents.Count > 0)
				{	
					if (ret.IndexOf("~") == 0)
						break;
					ret = m.Parents[0].url + "/" + ret;
					m = m.Parents[0];
				}
				 * */

				if (ret.IndexOf("~") == 0)
                {
                    //ret = ret.Replace("~", WebContext.Root);
					ret = DefaultSecureDomain(ret);
                }
				_url = ret;
			}
			return _url;
		}

		/// <summary>
		/// Changes the PathUrl to secure or default domain
		/// </summary>
		/// <param name="PathUrl">URL to be changed</param>
		/// <returns>String</returns>
		public string DefaultSecureDomain(string PathUrl)
		{
			//if (this.IsSecure && !String.IsNullOrWhiteSpace(_secureDomain))
			if (!String.IsNullOrWhiteSpace(_secureDomain))
				PathUrl = PathUrl.IndexOf("~") == 0 ? PathUrl.Replace("~", "https://" + _secureDomain) : "https://" + _secureDomain;
			else
				if (!String.IsNullOrWhiteSpace(_defaultDomain))
					PathUrl = PathUrl.IndexOf("~") == 0 ? PathUrl.Replace("~", "http://" + _defaultDomain) : "http://" + _defaultDomain;
				else PathUrl = PathUrl.IndexOf("~") == 0 ? PathUrl.Replace("~", WebContext.Root) : WebContext.Root;
			return PathUrl;
		}

		/// <summary>
		/// Adds a child to the menu
		/// </summary>
		/// <param name="m">The added menu</param>
		/// <returns>The created menu</returns>
		public lwMenu AddChild(lwMenu m)
		{
			this.Children.Add(m);
			m.IndexFromParent[this.Name] = this.Children.Count - 1;
			m.Parents.Add(this);
			return m;
		}

		/// <summary>
		/// Adds a child to the menu
		/// </summary>
		/// <param name="menuName">The name of the child</param>
		/// <param name="url">the URL</param>
		/// <returns>The created menu</returns>
		public lwMenu AddChild(string menuName, string url)
		{
			lwMenu m = new lwMenu(menuName, url);
			AddChild(m);
			return m;
		}

		/// <summary>
		/// Adds a child to the menu
		/// </summary>
		/// <param name="menuName">The name of the child</param>
		/// <returns>The created menu</returns>
		public lwMenu AddChild(string menuName)
		{
			lwMenu m = new lwMenu(menuName, StringUtils.ToURL(menuName));
			AddChild(m);
			return m;
		}




		/// <summary>
		/// Adds a child to the menu
		/// </summary>
		/// <param name="menuName">The name of the child</param>
		/// <param name="url">the URL</param>
		/// <param name="id">the ID</param>
		/// <returns>The created menu</returns>
		public lwMenu AddChild(string menuName, string url, int id)
		{
			lwMenu m = new lwMenu(menuName, url);
			m.id = id;
			m.parentId = this.id;
            AddChild(m);
            return m;
        }


        public lwMenu AddChild(string menuName, string url, int id, bool secure)
        {
            lwMenu m = new lwMenu(menuName, url);
            m.id = id;
            m.parentId = this.id;
            m.IsSecure = secure;
			AddChild(m);
			return m;
		}


		/// <summary>
		/// Returns the Next Menu
		/// </summary>
		/// <returns>the next menu</returns>
		public lwMenu NextMenu()
		{
			lwMenu p = Parents[0];
			int i = IndexFromParent[p.Name];

			i++;

			if (i >= p.Children.Count)
			{
				return null;
			}
			return p.Children[i];
		}


		/// <summary>
		/// Returns the Previous menu
		/// </summary>
		/// <returns>the Previous menu</returns>
		public lwMenu PrevMenu()
		{
			lwMenu p = Parents[0];
			int i = IndexFromParent[p.Name];

			i--;

			if (i < 0)
			{
				return null;
			}
			return p.Children[i];
		}

		/// <summary>
		/// Searches and returns the menu with the specified search patern
		/// Will search in Names and URLs
		/// </summary>
		/// <param name="search">Search Keyword</param>
		/// <returns>The found menu or null</returns>
		public lwMenu GetMenu(string search)
		{
			if (search == null)
				return null;
			if (search == "")
				return this;
			lwMenu ret = this.Children.Find(m => m.Name.Contains(search)
				|| m.url.ToLower().Contains(search));

			if (ret == null)
			{
				foreach (lwMenu child in this.Children)
				{
					lwMenu temp = child.GetMenu(search);
					if (temp != null)
						return temp;
				}
			}

			return ret;
		}


		public string GetHistory(string sep, MenuDirection menuDirection)
		{
			string key = GetKey(false, "") + "-h";
			if (WebContext.Cache[key] != null)
				return WebContext.Cache[key].ToString();

			StringBuilder ret = new StringBuilder();

			Regex re = new Regex("&reg;|®", RegexOptions.IgnoreCase);
			ret.Append(re.Replace(this.DisplayName, "<sup>&reg;</sup>"));
			if (menuDirection == MenuDirection.LTR)
				ret.Insert(0, sep);
			else
				ret.Append(sep);

			lwMenu m = this;
			while (m.Parents.Count > 0)
			{
				m = m.Parents[0];
				if (m.IsRoot)
					break;
				if (menuDirection == MenuDirection.LTR)
				{
					ret.Insert(0, m.GetLink());
					ret.Insert(0, sep);
				}
				else
				{
					ret.Append(m.GetLink());
					ret.Append(sep);
				}
			}

			if (menuDirection == MenuDirection.LTR)
				ret.Remove(0, sep.Length);
			else
				ret.Remove(ret.Length - sep.Length, sep.Length);

			WebContext.Cache.Insert(
				key,
				ret.ToString(),
				new System.Web.Caching.CacheDependency(WebContext.Server.MapPath("~/App_Code/__Page.cs"))
			);

			return ret.ToString();
		}


		/// <summary>
		/// Runs through the menu and all children to change the AddPageExtension property
		/// You can use this function instead of calling the menu and each of the children one by one
		/// </summary>
		/// <param name="extension">boolean to either add entensions or not</param>
		public void SetAddPageExtensionsRecursive(bool newStatus)
		{
			this.AddExtensions = newStatus;
			foreach (lwMenu _m in this.Children)
			{
				_m.SetAddPageExtensionsRecursive(newStatus);
			}
		}

		/// <summary>
		/// Runs through the menu and all children to change the page extension
		/// You can use this function instead of calling the menu and each of the children one by one
		/// </summary>
		/// <param name="extension">extension string def: .aspx and can be empty</param>
		public void SetPagesExtensionRecursive(string extension)
		{
			this.PagesExtension = extension;
			foreach (lwMenu _m in this.Children)
			{
				_m.SetPagesExtensionRecursive(extension);
			}
		}

		#region Properties

		/// <summary>
		/// Break the menu at a specified number
		/// creating containers for each set of menu numbers
		/// ex: BreakAt = 2 and ContainerTag = ul
		/// we will have <ul></ul> for each set of 2 menu items
		/// </summary>
		public int? BreakAt
		{
			get { return _breakAt; }
			set { _breakAt = value; }
		}

		/// <summary>
		/// Container Tag "ul" or "ol" are the best choices
		/// def: ul
		/// </summary>
		public string ContainerTag
		{
			get { return _containerTag; }
			set { _containerTag = value; }
		}

		/// <summary>
		/// Returns if the menu item is currently active
		/// This variable is recursive
		/// If a menu Item has a child that is active it will become active as well.
		/// </summary>
		public bool Current
		{
			get
			{
				//Since the object is cached current will have to be rest
				//Change back when caching is fixed ToString()
				if (true)// && _current == null)
				{
					_current = false;

					string r = "";
					r = DefaultSecureDomain(r);

					string rawUrl = r + WebContext.Request.RawUrl.Split('?')[0];
					if (rawUrl.ToLower().IndexOf("/default.aspx") > 0)
					{
						rawUrl = rawUrl.Substring(0, rawUrl.ToLower().IndexOf("/default.aspx"));
					}
					string _getUrl = GetUrl();
					if (_getUrl.IndexOf("/default.aspx") > 0)
					{
						_getUrl = _getUrl.Substring(0, _getUrl.ToLower().IndexOf("/default.aspx"));
					}

					if (rawUrl.Equals(_getUrl, StringComparison.OrdinalIgnoreCase))
						_current = true;

					if (!_current.Value)
					{
						foreach (lwMenu menu in this.Children)
						{
							if (menu.Current)
							{
								_currentFromChildren = true;
								_current = true;
								break;
							}
						}

					}
					return _current.Value;
				}
				return false;
			}
			set
			{
				_current = value;
			}
		}

		public bool IncludeParentLink
		{
			get
			{
				return _includeParentLink;
			}
			set
			{
				_includeParentLink = value;
			}
		}

		/// <summary>
		/// this is old please use CanRender = false instead.
		/// </summary>
		public bool noDisplay
		{
			get
			{
				return !CanRender;
			}
			set
			{
				CanRender = !value;
			}
		}
		public string Url
		{
			get
			{
				return __url;
			}
			set
			{
				if (value.IndexOf("~") == 0)
					IncludeParentLink = false;

				__url = value;
			}
		}
		public string url
		{
			get
			{
				return Url;
			}
			set
			{
				Url = value;
			}
		}
		public bool? RenderChildren
		{
			get
			{
				return _renderChildren;
			}
			set
			{
				_renderChildren = value;
			}
		}

		public string CachKey
		{
			get
			{
				return string.Format("{0}{1}", lw.CTE.CteCache.RenderedMenuPrefix, WebContext.AbsolutePath);
			}
		}

		public string PathUrl
		{
			get
			{
				if (_pathUrl == null)
				{
					if (IncludeParentLink)
					{
						if (this.Parents.Count > 0)
							_pathUrl = Path.Combine(this.Parents[0].PathUrl, url);
					}
					else
						_pathUrl = this.url;
				}
				return _pathUrl;
			}
			set
			{
				_pathUrl = value;
			}
		}

		#endregion

        public void AddChild(string p1, string p2, int p3, bool? nullable)
        {
            throw new NotImplementedException();
        }
	}
}