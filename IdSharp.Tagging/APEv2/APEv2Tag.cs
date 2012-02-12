using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using IdSharp.Common.Utils;

namespace IdSharp.Tagging.APEv2
{
    // Reference: http://wiki.hydrogenaudio.org/index.php?title=APEv2_specification
    // http://wiki.hydrogenaudio.org/index.php?title=APE_key
    // http://www.hydrogenaudio.org/musepack/klemm/www.personal.uni-jena.de/~pfk/mpp/sv8/apetag.html

    /// <summary>
    /// APEv2
    /// TODO: This class is a work in progress. 
    /// Doesn't handle binary encoded fields
    /// Doesn't provide mappings from standard field keys to properties
    /// </summary>
    public partial class APEv2Tag : IAPEv2Tag
    {
        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        private static readonly byte[] _APETAGEX = Encoding.ASCII.GetBytes("APETAGEX");
        private long _tagOffset;
        private int _tagSize;
        private int _version;
        private readonly Dictionary<string, string> _items = new Dictionary<string, string>();

        private string _title;
        private string _artist;
        private string _album;
        private string _publisher;
        private string _trackNumber;
        private string _comment;
        private string _catalog;
        private string _year;
        private string _recordDate;
        private string _genre;
        private string _media;
        private string _language;

        /// <summary>
        /// Initializes a new instance of the <see cref="APEv2Tag"/> class.
        /// </summary>
        public APEv2Tag()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="APEv2Tag"/> class.
        /// </summary>
        /// <param name="path">The full path of the file.</param>
        public APEv2Tag(string path)
            : this()
        {
            Read(path);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="APEv2Tag"/> class.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        public APEv2Tag(Stream stream)
            : this()
        {
            Read(stream);
        }

        internal long TagOffset
        {
            get { return _tagOffset; }
        }

        internal int TagSize
        {
            get { return _tagSize; }
        }

        /// <summary>
        /// Gets the bytes of the current APEv2 tag.
        /// </summary>
        /// <returns>The bytes of the current APEv2 tag.</returns>
        public byte[] GetBytes()
        {
            int tagSize;
            int elements = _items.Count;

            if (elements != 0)
            {
                tagSize = 32; // footer + all tag items, excluding header (to be compatible with APEv1)

                foreach (KeyValuePair<string, string> item in _items)
                {
                    int valueLength = Encoding.UTF8.GetByteCount(item.Value);
                    tagSize += 8 + item.Key.Length + 1 + valueLength;
                }
            }
            else
            {
                return new byte[0];
            }

            using (MemoryStream ms = new MemoryStream())
            {
                ms.Write(_APETAGEX); // Footer
                ms.WriteInt32LittleEndian(2000); // Version (2000) ; 0xD0 0x07 0x00 0x00
                ms.WriteInt32LittleEndian(tagSize); // Tag size
                ms.WriteInt32LittleEndian(elements); // Element count
                ms.Write(new byte[] { 0, 0, 0, 0xA0 }); // Tag flags + IsHeader, Contains Header, Contains Footer
                ms.Write(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 }); // reserved
                // 0000 0000
                // 0xA0 = 0000 1010

                // Elements
                foreach (KeyValuePair<string, string> item in _items)
                {
                    byte[] valueBytes = Encoding.UTF8.GetBytes(item.Value);

                    ms.WriteInt32LittleEndian(valueBytes.Length); // size
                    ms.Write(new byte[] { 0x00, 0x00, 0x00, 0x00}); // todo: account for encoding type
                    ms.Write(Encoding.ASCII.GetBytes(item.Key));
                    ms.WriteByte(0x00);
                    ms.Write(valueBytes);
                }

                ms.Write(_APETAGEX); // Footer
                ms.WriteInt32LittleEndian(2000); // Version (2000) ; 0xD0 0x07 0x00 0x00
                ms.WriteInt32LittleEndian(tagSize); // Tag size
                ms.WriteInt32LittleEndian(elements); // Element count
                ms.Write(new byte[] { 0, 0, 0 }); // Other fields
                ms.WriteByte(0x80); // IsHeader, Contains Header, Contains Footer
                // 0000 1000
                ms.Write(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 }); // reserved

                return ms.ToArray();
            }
        }

        /// <summary>
        /// Reads the raw data from a specified file.
        /// </summary>
        /// <param name="path">The file to read from.</param>
        public void Read(string path)
        {
            using (FileStream fs = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                Read(fs);
            }
        }

        /// <summary>
        /// Reads the raw data from a specified stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        public void Read(Stream stream)
        {
            Read(stream, readElements: true);
        }

        internal void Read(Stream stream, bool readElements)
        {
            _tagSize = 0;
            _tagOffset = 0;

            if (readElements)
                _items.Clear();

            int footerOffset = 0;

            byte[] buf = new byte[32];
            stream.Seek(-32, SeekOrigin.End);
            stream.Read(buf, 0, 32);

            if (ByteUtils.Compare(buf, _APETAGEX, 8) == false)
            {
                // skip past possible ID3v1 tag
                footerOffset = 128;
                stream.Seek(-128-32, SeekOrigin.End);
                stream.Read(buf, 0, 32);
                if (ByteUtils.Compare(buf, _APETAGEX, 8) == false)
                {
                    return;
                    // skip past possible Lyrics3 tag
                    // TODO!
                    /*CLyrics3* Lyrics3 = new CLyrics3();
                    Lyrics3->ReadTag(fp);
                    if (Lyrics3->TagString() != "")
                    {
                        fseek(fp, 0L - Lyrics3->ExistingOffset() - 32, SEEK_END);
                        fread(buf, 32, 1, fp);
                        strcpy(tmp, buf);
                        tmp[8] = 0;

                        if (strcmp(tmp, "APETAGEX"))
                        {
                            delete Lyrics3;
                            return;
                        }

                        footeroffset = Lyrics3->ExistingOffset();
                    }
                    else
                    {
                        return;
                    }*/
                }
            }

            // Check version
            _version = 0;
            for (int i = 8; i < 12; i++)
            {
                _version += (buf[i] << ((i - 8) * 8));
            }

            // Must be APEv2 or APEv1
            if (_version != 2000 && _version != 1000)
            {
                _version = 0;
                return;
            }

            // Size
            int tagSize = 0;
            for (int i = 12; i < 16; i++)
            {
                tagSize += (buf[i] << ((i - 12) * 8));
            }

            // Elements
            int elements = 0;
            for (int i = 16; i < 20; i++)
            {
                elements += (buf[i] << ((i - 16) * 8));
            }

            bool containsHeader = ((buf[23] >> 7) == 1);

            // The other item flags are uninteresting in the context of the header/footer
            // They include:
            // - Tag contains a footer
            // - Is Header/Is Footer
            // - Item Encoding
            // - Is Read Only

            _tagSize = tagSize + (containsHeader ? 32 : 0);
            _tagOffset = stream.Length - (footerOffset + _tagSize);

            if (readElements)
            {
                stream.Seek(0 - (footerOffset + tagSize), SeekOrigin.End);

                for (int i = 0; i < elements; i++)
                {
                    ReadField(stream);
                }
            }
        }

        private void ReadField(Stream stream)
        {
            byte[] buf = new byte[8];
            int size = 0;

            stream.Read(buf, 0, 8);
            for (int i = 0; i < 4; i++)
            {
                size += (buf[i] << (i*8));
            }

            //encoding = (buf[20] >> 1) & 0x03;
            /*if (encoding == 0) sencoding = "UTF-8";
            else if (encoding == 1) sencoding = "binary";
            else if (encoding == 2) sencoding = "external";
            else if (encoding == 3) sencoding = "reserved";*/
            // don't care what's in the item flags

            string itemKey = stream.ReadISO88591().ToUpper();
            string itemValue = stream.ReadUTF8(size);
            
            // does the key already exist?
            if (_items.ContainsKey(itemKey))
            {
                // is the value empty? of so, set the new value
                if (string.IsNullOrEmpty(_items[itemKey]))
                {
                    _items[itemKey] = itemValue;
                }
            }
            // new key
            else
            {
                _items.Add(itemKey, itemValue);
            }

            switch (itemKey)
            {
                case "TITLE":
                    Title = itemValue;
                    break;
                case "ARTIST":
                    Artist = itemValue;
                    break;
                case "ALBUM":
                    Album = itemValue;
                    break;
                case "PUBLISHER":
                    Publisher = itemValue;
                    break;
                case "TRACK":
                    TrackNumber = itemValue;
                    break;
                case "COMMENT":
                    Comment = itemValue;
                    break;
                case "CATALOG":
                    Catalog = itemValue;
                    break;
                case "YEAR":
                    Year = itemValue;
                    break;
                case "RECORD DATE":
                    RecordDate = itemValue;
                    break;
                case "GENRE":
                    Genre = itemValue;
                    break;
                case "MEDIA":
                    Media = itemValue;
                    break;
                case "LANGUAGE":
                    Language = itemValue;
                    break;
            }
        }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title
        {
            get { return _title; }
            set { _title = value; RaisePropertyChanged("Title"); }
        }

        /// <summary>
        /// Gets or sets the artist.
        /// </summary>
        /// <value>The artist.</value>
        public string Artist
        {
            get { return _artist; }
            set { _artist = value; RaisePropertyChanged("Artist"); }
        }

        /// <summary>
        /// Gets or sets the album.
        /// </summary>
        /// <value>The album.</value>
        public string Album
        {
            get { return _album; }
            set { _album = value; RaisePropertyChanged("Album"); }
        }

        /// <summary>
        /// Gets or sets the publisher.
        /// </summary>
        /// <value>The publisher.</value>
        public string Publisher
        {
            get { return _publisher; }
            set { _publisher = value; RaisePropertyChanged("Publisher"); }
        }

        /// <summary>
        /// Gets or sets the track.
        /// </summary>
        /// <value>The track.</value>
        public string TrackNumber
        {
            get { return _trackNumber; }
            set { _trackNumber = value; RaisePropertyChanged("TrackNumber"); }
        }

        /// <summary>
        /// Gets or sets the comment.
        /// </summary>
        /// <value>The comment.</value>
        public string Comment
        {
            get { return _comment; }
            set { _comment = value; RaisePropertyChanged("Comment"); }
        }

        /// <summary>
        /// Gets or sets the catalog.
        /// </summary>
        /// <value>The catalog.</value>
        public string Catalog
        {
            get { return _catalog; }
            set { _catalog = value; RaisePropertyChanged("Catalog"); }
        }

        /// <summary>
        /// Gets or sets the year.
        /// </summary>
        /// <value>The year.</value>
        public string Year
        {
            get { return _year; }
            set { _year = value; RaisePropertyChanged("Year"); }
        }

        /// <summary>
        /// Gets or sets the record date.
        /// </summary>
        /// <value>The record date.</value>
        public string RecordDate
        {
            get { return _recordDate; }
            set { _recordDate = value; RaisePropertyChanged("RecordDate"); }
        }

        /// <summary>
        /// Gets or sets the genre.
        /// </summary>
        /// <value>The genre.</value>
        public string Genre
        {
            get { return _genre; }
            set { _genre = value; RaisePropertyChanged("Genre"); }
        }

        /// <summary>
        /// Gets or sets the media.
        /// </summary>
        /// <value>The media.</value>
        public string Media
        {
            get { return _media; }
            set { _media = value; RaisePropertyChanged("Media"); }
        }

        /// <summary>
        /// Gets or sets the language.
        /// </summary>
        /// <value>The language.</value>
        public string Language
        {
            get { return _language; }
            set { _language = value; RaisePropertyChanged("Language"); }
        }

        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler propertyChanged = PropertyChanged;
            if (propertyChanged != null)
                propertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
