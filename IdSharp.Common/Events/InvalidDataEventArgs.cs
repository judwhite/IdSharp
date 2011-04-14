using System;

namespace IdSharp.Common.Events
{
    /// <summary>
    /// Provides data for the <see cref="INotifyInvalidData.InvalidData"/> event.
    /// </summary>
    public sealed class InvalidDataEventArgs : EventArgs
    {
        private readonly string _propertyName;
        private readonly string _message;
        private readonly ErrorType _errorType;

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidDataEventArgs"/> class.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="message">The error or warning message.</param>
        public InvalidDataEventArgs(string propertyName, string message)
            : this(propertyName, message, ErrorType.Warning)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidDataEventArgs"/> class.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="errorType">Type of the error.</param>
        public InvalidDataEventArgs(string propertyName, ErrorType errorType)
            : this(propertyName, string.Empty, errorType)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidDataEventArgs"/> class.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="message">The error or warning message.</param>
        /// <param name="errorType">Type of the error.</param>
        public InvalidDataEventArgs(string propertyName, string message, ErrorType errorType)
        {
            _propertyName = propertyName;
            _message = message;
            _errorType = errorType;
        }

        /// <summary>
        /// Gets the property name.
        /// </summary>
        /// <value>The property name.</value>
        public string Property
        {
            get
            {
                return _propertyName;
            }
        }

        /// <summary>
        /// Gets the error or warning message.
        /// </summary>
        /// <value>The error or warning message.</value>
        public string Message
        {
            get
            {
                return _message;
            }
        }

        /// <summary>
        /// Gets the type of the error.
        /// </summary>
        /// <value>The type of the error.</value>
        public ErrorType ErrorType
        {
            get
            {
                return _errorType;
            }
        }
    }
}