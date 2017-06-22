using System.Collections;
using System.Web.UI.WebControls;


namespace lw.DataControls
{
	public class CustomGridView :System.Web.UI.WebControls.GridView
	{
		public CustomGridView()
		{
		}
		protected override System.Collections.ICollection CreateColumns(System.Web.UI.WebControls.PagedDataSource dataSource, bool useDataSource)
		{
			DataControlFieldCollection MarkupColumns = base.Columns.CloneFields();
			ArrayList GeneratedColumns = (ArrayList)base.CreateColumns(dataSource, useDataSource);
			ArrayList FinalColumns = new ArrayList();
			for (int i = MarkupColumns.Count; i < GeneratedColumns.Count; i++ )
			{
				FinalColumns.Add(GeneratedColumns[i]);
			}
			for (int i = 0; i < MarkupColumns.Count; i++)
			{
				FinalColumns.Add(MarkupColumns[i]);
			}
			return FinalColumns;
		}
	}
}