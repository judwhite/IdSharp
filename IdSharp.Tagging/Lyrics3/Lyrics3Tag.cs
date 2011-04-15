using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using IdSharp.Common.Utils;

namespace IdSharp.Tagging.Lyrics3
{
    // Reference: http://www.mpx.cz/mp3manager/tags.htm#Lyrics3Tag

    /// <summary>
    /// Lyrics3
    /// </summary>
    public partial class Lyrics3Tag
    {
        private readonly byte[] _lyrics200 = Encoding.ASCII.GetBytes("LYRICS200");
        private readonly byte[] _lyricsBegin = Encoding.ASCII.GetBytes("LYRICSBEGIN");
        //private readonly byte[] _lyricsEnd = Encoding.ASCII.GetBytes("LYRICSEND"); // ignore Lyrics3 v1

        private readonly Dictionary<string, string> _keyValues = new Dictionary<string, string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Lyrics3Tag"/> class.
        /// </summary>
        public Lyrics3Tag()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Lyrics3Tag"/> class.
        /// </summary>
        /// <param name="path">The full path of the file.</param>
        public Lyrics3Tag(string path)
            : this()
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");

            using (FileStream stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                Read(stream);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Lyrics3Tag"/> class.
        /// </summary>
        /// <param name="stream">The stream from which to read the tag.</param>
        public Lyrics3Tag(Stream stream)
            : this()
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            Read(stream);
        }

        /// <summary>
        /// Writes the tag to the specified path.
        /// </summary>
        /// <param name="path">The full path of the file which to write the tag.</param>
        public void Write(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");

            if (_keyValues.Count == 0)
                return;

            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the total size of the tag in bytes.
        /// </summary>
        /// <value>The total size of the tag in bytes.</value>
        public int TotalTagSize
        {
            get;
            private set;
        }

        internal long? TagOffset
        {
            get;
            private set;
        }

        private void Read(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            _keyValues.Clear();
            TotalTagSize = 0;
            TagOffset = null;

            stream.Seek(-128 - 9, SeekOrigin.End);

            byte[] endMarker = stream.Read(9);

            if (!ByteUtils.Compare(endMarker, _lyrics200))
                return;
            stream.Seek(-9 - 6, SeekOrigin.Current);
            byte[] lszBytes = stream.Read(6);
            int lsz = GetLengthFromByteArray(lszBytes);
            if (lsz <= 11 + 3 + 5 + 1)
            {
                // invalid, lsz is too small
                return;
            }

            stream.Seek(-6 - lsz, SeekOrigin.Current);
            byte[] beginMarker = stream.Read(11);
            if (!ByteUtils.Compare(beginMarker, _lyricsBegin))
            {
                // invalid, no begin marker found
                return;
            }

            int totalRead = 0;
            while (totalRead < lsz)
            {
                byte[] fieldIDBytes = stream.Read(3);
                string fieldID = Encoding.ASCII.GetString(fieldIDBytes);
                byte[] fszBytes = stream.Read(5);
                int fsz = GetLengthFromByteArray(fszBytes);
                totalRead += 3 + 5;
                
                // TODO: Indicate error reading Lyrics3 tag
                if (fsz <= 0)
                    break;
                if ((totalRead + fsz) > lsz)
                    break;
                
                byte[] valueBytes = stream.Read(fsz);
                string value = Encoding.ASCII.GetString(valueBytes);
                totalRead += fsz;

                SetValue(fieldID, value);
            }

            TotalTagSize = lsz + 15; // lsz + 6 bytes size + 9 bytes end identifier
            TagOffset = stream.Length - 128 - TotalTagSize;
        }

        /// <summary>
        /// Gets or sets the artist.
        /// </summary>
        /// <value>The artist.</value>
        public string Artist
        {
            get { return GetValue("EAR"); }
            set { SetValue("EAR", value); }
        }

        /// <summary>
        /// Gets or sets the album.
        /// </summary>
        /// <value>The album.</value>
        public string Album
        {
            get { return GetValue("EAL"); }
            set { SetValue("EAL", value); }
        }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title
        {
            get { return GetValue("ETT"); }
            set { SetValue("ETT", value); }
        }

        /// <summary>
        /// Gets or sets the genre.
        /// </summary>
        /// <value>The genre.</value>
        public string Genre
        {
            get { return GetValue("GRE"); }
            set { SetValue("GRE", value); }
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="fieldID">The field ID.</param>
        /// <returns></returns>
        public string GetValue(string fieldID)
        {
            string value;
            _keyValues.TryGetValue(fieldID, out value);
            return value;
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="fieldID">The field ID.</param>
        /// <param name="value">The value.</param>
        public void SetValue(string fieldID, string value)
        {
            if (value != null)
                value = value.Trim();

            if (_keyValues.ContainsKey(fieldID))
                _keyValues[fieldID] = value;
            else
                _keyValues.Add(fieldID, value);
        }

        private static int GetLengthFromByteArray(byte[] byteArray)
        {
            int length = 0;
            for (int i = byteArray.Length - 1, j = 1; i >= 0; i--, j *= 10)
            {
                int val = byteArray[i] - '0';
                if (val < 0 || val > 9)
                {
                    // invalid;
                    return 0;
                }
                length += j * val;
            }
            return length;
        }
    }
}
