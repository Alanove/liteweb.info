using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;
using System.Collections.Generic;
using System.Drawing;

namespace lw.GraphicUtils.SmartCrop
{

	/// <summary>
	/// Created by flask on 2015. 10. 30..
	/// </summary>
	public class SmartCrop
	{
		private Options options;
		private int[] cd;

		public SmartCrop() : this(Options.DEFAULT)
		{
		}

		public SmartCrop(Options options)
		{
			this.options = options;
		}

		public static CropResult analyze(Options options, Bitmap input)
		{
			return (new SmartCrop(options)).analyze(input);
		}

		public virtual CropResult analyze(Bitmap input)
		{
			Image inputI = new Image(input);
			Image outputI = new Image(input.Width, input.Height);

			prepareCie(inputI);
			edgeDetect(inputI, outputI);
			skinDetect(inputI, outputI);
			saturationDetect(inputI, outputI);

			//Bitmap output = new Bitmap(input.Width, input.Height);

			//for (int i = 0; i < input.Width; i++)
			//	for (int j = 0; j < input.Height; j++) {
			//		output.SetPixel(i, j, );
			//	}


			Bitmap score = new Bitmap(input.Width / options.ScoreDownSample, input.Height / options.ScoreDownSample);//, options.BufferedBitmapType);
			Image scoreI = new Image(score);

			float topScore = float.NegativeInfinity;
			Crop topCrop = null;
			List<Crop> crops = this.crops(scoreI);

			//int l = 0;
			foreach (Crop crop in crops)
			{
				crop.score = this.score(scoreI, crop);
				if (crop.score.total > topScore)
				{
					topCrop = crop;
					topScore = crop.score.total;
				}
				crop.x *= options.ScoreDownSample;
				crop.y *= options.ScoreDownSample;
				crop.width *= options.ScoreDownSample;
				crop.height *= options.ScoreDownSample;

				//Crop(input, crop.width, crop.height, crop.x, crop.y).Save(lw.Utils.StringUtils.ToURL(DateTime.Now.Ticks.ToString()) + ".jpg");
			}

			CropResult result = new CropResult(topCrop, crops);

			//Graphics graphics = output.Graphics;
			//graphics.Color = Color.cyan;
			//if (topCrop != null)
			//{
			//	graphics.drawRect(topCrop.x, topCrop.y, topCrop.width, topCrop.height);
			//}

			return result;
		}

		public virtual Bitmap createCrop(Bitmap input, Crop crop)
		{
			//int tw = options.CropWidth;
			//int th = options.CropHeight;
			//BufferedImage image = new BufferedImage(tw, th, options.BufferedBitmapType);
			//image.Graphics.drawImage(input, 0, 0, tw, th, crop.x, crop.y, crop.x + crop.width, crop.y + crop.height, null);
			return input;
		}

		private Bitmap createScaleDown(Bitmap image, float ratio)
		{
			//BufferedImage scaled = new BufferedImage((int)(ratio * image.Width), (int)(ratio * image.Height), options.BufferedBitmapType);
			//scaled.Graphics.drawImage(image, 0, 0, scaled.Width, scaled.Height, 0, 0, scaled.Width, scaled.Height, null);
			return image;
		}

		private List<Crop> crops(Image image)
		{
			List<Crop> crops = new List<Crop>();
			int width = image.width;
			int height = image.height;
			int minDimension = Math.Min(width, height);

			for (float scale = options.MaxScale; scale >= options.MinScale; scale -= options.ScaleStep)
			{
				for (int y = 0; y + minDimension * scale <= height; y += options.ScoreDownSample)
				{
					for (int x = 0; x + minDimension * scale <= width; x += options.ScoreDownSample)
					{
						crops.Add(new Crop(x, y, (int)(minDimension * scale), (int)(minDimension * scale)));
					}
				}
			}
			return crops;
		}

		public void Crop(string PathFrom, string PathTo, 
			int Width,
			int Height)
		{
			using (System.Drawing.Image image = System.Drawing.Image.FromFile(PathFrom))
			{
				using(System.Drawing.Image res = Crop(image, Width, Height))
				{
					using (var res1 = ImageUtils.FixedSize(res, Width, Height))
					{
						ImageUtils.SaveJpeg(PathTo, res1, 80);
					}
				}
			}
		}

		public System.Drawing.Image Crop(System.Drawing.Image imgPhoto, int Width,
		 int Height, int x, int y)
		{
			
			Rectangle cropRect = new Rectangle(
				x,
				y,
				Width,
				Height);

			Bitmap target = new Bitmap(Width, Height);

			using (Graphics g = Graphics.FromImage(target))
			{
				g.DrawImage(imgPhoto,
					new Rectangle(0, 0, target.Width, target.Height),
					cropRect,
					GraphicsUnit.Pixel);
			}
			return target;
		}

		public System.Drawing.Image Crop(System.Drawing.Image imgPhoto, int Width,
		 int Height)
		{
			Options options = new Options();
			options.cropWidth(Width);
			options.cropHeight(Height);

			CropResult cropResult = analyze(options, (Bitmap)imgPhoto);

			return Crop(imgPhoto, cropResult.topCrop.width, cropResult.topCrop.height, cropResult.topCrop.x, cropResult.topCrop.y);
		}


		private Score score(Image output, Crop crop)
		{
			Score score = new Score();
			int[] od = output.RGB;
			int width = output.width;
			int height = output.height;

			for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{
					int p = y * width + x;
					float importance = this.importance(crop, x, y);
					float detail = (od[p] >> 8 & 0xff) / 255f;
					score.skin += (od[p] >> 16 & 0xff) / 255f * (detail + options.SkinBias) * importance;
					score.detail += detail * importance;
					score.saturation += (od[p] & 0xff) / 255f * (detail + options.SaturationBias) * importance;
				}
			}
			score.total = (score.detail * options.DetailWeight + score.skin * options.SkinWeight + score.saturation * options.SaturationWeight) / crop.width / crop.height;
			return score;
		}

		private float importance(Crop crop, int x, int y)
		{
			if (crop.x > x || x >= crop.x + crop.width || crop.y > y || y >= crop.y + crop.height)
			{
				return options.OutsideImportance;
			}
			float fx = (float)(x - crop.x) / crop.width;
			float fy = (float)(y - crop.y) / crop.height;
			float px = Math.Abs(0.5f - fx) * 2;
			float py = Math.Abs(0.5f - fy) * 2;
			// distance from edg;
			float dx = Math.Max(px - 1.0f + options.EdgeRadius, 0);
			float dy = Math.Max(py - 1.0f + options.EdgeRadius, 0);
			float d = (dx * dx + dy * dy) * options.EdgeWeight;
			d += (float)(1.4142135f - Math.Sqrt(px * px + py * py));
			if (options.RuleOfThirds)
			{
				d += (Math.Max(0, d + 0.5f) * 1.2f) * (thirds(px) + thirds(py));
			}
			return d;
		}

		internal class Image
		{
			internal Bitmap bufferedImage;
			internal int width, height;
			internal int[] data;

			public Image(int width, int height)
			{
				this.width = width;
				this.height = height;
				this.data = new int[width * height];
				for (int i = 0; i < this.data.Length; i++)
				{
					data[i] = unchecked((int)0xff000000);
				}
			}

			public Image(Bitmap bufferedImage) : this(bufferedImage.Width, bufferedImage.Height)
			{
				this.bufferedImage = bufferedImage;
				for (int i = 0; i < bufferedImage.Width; i++)
					for (int j = 0; j < bufferedImage.Height; j++)
						data[i + j] = bufferedImage.GetPixel(i, j).ToArgb();
				//this.data = bufferedImage.getRGB(0, 0, width, height, null, 0, width);
			}

			public virtual int[] RGB
			{
				get
				{
					return data;
				}
			}
		}

		private void prepareCie(Image i)
		{
			int[] id = i.RGB;
			cd = new int[id.Length];
			int w = i.width;
			int h = i.height;

			int p;
			for (int y = 0; y < h; y++)
			{
				for (int x = 0; x < w; x++)
				{
					p = y * w + x;
					cd[p] = cie(id[p]);
				}
			}
		}

		private void edgeDetect(Image i, Image o)
		{
			int[] od = o.RGB;
			int w = i.width;
			int h = i.height;
			int p;
			int lightness;

			for (int y = 0; y < h; y++)
			{
				for (int x = 0; x < w; x++)
				{
					p = y * w + x;
					if (x == 0 || x >= w - 1 || y == 0 || y >= h - 1)
					{
						lightness = 0;
					}
					else
					{
						lightness = cd[p] * 8 - cd[p - w - 1] - cd[p - w] - cd[p - w + 1] - cd[p - 1] - cd[p + 1] - cd[p + w - 1] - cd[p + w] - cd[p + w + 1];
					}

					od[p] = clamp(lightness) << 8 | (od[p] & unchecked((int)0xffff00ff));
				}
			}
		}

		private void skinDetect(Image i, Image o)
		{
			int[] id = i.RGB;
			int[] od = o.RGB;
			int w = i.width;
			int h = i.height;
			float invSkinThreshold = 255f / (1 - options.SkinThreshold);

			for (int y = 0; y < h; y++)
			{
				for (int x = 0; x < w; x++)
				{
					int p = y * w + x;
					float lightness = cd[p] / 255f;
					float skin = calcSkinColor(id[p]);
					if (skin > options.SkinThreshold && lightness >= options.SkinBrightnessMin && lightness <= options.SkinBrightnessMax)
					{
						int test = (int)(Math.Round((skin - options.SkinThreshold) * invSkinThreshold));
						od[p] = (test & 0xff) << 16 | (od[p] & unchecked((int)0xff00ffff));
					}
					else
					{
						od[p] &= unchecked((int)0xff00ffff);
					}
				}
			}
		}

		private void saturationDetect(Image i, Image o)
		{
			int[] id = i.RGB;
			int[] od = o.RGB;
			int w = i.width;
			int h = i.height;
			float invSaturationThreshold = 255f / (1 - options.SaturationThreshold);

			for (int y = 0; y < h; y++)
			{
				for (int x = 0; x < w; x++)
				{
					int p = y * w + x;
					float lightness = cd[p] / 255f;
					float sat = saturation(id[p]);
					if (sat > options.SaturationThreshold && lightness >= options.SaturationBrightnessMin && lightness <= options.SaturationBrightnessMax)
					{
						od[p] = ((int)Math.Round((sat - options.SaturationThreshold) * invSaturationThreshold) & 0xff) | (od[p] & unchecked((int)0xffffff00));
					}
					else
					{
						od[p] &= unchecked((int)0xffffff00);
					}
				}
			}
		}

		private float calcSkinColor(int rgb)
		{
			int r = rgb >> 16 & 0xff;
			int g = rgb >> 8 & 0xff;
			int b = rgb & 0xff;

			float mag = (float)Math.Sqrt(r * r + g * g + b * b);
			float rd = (r / mag - options.SkinColor[0]);
			float gd = (g / mag - options.SkinColor[1]);
			float bd = (b / mag - options.SkinColor[2]);
			return 1f - (float)Math.Sqrt(rd * rd + gd * gd + bd * bd);
		}

		private int clamp(int v)
		{
			return Math.Max(0, Math.Min(v, 0xff));
		}

		private int cie(int rgb)
		{
			int r = rgb >> 16 & 0xff;
			int g = rgb >> 8 & 0xff;
			int b = rgb & 0xff;
			return Math.Min(0xff, (int)(0.2126f * b + 0.7152f * g + 0.0722f * r + .5f));
		}

		private float saturation(int rgb)
		{
			float r = (rgb >> 16 & 0xff) / 255f;
			float g = (rgb >> 8 & 0xff) / 255f;
			float b = (rgb & 0xff) / 255f;

			float maximum = Math.Max(r, Math.Max(g, b));
			float minimum = Math.Min(r, Math.Min(g, b));
			if (maximum == minimum)
			{
				return 0;
			}
			float l = (maximum + minimum) / 2f;
			float d = maximum - minimum;
			return l > 0.5f ? d / (2f - maximum - minimum) : d / (maximum + minimum);
		}

		// gets value in the range of [0, 1] where 0 is the center of the pictures
		// returns weight of rule of thirds [0, 1]
		private float thirds(float x)
		{
			x = ((x - (1 / 3f) + 1.0f) % 2.0f * 0.5f - 0.5f) * 16f;
			return Math.Max(1.0f - x * x, 0);
		}
	}


}