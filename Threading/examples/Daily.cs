using System;

namespace lw.Threading
{
	public class Daily : ThreadingBase
	{
		public Daily()
			//: base(RepeatPattern.Daily, new DateTime(2012, 5, 18, 11, 39, 0))
		{
		}
		public override void Action()
		{//send email or anything else
			lw.WebTools.ErrorHandler.Log(string.Format("Thread running at: {0}, key: {1}", DateTime.Now, this.Key));
			return;
		}
	}
}
