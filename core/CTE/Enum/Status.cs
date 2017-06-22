
namespace lw.CTE.Enum
{
	public enum Status
	{
		Disabled = 1, //2^0
		Enabled = 2, // 2^1
		Pending = 4, // 2^2 
		Flagged = 10, //2^3|2
		Deleted = 16, //2^4|1
		Modified = 34 //2^5|2
	}
}
