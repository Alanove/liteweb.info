using System;

namespace Cyotek.GhostScript
{
  /// <summary>
  ///  Exception for signalling GhostScript errors. 
  /// </summary>
  /// <seealso cref="System.Exception"/>
  public class GhostScriptException : Exception
  {
  #region  Public Constructors  

    /// <summary>
    ///  Initializes a new instance of the GhostScriptException class.
    /// </summary>
    /// <param name="errorCode">  The error code. </param>
    public GhostScriptException(int errorCode)
      : this(errorCode, new string[] { })
    { }

    /// <summary>
    ///  Initializes a new instance of the GhostScriptException class.
    /// </summary>
    /// <param name="errorCode">  The error code. </param>
    /// <param name="arguments">  The arguments. </param>
    public GhostScriptException(int errorCode, string[] arguments)
      : this(errorCode, arguments, "Failed to process GhostScript command.")
    { }

    /// <summary>
    ///  Initializes a new instance of the GhostScriptException class.
    /// </summary>
    /// <param name="errorCode">  The error code. </param>
    /// <param name="arguments">  The arguments. </param>
    /// <param name="message">    The message. </param>
    public GhostScriptException(int errorCode, string[] arguments, string message)
      : this(errorCode, arguments, message, null)
    { }

    /// <summary>
    ///  Initializes a new instance of the GhostScriptException class.
    /// </summary>
    /// <param name="errorCode">      The error code. </param>
    /// <param name="arguments">  The arguments. </param>
    /// <param name="message">        The message. </param>
    /// <param name="innerException"> The inner exception. </param>
    public GhostScriptException(int errorCode, string[] arguments, string message, Exception innerException)
      : base(message, innerException)
    {
      this.Arguments = arguments;
      this.ErrorCode = errorCode;
    }

  #endregion  Public Constructors  

  #region  Protected Constructors  

    /// <summary>
    ///  Initializes a new instance of the GhostScriptException class.
    /// </summary>
    protected GhostScriptException()
    { }

  #endregion  Protected Constructors  

  #region  Public Properties  

    /// <summary>
    ///  Gets or sets the arguments associated the exception.
    /// </summary>
    /// <value>
    ///  The arguments.
    /// </value>
    public string[] Arguments { get; protected set; }

    /// <summary>
    ///  Gets or sets the error code.
    /// </summary>
    /// <value>
    ///  The error code.
    /// </value>
    public int ErrorCode { get; protected set; }

  #endregion  Public Properties  
  }
}
