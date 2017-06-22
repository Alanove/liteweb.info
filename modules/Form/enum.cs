
using lw.Utils;

namespace lw.Forms
{
	/// <summary>
	/// Types of form submission action
	/// </summary>
	public enum FormMethod
	{
		Post, Get
	}

	/// <summary>
	/// Types of Forms
	/// </summary>
	public enum FormEncType
	{
		[Description("")]
		None,

		[Description("application/x-www-form-urlencoded")]
		Form,

		[Description("multipart/form-data")]
		Data
	}

	/// <summary>
	/// Defines the expected datatype for form fields to be used in validation
	/// </summary>
	public enum DataType
	{
		String,
		Number,
		Integer,
		Decimal,
		Email,
		Date,
		Image,
		Picture,
		CheckBox,
		RadioList,
		RegularExpression,
		File,
		Array,
		BitwiseNumber,
		HTMLSelect
	}

	/// <summary>
	/// Defines the type of camparisions that might be used for CompareTo validation behavior
	/// </summary>
	public enum CompareCondition
	{
		[Description("=")]
		Equal,

		[Description("<")]
		LessThan,

		[Description(">")]
		GreaterThan,

		[Description("<>")]
		Different,

		[Description(".")]
		HaveValue
	}
}
