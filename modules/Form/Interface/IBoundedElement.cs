using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace lw.Forms.Interface
{
	/// <summary>
	/// Interface for all data-bounded form elements
	/// </summary>
	public interface IBoundedElement
	{
		/// <summary>
		/// The data field from which this form should take the name to be used in the tag name attribute.
		/// Ex: NamingFrom="Id"
		/// </summary>
		string NamingFrom { get; set; }
		/// <summary>
		/// Format, the name of the field shold follow.
		/// Important: Do not forget to add {0}
		/// Ex: NamingFormat="Photo_{0}", in this case {0} will be replaced by the value of the NamingFrom field => Converted to Photo_1, Photo_2 (depending on the ids)/
		/// </summary>
		string NamingFormat { get; set; }
		/// <summary>
		/// The data field on which this tag should be bound.
		/// Ex: BoundTo="Photo", in this case the field's value will be the "Photo" value from the related data item.
		/// </summary>
		string BoundTo { get; set; }
		/// <summary>
		/// Format of the value of the field
		/// </summary>
		string Format { get; set; }
	}
}
