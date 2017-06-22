using System;

namespace lw.Threading
{
	public class Yearly : ThreadingBase
	{
		public Yearly()
			//: base(RepeatPattern.Yearly, new DateTime(2011, 5, 18, 12, 46, 0))
		{
		}
		public override void Action()
		{
			lw.WebTools.ErrorHandler.Log(string.Format("Thread running at: {0}, key: {1}", DateTime.Now, this.Key));
			return;
		}
	}
}
