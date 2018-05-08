using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Testing
{
	public class TestGraphics
	{
		public static void TestSmartCrop()
		{
			lw.GraphicUtils.SmartCrop.SmartCrop sc = new lw.GraphicUtils.SmartCrop.SmartCrop();

			DirectoryInfo path = new DirectoryInfo("set2");

			foreach(FileInfo img in path.GetFiles("*.Jpg"))
				sc.Crop(img.FullName, "set2-res/" + img.Name, 512, 512);
		}
	}
}
