
using lw.Utils;

namespace lw.Testimonials
{
	public enum TestimonialStatus
	{
		NoDisplay = 0,
		Archive = 2,
		MainPage = 3
	}

	public enum TestimonialTagStatus
	{
		Inherit = 0,
		Archive = 2,
		MainPage = 3
	}

	public enum TestimonialsSort
	{
		Custom,

		[Description("Id Asc")]
		IdAsc,

		[Description("Id Desc")]
		IdDesc,

		[Description("Date Asc")]
		DateAsc,

		[Description("Date Desc")]
		DateDesc,

		[Description("DateModified Asc")]
		ModifiedDateAsc,

		[Description("DateModified Desc")]
		ModifiedDateDesc,
	}
}
