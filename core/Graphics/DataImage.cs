using System.Drawing;

namespace lw.GraphicUtils
{
	public class DataImages
	{
		public static System.Drawing.Bitmap DataImage
			(string str,
			string font,
			int fontSize,
			int fontRed,
			int fontGreen,
			int fontBlue)
		{
			int width = str.Length*8;
			int height = 16;
			Bitmap bt = new Bitmap(width, height);
			System.Drawing.Graphics gr = System.Drawing.Graphics.FromImage(bt);
			gr.FillRectangle(new SolidBrush(Color.White), 
				0, 0, width, height);

			gr.DrawString(str, 
				new Font(font, fontSize, FontStyle.Regular), 
				new SolidBrush(Color.FromArgb(fontRed, fontGreen, fontBlue)), 0, 0);
			
			return bt;
		}
	}
}
