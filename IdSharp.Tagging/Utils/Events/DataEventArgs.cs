using System;

namespace IdSharp.Tagging.Utils.Events
{
    /// <summary>
    /// A helper class for providing generic EventArgs data.
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="DataEventArgs{T}.Data"/> property.</typeparam>
    public class DataEventArgs<T> : EventArgs
    {
        private readonly T m_Data;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataEventArgs&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="data">The data item to send to event handlers.</param>
        public DataEventArgs(T data)
        {
            m_Data = data;
        }

        /// <summary>
        /// Gets the data value.
        /// </summary>
        /// <value>The data value.</value>
        public T Data
        {
            get { return m_Data; }
        }
    }
}