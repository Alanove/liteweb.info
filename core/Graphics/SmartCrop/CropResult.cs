using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;

namespace lw.GraphicUtils.SmartCrop
{
	public class CropResult
	{
		public Crop topCrop;
		public List<Crop> crops;
		public Bitmap debugImage;
		public Bitmap resultImage;

		public CropResult(Crop topCrop, List<Crop> crops, Bitmap debugImage, Bitmap resultImage)
		{
			this.topCrop = topCrop;
			this.crops = crops;
			this.debugImage = debugImage;
			this.resultImage = resultImage;
		}


		public CropResult(Crop topCrop, List<Crop> crops)
		{
			this.topCrop = topCrop;
			this.crops = crops;
		}

		static CropResult newInstance(Crop topCrop, List<Crop> crops, Bitmap debugImage, Bitmap resultImage)
		{
			return new CropResult(topCrop, crops, debugImage, resultImage);
		}
	}
}
