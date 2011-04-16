using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using ComponentAce.Compression.Libs.zlib;
using IdSharp.Common.Utils;

namespace IdSharp.Tagging.ID3v2
{
    internal static class ID3v2Utils
    {
        public static int ReadInt32SyncSafe(Stream stream)
        {
            byte[] byteArray = stream.Read(4);
            int returnValue = ((byteArray[0] << 21) +
                                 (byteArray[1] << 14) +
                                 (byteArray[2] << 7) +
                                  byteArray[3]);
            return returnValue;
        }

        private static void CopyStream(Stream input, Stream output, int size)
        {
            byte[] buffer = new byte[size];
            input.Read(buffer, 0, size);
            output.Write(buffer, 0, size);
            output.Flush();
        }

        public static Stream DecompressFrame(Stream stream, int compressedSize)
        {
            Stream outStream = new MemoryStream();
            ZOutputStream outZStream = new ZOutputStream(outStream);
            CopyStream(stream, outZStream, compressedSize);
            outStream.Position = 0;
            return outStream;
        }

        public static Stream ReadUnsynchronizedStream(Stream stream, int length)
        {
            Stream newStream = new MemoryStream(ReadUnsynchronized(stream, length), 0, length);
            newStream.Position = 0;
            return newStream;
        }

        public static byte[] ReadUnsynchronized(byte[] stream)
        {
            using (MemoryStream byteList = new MemoryStream(stream.Length))
            {
                for (int i = 0, j = 0; i < stream.Length; i++)
                {
                    byte myByte = stream[j++];
                    byteList.WriteByte(myByte);
                    if (myByte == 0xFF)
                    {
                        myByte = stream[j++]; // skip 0x00
                        if (myByte != 0)
                        {
                            byteList.WriteByte(myByte);
                            i++;
                        }
                    }
                }

                return byteList.ToArray();
            }
        }

        public static byte[] ReadUnsynchronized(Stream stream, int size)
        {
            using (MemoryStream byteList = new MemoryStream(size))
            {
                for (int i = 0; i < size; i++)
                {
                    byte myByte = stream.Read1();
                    byteList.WriteByte(myByte);
                    if (myByte == 0xFF)
                    {
                        myByte = stream.Read1(); // skip 0x00
                        if (myByte != 0)
                        {
                            byteList.WriteByte(myByte);
                            i++;
                        }
                    }
                }

                return byteList.ToArray();
            }
        }

        public static int ReadInt32Unsynchronized(Stream stream)
        {
            byte[] byteArray = ReadUnsynchronized(stream, 4);
            int returnValue = (byteArray[0] << 24) +
                              (byteArray[1] << 16) +
                              (byteArray[2] << 8) +
                               byteArray[3];
            return returnValue;
        }

        public static int ReadInt24Unsynchronized(Stream stream)
        {
            byte[] byteArray = ReadUnsynchronized(stream, 3);
            int returnValue = (byteArray[0] << 16) +
                              (byteArray[1] << 8) +
                               byteArray[2];
            return returnValue;
        }

        public static byte[] GetStringBytes(ID3v2TagVersion tagVersion, EncodingType encodingType, string value, bool isTerminated)
        {
            List<byte> byteList = new List<byte>();

            switch (tagVersion)
            {
                case ID3v2TagVersion.ID3v22:
                    switch (encodingType)
                    {
                        case EncodingType.Unicode:
                            if (!string.IsNullOrEmpty(value))
                            {
                                byteList.Add(0xFF);
                                byteList.Add(0xFE);
                                byteList.AddRange(Encoding.Unicode.GetBytes(value)); // WITH BOM
                            }
                            if (isTerminated)
                                byteList.AddRange(new byte[] { 0, 0 });
                            break;
                        default:
                            byteList.AddRange(ByteUtils.ISO88591GetBytes(value));
                            if (isTerminated)
                                byteList.Add(0);
                            break;
                    }
                    break;

                case ID3v2TagVersion.ID3v23:
                    switch (encodingType)
                    {
                        case EncodingType.Unicode:
                            if (!string.IsNullOrEmpty(value))
                            {
                                byteList.Add(0xFF);
                                byteList.Add(0xFE);
                                byteList.AddRange(Encoding.Unicode.GetBytes(value)); // WITH BOM
                            }
                            if (isTerminated)
                                byteList.AddRange(new byte[] { 0, 0 });
                            break;
                        default:
                            byteList.AddRange(ByteUtils.ISO88591GetBytes(value));
                            if (isTerminated)
                                byteList.Add(0);
                            break;
                    }
                    break;

                case ID3v2TagVersion.ID3v24:
                    switch (encodingType)
                    {
                        case EncodingType.UTF8:
                            if (!string.IsNullOrEmpty(value))
                            {
                                byteList.AddRange(Encoding.UTF8.GetBytes(value));
                            }
                            if (isTerminated)
                                byteList.Add(0);
                            break;
                        case EncodingType.UTF16BE:
                            if (!string.IsNullOrEmpty(value))
                            {
                                byteList.AddRange(Encoding.BigEndianUnicode.GetBytes(value)); // no BOM
                            }
                            if (isTerminated)
                                byteList.AddRange(new byte[] { 0, 0 });
                            break;
                        case EncodingType.Unicode:
                            if (!string.IsNullOrEmpty(value))
                            {
                                byteList.Add(0xFF);
                                byteList.Add(0xFE);
                                byteList.AddRange(Encoding.Unicode.GetBytes(value)); // WITH BOM
                            }
                            if (isTerminated)
                                byteList.AddRange(new byte[] { 0, 0 });
                            break;
                        default:
                            byteList.AddRange(ByteUtils.ISO88591GetBytes(value));
                            if (isTerminated)
                                byteList.Add(0);
                            break;
                    }
                    break;

                default:
                    throw new ArgumentException("Unknown tag version");
            }

            return byteList.ToArray();
        }

        public static byte[] ConvertToUnsynchronized(byte[] data)
        {
            using (MemoryStream newStream = new MemoryStream((int)(data.Length * 1.05)))
            {
                for (int i = 0; i < data.Length; i++)
                {
                    newStream.WriteByte(data[i]);
                    if (data[i] == 0xFF)
                    {
                        if (i != data.Length - 1)
                        {
                            if (data[i + 1] == 0x00 || ((data[i + 1] & 0xE0) == 0xE0)) // 0xE0 = 1110 0000
                            {
                                newStream.WriteByte(0);
                            }
                        }
                    }
                }

                return newStream.ToArray();
            }
        }

        public static string ReadString(EncodingType textEncoding, byte[] bytes, int length)
        {
            using (MemoryStream memoryStream = new MemoryStream(bytes))
            {
                return ReadString(textEncoding, memoryStream, length);
            }
        }

        public static string ReadString(EncodingType textEncoding, Stream stream, int length)
        {
            string returnValue;

            byte[] byteArray = stream.Read(length);

            if (textEncoding == EncodingType.ISO88591)
            {
                returnValue = ByteUtils.ISO88591GetString(byteArray);
            }
            else if (textEncoding == EncodingType.Unicode)
            {
                if (length > 2)
                {
                    if (byteArray.Length >= 2)
                    {
                        // If BOM is part of the string, decode as the appropriate Unicode type.
                        // If no BOM is present use Little Endian Unicode.
                        if (byteArray[0] == 0xFF && byteArray[1] == 0xFE)
                            returnValue = Encoding.Unicode.GetString(byteArray, 2, byteArray.Length - 2);
                        else if (byteArray[0] == 0xFE && byteArray[1] == 0xFF)
                            returnValue = Encoding.BigEndianUnicode.GetString(byteArray, 2, byteArray.Length - 2);
                        else
                            returnValue = Encoding.Unicode.GetString(byteArray, 0, byteArray.Length);
                    }
                    else
                    {
                        returnValue = Encoding.Unicode.GetString(byteArray, 0, byteArray.Length);
                    }
                }
                else
                {
                    returnValue = "";
                }
            }
            else if (textEncoding == EncodingType.UTF16BE)
            {
                if (byteArray.Length >= 2)
                {
                    // If BOM is part of the string, remove before decoding.
                    if (byteArray[0] == 0xFE && byteArray[1] == 0xFF)
                        returnValue = Encoding.BigEndianUnicode.GetString(byteArray, 2, byteArray.Length - 2);
                    else
                        returnValue = Encoding.BigEndianUnicode.GetString(byteArray, 0, byteArray.Length);
                }
                else
                {
                    returnValue = Encoding.BigEndianUnicode.GetString(byteArray, 0, byteArray.Length);
                }
            }
            else if (textEncoding == EncodingType.UTF8)
            {
                returnValue = Encoding.UTF8.GetString(byteArray, 0, length);
            }
            else
            {
                // Most likely bad data
                string msg = string.Format("Text Encoding '{0}' unknown at position {1}", textEncoding, stream.Position);
                Trace.WriteLine(msg);
                return "";
            }

            returnValue = returnValue.TrimEnd('\0');
            return returnValue;
        }

        public static string ReadString(EncodingType textEncoding, byte[] bytes)
        {
            if (bytes == null)
                throw new ArgumentNullException("bytes");

            using (MemoryStream memoryStream = new MemoryStream(bytes))
            {
                return ReadString(textEncoding, memoryStream);
            }
        }

        public static string ReadString(EncodingType textEncoding, Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");

            string returnValue;
            List<byte> byteList = new List<byte>();

            if (textEncoding == EncodingType.ISO88591)
            {
                byte readByte = stream.Read1();
                while (readByte != 0)
                {
                    byteList.Add(readByte);

                    readByte = stream.Read1();
                }

                returnValue = ByteUtils.ISO88591GetString(byteList.ToArray());
            }
            else if (textEncoding == EncodingType.Unicode)
            {
                byte byte1;
                byte byte2;
                do
                {
                    byte1 = stream.Read1();
                    byteList.Add(byte1);

                    byte2 = stream.Read1();
                    byteList.Add(byte2);
                } while (byte1 != 0 || byte2 != 0);

                byte[] byteArray = byteList.ToArray();
                if (byteArray.Length >= 2)
                {
                    // If BOM is part of the string, decode as the appropriate Unicode type.
                    // If no BOM is present use Little Endian Unicode.
                    if (byteArray[0] == 0xFF && byteArray[1] == 0xFE)
                        returnValue = Encoding.Unicode.GetString(byteArray, 2, byteArray.Length - 2);
                    else if (byteArray[0] == 0xFE && byteArray[1] == 0xFF)
                        returnValue = Encoding.BigEndianUnicode.GetString(byteArray, 2, byteArray.Length - 2);
                    else
                        returnValue = Encoding.Unicode.GetString(byteArray, 0, byteArray.Length);
                }
                else
                {
                    returnValue = Encoding.Unicode.GetString(byteArray, 0, byteArray.Length);
                }
            }
            else if (textEncoding == EncodingType.UTF16BE)
            {
                byte byte1;
                byte byte2;
                do
                {
                    byte1 = stream.Read1();
                    byteList.Add(byte1);

                    byte2 = stream.Read1();
                    byteList.Add(byte2);
                } while (byte1 != 0 || byte2 != 0);

                byte[] byteArray = byteList.ToArray();
                if (byteArray.Length >= 2)
                {
                    // If BOM is part of the string, remove before decoding.
                    if (byteArray[0] == 0xFE && byteArray[1] == 0xFF)
                        returnValue = Encoding.BigEndianUnicode.GetString(byteArray, 2, byteArray.Length - 2);
                    else
                        returnValue = Encoding.BigEndianUnicode.GetString(byteArray, 0, byteArray.Length);
                }
                else
                {
                    returnValue = Encoding.BigEndianUnicode.GetString(byteArray, 0, byteArray.Length);
                }
            }
            else if (textEncoding == EncodingType.UTF8)
            {
                byte readByte = stream.Read1();
                while (readByte != 0)
                {
                    byteList.Add(readByte);

                    readByte = stream.Read1();
                }

                returnValue = Encoding.UTF8.GetString(byteList.ToArray());
            }
            else
            {
                // Most likely bad data
                string msg = string.Format("Text Encoding '{0}' unknown at position {1}", textEncoding, stream.Position);
                Trace.WriteLine(msg);
                return "";
            }

            returnValue = returnValue.TrimEnd('\0');
            return returnValue;
        }

        public static string ReadString(EncodingType textEncoding, Stream stream, ref int bytesLeft)
        {
            if (bytesLeft <= 0)
            {
                //String msg = String.Format("ReadString (unknown length) called with {0} bytes left at position {1}", bytesLeft, stream.Position);
                //Trace.WriteLine(msg);
                return string.Empty;
            }

            string returnValue;
            List<byte> byteList = new List<byte>();

            if (textEncoding == EncodingType.ISO88591)
            {
                byte readByte = stream.Read1();
                --bytesLeft;
                if (bytesLeft == 0)
                {
                    //String msg = String.Format("End of frame reached while reading unknown length string at position {0}", stream.Position);
                    //Trace.WriteLine(msg);
                    return "";
                }
                while (readByte != 0)
                {
                    byteList.Add(readByte);

                    readByte = stream.Read1();
                    --bytesLeft;
                    if (bytesLeft == 0)
                    {
                        //String msg = String.Format("End of frame reached while reading unknown length string at position {0}", stream.Position);
                        //Trace.WriteLine(msg);
                        if (readByte != 0) byteList.Add(readByte);
                        return ByteUtils.ISO88591GetString(byteList.ToArray());
                    }
                }

                returnValue = ByteUtils.ISO88591GetString(byteList.ToArray());
            }
            else if (textEncoding == EncodingType.Unicode)
            {
                byte byte1;
                byte byte2;
                do
                {
                    byte1 = stream.Read1();
                    byteList.Add(byte1);
                    --bytesLeft;
                    if (bytesLeft == 0)
                    {
                        //String msg = String.Format("End of frame reached while reading unknown length string at position {0}", stream.Position);
                        //Trace.WriteLine(msg);
                        return "";
                    }

                    byte2 = stream.Read1();
                    byteList.Add(byte2);
                    --bytesLeft;
                    if (bytesLeft == 0)
                    {
                        //String msg = String.Format("End of frame reached while reading unknown length string at position {0}", stream.Position);
                        //Trace.WriteLine(msg);
                        break;
                        //return "";
                    }
                } while (byte1 != 0 || byte2 != 0);

                byte[] byteArray = byteList.ToArray();
                if (byteArray.Length >= 2)
                {
                    // If BOM is part of the string, decode as the appropriate Unicode type.
                    // If no BOM is present use Little Endian Unicode.
                    if (byteArray[0] == 0xFF && byteArray[1] == 0xFE)
                        returnValue = Encoding.Unicode.GetString(byteArray, 2, byteArray.Length - 2);
                    else if (byteArray[0] == 0xFE && byteArray[1] == 0xFF)
                        returnValue = Encoding.BigEndianUnicode.GetString(byteArray, 2, byteArray.Length - 2);
                    else
                        returnValue = Encoding.Unicode.GetString(byteArray, 0, byteArray.Length);
                }
                else
                {
                    returnValue = Encoding.Unicode.GetString(byteArray, 0, byteArray.Length);
                }
            }
            else if (textEncoding == EncodingType.UTF16BE)
            {
                byte byte1;
                byte byte2;
                do
                {
                    byte1 = stream.Read1();
                    byteList.Add(byte1);
                    --bytesLeft;
                    if (bytesLeft == 0)
                    {
                        //String msg = String.Format("End of frame reached while reading unknown length string at position {0}", stream.Position);
                        //Trace.WriteLine(msg);
                        return "";
                    }

                    byte2 = stream.Read1();
                    byteList.Add(byte2);
                    --bytesLeft;
                    if (bytesLeft == 0)
                    {
                        //String msg = String.Format("End of frame reached while reading unknown length string at position {0}", stream.Position);
                        //Trace.WriteLine(msg);
                        break;
                        //return "";
                    }
                } while (byte1 != 0 || byte2 != 0);

                byte[] byteArray = byteList.ToArray();
                if (byteArray.Length >= 2)
                {
                    // If BOM is part of the string, remove before decoding.
                    if (byteArray[0] == 0xFE && byteArray[1] == 0xFF)
                        returnValue = Encoding.BigEndianUnicode.GetString(byteArray, 2, byteArray.Length - 2);
                    else
                        returnValue = Encoding.BigEndianUnicode.GetString(byteArray, 0, byteArray.Length);
                }
                else
                {
                    returnValue = Encoding.BigEndianUnicode.GetString(byteArray, 0, byteArray.Length);
                }
            }
            else if (textEncoding == EncodingType.UTF8)
            {
                byte readByte = stream.Read1();
                --bytesLeft;
                if (bytesLeft == 0)
                {
                    //String msg = String.Format("End of frame reached while reading unknown length string at position {0}", stream.Position);
                    //Trace.WriteLine(msg);
                    return "";
                }
                while (readByte != 0)
                {
                    byteList.Add(readByte);

                    readByte = stream.Read1();
                    --bytesLeft;
                    if (bytesLeft == 0)
                    {
                        //String msg = String.Format("End of frame reached while reading unknown length string at position {0}", stream.Position);
                        //Trace.WriteLine(msg);
                        return "";
                    }
                }

                returnValue = Encoding.UTF8.GetString(byteList.ToArray());
            }
            else
            {
                // Most likely bad data
                string msg = string.Format("Text Encoding '{0}' unknown at position {1}", textEncoding, stream.Position);
                Trace.WriteLine(msg);
                return "";
            }

            returnValue = returnValue.TrimEnd('\0');
            return returnValue;
        }
    }
}
