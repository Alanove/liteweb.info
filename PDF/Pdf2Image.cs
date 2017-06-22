using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;

namespace Cyotek.GhostScript.PdfConversion
{
  // Cyotek PDF to Image Conversion
  // Copyright (c) 2011 Cyotek. All Rights Reserved.
  // http://cyotek.com

  // If you use this library in your applications, attribution or donations are welcome.

  public class Pdf2Image
  {
  #region  Private Member Declarations  

    private string _pdfFileName;
    private int _pdfPageCount;

  #endregion  Private Member Declarations  

  #region  Public Constructors  

    public Pdf2Image()
    {
      this.Settings = new Pdf2ImageSettings();
    }

    public Pdf2Image(string pdfFileName)
      : this()
    {
      if (string.IsNullOrEmpty(pdfFileName))
        throw new ArgumentNullException("pdfFileName");

      if (!File.Exists(pdfFileName))
        throw new FileNotFoundException();

      this.PdfFileName = pdfFileName;
    }

    public Pdf2Image(string pdfFileName, Pdf2ImageSettings settings)
      : this(pdfFileName)
    {
      if (settings == null)
        throw new ArgumentNullException("setting");

      this.Settings = settings;
    }

  #endregion  Public Constructors  

  #region  Public Methods  

    public void ConvertPdfPageToImage(string outputFileName, int pageNumber)
    {
      if (pageNumber < 1 || pageNumber > this.PageCount)
        throw new ArgumentException("Page number is out of bounds", "pageNumber");

      using (GhostScriptAPI api = new GhostScriptAPI())
        api.Execute(this.GetConversionArguments(this._pdfFileName, outputFileName, pageNumber, this.PdfPassword, this.Settings));
    }

    public Bitmap GetImage()
    {
      return this.GetImage(1);
    }

    public Bitmap GetImage(int pageNumber)
    {
      Bitmap result;
      string workFile;

      if (pageNumber < 1 || pageNumber > this.PageCount)
        throw new ArgumentException("Page number is out of bounds", "pageNumber");

      workFile = Path.GetTempFileName();

      try
      {
        this.ConvertPdfPageToImage(workFile, pageNumber);
        using (FileStream stream = new FileStream(workFile, FileMode.Open, FileAccess.Read))
          result = new Bitmap(stream);
      }
      finally
      {
        File.Delete(workFile);
      }

      return result;
    }

    public Bitmap[] GetImages()
    {
      return this.GetImages(1, this.PageCount);
    }

    public Bitmap[] GetImages(int startPage, int lastPage)
    {
      List<Bitmap> results;

      if (startPage < 1 || startPage > this.PageCount)
        throw new ArgumentException("Start page number is out of bounds", "startPage");

      if (lastPage < 1 || lastPage > this.PageCount)
        throw new ArgumentException("Last page number is out of bounds", "lastPage");
      else if (lastPage < startPage)
        throw new ArgumentException("Last page cannot be less than start page", "lastPage");

      results = new List<Bitmap>();
      for (int i = startPage; i <= lastPage; i++)
        results.Add(this.GetImage(i));

      return results.ToArray();
    }

  #endregion  Public Methods  

  #region  Public Properties  

    [DefaultValue(0)]
    public int PageCount
    {
      get
      {
        if (_pdfPageCount == 0 && !string.IsNullOrEmpty(_pdfFileName))
          _pdfPageCount = this.GetPdfPageCount(_pdfFileName);

        return _pdfPageCount;
      }
    }

    [DefaultValue("")]
    public string PdfFileName
    {
      get { return _pdfFileName; }
      set { _pdfFileName = value; }
    }

    [DefaultValue("")]
    public string PdfPassword { get; set; }

    public Pdf2ImageSettings Settings { get; set; }

  #endregion  Public Properties  

  #region  Private Methods  

    private int GetPdfPageCount(string fileName)
    {
      // http://stackoverflow.com/questions/320281/determine-number-of-pages-in-a-pdf-file-using-c-net-2-0

      return Regex.Matches(File.ReadAllText(fileName), @"/Type\s*/Page[^s]").Count;
    }

  #endregion  Private Methods  

  #region  Protected Methods  

    protected virtual IDictionary<GhostScriptCommand, object> GetConversionArguments(string pdfFileName, string outputImageFileName, int pageNumber, string password, Pdf2ImageSettings settings)
    {
      Dictionary<GhostScriptCommand, object> arguments;

      arguments = new Dictionary<GhostScriptCommand, object>();

      // basic GhostScript setup
      arguments.Add(GhostScriptCommand.Silent, null);
      arguments.Add(GhostScriptCommand.Safer, null);
      arguments.Add(GhostScriptCommand.Batch, null);
      arguments.Add(GhostScriptCommand.NoPause, null);

      // specify the output
      arguments.Add(GhostScriptCommand.Device, GhostScriptAPI.GetDeviceName(settings.ImageFormat));
      arguments.Add(GhostScriptCommand.OutputFile, outputImageFileName);

      // page numbers
      arguments.Add(GhostScriptCommand.FirstPage, pageNumber);
      arguments.Add(GhostScriptCommand.LastPage, pageNumber);

      // graphics options
      arguments.Add(GhostScriptCommand.UseCIEColor, null);

      if (settings.AntiAliasMode != AntiAliasMode.None)
      {
        arguments.Add(GhostScriptCommand.TextAlphaBits, settings.AntiAliasMode);
        arguments.Add(GhostScriptCommand.GraphicsAlphaBits, settings.AntiAliasMode);
      }

      arguments.Add(GhostScriptCommand.GridToFitTT, settings.GridFitMode);

      // image size
      if (settings.TrimMode != PdfTrimMode.PaperSize)
        arguments.Add(GhostScriptCommand.Resolution, settings.Dpi.ToString());

      switch (settings.TrimMode)
      {
        case PdfTrimMode.PaperSize:
          if (settings.PaperSize != PaperSize.Default)
          {
            arguments.Add(GhostScriptCommand.FixedMedia, true);
            arguments.Add(GhostScriptCommand.PaperSize, settings.PaperSize);
          }
          break;
        case PdfTrimMode.TrimBox:
          arguments.Add(GhostScriptCommand.UseTrimBox, true);
          break;
        case PdfTrimMode.CropBox:
          arguments.Add(GhostScriptCommand.UseCropBox, true);
          break;
      }

      // pdf password
      if (!string.IsNullOrEmpty(password))
        arguments.Add(GhostScriptCommand.PDFPassword, password);

      // pdf filename
      arguments.Add(GhostScriptCommand.InputFile, pdfFileName);

      return arguments;
    }

  #endregion  Protected Methods  
  }
}

