using System;
using System.Data;

using lw.WebTools;

namespace lw.Quotes
{
	public class QuotesManager
	{

		Quotes ds;

		string config = cte.ConfigFile;

		public QuotesManager()
		{
		}

		#region Methods

		/* Start Testimonials */

		public bool AddQuote(string quote, string author, DateTime date)
		{
			Quotes.QuotesRow row = DS._Quotes.NewQuotesRow();

			row.Quote = quote;
			row.Author = author;
			row.Date = date;

			ds._Quotes.Rows.Add(row);

			AcceptChanges();

			return true;
		}

		public bool UpdateQuote(int Id, string quote, string author, DateTime date)
		{
			DataView dv = this.GetQuotes(string.Format("Id = {0}", Id));

			if (dv.Count <= 0)
				return false;

			Quotes.QuotesRow row = (Quotes.QuotesRow)dv[0].Row;

			row.Quote = quote;
			row.Author = author;
			row.Date = date;

			row.Table.AcceptChanges();
			AcceptChanges();

			return true;
		}

		public bool DeleteQuote(int Id)
		{
			DataView dv = this.GetQuotes(string.Format("Id = {0}", Id));

			if (dv.Count <= 0)
				return false;

			Quotes.QuotesRow row = (Quotes.QuotesRow)dv[0].Row;

			row.Delete();
			AcceptChanges();

			return true;
		}

		public Quotes.QuotesRow GetQuote(int Id)
		{
			DataView dv = GetQuotes(string.Format("{0}", Id));
			Quotes.QuotesRow row = (Quotes.QuotesRow)dv[0].Row;
			return row;
		}
		public DataView GetQuotes()
		{
			return GetQuotes("");
		}
		public DataView GetQuotes(string cond)
		{
			return new DataView(DS._Quotes, cond, "Id", DataViewRowState.CurrentRows);
		}

		public DataRow GetRandomQuote()
		{
			DataView dv = GetQuotes();
			dv = lw.Data.DBUtils.Randomize(dv.Table, dv.Count);
			return dv[0].Row;
		}
		
		/* End Testimonial Groups */

		void AcceptChanges()
		{
			DS.AcceptChanges();
			XmlManager.SetDataSet(config, DS);
		}

		#endregion

		#region variables
		public Quotes DS
		{
			get
			{
				if (ds == null)
				{
					ds = new Quotes();
					DataSet _ds = XmlManager.GetDataSet(config);
					if (_ds != null)
						ds.Merge(_ds);
				}
				return ds;
			}
		}
		#endregion
	}
}
