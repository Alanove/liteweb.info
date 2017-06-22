using System;

namespace lw.Threading
{
	public class Monthly : ThreadingBase
	{
		public Monthly()
			//: base(RepeatPattern.Monthly, new DateTime(2012, 4, 21, 9, 02, 0))
		{
		}
		public override void Action()
		{
			lw.WebTools.ErrorHandler.Log(string.Format("Thread running at: {0}, key: {1}", DateTime.Now, this.Key));
			return;
		}
	}
}
