using System;

namespace lw.Threading
{
	public class Weekly : ThreadingBase
	{
		public Weekly()
			//: base(RepeatPattern.Weekly, new DateTime(2012, 5, 11, 14, 40, 0))
		{
		}
		public override void Action()
		{
			lw.WebTools.ErrorHandler.Log(string.Format("Thread running at: {0}, key: {1}", DateTime.Now, this.Key));
			return;
		}
	}
}
