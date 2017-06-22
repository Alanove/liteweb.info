using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace Cyotek.GhostScript
{
  // Cyotek GhostScript API Integration
  // Copyright (c) 2011 Cyotek. All Rights Reserved.
  // http://cyotek.com

  // If you use this library in your applications, attribution or donations are welcome.

  /// <summary>
  ///  Wrapper class for the GhostScript native API
  /// </summary>
  public class GhostScriptAPI : IDisposable
  {
  #region  Private Member Declarations  

    private GCHandle[] _argumentHandles;
    private GCHandle _argumentPointersHandle;
    private IntPtr _ghostScriptInstance;

  #endregion  Private Member Declarations  

  #region  Public Constructors  

    /// <summary>
    ///  Initializes a new instance of the GhostScriptAPI class.
    /// </summary>
    public GhostScriptAPI()
    {
      NativeMethods.gsapi_new_instance(out _ghostScriptInstance, IntPtr.Zero);
    }

  #endregion  Public Constructors  

  #region  Public Class Methods  

    /// <summary>
    ///  Returns the GhostScript command name for the given enumerated command
    /// </summary>
    /// <exception cref="ArgumentException">  Thrown when one or more arguments have unsupported or illegal values. </exception>
    /// <param name="command">  The command. </param>
    /// <returns>
    ///  The name of the specified command required for use with the GhostScript API.
    /// </returns>
    public static string GetCommandName(GhostScriptCommand command)
    {
      string result;

      switch (command)
      {
        case GhostScriptCommand.ColorScreen:
          result = "dCOLORSCREEN";
          break;
        case GhostScriptCommand.DitherPPI:
          result = "dDITHERPPI";
          break;
        case GhostScriptCommand.Interpolate:
          result = "dDOINTERPOLATE";
          break;
        case GhostScriptCommand.NoInterpolate:
          result = "dNOINTERPOLATE";
          break;
        case GhostScriptCommand.TextAlphaBits:
          result = "dTextAlphaBits";
          break;
        case GhostScriptCommand.GraphicsAlphaBits:
          result = "dGraphicsAlphaBits";
          break;
        case GhostScriptCommand.AlignToPixels:
          result = "dAlignToPixels";
          break;
        case GhostScriptCommand.GridToFitTT:
          result = "dGridFitTT";
          break;
        case GhostScriptCommand.UseCIEColor:
          result = "dUseCIEColor";
          break;
        case GhostScriptCommand.NoCIE:
          result = "dNOCIE";
          break;
        case GhostScriptCommand.NoSubstituteDeviceColors:
          result = "dNOSUBSTDEVICECOLORS";
          break;
        case GhostScriptCommand.NoPSICC:
          result = "dNOPSICC";
          break;
        case GhostScriptCommand.NoTransparency:
          result = "dNOTRANSPARENCY";
          break;
        case GhostScriptCommand.NoTN5044:
          result = "dNO_TN5044";
          break;
        case GhostScriptCommand.DoPS:
          result = "dDOPS";
          break;
        case GhostScriptCommand.FixedMedia:
          result = "dFIXEDMEDIA";
          break;
        case GhostScriptCommand.FixedResolution:
          result = "dFIXEDRESOLUTION";
          break;
        case GhostScriptCommand.Orient1:
          result = "dORIENT1";
          break;
        case GhostScriptCommand.DeviceWidthPoints:
          result = "dDEVICEWIDTHPOINTS";
          break;
        case GhostScriptCommand.DeviceHeightPoint:
          result = "dDEVICEHEIGHTPOINTS";
          break;
        case GhostScriptCommand.DefaultPaperSize:
          result = "sDEFAULTPAPERSIZE";
          break;
        case GhostScriptCommand.DiskFonts:
          result = "dDISKFONTS";
          break;
        case GhostScriptCommand.LocalFonts:
          result = "dLOCALFONTS";
          break;
        case GhostScriptCommand.NoCCFonts:
          result = "dNOCCFONTS";
          break;
        case GhostScriptCommand.NoFontMap:
          result = "dNOFONTMAP";
          break;
        case GhostScriptCommand.NoFontPath:
          result = "dNOFONTPATH";
          break;
        case GhostScriptCommand.NoPlatformFonts:
          result = "dNOPLATFONTS";
          break;
        case GhostScriptCommand.NoNativeFontMap:
          result = "dNONATIVEFONTMAP";
          break;
        case GhostScriptCommand.FontMap:
          result = "sFONTMAP";
          break;
        case GhostScriptCommand.FontPath:
          result = "sFONTPATH";
          break;
        case GhostScriptCommand.SubstituteFont:
          result = "sSUBSTFONT";
          break;
        case GhostScriptCommand.OldCFFParser:
          result = "dOLDCFF";
          break;
        case GhostScriptCommand.GenericResourceDirectory:
          result = "sGenericResourceDir";
          break;
        case GhostScriptCommand.FontResourceDirectory:
          result = "sFontResourceDir";
          break;
        case GhostScriptCommand.Batch:
          result = "dBATCH";
          break;
        case GhostScriptCommand.NoPagePrompt:
          result = "dNOPAGEPROMPT";
          break;
        case GhostScriptCommand.NoPause:
          result = "dNOPAUSE";
          break;
        case GhostScriptCommand.NoPrompt:
          result = "dNOPROMPT";
          break;
        case GhostScriptCommand.Silent:
          result = "q";
          break;
        case GhostScriptCommand.Quiet:
          result = "dQUIET";
          break;
        case GhostScriptCommand.ShortErrors:
          result = "dSHORTERRORS";
          break;
        case GhostScriptCommand.StandardOut:
          result = "sstdout";
          break;
        case GhostScriptCommand.TTYPause:
          result = "dTTYPAUSE";
          break;
        case GhostScriptCommand.NoDisplay:
          result = "dNODISPLAY";
          break;
        case GhostScriptCommand.Device:
          result = "sDEVICE";
          break;
        case GhostScriptCommand.OutputFile:
          result = "sOutputFile";
          break;
        case GhostScriptCommand.IgnoreMultipleCopies:
          result = "d.IgnoreNumCopies";
          break;
        case GhostScriptCommand.EPSCrop:
          result = "dEPSCrop";
          break;
        case GhostScriptCommand.EPPSFitPage:
          result = "dEPSFitPage";
          break;
        case GhostScriptCommand.NoEPS:
          result = "dNOEPS";
          break;
        case GhostScriptCommand.DefaultGrayProfile:
          result = "sDefaultGrayProfile";
          break;
        case GhostScriptCommand.DefaultRGBProfile:
          result = "sDefaultRGBProfile";
          break;
        case GhostScriptCommand.DefaultCMYKProfile:
          result = "sDefaultCMYKProfile";
          break;
        case GhostScriptCommand.DeviceNProfile:
          result = "sDeviceNProfile";
          break;
        case GhostScriptCommand.ProofProfile:
          result = "sProofProfile";
          break;
        case GhostScriptCommand.DeviceLinkProfile:
          result = "sDeviceLinkProfile";
          break;
        case GhostScriptCommand.NamedProfile:
          result = "sNamedProfile";
          break;
        case GhostScriptCommand.OutputICCProfile:
          result = "sOutputICCProfile";
          break;
        case GhostScriptCommand.RenderIntent:
          result = "sRenderIntent";
          break;
        case GhostScriptCommand.GraphicICCProfile:
          result = "sGraphicICCProfile";
          break;
        case GhostScriptCommand.GraphicIntent:
          result = "sGraphicIntent";
          break;
        case GhostScriptCommand.ImageICCProfile:
          result = "sImageICCProfile";
          break;
        case GhostScriptCommand.ImageIntent:
          result = "sImageIntent";
          break;
        case GhostScriptCommand.TextICCProfile:
          result = "sTextICCProfile";
          break;
        case GhostScriptCommand.TextIntent:
          result = "sTextIntent";
          break;
        case GhostScriptCommand.OverrideICC:
          result = "dOverrideICC";
          break;
        case GhostScriptCommand.OverrideRI:
          result = "dOverrideRI";
          break;
        case GhostScriptCommand.SourceObjectICC:
          result = "sSourceObjectICC";
          break;
        case GhostScriptCommand.DeviceGrayToK:
          result = "dDeviceGrayToK";
          break;
        case GhostScriptCommand.ICCProfilesDirectory:
          result = "sICCProfilesDir";
          break;
        case GhostScriptCommand.DelayBind:
          result = "dDELAYBIND";
          break;
        case GhostScriptCommand.PdfMarks:
          result = "dDOPDFMARKS";
          break;
        case GhostScriptCommand.JobServer:
          result = "dJOBSERVER";
          break;
        case GhostScriptCommand.NoBind:
          result = "dNOBIND";
          break;
        case GhostScriptCommand.NoCache:
          result = "dNOCACHE";
          break;
        case GhostScriptCommand.NoGC:
          result = "dNOGC";
          break;
        case GhostScriptCommand.NoOuterSave:
          result = "dNOOUTERSAVE";
          break;
        case GhostScriptCommand.NoSafer:
          result = "dNOSAFER";
          break;
        case GhostScriptCommand.Safer:
          result = "dSAFER";
          break;
        case GhostScriptCommand.Strict:
          result = "dSTRICT";
          break;
        case GhostScriptCommand.SystemDictionaryWritable:
          result = "dWRITESYSTEMDICT";
          break;
        case GhostScriptCommand.FirstPage:
          result = "dFirstPage";
          break;
        case GhostScriptCommand.LastPage:
          result = "dLastPage";
          break;
        case GhostScriptCommand.PDFFitPage:
          result = "dPDFFitPage";
          break;
        case GhostScriptCommand.Printed:
          result = "dPrinted";
          break;
        case GhostScriptCommand.UseCropBox:
          result = "dUseCropBox";
          break;
        case GhostScriptCommand.UseTrimBox:
          result = "dUseTrimBox";
          break;
        case GhostScriptCommand.PDFPassword:
          result = "sPDFPassword";
          break;
        case GhostScriptCommand.ShowAnnotations:
          result = "dShowAnnots";
          break;
        case GhostScriptCommand.ShowAcroForm:
          result = "dShowAcroForm";
          break;
        case GhostScriptCommand.NoUserInit:
          result = "dNoUserUnit";
          break;
        case GhostScriptCommand.RenderTTNotDef:
          result = "dRENDERTTNOTDEF";
          break;
        case GhostScriptCommand.Resolution:
          result = "r";
          break;
        case GhostScriptCommand.PaperSize:
          result = "sPAPERSIZE";
          break;
        case GhostScriptCommand.InputFile:
          result = string.Empty;
          break;
        default:
          throw new ArgumentException("Invalid command", "command");
      }

      if (!string.IsNullOrEmpty(result))
        result = "-" + result;

      return result;
    }

    /// <summary>
    ///  Gets a device name for the given image format
    /// </summary>
    /// <exception cref="ArgumentException">  Thrown when one or more arguments have unsupported or illegal values. </exception>
    /// <param name="format"> Describes the image format to use. </param>
    /// <returns>
    ///  The device name which corrosponds to the given image format.
    /// </returns>
    public static string GetDeviceName(ImageFormat format)
    {
      string result;

      switch (format)
      {
        case ImageFormat.Bitmap24:
          result = "bmp16m";
          break;
        case ImageFormat.Bitmap8:
          result = "bmp256";
          break;
        case ImageFormat.BitmapMono:
          result = "bmpgray";
          break;
        case ImageFormat.Jpeg:
          result = "jpeg";
          break;
        case ImageFormat.Png8:
          result = "png256";
          break;
        case ImageFormat.Png24:
          result = "png16m";
          break;
        case ImageFormat.PngMono:
          result = "pnggray";
          break;
        case ImageFormat.Tiff24:
          result = "tiff24nc";
          break;
        case ImageFormat.TiffMono:
          result = "tiffgray";
          break;
        default:
          throw new ArgumentException("Invalid format", "format");
      }

      return result;
    }

    /// <summary>
    ///  Returns the given command and value as a formatted switch for use with the GhostScript API
    /// </summary>
    /// <param name="command">  The command. </param>
    /// <param name="value">    The value. </param>
    /// <returns>
    ///  The format switch.
    /// </returns>
    public static string GetFormattedArgument(GhostScriptCommand command, object value)
    {
      string commandName;
      string operatorChar;
      string commandValue;

      // reformat some values depending on their type
      if (value != null)
      {
        if (value is Enum)
          value = (int)value;
        else if (value is bool)
          value = (bool)value ? "true" : "false";
      }

      // and work out how the names/values will be combined
      commandName = GhostScriptAPI.GetCommandName(command);
      operatorChar = value == null || string.IsNullOrEmpty(commandName) || command == GhostScriptCommand.Resolution ? string.Empty : "=";
      commandValue = value == null ? string.Empty : value.ToString();

      return commandName + operatorChar + commandValue;
    }

    /// <summary>
    ///  Returns the given argument dictionary as an array of formatted switches for use with the GhostScript API
    /// </summary>
    /// <param name="args"> The arguments. </param>
    /// <returns>
    ///  The formatted arguments.
    /// </returns>
    public static string[] GetFormattedArguments(IDictionary<GhostScriptCommand, object> args)
    {
      List<string> translatedArgs;

      translatedArgs = new List<string>();
      foreach (KeyValuePair<GhostScriptCommand, object> pair in args)
        translatedArgs.Add(GhostScriptAPI.GetFormattedArgument(pair.Key, pair.Value));

      return translatedArgs.ToArray();
    }

    public static string GetPaperSizeName(PaperSize paperSize)
    {
      string result;

      switch (paperSize)
      {
        case PaperSize.LedgerPortrait:
          result = "11x17";
          break;
        case PaperSize.Ledger:
          result = "ledger";
          break;
        case PaperSize.Legal:
          result = "legal";
          break;
        case PaperSize.Letter:
          result = "letter";
          break;
        case PaperSize.LetterSmall:
          result = "lettersmall";
          break;
        case PaperSize.ArchE:
          result = "archE";
          break;
        case PaperSize.ArchD:
          result = "archD";
          break;
        case PaperSize.ArchC:
          result = "archC";
          break;
        case PaperSize.ArchB:
          result = "archB";
          break;
        case PaperSize.ArchA:
          result = "archA";
          break;
        case PaperSize.A0:
          result = "a0";
          break;
        case PaperSize.A1:
          result = "a1";
          break;
        case PaperSize.A2:
          result = "a2";
          break;
        case PaperSize.A3:
          result = "a3";
          break;
        case PaperSize.A4:
          result = "a4";
          break;
        case PaperSize.A5:
          result = "a5";
          break;
        case PaperSize.A6:
          result = "a6";
          break;
        case PaperSize.A7:
          result = "a7";
          break;
        case PaperSize.A8:
          result = "a8";
          break;
        case PaperSize.A9:
          result = "a9";
          break;
        case PaperSize.A10:
          result = "a10";
          break;
        case PaperSize.B0:
          result = "isob0";
          break;
        case PaperSize.B1:
          result = "isob1";
          break;
        case PaperSize.B2:
          result = "isob2";
          break;
        case PaperSize.B3:
          result = "isob3";
          break;
        case PaperSize.B4:
          result = "isob4";
          break;
        case PaperSize.B5:
          result = "isob5";
          break;
        case PaperSize.B6:
          result = "isob6";
          break;
        case PaperSize.C0:
          result = "c0";
          break;
        case PaperSize.C1:
          result = "c1";
          break;
        case PaperSize.C2:
          result = "c2";
          break;
        case PaperSize.C3:
          result = "c3";
          break;
        case PaperSize.C4:
          result = "c4";
          break;
        case PaperSize.C5:
          result = "c5";
          break;
        case PaperSize.C6:
          result = "c6";
          break;
        case PaperSize.Foolscap:
          result = "fse";
          break;
        case PaperSize.HalfLetter:
          result = "halfletter";
          break;
        case PaperSize.Hagaki:
          result = "hagaki";
          break;
        default:
          throw new ArgumentException("Invalid paper size", "paperSize");
      }

      return result;
    }

  #endregion  Public Class Methods  

  #region  Public Methods  

    /// <summary>
    ///  Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public virtual void Dispose()
    {
      try
      {
        this.FreeHandles();
        NativeMethods.gsapi_exit(_ghostScriptInstance);
        NativeMethods.gsapi_delete_instance(_ghostScriptInstance);
      }
      catch
      {
        // don't care about errors
      }
    }

    /// <summary>
    ///  Calls the gsapi_init_with_args GhostScript API with the given arguments.
    /// </summary>
    /// <exception cref="GhostScriptException"> Thrown when ghostscript. </exception>
    /// <param name="args"> The arguments. </param>
    /// <returns>
    ///  The GhostScript result code
    /// </returns>
    public int Execute(IDictionary<GhostScriptCommand, object> args)
    {
      return this.Execute(GhostScriptAPI.GetFormattedArguments(args));
    }

    /// <summary>
    ///  Calls the gsapi_init_with_args GhostScript API with the given arguments.
    /// </summary>
    /// <exception cref="GhostScriptException"> Thrown when ghostscript. </exception>
    /// <param name="args"> The arguments. </param>
    /// <returns>
    ///  The GhostScript result code
    /// </returns>
    public int Execute(string[] args)
    {
      int result;
      IntPtr[] argumentHandlePointers;

      if (!string.IsNullOrEmpty(args[0]))
      {
        string[] workArgs;

        // ghostscript ignores the first argument in the array
        // therefore, if the array doesn't have a blank item as the first item, create one
        // saves callers having to think about dummy values etc
        workArgs = new string[args.Length + 1];
        args.CopyTo(workArgs, 1);
        workArgs[0] = string.Empty;
        args = workArgs;
      }

      Debug.Print(string.Join(" ", args));

      // set up pointers
      _argumentHandles = new GCHandle[args.Length];
      argumentHandlePointers = new IntPtr[args.Length];

      // initialize pointers
      for (int i = 0; i < args.Length; i++)
      {
        _argumentHandles[i] = GCHandle.Alloc(Encoding.ASCII.GetBytes(args[i]), GCHandleType.Pinned);
        argumentHandlePointers[i] = _argumentHandles[i].AddrOfPinnedObject();
      }
      _argumentPointersHandle = GCHandle.Alloc(argumentHandlePointers, GCHandleType.Pinned);

      // call ghostscript
      try
      {
        result = NativeMethods.gsapi_init_with_args(_ghostScriptInstance, args.Length, _argumentPointersHandle.AddrOfPinnedObject());
      }
      finally
      {
        this.FreeHandles();
      }

      if (result < (int)ErrorCode.Success)            // TODO: often if GhostScript fails because you've passed the wrong combination of params etc, it still returns zero... unhelpful
        throw new GhostScriptException(result, args);

      return result;
    }

  #endregion  Public Methods  

  #region  Protected Methods  

    /// <summary>
    ///  Free handles.
    /// </summary>
    protected virtual void FreeHandles()
    {
      if (_argumentHandles != null && _argumentHandles.Length != 0)
      {
        for (int i = 0; i < _argumentHandles.Length; i++)
          _argumentHandles[i].Free();
      }
      _argumentHandles = null;

      if (_argumentPointersHandle.IsAllocated)
        _argumentPointersHandle.Free();
    }

  #endregion  Protected Methods  
  }
}
