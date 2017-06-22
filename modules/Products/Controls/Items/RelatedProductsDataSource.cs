using System.Web.UI;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;

using lw.DataControls;
using lw.Data;
using lw.WebTools;


namespace lw.Products.Controls
{
	public class RelatedProductsDataSource : CustomDataSource
	{
		bool bound = false;
		EmptyDataSrc _dataSrc = null;

		public RelatedProductsDataSource()
		{
		}
		public override void DataBind()
		{
			if (bound)
				return;
			bound = true;

			int? itemId = null;

			object obj = ControlUtils.GetBoundedDataField(this.NamingContainer, "ItemId");
			if (obj != null)
				itemId = (int)obj;

			if (itemId != null)
			{
				_dataSrc = new EmptyDataSrc();

				SqlCommand cmd = DBUtils.StoredProcedure("[Shopping_GetRelatedProducts]", cte.lib);
				DBUtils.AddCommandParameter(cmd, "ItemId", SqlDbType.Int, itemId.Value, ParameterDirection.Input);
				DataTable dt = DBUtils.GetDataSet(cmd, cte.lib).Tables[0];

				this.Data = dt;

				_dataSrc.RowsCount = dt.Rows.Count;
				_dataSrc.Data = this.Data;
				_dataSrc.HasData = _dataSrc.RowsCount > 0;
			}

			base.DataBind();
		}


		public override Data.IDataSource DataSrc
		{
			get
			{
				return _dataSrc;
			}
			set
			{
				base.DataSrc = value;
			}
		}
	}
}