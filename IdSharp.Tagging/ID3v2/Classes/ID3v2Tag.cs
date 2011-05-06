using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using IdSharp.Common.Utils;
using IdSharp.Tagging.ID3v2.Frames;

namespace IdSharp.Tagging.ID3v2
{
    /// <summary>
    /// ID3v2
    /// </summary>
    public partial class ID3v2Tag : FrameContainer, IID3v2Tag
    {
        private ID3v2Header _id3v2Header;
        private ID3v2ExtendedHeader _id3v2ExtendedHeader;

        #region <<< Constructor >>>

        /// <summary>
        /// Initializes a new instance of the <see cref="ID3v2Tag"/> class.
        /// </summary>
        public ID3v2Tag()
        {
            _id3v2Header = new ID3v2Header();
            _id3v2ExtendedHeader = new ID3v2ExtendedHeader();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ID3v2Tag"/> class.
        /// </summary>
        /// <param name="path">The full path of the file.</param>
        public ID3v2Tag(string path)
            : this()
        {
            Read(path);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ID3v2Tag"/> class.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        public ID3v2Tag(Stream stream)
            : this()
        {
            Read(stream);
        }

        #endregion <<< Constructor >>>

        #region <<< Public Methods >>>

        /// <summary>
        /// Reads the raw data from a specified file.
        /// </summary>
        /// <param name="path">The file to read from.</param>
        public void Read(string path)
        {
            try
            {
                // Open the file and read from the stream
                using (Stream stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    if (stream.Length < 10) 
                        return;
                    Read(stream);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("Error reading '{0}'", path), ex);
            }
        }

        /// <summary>
        /// Saves the tag to the specified path.
        /// </summary>
        /// <param name="path">The path to save the tag.</param>
        public void Save(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");

            int originalTagSize = GetTagSize(path);

            byte[] tagBytes = GetBytes(originalTagSize);

            if (tagBytes.Length < originalTagSize)
            {
                // Eventually this won't be a problem, but for now we won't worry about shrinking tags
                throw new Exception("GetBytes() returned a size less than the minimum size");
            }
            else if (tagBytes.Length > originalTagSize)
            {
                ByteUtils.ReplaceBytes(path, originalTagSize, tagBytes);
            }
            else
            {
                // Write tag of equal length
                using (FileStream fileStream = File.Open(path, FileMode.Open, FileAccess.Write, FileShare.None))
                {
                    fileStream.Write(tagBytes, 0, tagBytes.Length);
                }
            }
        }

        /// <summary>
        /// Gets all frames in the ID3v2 tag as a collection of IFrames.
        /// </summary>
        /// <returns>All frames in the ID3v2 tag as a collection of IFrames.</returns>
        public List<IFrame> GetFrames()
        {
            return GetAllFrames(_id3v2Header.TagVersion);
        }

        /// <summary>
        /// Gets all frames in the ID3v2 tag with the specified <paramref name="frameID"/> as a collection of IFrames.
        /// </summary>
        /// <returns>All frames in the ID3v2 tag with the specified <paramref name="frameID"/> as a collection of IFrames.</returns>
        public List<IFrame> GetFrames(string frameID)
        {
            if (string.IsNullOrEmpty(frameID))
                throw new ArgumentNullException("frameID");

            return GetAllFrames(_id3v2Header.TagVersion, frameID);
        }

        /// <summary>
        /// Gets all frames in the ID3v2 tag with the specified <paramref name="frameIDs"/> as a collection of IFrames.
        /// </summary>
        /// <returns>All frames in the ID3v2 tag with the specified <paramref name="frameIDs"/> as a collection of IFrames.</returns>
        public List<IFrame> GetFrames(IEnumerable<string> frameIDs)
        {
            if (frameIDs == null)
                throw new ArgumentNullException("frameIDs");

            return GetAllFrames(_id3v2Header.TagVersion, frameIDs);
        }

        /// <summary>
        /// Gets all frames in the ID3v2 tag which implement the specified interface <typeparamref name="T"/>.
        /// </summary>
        /// <returns>All frames in the ID3v2 tag which implement the specified interface <typeparamref name="T"/>.</returns>
        public List<T> GetFrames<T>()
        {
            return GetAllFrames(_id3v2Header.TagVersion).OfType<T>().ToList();
        }

        /// <summary>
        /// Gets the bytes of the current ID3v2 tag.
        /// </summary>
        /// <param name="minimumSize">The minimum size of the new tag, including the header and footer.</param>
        /// <returns>The bytes of the current ID3v2 tag.</returns>
        public byte[] GetBytes(int minimumSize)
        {
            ID3v2TagVersion tagVersion = _id3v2Header.TagVersion;

            using (MemoryStream tag = new MemoryStream())
            {
                byte[] framesByteArray = GetBytes(tagVersion);

                int tagSize = framesByteArray.Length;

                _id3v2Header.UsesUnsynchronization = false; // hack
                _id3v2Header.IsExperimental = true; // hack
                //_id3v2ExtendedHeader.IsCRCDataPresent = true; // hack: for testing
                //_id3v2Header.HasExtendedHeader = true; // hack: for testing

                if (_id3v2Header.HasExtendedHeader)
                {
                    // Add total size of extended header
                    tagSize += _id3v2ExtendedHeader.SizeExcludingSizeBytes + 4;
                }

                int paddingSize = minimumSize - (tagSize + 10);
                if (paddingSize < 0)
                {
                    paddingSize = 2000;
                }

                tagSize += paddingSize;

                // Set tag size in ID3v2 header
                _id3v2Header.TagSize = tagSize;

                byte[] id3v2Header = _id3v2Header.GetBytes();
                tag.Write(id3v2Header, 0, id3v2Header.Length);
                if (_id3v2Header.HasExtendedHeader)
                {
                    if (_id3v2ExtendedHeader.IsCRCDataPresent)
                    {
                        // TODO: Calculate total frame CRC (before or after unsync? compression? encr?)
                        _id3v2ExtendedHeader.CRC32 = CRC32.CalculateInt32(framesByteArray);
                    }

                    // Set padding size
                    _id3v2ExtendedHeader.PaddingSize = paddingSize; // todo: check

                    byte[] id3v2ExtendedHeader = _id3v2ExtendedHeader.GetBytes(tagVersion);
                    tag.Write(id3v2ExtendedHeader, 0, id3v2ExtendedHeader.Length);
                }

                tag.Write(framesByteArray, 0, framesByteArray.Length);
                byte[] padding = new byte[paddingSize];
                tag.Write(padding, 0, paddingSize);

                // Make sure WE can read it without throwing errors
                // TODO: Take this out eventually, this is just a precaution.
                tag.Position = 0;
                ID3v2Tag newID3v2 = new ID3v2Tag();
                newID3v2.Read(tag);

                return tag.ToArray();
            }
        }

        #endregion <<< Public Methods >>>

        #region <<< Public Properties >>>

        /// <summary>
        /// Gets the ID3v2 header.
        /// </summary>
        /// <value>The ID3v2 header.</value>
        public IID3v2Header Header
        {
            get { return _id3v2Header; }
        }

        /// <summary>
        /// Gets the ID3v2 extended header.
        /// </summary>
        /// <value>The ID3v2 extended header.</value>
        public IID3v2ExtendedHeader ExtendedHeader
        {
            get { return _id3v2ExtendedHeader; }
        }

        #endregion <<< Public Properties >>>

        /// <summary>
        /// Reads the raw data from a specified stream.
        /// </summary>
        /// <param name="stream">The stream to read from.</param>
        public void Read(Stream stream)
        {
            // Check for 'ID3' marker
            byte[] identifier = stream.Read(3);
            if (!(identifier[0] == 0x49 && identifier[1] == 0x44 && identifier[2] == 0x33))
            {
                return;
            }

            // Read the header
            _id3v2Header = new ID3v2Header(stream, false);

            TagReadingInfo tagReadingInfo = new TagReadingInfo(_id3v2Header.TagVersion);
            if (_id3v2Header.UsesUnsynchronization)
                tagReadingInfo.TagVersionOptions = TagVersionOptions.Unsynchronized;
            else
                tagReadingInfo.TagVersionOptions = TagVersionOptions.None;

            if (_id3v2Header.HasExtendedHeader)
            {
                _id3v2ExtendedHeader = new ID3v2ExtendedHeader(tagReadingInfo, stream);
            }

            int frameIDSize = (tagReadingInfo.TagVersion == ID3v2TagVersion.ID3v22 ? 3 : 4);
            int bytesRead;
            int readUntil;

            #region <<< ID3v2.4 - Guess if syncsafe frame size was used or not >>>

            if (_id3v2Header.TagVersion == ID3v2TagVersion.ID3v24)
            {
                bool isID3v24SyncSafe = true;

                bytesRead = 0;
                readUntil = _id3v2Header.TagSize - _id3v2ExtendedHeader.SizeIncludingSizeBytes - frameIDSize;
                long initialPosition = stream.Position;
                while (bytesRead < readUntil)
                {
                    byte[] frameIDBytes = stream.Read(frameIDSize);

                    // TODO: Noticed some tags contain 0x00 'E' 'N' as a FrameID.  Frame is well structured
                    // and other frames follow.  I believe the below (keep reading+looking) will cover this issue.

                    // If character is not a letter or number, padding reached, audio began,
                    // or otherwise the frame is not readable
                    if (frameIDBytes[0] < 0x30 || frameIDBytes[0] > 0x5A ||
                        frameIDBytes[1] < 0x30 || frameIDBytes[1] > 0x5A ||
                        frameIDBytes[2] < 0x30 || frameIDBytes[2] > 0x5A ||
                        frameIDBytes[3] < 0x30 || frameIDBytes[3] > 0x5A)
                    {
                        // TODO: Try to keep reading and look for a valid frame
                        if (frameIDBytes[0] != 0 && frameIDBytes[0] != 0xFF)
                        {
                            /*String msg = String.Format("Out of range FrameID - 0x{0:X}|0x{1:X}|0x{2:X}|0x{3:X}",
                                                       tmpFrameIDBytes[0], tmpFrameIDBytes[1], tmpFrameIDBytes[2],
                                                       tmpFrameIDBytes[3]);
                            Trace.WriteLine(msg);*/
                        }

                        break;
                    }

                    int frameSize = stream.ReadInt32();
                    if (frameSize > 0xFF)
                    {
                        if ((frameSize & 0x80) == 0x80) { isID3v24SyncSafe = false; break; }
                        if ((frameSize & 0x8000) == 0x8000) { isID3v24SyncSafe = false; break; }
                        if ((frameSize & 0x800000) == 0x800000) { isID3v24SyncSafe = false; break; }

                        if (bytesRead + frameSize + 10 == _id3v2Header.TagSize)
                        {
                            // Could give a false positive, but relatively unlikely (famous last words, huh?)
                            isID3v24SyncSafe = false;
                            break;
                        }
                        else
                        {
                            stream.Seek(-4, SeekOrigin.Current); // go back to read sync-safe version
                            int syncSafeFrameSize = ID3v2Utils.ReadInt32SyncSafe(stream);

                            long currentPosition = stream.Position;

                            bool isValidAtSyncSafe = true;
                            bool isValidAtNonSyncSafe = true;

                            // TODO - if it's the last frame and there is padding, both would indicate false
                            // Use the one that returns some padding bytes opposed to bytes with non-zero values (could be frame data)

                            // If non sync-safe reads past the end of the tag, then it's sync safe
                            // Testing non-sync safe since it will always be bigger than the sync safe integer
                            if (currentPosition + frameSize + 2 >= readUntil) { isID3v24SyncSafe = true; break; }

                            // Test non-sync safe
                            stream.Seek(currentPosition + frameSize + 2, SeekOrigin.Begin);
                            frameIDBytes = stream.Read(frameIDSize);
                            if (frameIDBytes[0] < 0x30 || frameIDBytes[0] > 0x5A ||
                                frameIDBytes[1] < 0x30 || frameIDBytes[1] > 0x5A ||
                                frameIDBytes[2] < 0x30 || frameIDBytes[2] > 0x5A ||
                                frameIDBytes[3] < 0x30 || frameIDBytes[3] > 0x5A)
                            {
                                isValidAtNonSyncSafe = false;
                            }

                            // Test sync-safe
                            stream.Seek(currentPosition + syncSafeFrameSize + 2, SeekOrigin.Begin);
                            frameIDBytes = stream.Read(frameIDSize);
                            if (frameIDBytes[0] < 0x30 || frameIDBytes[0] > 0x5A ||
                                frameIDBytes[1] < 0x30 || frameIDBytes[1] > 0x5A ||
                                frameIDBytes[2] < 0x30 || frameIDBytes[2] > 0x5A ||
                                frameIDBytes[3] < 0x30 || frameIDBytes[3] > 0x5A)
                            {
                                isValidAtSyncSafe = false;
                            }

                            // if they're equal, we'll just have to go with syncsafe, since that's the spec

                            if (isValidAtNonSyncSafe != isValidAtSyncSafe)
                            {
                                isID3v24SyncSafe = isValidAtSyncSafe;
                            }
                            break;
                        }
                    }

                    stream.Seek(frameSize + 2, SeekOrigin.Current);
                    bytesRead += frameSize + 10;
                }

                stream.Position = initialPosition;
                if (isID3v24SyncSafe == false)
                {
                    tagReadingInfo.TagVersionOptions |= TagVersionOptions.UseNonSyncSafeFrameSizeID3v24;
                }
            }
            else if (_id3v2Header.TagVersion == ID3v2TagVersion.ID3v22)
            {
                bool isID3v22CorrectSize = true;

                bytesRead = 0;
                readUntil = _id3v2Header.TagSize - _id3v2ExtendedHeader.SizeIncludingSizeBytes - frameIDSize;
                long initialPosition = stream.Position;

                stream.Read(frameIDSize);
                UnknownFrame unknownFrame = new UnknownFrame(null, tagReadingInfo, stream);
                bytesRead += unknownFrame.FrameHeader.FrameSizeTotal;
                if (bytesRead < readUntil)
                {
                    byte[] frameIDBytes = stream.Read(frameIDSize);

                    // TODO: Noticed some tags contain 0x00 'E' 'N' as a FrameID.  Frame is well structured
                    // and other frames follow.  I believe the below (keep reading+looking) will cover this issue.

                    // If character is not a letter or number, padding reached, audio began,
                    // or otherwise the frame is not readable
                    if (frameIDBytes[0] < 0x30 || frameIDBytes[0] > 0x5A)
                    {
                        if (frameIDBytes[1] >= 0x30 && frameIDBytes[1] <= 0x5A &&
                            frameIDBytes[2] >= 0x30 && frameIDBytes[2] <= 0x5A)
                        {
                            Trace.WriteLine("ID3v2.2 frame size off by 1 byte");
                            isID3v22CorrectSize = false;
                        }
                    }
                }

                stream.Position = initialPosition;
                if (isID3v22CorrectSize == false)
                {
                    tagReadingInfo.TagVersionOptions |= TagVersionOptions.AddOneByteToSize;
                }
            }

            #endregion <<< ID3v2.4 - Guess if syncsafe frame size was used or not >>>

            readUntil = _id3v2Header.TagSize - _id3v2ExtendedHeader.SizeIncludingSizeBytes - frameIDSize;
            Read(stream, _id3v2Header.TagVersion, tagReadingInfo, readUntil, frameIDSize);
        }
    }
}
