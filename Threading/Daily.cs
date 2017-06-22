using System;
using System.IO;

namespace lw.Threading
{
	class Daily : ThreadingBase
	{
		public Daily()
			: base(RepeatPattern.Daily, new DateTime(2012, 5, 17, 12, 09, 0))
		{
		}
		public override void Action()
		{
			string fileName = "D:\\Work\\ballout1.txt";

			StreamWriter sr = new StreamWriter(fileName, true);

			sr.WriteLine(DateTime.Now);

			sr.Close();
		}
	}
}
