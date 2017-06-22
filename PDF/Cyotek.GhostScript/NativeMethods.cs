using System;
using System.Runtime.InteropServices;

namespace Cyotek.GhostScript
{
  internal class NativeMethods
  {
  #region  Public Class Methods  

    // void gsapi_delete_instance (void *instance);
    [DllImport("gsdll64.dll", EntryPoint = "gsapi_delete_instance")]
    public static extern void gsapi_delete_instance(IntPtr instance);

    // int gsapi_exit (void *instance);
	[DllImport("gsdll64.dll", EntryPoint = "gsapi_exit")]
    public static extern int gsapi_exit(IntPtr instance);

    // int gsapi_set_poll (void *instance, int(*poll_fn)(void *caller_handle));
    // int gsapi_set_display_callback (void *instance, display_callback *callback);
    // int gsapi_init_with_args (void *instance, int argc, char **argv);
	[DllImport("gsdll64.dll", EntryPoint = "gsapi_init_with_args")]
    public static extern int gsapi_init_with_args(IntPtr instance, int argc, IntPtr argv);

    //int gsapi_new_instance (void **pinstance, void *caller_handle);
	[DllImport("gsdll64.dll", EntryPoint = "gsapi_new_instance")]
    public static extern int gsapi_new_instance(out IntPtr pinstance, IntPtr caller_handle);

    // int gsapi_revision (gsapi_revision_t *pr, int len);
	[DllImport("gsdll64.dll", EntryPoint = "gsapi_revision")]
    public static extern int gsapi_revision(out NativeStructs.GS_Revision pr, int len);

    // int gsapi_run_string_begin (void *instance, int user_errors, int *pexit_code);
    // int gsapi_run_string_continue (void *instance, const char *str, unsigned int length, int user_errors, int *pexit_code);
    // int gsapi_run_string_end (void *instance, int user_errors, int *pexit_code);
    // int gsapi_run_string_with_length (void *instance, const char *str, unsigned int length, int user_errors, int *pexit_code);
    // int gsapi_run_string (void *instance, const char *str, int user_errors, int *pexit_code);
    // int gsapi_run_file (void *instance, const char *file_name, int user_errors, int *pexit_code);
	[DllImport("gsdll64.dll", EntryPoint = "gsapi_run_file")]
    public static extern int gsapi_run_file(IntPtr instance, string file_name, int user_errors, int pexit_code);

    //int gsapi_set_stdio (void *instance, int(*stdin_fn)(void *caller_handle, char *buf, int len), int(*stdout_fn)(void *caller_handle, const char *str, int len), int(*stderr_fn)(void *caller_handle, const char *str, int len));
	[DllImport("gsdll64.dll", EntryPoint = "gsapi_set_stdio")]
    public static extern int gsapi_set_stdio(IntPtr instance, StdioCallBack stdin_fn, StdioCallBack stdout_fn, StdioCallBack stderr_fn);

  #endregion  Public Class Methods  

    // int gsapi_set_visual_tracer (gstruct vd_trace_interface_s *I);
  }
}
