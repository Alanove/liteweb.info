using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace lw.GraphicUtils.Filters
{
  /// <summary>
  /// Summary description for Class1.
  /// </summary>
  public class SkewFilter : BasicFilter
  {
    #region Basic Members
    public const string RIGHT_SHIFT_TOKEN_NAME = "RightShift";
    public const string UP_SHIFT_TOKEN_NAME = "UpShift";

    private int _rightShift = -20; //Defualts
    private int _upShift = 0;


    public int RightShift
    {
      get
      {
        return _rightShift;
      }
      set
      {
        _rightShift = value;
      }
    }

    public int UpShift
    {
      get
      {
        return _upShift;
      }
      set
      {
        _upShift = value;
      }
    }

    /// <summary>
    /// Executes this filter on the input image and returns the result
    /// </summary>
    /// <param name="inputImage">input image</param>
    /// <returns>transformed image</returns>
    /// <example>
    /// <code>
    /// SkewFilter skewFilter = new SkewFilter();
    /// skewFilter.UpShift = 0;
    /// skewFilter.RightShift = 5;
    /// transformed = skewFilter.ExecuteFilter(myImg);
    /// </code>
    /// </example>
    public override Image ExecuteFilter(Image rawImage)
    {

      Bitmap result = new Bitmap(rawImage.Width + Math.Abs(_rightShift), rawImage.Height + Math.Abs(_upShift));
      Graphics g = Graphics.FromImage(result);
	  
		Pen pen = new Pen(this.BackGroundColor);

		Brush backGroundBrush = new SolidBrush(BackGroundColor);

		g.FillRectangle(backGroundBrush, 0, 0, result.Width, result.Height);

      g.InterpolationMode = InterpolationMode.HighQualityBicubic;
		g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
		Point[] points = new Point[3];
      int horShiftCorrections = 0;
      int verShiftCorrections = 0;
      if (_rightShift < 0)
      {
        horShiftCorrections = _rightShift * (-1);
      }
      if (_upShift < 0)
      {
        verShiftCorrections = _upShift * (-1);
      }
      points[0] = new Point(horShiftCorrections + _rightShift, verShiftCorrections);
      points[1] = new Point(horShiftCorrections + _rightShift + rawImage.Width, verShiftCorrections + _upShift);
      points[2] = new Point(horShiftCorrections, verShiftCorrections+rawImage.Height );
		

      try
      {
        g.DrawImage(rawImage,points);
      }
      catch (Exception e)
      {

      }
      return result;
    }


    #endregion

  }
}
