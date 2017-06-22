using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Xml.Linq;

namespace lw.WebTools
{
	/// <summary>
	/// Provide utilities methods related to <see cref="Control"/> objects
	/// </summary>
	public static class ControlUtils
	{
		/// <summary>
		/// Returns the bounded field to a NamingContainer.
		/// 1 - It tries the regular methos
		/// 2 - Convers the NamingContainer.dataItem to datarows
		/// 3 - to DataRowView
		/// </summary>
		/// <param name="NamingContainer">Usually this.NamingContainer should work</param>
		/// <param name="DataField">The database field or property</param>
		/// <returns>The value of the property</returns>
		public static object GetBoundedDataField(Control NamingContainer, string DataField)
		{
			return GetBoundedDataField(NamingContainer, DataField, false);
		}

		/// <summary>
		/// Returns the bounded field to a NamingContainer.
		/// 1 - It tries the regular methos
		/// 2 - Convers the NamingContainer.dataItem to datarows
		/// 3 - to DataRowView
		/// </summary>
		/// <param name="NamingContainer">Usually this.NamingContainer should work</param>
		/// <param name="DataField">The database field or property</param>
		/// <param name="LoadfromDraft">Defines if the property should load from draft (draft is usually an xml field wihtin the table that holds the draft fields of the row)</param>
		/// <returns>The value of the property</returns>
		public static object GetBoundedDataField(Control NamingContainer, string DataField, bool LoadfromDraft)
		{
			object DataObj = null;
			if (LoadfromDraft)
			{
				string xml = "";
				bool loaded = false;
				try
				{
					object obj = DataBinder.Eval(NamingContainer, "DataItem.History");
					if (obj != DBNull.Value && obj != null)
						xml = obj.ToString();
				}
				catch
				{
					DataRow row = DataBinder.Eval(NamingContainer, "DataItem") as DataRow;
					if (row == null)
					{
						DataRowView rv = DataBinder.Eval(NamingContainer, "DataItem") as DataRowView;
						if (rv == null)
						{
							return GetBoundedDataField(NamingContainer, DataField, false);
						}
						row = rv.Row;
					}
					if (row.Table.Columns.Contains("History"))
					{
						xml = row["History"].ToString();
					}
				}
				if (!String.IsNullOrWhiteSpace(xml))
				{
					XDocument draft = XDocument.Parse(xml);

					if (draft.Root.Element("draft") != null)
					{
						loaded = true;
						XElement temp = draft.Root.Element("draft").Element(DataField);
						if (temp != null)
						{
							DataObj = temp.Value;
						}
					}
				}
				if (!loaded)
					return GetBoundedDataField(NamingContainer, DataField, false);
			}
			else
			{
				try
				{
					return DataBinder.Eval(NamingContainer, "DataItem." + DataField);
				}
				catch
				{
					DataRow row = DataBinder.Eval(NamingContainer, "DataItem") as DataRow;
					if (row == null)
					{
						DataRowView rv = DataBinder.Eval(NamingContainer, "DataItem") as DataRowView;
						if (rv == null)
						{
							throw new Exception("Invalid Property: " + DataField);
						}
						else
							DataObj = rv[DataField];
					}
					else
						DataObj = row[DataField];
				}

			}

			return DataObj;
		}

		/// <summary>
		/// Find the first ancestor of the selected control in the control tree
		/// </summary>
		/// <typeparam name="TControl">Type of the ancestor to look for</typeparam>
		/// <param name="control">The control to look for its ancestors</param>
		/// <returns>The first ancestor of the specified type, or null if no ancestor is found.</returns>
		public static TControl FindAncestor<TControl>(this Control control) where TControl : Control
		{
			if (control == null) throw new ArgumentNullException("control");

			Control parent = control;
			do
			{
				parent = parent.Parent;
				var candidate = parent as TControl;
				if (candidate != null)
				{
					return candidate;
				}
			} while (parent != null);
			return null;
		}

		/// <summary>
		/// Finds all descendants of a certain type of the specified control.
		/// </summary>
		/// <typeparam name="TControl">The type of descendant controls to look for.</typeparam>
		/// <param name="parent">The parent control where to look into.</param>
		/// <returns>All corresponding descendants</returns>
		public static IEnumerable<TControl> FindDescendants<TControl>(this Control parent) where TControl : Control
		{
			if (parent == null) throw new ArgumentNullException("control");

			if (parent.HasControls())
			{
				foreach (Control childControl in parent.Controls)
				{
					var candidate = childControl as TControl;
					if (candidate != null) yield return candidate;

					foreach (var nextLevel in FindDescendants<TControl>(childControl))
					{
						yield return nextLevel;
					}
				}
			}
		}
	}
}
