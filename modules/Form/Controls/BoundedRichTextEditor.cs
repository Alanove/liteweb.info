using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Data;

using lw.Utils;
using lw.WebTools;
using lw.Forms.Interface;
using lw.Base;

namespace lw.Forms.Controls
{
	/// <summary>
	/// Creates a rich text editor <see cref="BoundedTextArea"/>.
	/// Rich editing functionalaties based on CK Editor.
	/// </summary>
	/// <remarks>
	/// For this to work the folder CKeditor must be in the js folder of the website /js/ckeditor.
	/// CKEditor is downloaded from the CKeditor website http://ckeditor.com/
	/// </remarks>
	public class BoundedRichTextEditor : BoundedTextArea
	{
		/// <summary>
		/// Binds the object to its datasource
		/// </summary>
		public override void DataBind()
		{
			CustomPage page = this.Page as CustomPage;

			page.RegisterScriptFile(lw.CTE.Files.CkEditor, lw.CTE.Files.CkEditorFile);


			this.Attributes.Add("class", "ckeditor");
			base.DataBind();
		}
	}
}
