
using lw.Utils;

namespace lw.Ads
{
	public enum AdStatus
	{
		Default = 1,
		Active = 2,
		Expired = 3,
		Disabled = 4
	}
	public enum AdPaymentTypes
	{
		None = 1,

		[Description("Per Impression")]
		PerImpression = 2,

		[Description("Per Click")]
		PerClick = 3//,

		//[Description("Per Acquisition")]
		//PerAcquisition = 3
	}

	public enum AdRenderType
	{
		Image, Html, Name
	}
}
