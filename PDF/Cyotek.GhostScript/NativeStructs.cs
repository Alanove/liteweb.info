using System;
using System.Runtime.InteropServices;

namespace Cyotek.GhostScript
{
  internal class NativeStructs
  {

    [StructLayout(LayoutKind.Sequential)]
    public struct GS_Revision
    {
  #region  Public Properties  

      public int intRevision { get; set; }

      public int intRevisionDate { get; set; }

      public IntPtr strCopyright { get; set; }

      public IntPtr strProduct { get; set; }

  #endregion  Public Properties  
    }
  }
}
