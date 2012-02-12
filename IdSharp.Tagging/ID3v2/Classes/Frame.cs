using System.ComponentModel;
using System.IO;
using IdSharp.Tagging.ID3v2.Frames;

namespace IdSharp.Tagging.ID3v2
{
    /// <summary>
    /// ID3v2 Frame
    /// </summary>
    internal abstract class Frame : IFrame
    {
        protected readonly FrameHeader _frameHeader;

        protected Frame()
        {
            _frameHeader = new FrameHeader();
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets the frame header.
        /// </summary>
        /// <value>The frame header.</value>
        public IFrameHeader FrameHeader 
        {
            get { return _frameHeader; }
        }

        /// <summary>
        /// Fires the property changed event.
        /// </summary>
        /// <param name="propertyName">The name of the property.</param>
        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler propertyChanged = PropertyChanged;
            if (propertyChanged != null)
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public abstract string GetFrameID(ID3v2TagVersion tagVersion);

        public abstract void Read(TagReadingInfo tagReadingInfo, Stream stream);

        public abstract byte[] GetBytes(ID3v2TagVersion tagVersion);
    }
}
