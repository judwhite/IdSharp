using System.ComponentModel;
using System.IO;
using System.Text;
using IdSharp.Common.Utils;

namespace IdSharp.Tagging.ID3v1
{
    /// <summary>
    /// ID3v1
    /// </summary>
    public partial class ID3v1Tag : IID3v1Tag
    {
        private string _title;
        private string _artist;
        private string _album;
        private string _year;
        private string _comment;
        private int? _trackNumber;
        private int _genreIndex;
        private ID3v1TagVersion _tagVersion;

        /// <summary>
        /// Initializes a new instance of the <see cref="ID3v1Tag"/> class.
        /// </summary>
        public ID3v1Tag()
        {
            _tagVersion = ID3v1TagVersion.ID3v11;
            _genreIndex = 12; // Other
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ID3v1Tag"/> class.
        /// </summary>
        /// <param name="path">The full path of the file.</param>
        public ID3v1Tag(string path)
        {
            _tagVersion = ID3v1TagVersion.ID3v11;
            _genreIndex = 12; // Other
            Read(path);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ID3v1Tag"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public ID3v1Tag(Stream stream)
        {
            _tagVersion = ID3v1TagVersion.ID3v11;
            _genreIndex = 12; // Other
            Read(stream);
        }

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title
        {
            get { return _title; }
            set
            {
                _title = GetTrimmedString(value, 30);
                RaisePropertyChanged("Title");
            }
        }

        /// <summary>
        /// Gets or sets the artist.
        /// </summary>
        /// <value>The artist.</value>
        public string Artist
        {
            get { return _artist; }
            set
            {
                _artist = GetTrimmedString(value, 30);
                RaisePropertyChanged("Artist");
            }
        }

        /// <summary>
        /// Gets or sets the album.
        /// </summary>
        /// <value>The album.</value>
        public string Album
        {
            get { return _album; }
            set
            {
                _album = GetTrimmedString(value, 30);
                RaisePropertyChanged("Album");
            }
        }

        /// <summary>
        /// Gets or sets the year.
        /// </summary>
        /// <value>The year.</value>
        public string Year
        {
            get { return _year; }
            set
            {
                _year = GetTrimmedString(value, 4);
                RaisePropertyChanged("Year");
            }
        }

        /// <summary>
        /// Gets or sets the comment.
        /// </summary>
        /// <value>The comment.</value>
        public string Comment
        {
            get { return _comment; }
            set
            {
                if (_tagVersion == ID3v1TagVersion.ID3v11)
                    _comment = GetTrimmedString(value, 28);
                else
                    _comment = GetTrimmedString(value, 30);

                RaisePropertyChanged("Comment");
            }
        }

        /// <summary>
        /// Gets or sets the track number.
        /// </summary>
        /// <value>The track number.</value>
        public int? TrackNumber
        {
            get { return _trackNumber; }
            set
            {
                if (value == null || value == 0)
                {
                    _trackNumber = null;
                    if (_tagVersion == ID3v1TagVersion.ID3v11)
                        TagVersion = ID3v1TagVersion.ID3v10;
                }
                else if (value > 0 && value <= 255)
                {
                    _trackNumber = value;
                    if (_tagVersion == ID3v1TagVersion.ID3v10)
                        TagVersion = ID3v1TagVersion.ID3v11;
                }
                RaisePropertyChanged("TrackNumber");
            }
        }

        /// <summary>
        /// Gets or sets the index of the genre.
        /// </summary>
        /// <value>The index of the genre.</value>
        public int GenreIndex
        {
            get { return _genreIndex; }
            set
            {
                if (value >= 0 && value <= 147)
                    _genreIndex = value;
                RaisePropertyChanged("GenreIndex");
            }
        }

        /// <summary>
        /// Gets or sets the ID3v1 tag version.
        /// </summary>
        /// <value>The ID3v1 tag version.</value>
        public ID3v1TagVersion TagVersion
        {
            get { return _tagVersion; }
            set
            {
                _tagVersion = value;
                RaisePropertyChanged("TagVersion");
                if (value == ID3v1TagVersion.ID3v11)
                {
                    Comment = _comment;
                }
            }
        }

        /// <summary>
        /// Reads the ID3v1 tag from the specified path.
        /// </summary>
        /// <param name="path">The full path of the file.</param>
        public void Read(string path)
        {
            using (FileStream fileStream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                Read(fileStream);
            }
        }

        /// <summary>
        /// Reads the ID3v1 tag from the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public void Read(Stream stream)
        {
            if (stream.Length >= 128)
            {
                stream.Seek(-128, SeekOrigin.End);

                if (GetString(stream, 3) == "TAG")
                {
                    Title = GetString(stream, 30);
                    Artist = GetString(stream, 30);
                    Album = GetString(stream, 30);
                    Year = GetString(stream, 4);

                    // Comment
                    byte[] buf = new byte[30];
                    stream.Read(buf, 0, 30);
                    string comment = GetString(buf);

                    // ID3v1.1
                    if (buf[28] == 0 && buf[29] != 0)
                    {
                        TagVersion = ID3v1TagVersion.ID3v11;
                        Comment = GetTrimmedString(comment, 28);
                        TrackNumber = buf[29];
                    }
                    else
                    {
                        TagVersion = ID3v1TagVersion.ID3v10;
                        Comment = comment;
                        TrackNumber = null;
                    }

                    int genreIndex = stream.Read1();
                    if (genreIndex < 0 || genreIndex > 147)
                        genreIndex = 12; // "Other"

                    GenreIndex = genreIndex;
                }
                else
                {
                    Reset();
                }
            }
            else
            {
                Reset();
            }
        }

        /// <summary>
        /// Resets the properties of the ID3v1 tag to their default values.
        /// </summary>
        public void Reset()
        {
            Title = null;
            Artist = null;
            Album = null;
            Year = null;
            Comment = null;
            TrackNumber = null;
            GenreIndex = 12; /* Other */
            TagVersion = ID3v1TagVersion.ID3v11;
        }

        /// <summary>
        /// Saves the ID3v1 tag to the specified path.
        /// </summary>
        /// <param name="path">The full path of the file.</param>
        public void Save(string path)
        {
            using (FileStream fileStream = File.Open(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
            {
                fileStream.Seek(0 - GetTagSize(fileStream), SeekOrigin.End);

                byte[] titleBytes = SafeGetBytes(_title);
                byte[] artistBytes = SafeGetBytes(_artist);
                byte[] albumBytes = SafeGetBytes(_album);
                byte[] yearBytes = SafeGetBytes(_year);
                byte[] commentBytes;

                fileStream.Write(Encoding.ASCII.GetBytes("TAG"));
                WriteBytesPadded(fileStream, titleBytes, 30);
                WriteBytesPadded(fileStream, artistBytes, 30);
                WriteBytesPadded(fileStream, albumBytes, 30);
                WriteBytesPadded(fileStream, yearBytes, 4);

                if (_tagVersion == ID3v1TagVersion.ID3v11)
                {
                    commentBytes = SafeGetBytes(_comment);
                    WriteBytesPadded(fileStream, commentBytes, 28);
                    fileStream.WriteByte(0);
                    fileStream.WriteByte((byte)(_trackNumber ?? 0));
                }
                else
                {
                    commentBytes = SafeGetBytes(_comment);
                    WriteBytesPadded(fileStream, commentBytes, 30);
                }

                fileStream.WriteByte((byte)_genreIndex);
            }
        }

        private void RaisePropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Writes a specified number of bytes to a stream, padding any missing bytes with 0x00.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="byteArray">The byte array.</param>
        /// <param name="length">The number of bytes to be written.</param>
        private static void WriteBytesPadded(Stream stream, byte[] byteArray, int length)
        {
            int i;
            for (i = 0; i < length && i < byteArray.Length && byteArray[i] != 0; i++)
            {
                stream.WriteByte(byteArray[i]);
            }
            for (; i < length; i++)
            {
                stream.WriteByte(0);
            }
        }

        /// <summary>
        /// Gets a string from a specified stream using ISO-8859-1 encoding.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        /// <param name="length">The length of the string in bytes.</param>
        private static string GetString(Stream stream, int length)
        {
            byte[] byteArray = new byte[length];
            stream.Read(byteArray, 0, length);
            return GetString(byteArray);
        }

        /// <summary>
        /// Gets a string from a specified byte array using ISO-8859-1 encoding.
        /// </summary>
        /// <param name="byteArray">The byte array.</param>
        private static string GetString(byte[] byteArray)
        {
            if (byteArray == null || byteArray.Length == 0)
                return null;

            int maxLength;
            for (maxLength = byteArray.Length; maxLength > 0; maxLength--)
            {
                if (byteArray[maxLength - 1] >= 32)
                    break;
            }

            string returnValue = ByteUtils.ISO88591.GetString(byteArray, 0, maxLength).TrimEnd('\0').TrimEnd(' ');
            return returnValue;
        }

        /// <summary>
        /// Gets a trimmed string with a maximum length from a specified string.
        /// </summary>
        /// <param name="value">The original string value.</param>
        /// <param name="maxLength">Maximum length of the string.</param>
        private static string GetTrimmedString(string value, int maxLength)
        {
            if (value == null)
                return null;

            value = value.TrimEnd('\0').Trim();

            if (value.Length > maxLength)
                return value.Substring(0, maxLength).Trim();
            else
                return value;
        }

        private static byte[] SafeGetBytes(string value)
        {
            if (value == null)
                return new byte[0];
            else
                return ByteUtils.ISO88591.GetBytes(value);
        }
    }
}
