using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace lw.GraphicUtils.SmartCrop
{
	public class Options
	{
		public static readonly Options DEFAULT = new Options();

		//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		private int cropWidth_Renamed = 500;
		//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		private int cropHeight_Renamed = 500;
		//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		private float detailWeight_Renamed = .2f;
		//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		private float[] skinColor_Renamed = new float[] { 0.7f, 0.57f, 0.44f };
		//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		private float skinBias_Renamed = .01f;
		//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		private float skinBrightnessMin_Renamed = 0.2f;
		//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		private float skinBrightnessMax_Renamed = 1.0f;
		//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		private float skinThreshold_Renamed = 0.8f;
		//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		private float skinWeight_Renamed = 1.8f;
		//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		private float saturationBrightnessMin_Renamed = 0.05f;
		//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		private float saturationBrightnessMax_Renamed = 0.9f;
		//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		private float saturationThreshold_Renamed = 0.4f;
		//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		private float saturationBias_Renamed = 0.2f;
		//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		private float saturationWeight_Renamed = 0.3f;
		// step * minscale rounded down to the next power of two should be good
		//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		private int scoreDownSample_Renamed = 25;
		//	private int step = 8;
		//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		private float scaleStep_Renamed = 0.1f;
		//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		private float minScale_Renamed = 0.8f;
		//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		private float maxScale_Renamed = 1.0f;
		//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		private float edgeRadius_Renamed = 0.4f;
		//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		private float edgeWeight_Renamed = -20f;
		//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		private float outsideImportance_Renamed = -.5f;
		//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		private bool ruleOfThirds_Renamed = false;
		//JAVA TO C# CONVERTER NOTE: Fields cannot have the same name as methods:
		private int bufferedBitmapType_Renamed = 1;// BufferedImage.TYPE_INT_ARGB;

		public virtual int CropWidth
		{
			get
			{
				return cropWidth_Renamed;
			}
		}

		public virtual Options cropWidth(int cropWidth)
		{
			this.cropWidth_Renamed = cropWidth;
			return this;
		}

		public virtual int CropHeight
		{
			get
			{
				return cropHeight_Renamed;
			}
		}

		public virtual Options cropHeight(int cropHeight)
		{
			this.cropHeight_Renamed = cropHeight;
			return this;
		}

		public virtual float DetailWeight
		{
			get
			{
				return detailWeight_Renamed;
			}
		}

		public virtual Options detailWeight(float detailWeight)
		{
			this.detailWeight_Renamed = detailWeight;
			return this;
		}

		public virtual float[] SkinColor
		{
			get
			{
				return skinColor_Renamed;
			}
		}

		public virtual Options skinColor(float[] skinColor)
		{
			this.skinColor_Renamed = skinColor;
			return this;
		}

		public virtual float SkinBias
		{
			get
			{
				return skinBias_Renamed;
			}
		}

		public virtual Options skinBias(float skinBias)
		{
			this.skinBias_Renamed = skinBias;
			return this;
		}

		public virtual float SkinBrightnessMin
		{
			get
			{
				return skinBrightnessMin_Renamed;
			}
		}

		public virtual Options skinBrightnessMin(float skinBrightnessMin)
		{
			this.skinBrightnessMin_Renamed = skinBrightnessMin;
			return this;
		}

		public virtual float SkinBrightnessMax
		{
			get
			{
				return skinBrightnessMax_Renamed;
			}
		}

		public virtual Options skinBrightnessMax(float skinBrightnessMax)
		{
			this.skinBrightnessMax_Renamed = skinBrightnessMax;
			return this;
		}

		public virtual float SkinThreshold
		{
			get
			{
				return skinThreshold_Renamed;
			}
		}

		public virtual Options skinThreshold(float skinThreshold)
		{
			this.skinThreshold_Renamed = skinThreshold;
			return this;
		}

		public virtual float SkinWeight
		{
			get
			{
				return skinWeight_Renamed;
			}
		}

		public virtual Options skinWeight(float skinWeight)
		{
			this.skinWeight_Renamed = skinWeight;
			return this;
		}

		public virtual float SaturationBrightnessMin
		{
			get
			{
				return saturationBrightnessMin_Renamed;
			}
		}

		public virtual Options saturationBrightnessMin(float saturationBrightnessMin)
		{
			this.saturationBrightnessMin_Renamed = saturationBrightnessMin;
			return this;
		}

		public virtual float SaturationBrightnessMax
		{
			get
			{
				return saturationBrightnessMax_Renamed;
			}
		}

		public virtual Options saturationBrightnessMax(float saturationBrightnessMax)
		{
			this.saturationBrightnessMax_Renamed = saturationBrightnessMax;
			return this;
		}

		public virtual float SaturationThreshold
		{
			get
			{
				return saturationThreshold_Renamed;
			}
		}

		public virtual Options saturationThreshold(float saturationThreshold)
		{
			this.saturationThreshold_Renamed = saturationThreshold;
			return this;
		}

		public virtual float SaturationBias
		{
			get
			{
				return saturationBias_Renamed;
			}
		}

		public virtual Options saturationBias(float saturationBias)
		{
			this.saturationBias_Renamed = saturationBias;
			return this;
		}

		public virtual float SaturationWeight
		{
			get
			{
				return saturationWeight_Renamed;
			}
		}

		public virtual Options saturationWeight(float saturationWeight)
		{
			this.saturationWeight_Renamed = saturationWeight;
			return this;
		}

		public virtual int ScoreDownSample
		{
			get
			{
				return scoreDownSample_Renamed;
			}
		}

		public virtual Options scoreDownSample(int scoreDownSample)
		{
			this.scoreDownSample_Renamed = scoreDownSample;
			return this;
		}

		public virtual float ScaleStep
		{
			get
			{
				return scaleStep_Renamed;
			}
		}

		public virtual Options scaleStep(float scaleStep)
		{
			this.scaleStep_Renamed = scaleStep;
			return this;
		}

		public virtual float MinScale
		{
			get
			{
				return minScale_Renamed;
			}
		}

		public virtual Options minScale(float minScale)
		{
			this.minScale_Renamed = minScale;
			return this;
		}

		public virtual float MaxScale
		{
			get
			{
				return maxScale_Renamed;
			}
		}

		public virtual Options maxScale(float maxScale)
		{
			this.maxScale_Renamed = maxScale;
			return this;
		}

		public virtual float EdgeRadius
		{
			get
			{
				return edgeRadius_Renamed;
			}
		}

		public virtual Options edgeRadius(float edgeRadius)
		{
			this.edgeRadius_Renamed = edgeRadius;
			return this;
		}

		public virtual float EdgeWeight
		{
			get
			{
				return edgeWeight_Renamed;
			}
		}

		public virtual Options edgeWeight(float edgeWeight)
		{
			this.edgeWeight_Renamed = edgeWeight;
			return this;
		}

		public virtual float OutsideImportance
		{
			get
			{
				return outsideImportance_Renamed;
			}
		}

		public virtual Options outsideImportance(float outsideImportance)
		{
			this.outsideImportance_Renamed = outsideImportance;
			return this;
		}

		public virtual bool RuleOfThirds
		{
			get
			{
				return ruleOfThirds_Renamed;
			}
		}

		public virtual Options ruleOfThirds(bool ruleOfThirds)
		{
			this.ruleOfThirds_Renamed = ruleOfThirds;
			return this;
		}

		public virtual int BufferedBitmapType
		{
			get
			{
				return bufferedBitmapType_Renamed;
			}
		}

		public virtual Options bufferedBitmapType(int bufferedBitmapType)
		{
			this.bufferedBitmapType_Renamed = bufferedBitmapType;
			return this;
		}
	}
}