
namespace Cyotek.GhostScript
{
  public enum GhostScriptCommand
  {
    // http://www.ghostscript.com/doc/current/Use.htm#Parameter_switches

    // rendering
    ColorScreen,
    DitherPPI,
    Interpolate,
    NoInterpolate,
    TextAlphaBits,
    GraphicsAlphaBits,
    AlignToPixels,
    GridToFitTT,
    UseCIEColor,
    NoCIE,
    NoSubstituteDeviceColors,
    NoPSICC,
    NoTransparency,
    NoTN5044,
    DoPS,

    //page
    FixedMedia,
    FixedResolution,
    Orient1,
    DeviceWidthPoints,
    DeviceHeightPoint,
    DefaultPaperSize,

    // font
    DiskFonts,
    LocalFonts,
    NoCCFonts,
    NoFontMap,
    NoFontPath,
    NoPlatformFonts,
    NoNativeFontMap,
    FontMap,
    FontPath,
    SubstituteFont,
    OldCFFParser,

    // resource
    GenericResourceDirectory,
    FontResourceDirectory,

    // interaction
    Batch,
    NoPagePrompt,
    NoPause,
    NoPrompt,
    Quiet,
    ShortErrors,
    StandardOut,
    TTYPause,

    // device
    NoDisplay,
    Device,
    OutputFile,
    IgnoreMultipleCopies,

    // EPS
    EPSCrop,
    EPPSFitPage,
    NoEPS,

    // ICC
    DefaultGrayProfile,
    DefaultRGBProfile,
    DefaultCMYKProfile,
    DeviceNProfile,
    ProofProfile,
    DeviceLinkProfile,
    NamedProfile,
    OutputICCProfile,
    RenderIntent,
    GraphicICCProfile,
    GraphicIntent,
    ImageICCProfile,
    ImageIntent,
    TextICCProfile,
    TextIntent,
    OverrideICC,
    OverrideRI,
    SourceObjectICC,
    DeviceGrayToK,
    ICCProfilesDirectory,

    // Others
    DelayBind,
    PdfMarks,
    JobServer,
    NoBind,
    NoCache,
    NoGC,
    NoOuterSave,
    NoSafer,
    DelayedSave = NoSafer,
    Safer,
    Strict,
    SystemDictionaryWritable,

    // http://www.ghostscript.com/doc/current/Use.htm#PDF_switches

    // PDF
    FirstPage,
    LastPage,
    PDFFitPage,
    Printed,
    UseCropBox,
    UseTrimBox,
    PDFPassword,
    ShowAnnotations,
    ShowAcroForm,
    NoUserInit,
    RenderTTNotDef,

    // uncategorized
    Resolution,
    PaperSize,
    Silent,

    // not really named commands but added for consistence
    InputFile
  }
}
