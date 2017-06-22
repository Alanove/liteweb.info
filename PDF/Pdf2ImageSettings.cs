using System.ComponentModel;

namespace Cyotek.GhostScript.PdfConversion
{
  [TypeConverter(typeof(ExpandableObjectConverter))]
  public class Pdf2ImageSettings : INotifyPropertyChanged
  {
  #region  Private Member Declarations  

    private AntiAliasMode _antiAliasMode;
    private int _dpi;
    private GridFitMode _gridFitMode;
    private ImageFormat _imageFormat;
    private PaperSize _paperSize;
    private PdfTrimMode _trimMode;

  #endregion  Private Member Declarations  

  #region  Public Constructors  

    public Pdf2ImageSettings()
    {
      this.Dpi = 150;
      this.GridFitMode = GridFitMode.Topological;
      this.AntiAliasMode = AntiAliasMode.High;
      this.ImageFormat = ImageFormat.Png24;
      this.PaperSize = PaperSize.Default;
      this.TrimMode = PdfTrimMode.CropBox;
    }

  #endregion  Public Constructors  

  #region  Events  

    public event PropertyChangedEventHandler PropertyChanged;

  #endregion  Events  

  #region  Public Properties  

    [DefaultValue(typeof(AntiAliasMode), "High")]
    public AntiAliasMode AntiAliasMode
    {
      get { return _antiAliasMode; }
      set
      {
        if (this.AntiAliasMode != value)
        {
          _antiAliasMode = value;
          this.OnPropertyChanged(new PropertyChangedEventArgs("AntiAliasMode"));
        }
      }
    }

    [DefaultValue(150)]
    public int Dpi
    {
      get { return _dpi; }
      set
      {
        if (this.Dpi != value)
        {
          _dpi = value;
          this.OnPropertyChanged(new PropertyChangedEventArgs("Dpi"));
        }
      }
    }

    [DefaultValue(typeof(GridFitMode), "Topological")]
    public GridFitMode GridFitMode
    {
      get { return _gridFitMode; }
      set
      {
        if (this.GridFitMode != value)
        {
          _gridFitMode = value;
          this.OnPropertyChanged(new PropertyChangedEventArgs("GridFitMode"));
        }
      }
    }

    [DefaultValue(typeof(ImageFormat), "Png24")]
    public ImageFormat ImageFormat
    {
      get { return _imageFormat; }
      set
      {
        if (this.ImageFormat != value)
        {
          _imageFormat = value;
          this.OnPropertyChanged(new PropertyChangedEventArgs("ImageFormat"));
        }
      }
    }

    [DefaultValue(typeof(PaperSize), "Default")]
    public PaperSize PaperSize
    {
      get { return _paperSize; }
      set
      {
        if (this.PaperSize != value)
        {
          _paperSize = value;
          this.OnPropertyChanged(new PropertyChangedEventArgs("PaperSize"));
        }
      }
    }

    [DefaultValue(typeof(PdfTrimMode), "CropBox")]
    public PdfTrimMode TrimMode
    {
      get { return _trimMode; }
      set
      {
        if (this.TrimMode != value)
        {
          _trimMode = value;
          this.OnPropertyChanged(new PropertyChangedEventArgs("TrimMode"));
        }
      }
    }

  #endregion  Public Properties  

  #region  Protected Methods  

    protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
    {
      if (this.PropertyChanged != null)
        this.PropertyChanged(this, e);
    }

  #endregion  Protected Methods  
  }
}
