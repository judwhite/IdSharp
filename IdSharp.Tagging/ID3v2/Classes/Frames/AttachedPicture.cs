using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using IdSharp.Common.Utils;
using IdSharp.Tagging.ID3v2.Extensions;

namespace IdSharp.Tagging.ID3v2.Frames
{
    internal sealed class AttachedPicture : Frame, IAttachedPicture
    {
        private EncodingType _textEncoding;
        private string _mimeType;
        private PictureType _pictureType;
        private string _description;
        private byte[] _pictureData;
        private Image _picture;

        private bool _loadingPicture;
        private bool _readingTag;
        private bool _pictureCached;

        public AttachedPicture()
        {
            _pictureType = PictureType.CoverFront;
        }

        public EncodingType TextEncoding
        {
            get { return _textEncoding; }
            set
            {
                if (_textEncoding != value)
                {
                    // TODO: From what I can tell, it looks like iTunes can't handle a picture with a Unicode encoded description. 
                    // You add an ID3PictureFrame with the TextEncoding set to Unicode and iTunes doesn't recognize the picture. 
                    // Interestingly, Windows Media Player does recognize it. If the ID3PictureFrame has a TextEncoding of ISO-8859-1, 
                    // iTunes will recognize it.

                    _textEncoding = value;
                    RaisePropertyChanged("TextEncoding");
                }
            }
        }

        public string MimeType
        {
            get { return _mimeType; }
            set
            {
                if (_mimeType != value)
                {
                    _mimeType = value;
                    RaisePropertyChanged("MimeType");
                }
            }
        }

        public PictureType PictureType
        {
            get { return _pictureType; }
            set
            {
                // TODO: Validate enum
                // TODO: Validate uniqueness for 0x01 and 0x02
                if (_pictureType != value)
                {
                    _pictureType = value;
                    RaisePropertyChanged("PictureType");
                }
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                if (_description != value)
                {
                    _description = value;
                    RaisePropertyChanged("Description");
                }
            }
        }

        public byte[] PictureData
        {
            get { return ByteUtils.Clone(_pictureData); }
            set
            {
                if (!ByteUtils.Compare(_pictureData, value))
                {
                    _pictureData = ByteUtils.Clone(value);

                    if (value != null && _readingTag == false)
                    {
                        LoadPicture();
                    }
                    RaisePropertyChanged("PictureData");
                }
            }
        }

        public string PictureExtension
        {
            get
            {
                LoadPicture();

                if (_picture == null)
                    return null;

                if (_picture.RawFormat.Equals(ImageFormat.Bmp))
                    return "bmp";
                else if (_picture.RawFormat.Equals(ImageFormat.Emf))
                    return "emf";
                else if (_picture.RawFormat.Equals(ImageFormat.Exif))
                    return null;  // TODO - Unsure of MIME type?
                else if (_picture.RawFormat.Equals(ImageFormat.Gif))
                    return "gif";
                else if (_picture.RawFormat.Equals(ImageFormat.Icon))
                    return "ico";
                else if (_picture.RawFormat.Equals(ImageFormat.Jpeg))
                    return "jpg";
                else if (_picture.RawFormat.Equals(ImageFormat.MemoryBmp))
                    return "bmp";
                else if (_picture.RawFormat.Equals(ImageFormat.Png))
                    return "png";
                else if (_picture.RawFormat.Equals(ImageFormat.Tiff))
                    return "tif";
                else if (_picture.RawFormat.Equals(ImageFormat.Wmf))
                    return "wmf";
                else
                    return "";
            }
        }

        public Image Picture
        {
            get
            {
                if (_pictureCached == false)
                {
                    LoadPicture();
                }

                return (_picture == null ? null : (Image)_picture.Clone());
            }
            set
            {
                if (_picture != value)
                {
                    if (_picture != null)
                    {
                        _picture.Dispose();
                    }

                    _picture = value;

                    if (value == null)
                    {
                        _pictureData = null;
                    }
                    else
                    {
                        if (_loadingPicture == false)
                        {
                            using (MemoryStream memoryStream = new MemoryStream())
                            {
                                value.Save(memoryStream, value.RawFormat);
                                _pictureData = memoryStream.ToArray();
                            }

                            SetMimeType();
                        }
                    }
                }

                RaisePropertyChanged("Picture");
            }
        }

        private void SetMimeType()
        {
            LoadPicture();

            if (_picture != null)
            {
                if (_picture.RawFormat.Equals(ImageFormat.Bmp))
                {
                    MimeType = "image/bmp";
                }
                else if (_picture.RawFormat.Equals(ImageFormat.Emf))
                {
                    MimeType = "image/x-emf";
                }
                else if (_picture.RawFormat.Equals(ImageFormat.Exif))
                {
                    // TODO - Unsure of MIME type?
                }
                else if (_picture.RawFormat.Equals(ImageFormat.Gif))
                {
                    MimeType = "image/gif";
                }
                else if (_picture.RawFormat.Equals(ImageFormat.Icon))
                {
                    // TODO - How to handle this?
                }
                else if (_picture.RawFormat.Equals(ImageFormat.Jpeg))
                {
                    MimeType = "image/jpeg";
                }
                else if (_picture.RawFormat.Equals(ImageFormat.MemoryBmp))
                {
                    MimeType = "image/bmp";
                }
                else if (_picture.RawFormat.Equals(ImageFormat.Png))
                {
                    MimeType = "image/png";
                }
                else if (_picture.RawFormat.Equals(ImageFormat.Tiff))
                {
                    MimeType = "image/tiff";
                }
                else if (_picture.RawFormat.Equals(ImageFormat.Wmf))
                {
                    MimeType = "image/x-wmf";
                }
                else
                {
                    // TODO
                    //MimeType = "image/";
                }
            }
        }

        public override string GetFrameID(ID3v2TagVersion tagVersion)
        {
            switch (tagVersion)
            {
                case ID3v2TagVersion.ID3v24:
                case ID3v2TagVersion.ID3v23:
                    return "APIC";
                case ID3v2TagVersion.ID3v22:
                    return "PIC";
                default:
                    throw new ArgumentException("Unknown tag version");
            }
        }

        public override void Read(TagReadingInfo tagReadingInfo, Stream stream)
        {
            // Read header
            _frameHeader.Read(tagReadingInfo, ref stream);

            // Read frame data
            int bytesLeft = _frameHeader.FrameSizeExcludingAdditions;
            if (bytesLeft >= 6) // note: 6 was chosen arbitrarily
            {
                // Read text encoding
                TextEncoding = (EncodingType)stream.Read1(ref bytesLeft);

                if (tagReadingInfo.TagVersion == ID3v2TagVersion.ID3v22)
                {
                    // TODO: Do something with this?
                    string imageFormat = ID3v2Utils.ReadString(EncodingType.ISO88591, stream, 3);
                    bytesLeft -= 3;
                }
                else
                {
                    // Read MIME type
                    MimeType = ID3v2Utils.ReadString(EncodingType.ISO88591, stream, ref bytesLeft);
                }

                // Read picture type
                PictureType = (PictureType)stream.Read1(ref bytesLeft);

                // Short description
                Description = ID3v2Utils.ReadString(TextEncoding, stream, ref bytesLeft);

                // Picture data
                if (bytesLeft > 0)
                {
                    byte[] pictureData = stream.Read(bytesLeft);
                    bytesLeft = 0;
                    _readingTag = true;
                    try
                    {
                        _pictureCached = false;
                        PictureData = pictureData;
                    }
                    finally
                    {
                        _readingTag = false;
                    }
                }
                else
                {
                    // Incomplete frame
                    PictureData = null;
                }
            }
            else
            {
                // Incomplete frame
                TextEncoding = EncodingType.ISO88591;
                Description = null;
                MimeType = null;
                PictureType = PictureType.CoverFront;
                PictureData = null;
            }

            // Seek to end of frame
            if (bytesLeft > 0)
            {
                stream.Seek(bytesLeft, SeekOrigin.Current);
            }
        }

        public override byte[] GetBytes(ID3v2TagVersion tagVersion)
        {
            if (_pictureData == null || _pictureData.Length == 0)
                return new byte[0];

            // iTunes doesn't like Unicode in APIC descriptions - fixed in iTunes 7.1.0.59
            //TextEncoding = EncodingType.ISO88591;

            using (MemoryStream stream = new MemoryStream())
            {
                byte[] descriptionData;

                do
                {
                    descriptionData = ID3v2Utils.GetStringBytes(tagVersion, _textEncoding, _description, true);
                } while (
                    this.RequiresFix(tagVersion, _description, descriptionData)
                );

                stream.WriteByte((byte)_textEncoding);
                if (tagVersion == ID3v2TagVersion.ID3v22)
                {
                    string format = PictureExtension;
                    if (string.IsNullOrEmpty(format) || format.Length < 3)
                        format = "   ";
                    else if (format.Length > 3)
                        format = format.Substring(0, 3);

                    stream.Write(Encoding.ASCII.GetBytes(format));
                }
                else
                {
                    SetMimeType(); // iTunes needs this set properly
                    stream.Write(ByteUtils.ISO88591GetBytes(_mimeType));
                    stream.WriteByte(0); // terminator
                }
                stream.WriteByte((byte)_pictureType);
                stream.Write(descriptionData);
                stream.Write(_pictureData);
                return _frameHeader.GetBytes(stream, tagVersion, GetFrameID(tagVersion));
            }
        }

        private void LoadPicture()
        {
            _pictureCached = true;

            if (_pictureData == null)
            {
                Picture = null;
                return;
            }

            using (MemoryStream memoryStream = new MemoryStream(_pictureData))
            {
                bool isInvalidImage = false;

                try
                {
                    _loadingPicture = true;
                    try
                    {
                        Picture = Image.FromStream(memoryStream);
                    }
                    finally
                    {
                        _loadingPicture = false;
                    }
                }
                catch (OutOfMemoryException)
                {
                    string msg = string.Format("OutOfMemoryException caught in APIC's PictureData setter");
                    Trace.WriteLine(msg);

                    isInvalidImage = true;
                }
                catch (ArgumentException)
                {
                    string msg = string.Format("ArgumentException caught in APIC's PictureData setter");
                    Trace.WriteLine(msg);

                    isInvalidImage = true;
                }

                if (isInvalidImage)
                {
                    // Invalid image
                    if (_picture != null)
                    {
                        _picture.Dispose();
                    }

                    _picture = null;
                    try
                    {
                        string url = ByteUtils.ISO88591GetString(_pictureData);
                        if (url.Contains("://"))
                        {
                            MimeType = "-->";
                        }
                    }
                    catch (Exception ex)
                    {
                        // don't throw an exception
                        Trace.WriteLine(ex);
                    }
                }
            }
        }
    }
}
