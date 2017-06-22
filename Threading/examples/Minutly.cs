using System;

namespace lw.Threading
{
	public class Minutly : ThreadingBase
	{
		public Minutly()//:base(RepeatPattern.Minutly, new DateTime(2011, 1, 1, 9, 0, 3))
		{
		}
		public override void Action()
		{
			lw.WebTools.ErrorHandler.Log(string.Format("Thread running at: {0}, key: {1}", DateTime.Now, this.Key));
			return;
		}
	}
}
