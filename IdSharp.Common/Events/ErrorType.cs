namespace IdSharp.Common.Events
{
    /// <summary>
    /// The type of error.  See <see cref="INotifyInvalidData"/>.
    /// </summary>
    public enum ErrorType
    {
        /// <summary>
        /// None.
        /// </summary>
        None,
        /// <summary>
        /// Error.
        /// </summary>
        Error,
        /// <summary>
        /// Warning.
        /// </summary>
        Warning,
        /// <summary>
        /// Information.
        /// </summary>
        Information
    }
}