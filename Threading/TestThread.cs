using lw.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lw.Threading
{
	public class TestThread : ThreadingBase
    {
		 public override void Action()
		{
			 lw.WebTools.ErrorHandler.Log(string.Format("Thread running at: {0}, key: {1}", DateTime.Now, this.Key));
		}
	}
}
