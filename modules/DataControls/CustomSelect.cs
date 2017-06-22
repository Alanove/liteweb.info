using System.Web.UI;
using System.Web.UI.HtmlControls;



namespace lw.DataControls
{
	public class CustomSelect : HtmlSelect
	{
		string source = "";
		bool _bound = false;
		string _emptyText = "";
		bool _addEmptyText = false;


		public override void DataBind()
		{
			if (_bound)
				return;
			_bound = true;

			if (source != "")
			{
				Control ctrl = Page.FindControl(source);
				if (ctrl != null)
				{
					CustomDataSource dataSrc = ctrl as CustomDataSource;
					if (dataSrc != null)
					{
						DataSource = dataSrc.Data;
					}
				}
			}
			base.DataBind();
		}

		protected override void Render(HtmlTextWriter writer)
		{
			if (AddEmptyText)
			{
				this.Items.Add(EmptyText);
				int len = this.Items.Count;
				for (int i = 0; i < len-1; i++)
				{
					Items.Add(Items[0]);
					Items.RemoveAt(0);
				}
			}
			base.Render(writer);
		}

		public string Source
		{
			get
			{
				return Source;
			}
			set
			{
				source = value;
			}
		}
		public bool AddEmptyText
		{
			get
			{
				return _addEmptyText;
			}
			set
			{
				_addEmptyText = value;
			}
		}
		public string EmptyText
		{
			get
			{
				return _emptyText;
			}
			set
			{
				_emptyText = value;
			}
		}
	}
}
