using System.Collections.Generic;
using System.IO;
using System.Text;

namespace IdSharp.Common.Utils
{
    /// <summary>
    /// StreamUtils
    /// </summary>
    public static class StreamUtils
    {
        /// <summary>
        /// Reads one byte from a stream. Throws an <see cref="InvalidDataException"/> if read past the end of the stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public static byte Read1(this Stream stream)
        {
            int readByte = stream.ReadByte();
            if (readByte == -1)
            {
                string msg = string.Format("Attempted to read past the end of the stream at position {0}", stream.Position);
                throw new InvalidDataException(msg);
            }
            return (byte)readByte;
        }

        /// <summary>
        /// Reads one byte from a stream and decreases the bytesLeft parameter by 1.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="bytesLeft">The number of bytes left.</param>
        public static byte Read1(this Stream stream, ref int bytesLeft)
        {
            if (bytesLeft > 0)
            {
                bytesLeft--;
                return Read1(stream);
            }
            else
            {
                string msg = string.Format("Attempted to read past the end of the frame at position {0}", stream.Position);
                throw new InvalidDataException(msg);
            }
        }

        /// <summary>
        /// Reads a specified number of bytes from a stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="count">The count.</param>
        public static byte[] Read(this Stream stream, int count)
        {
            byte[] buffer = new byte[count];
            if (stream.Read(buffer, 0, count) != count)
            {
                string msg = string.Format("Attempted to read past the end of the stream when requesting {0} bytes at position {1}", count, stream.Position);
                throw new InvalidDataException(msg);
            }
            return buffer;
        }

        /// <summary>
        /// Reads a specified number of bytes from a stream, and subtracts the count from the bytesLeft parameter.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="count">The count.</param>
        /// <param name="bytesLeft">The number of bytes left.</param>
        public static byte[] Read(this Stream stream, int count, ref int bytesLeft)
        {
            if (bytesLeft < count)
            {
                string msg = string.Format("Attempted to read past the end of the frame at position {0}", stream.Position);
                throw new InvalidDataException(msg);
            }
            bytesLeft -= count;
            return Read(stream, count);
        }

        /// <summary>
        /// Reads a 32-bit integer from a stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public static int ReadInt32(this Stream stream)
        {
            byte[] byteArray = Read(stream, 4);
            int returnValue = (byteArray[0] << 24) +
                              (byteArray[1] << 16) +
                              (byteArray[2] << 8) +
                               byteArray[3];
            return returnValue;
        }

        /// <summary>
        /// Reads a 24-bit integer from a stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public static int ReadInt24(this Stream stream)
        {
            byte[] byteArray = Read(stream, 3);
            int returnValue = (byteArray[0] << 16) +
                              (byteArray[1] << 8) +
                               byteArray[2];
            return returnValue;
        }

        /// <summary>
        /// Reads a 32-bit integer from a stream using Little Endian.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public static int ReadInt32LittleEndian(this Stream stream)
        {
            byte[] byteArray = Read(stream, 4);
            int returnValue = byteArray[0] +
                              (byteArray[1] << 8) +
                              (byteArray[2] << 16) +
                              (byteArray[3] << 24);
            return returnValue;
        }

        /// <summary>
        /// Writes a 32-bit integer to a stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="value">The value.</param>
        public static void WriteInt32(this Stream stream, int value)
        {
            byte[] byteArray = new[] { (byte)((value >> 24) & 0xFF), 
                                       (byte)((value >> 16) & 0xFF), 
                                       (byte)((value >> 8) & 0xFF), 
                                       (byte)(value & 0xFF)
                                     };
            Write(stream, byteArray);
        }

        /// <summary>
        /// Writes a 24-bit integer to a stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="value">The value.</param>
        public static void WriteInt24(this Stream stream, int value)
        {
            byte[] byteArray = new[] { (byte)((value >> 16) & 0xFF), 
                                       (byte)((value >> 8) & 0xFF), 
                                       (byte)(value & 0xFF)
                                     };
            Write(stream, byteArray);
        }

        /// <summary>
        /// Writes a 32-bit integer to a stream using Little Endian.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="value">The value.</param>
        public static void WriteInt32LittleEndian(this Stream stream, int value)
        {
            byte[] byteArray = new[] { (byte)(value & 0xFF), 
                                       (byte)((value >> 8) & 0xFF), 
                                       (byte)((value >> 16) & 0xFF), 
                                       (byte)((value >> 24) & 0xFF)
                                     };
            Write(stream, byteArray);
        }

        /// <summary>
        /// Writes a byte array to a stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="byteArray">The byte array.</param>
        public static void Write(this Stream stream, byte[] byteArray)
        {
            stream.Write(byteArray, 0, byteArray.Length);
        }

        /// <summary>
        /// Reads a 16-bit integer from a strea, and decrements bytesLeft.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="bytesLeft">The number of bytes left.</param>
        public static short ReadInt16(this Stream stream, ref int bytesLeft)
        {
            if (bytesLeft < 2)
            {
                string msg = string.Format("Attempted to read past the end of the stream at position {0}", stream.Position);
                throw new InvalidDataException(msg);
            }

            byte[] byteArray = Read(stream, 2);
            bytesLeft -= 2;
            short returnValue = (short)((byteArray[0] << 8) + byteArray[1]);
            return returnValue;
        }

        /// <summary>
        /// Writes an ISO-8859-1 string to a stream. Does NOT write a null terminator.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static void WriteISO88591(this Stream stream, string value)
        {
            byte[] bytes = ByteUtils.ISO88591GetBytes(value);
            stream.Write(bytes);
        }

        /// <summary>
        /// Reads an ISO-8859-1 encoded string for a stream up until the first null terminator.
        /// </summary>
        /// <param name="stream">The stream.</param>
        public static string ReadISO88591(this Stream stream)
        {
            List<byte> byteList = new List<byte>();

            byte readByte = Read1(stream);
            while (readByte != 0)
            {
                byteList.Add(readByte);
                readByte = Read1(stream);
            }

            string returnValue = ByteUtils.ISO88591GetString(byteList.ToArray());

            returnValue = returnValue.TrimEnd('\0');
            return returnValue;
        }

        /// <summary>
        /// Reads a UTF8 string of a specified length from a stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="length">The length.</param>
        public static string ReadUTF8(this Stream stream, int length)
        {
            byte[] byteArray = Read(stream, length);
            string returnValue = Encoding.UTF8.GetString(byteArray, 0, length);
            returnValue = returnValue.TrimEnd('\0');
            return returnValue;
        }
    }
}
