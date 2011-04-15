using System;
using System.Diagnostics;
using System.Text;
using System.IO;

namespace IdSharp.Common.Utils
{
    /// <summary>
    /// Common utils
    /// </summary>
    public static class ByteUtils
    {
        private static readonly Encoding m_ISO88591 = Encoding.GetEncoding(28591);

        /// <summary>
        /// Gets the ISO-8859-1 encoding.
        /// </summary>
        public static Encoding ISO88591
        {
            get { return m_ISO88591; }
        }

        /// <summary>
        /// Converts a byte array to a 64-bit integer using Big Endian.
        /// </summary>
        /// <param name="byteArray">The byte array.</param>
        public static long ConvertToInt64(byte[] byteArray)
        {
            long value = 0;
            for (int i = 0; i < byteArray.Length; i++)
            {
                value <<= 8;
                value += byteArray[i];
            }
            return value;
        }

        /// <summary>
        /// Determines whether a bit is set in the specified byte.
        /// </summary>
        /// <param name="byteToCheck">The byte to check.</param>
        /// <param name="bitToCheck">The bit to check.</param>
        /// <returns>
        /// 	<c>true</c> if the bit is set; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsBitSet(byte byteToCheck, byte bitToCheck)
        {
            if (bitToCheck > 7)
            {
                throw new ArgumentOutOfRangeException("bitToCheck", bitToCheck, "bitToCheck must be <= 7");
            }

            bool returnValue = ((byteToCheck >> bitToCheck) & 0x01) == 0x01;
            return returnValue;
        }

        /// <summary>
        /// Gets a byte array of the specified size.  TODO - See how this is used
        /// </summary>
        /// <param name="decimalValue">The decimal value.</param>
        /// <param name="byteArraySize">Size of the byte array.</param>
        public static byte[] GetBytesDecimal(decimal decimalValue, int byteArraySize)
        {
            byte[] byteArray = GetBytesMinimal((ulong)decimalValue);

            if (byteArray.Length == byteArraySize)
            {
                // Size is as requested, new byte array not needed
                return byteArray;
            }
            else if (byteArray.Length > byteArraySize)
            {
                // Size is greater than requested. Return new array with right 'byteArraySize' bytes.
                byte[] returnValue = new byte[byteArraySize];
                for (int i = byteArray.Length - byteArraySize, j = 0; i < byteArray.Length; i++, j++)
                {
                    returnValue[j] = byteArray[i];
                }
                return returnValue;
            }
            else
            {
                // Size is less than requested. Return new array left padded with 0's.
                byte[] returnValue = new byte[byteArraySize];
                for (int i = byteArraySize - byteArray.Length, j = 0; i < byteArraySize; i++, j++)
                {
                    returnValue[i] = byteArray[j];
                }
                return returnValue;
            }
        }

        /// <summary>
        /// Gets a byte array representing the value using the minimal number of bytes.
        /// </summary>
        /// <param name="value">The value.</param>
        public static byte[] GetBytesMinimal(long value)
        {
            return GetBytesMinimal((ulong)value);
        }

        /// <summary>
        /// Gets a byte array representing the value using the minimal number of bytes.
        /// </summary>
        /// <param name="value">The value.</param>
        public static byte[] GetBytesMinimal(ulong value)
        {
            byte[] returnValue;
            if (value <= byte.MaxValue)
                returnValue = new[] { (byte)value };
            else if (value <= ushort.MaxValue)
                returnValue = Get2Bytes((ushort)value);
            else if (value <= uint.MaxValue)
                returnValue = Get4Bytes((uint)value);
            else
                returnValue = Get8Bytes(value);

            return returnValue;
        }

        /// <summary>
        /// Gets an 8-byte length byte array representing the value.
        /// </summary>
        /// <param name="value">The value.</param>
        public static byte[] Get8Bytes(ulong value)
        {
            // BitConverter.GetBytes uses LE - we need BE
            byte[] byteArray = new[] { (byte)((value >> 56) & 0xFF),
                                       (byte)((value >> 48) & 0xFF),
                                       (byte)((value >> 40) & 0xFF),
                                       (byte)((value >> 32) & 0xFF),
                                       (byte)((value >> 24) & 0xFF),
                                       (byte)((value >> 16) & 0xFF),
                                       (byte)((value >> 8) & 0xFF),
                                       (byte)(value & 0xFF)
                                     };
            return byteArray;
        }

        /// <summary>
        /// Gets a 4-byte length byte array representing the value.
        /// </summary>
        /// <param name="value">The value.</param>
        public static byte[] Get4Bytes(uint value)
        {
            // BitConverter.GetBytes uses LE - we need BE
            byte[] byteArray = new[] { (byte)((value >> 24) & 0xFF),
                                       (byte)((value >> 16) & 0xFF),
                                       (byte)((value >> 8) & 0xFF),
                                       (byte)(value & 0xFF)
                                     };
            return byteArray;
        }

        /// <summary>
        /// Gets a 2-byte length byte array representing the value.
        /// </summary>
        /// <param name="value">The value.</param>
        public static byte[] Get2Bytes(ushort value)
        {
            // BitConverter.GetBytes uses LE - we need BE
            byte[] byteArray = new[] { (byte)((value >> 8) & 0xFF),
                                       (byte)(value & 0xFF)
                                     };
            return byteArray;
        }

        /// <summary>
        /// Gets a 4-byte length byte array representing the value.
        /// </summary>
        /// <param name="value">The value.</param>
        public static byte[] Get4Bytes(int value)
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException("value", value, "Value cannot be less than 0");
            return Get4Bytes((uint)value);
        }

        /// <summary>
        /// Gets a 2-byte length byte array representing the value.
        /// </summary>
        /// <param name="value">The value.</param>
        public static byte[] Get2Bytes(short value)
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException("value", value, "Value cannot be less than 0");
            return Get2Bytes((ushort)value);
        }

        /// <summary>
        /// Returns a byte array representing the ISO-8859-1 encoded value.  If value is <c>null</c>,
        /// a 0-byte array is returned.  <c>null</c> is never returned.
        /// </summary>
        /// <param name="value">The ISO-8859-1 encoded value.</param>
        /// <returns></returns>
        public static byte[] ISO88591GetBytes(string value)
        {
            if (value == null)
                return new byte[0];
            else
                return m_ISO88591.GetBytes(value);
        }

        /// <summary>
        /// Returns a clone of the byte array, or <c>null</c> if <paramref name="value"/> is <c>null</c>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>A clone of the byte array, or <c>null</c> if <paramref name="value"/> is <c>null</c>.</returns>
        public static byte[] Clone(byte[] value)
        {
            if (value == null)
                return null;
            else
                return (byte[])value.Clone();
        }

        /// <summary>
        /// Returns a string for the ISO-8859-1 encoded byte array.  If the byte array is <c>null</c>,
        /// String.Empty is returned.  <c>null</c> is never returned.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string ISO88591GetString(byte[] value)
        {
            if (value == null)
                return string.Empty;
            else
                return m_ISO88591.GetString(value);
        }

        /// <summary>
        /// Replaces a number of 'bytesToRemove' from 'path' with the 'bytesToAdd' byte array, at a specified offset.
        /// </summary>
        /// <param name="path">The full path of the file.</param>
        /// <param name="bytesToRemove">The number of bytes to remove.</param>
        /// <param name="bytesToAdd">The bytes to add.</param>
        /// <param name="offset">The offset which to start replacing bytes.</param>
        public static void ReplaceBytes(string path, int bytesToRemove, byte[] bytesToAdd, long offset)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");
            if (bytesToAdd == null)
                throw new ArgumentNullException("bytesToAdd");

            using (FileStream infile = File.Open(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
            {
                infile.Position = offset + bytesToRemove;
                byte[] extraBytes = new byte[infile.Length - offset - bytesToRemove];
                infile.Read(extraBytes, 0, extraBytes.Length);
                infile.SetLength(offset);
                infile.Position = offset;
                infile.Write(bytesToAdd, 0, bytesToAdd.Length);
                infile.Write(extraBytes, 0, extraBytes.Length);
            }
        }

        /// <summary>
        /// Replaces a number of 'bytesToRemove' from 'path' with the 'bytesToAdd' byte array, starting at the
        /// beginning of the file.
        /// </summary>
        /// <param name="path">The full path of the file.</param>
        /// <param name="bytesToRemove">The number of bytes to remove.</param>
        /// <param name="bytesToAdd">The bytes to add.</param>
        public static void ReplaceBytes(string path, int bytesToRemove, byte[] bytesToAdd)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException("path");
            if (bytesToAdd == null)
                throw new ArgumentNullException("bytesToAdd");

            const int BUF_SIZE = 32767;

            string tempPath = PathUtils.GetTemporaryFileNameBasedOnFileName(path);
            File.Move(path, tempPath);
            try
            {
                byte[] buffer = new byte[BUF_SIZE];

                using (FileStream infile = File.Open(tempPath, FileMode.Open, FileAccess.Read, FileShare.None))
                using (FileStream outfile = File.Open(path, FileMode.CreateNew, FileAccess.Write, FileShare.None))
                {
                    outfile.Write(bytesToAdd, 0, bytesToAdd.Length);

                    infile.Position = bytesToRemove;
                    int bytesRead = infile.Read(buffer, 0, BUF_SIZE);
                    while (bytesRead > 0)
                    {
                        outfile.Write(buffer, 0, bytesRead);
                        bytesRead = infile.Read(buffer, 0, BUF_SIZE);
                    }
                }
                File.Delete(tempPath);
            }
            catch
            {
                // try to put the file back
                try
                {
                    File.Move(tempPath, path);
                }
                catch (Exception ex)
                {
                    // swallow this one
                    Trace.WriteLine(ex);
                }
                throw;
            }
        }

        /// <summary>
        /// Compares the specified byte arrays.
        /// </summary>
        /// <param name="x">The first byte array.</param>
        /// <param name="y">The second byte arrayy.</param>
        /// <returns><c>true</c> if the byte arrays are equal; otherwise, <c>false</c></returns>
        public static bool Compare(byte[] x, byte[] y)
        {
            if (x == null && y == null)
                return true;

            if (x == null || y == null)
                return false;

            if (x == y)
                return true;

            if (x.Length != y.Length)
                return false;

            for (int i = 0; i < x.Length; i++)
            {
                if (x[i] != y[i])
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Compares the specified byte arrays, up to Min(x.Length, y.Length, maxLength)
        /// </summary>
        /// <param name="x">The first byte array.</param>
        /// <param name="y">The second byte arrayy.</param>
        /// <param name="maxLength">Maximum number of bytes to compare.</param>
        /// <returns>
        /// 	<c>true</c> if the byte arrays are equal; otherwise, <c>false</c>
        /// </returns>
        public static bool Compare(byte[] x, byte[] y, int maxLength)
        {
            if (maxLength < 0)
                throw new ArgumentOutOfRangeException("maxLength", "maxLength must be greater than or equal to 0.");

            if (x == null && y == null)
                return true;

            if (x == null || y == null)
                return false;

            if (x == y)
                return true;

            int compareTo = MathUtils.Min(x.Length, y.Length, maxLength);

            for (int i = 0; i < compareTo; i++)
            {
                if (x[i] != y[i])
                    return false;
            }

            return true;
        }
    }
}
