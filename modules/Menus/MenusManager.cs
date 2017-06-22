using System.Linq;

using lw.Data;
using lw.Utils;

namespace lw.Menus
{
	public class MenusManager: LINQManager
	{
		public MenusManager():base (cte.lib)
		{ }


		#region Menu

		public IQueryable<Menu> GetMenus()
		{
			var query = from menu in MenusData.Menus
						select menu;
			return query;
		}

		public Menu GetMenu(int MenuId)
		{
			return MenusData.Menus.Single(m => m.MenuId == MenuId);
		}

		public int CreateMenu(string Name, string Description)
		{
			if (StringUtils.IsNullOrWhiteSpace(Name))
				return -1;

			var q = from _menu in GetMenus()
					where _menu.Name == Name
					select _menu;

			if (q.Count() > 0)
				return -1;

			Menu menu = new Menu
			{
				Name = Name,
				Description = Description
			};

			MenusData.Menus.InsertOnSubmit(menu);
			MenusData.SubmitChanges();

			return menu.MenuId;
		}

		public int UpdateMenu(int MenuId, string Name, string Description)
		{
			var q = from _menu in GetMenus()
					where _menu.Name == Name && _menu.MenuId != MenuId
					select _menu;

			if (q.Count() > 0)
				return -1;

			Menu menu = MenusData.Menus.Single(temp => temp.MenuId == MenuId);

			menu.Name = Name;
			menu.Description = Description;

			MenusData.SubmitChanges();
			
			return MenuId;
		}

		public void DeleteMenu(int MenuId)
		{
			var menu = GetMenu(MenuId);

			MenusData.Menus.DeleteOnSubmit(menu);
			MenusData.SubmitChanges();
		}

		#endregion

		#region Menu Items

		public IQueryable<Menu_Item> GetMenuItems()
		{
			var query = from menu in MenusData.Menu_Items
						select menu;
			return query;
		}

		public Menu_Item GetMenuItem(int Id)
		{
			return MenusData.Menu_Items.Single(m => m.Id == Id);
		}

		public int CreateMenuItem(int? MenuId, int? ItemId, string Title, string Description, short Type, int? SubMenuId, bool? NewWindow)
		{
			Menu_Item menu = new Menu_Item
			{
				MenuId = MenuId,
				ItemId = ItemId,
				Title = Title,
				Description = Description,
				Type = Type,
				SubMenuId = SubMenuId,
				NewWindow = NewWindow
			};

			MenusData.Menu_Items.InsertOnSubmit(menu);
			MenusData.SubmitChanges();

			return menu.Id;
		}

		public int UpdateMenuItem(int Id, int? MenuId, int? ItemId, string Title, string Description, short Type, int? SubMenuId, bool? NewWindow)
		{
			var q = from _menu in GetMenuItems()
					where _menu.Id == Id
					select _menu;

			if (q.Count() > 0)
				return -1;

			Menu_Item menu = MenusData.Menu_Items.Single(temp => temp.Id == Id);

			menu.MenuId = MenuId;
			menu.ItemId = ItemId;
			menu.Title = Title;
			menu.Description = Description;
			menu.Type = Type;
			menu.SubMenuId = SubMenuId;
			menu.NewWindow = NewWindow;

			MenusData.SubmitChanges();

			return Id;
		}

		public void DeleteMenuItem(int Id)
		{
			var menu = GetMenuItem(Id);

			MenusData.Menu_Items.DeleteOnSubmit(menu);
			MenusData.SubmitChanges();
		}

		#endregion


		#region Pages

		public IQueryable<Page> GetPages()
		{
			var query = from page in MenusData.Pages
						select page;
			return query;
		}

		public Page GetPage(int PageId)
		{
			return MenusData.Pages.Single(p => p.PageId == PageId);
		}

		public int CreatePage(string DisplayName, string Title, string Description, string Keywords, string Url, int? GroupId, bool Virtual)
		{
			if (StringUtils.IsNullOrWhiteSpace(DisplayName))
				return -1;

			var q = from _page in GetPages()
					where _page.DisplayName == DisplayName
					select _page;

			if (q.Count() > 0)
				return -1;

			Page page = new Page
			{
				DisplayName = DisplayName,
				Title = Title,
				Description = Description,
				Keywords = Keywords,
				Url = Url,
				GroupId = GroupId,
				Virtual = Virtual
			};

			MenusData.Pages.InsertOnSubmit(page);
			MenusData.SubmitChanges();

			return page.PageId;
		}

		public int UpdatePage(int PageId, string DisplayName, string Title, string Description, string Keywords, string Url, int? GroupId, bool Virtual)
		{
			var q = from _page in GetPages()
					where _page.DisplayName == DisplayName && _page.PageId != PageId
					select _page;

			if (q.Count() > 0)
				return -1;

			Page page = MenusData.Pages.Single(temp => temp.PageId == PageId);

			page.DisplayName = DisplayName;
			page.Title = Title;
			page.Description = Description;
			page.Keywords = Keywords;
			page.Url = Url;
			page.GroupId = GroupId;
			page.Virtual = Virtual;

			MenusData.SubmitChanges();

			return PageId;
		}

		public void DeletePage(int PageId)
		{
			var page = GetPage(PageId);

			MenusData.Pages.DeleteOnSubmit(page);
			MenusData.SubmitChanges();
		}

		#endregion

		#region Menu Externals

		public IQueryable<Menu_External> GetMenuExternals()
		{
			var query = from menu in MenusData.Menu_Externals
						select menu;
			return query;
		}

		public Menu_External GetMenuExternal(int ItemId)
		{
			return MenusData.Menu_Externals.Single(m => m.ItemId == ItemId);
		}

		public int CreateMenuExternal(string Name, string Link)
		{
			if (StringUtils.IsNullOrWhiteSpace(Name))
				return -1;

			var q = from _menu in GetMenuExternals()
					where _menu.Name == Name
					select _menu;

			if (q.Count() > 0)
				return -1;

			Menu_External menu = new Menu_External
			{
				Name = Name,
				Link = Link
			};

			MenusData.Menu_Externals.InsertOnSubmit(menu);
			MenusData.SubmitChanges();

			return menu.ItemId;
		}

		public int UpdateMenuExternal(int ItemId, string Name, string Link)
		{
			var q = from _menu in GetMenuExternals()
					where _menu.Name == Name && _menu.ItemId != ItemId
					select _menu;

			if (q.Count() > 0)
				return -1;

			Menu_External menu = MenusData.Menu_Externals.Single(temp => temp.ItemId == ItemId);

			menu.Name = Name;
			menu.Link = Link;

			MenusData.SubmitChanges();

			return ItemId;
		}

		public void DeleteMenuExternal(int ItemId)
		{
			var menu = GetMenuExternal(ItemId);

			MenusData.Menu_Externals.DeleteOnSubmit(menu);
			MenusData.SubmitChanges();
		}

		#endregion

		#region Variables

		public MenusDataContext MenusData
		{
			get
			{
				return (MenusDataContext)this.dataContext;
			}
		}

		#endregion
	}
}
