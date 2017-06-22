
namespace lw.UrlMapping
{
    /// <summary>
    /// Exception thrown when the UrlMappingModule cannot match an incoming URL for
    /// redirection, provided the <see cref="UrlMappingProviderConfiguration.NoMatchAction">NoMatchAction</see>
    /// configuration property is set to
    /// <see cref="NoMatchActionEnum.ThrowException">ThrowException</see>.
    /// </summary>
    [global::System.Serializable]
    public class NoMatchFoundException : System.Exception
    {
        /// <summary>
        /// Initializes a NoMatchFoundException object
        /// </summary>
        public NoMatchFoundException() { }

        /// <summary>
        /// Initializes a NoMatchFoundException object with a message
        /// </summary>
        /// <param name="message">a string defining the exception message text</param>
        public NoMatchFoundException(string message) : base(message) { }

        /// <summary>
        /// Initializes a NoMatchFoundException object with a message and a wrapped
        /// inner exception
        /// </summary>
        /// <param name="message">a string defining the exception message text</param>
        /// <param name="inner">The inner exception to wrap with this NoMatchFoundException</param>
        public NoMatchFoundException(string message, System.Exception inner) : base(message, inner) { }

        /// <summary>
        /// Initializes a NoMatchFoundException
        /// </summary>
        /// <param name="info">serialization information relevant to the context</param>
        /// <param name="context">a serialization streaming context</param>
        protected NoMatchFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context)
            : base(info, context) { }
    }
}