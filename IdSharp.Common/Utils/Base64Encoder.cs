using System;
using System.IO;
using System.Text;

namespace IdSharp.Common.Utils
{
    /// <summary>
    /// Base64 Encode/Decode
    /// </summary>
    public static class Base64Encoder
    {
        private static readonly Org.BouncyCastle.Utilities.Encoders.Base64Encoder _encoder = new Org.BouncyCastle.Utilities.Encoders.Base64Encoder();

        /// <summary>
        /// Encodes the specified data.
        /// </summary>
        /// <param name="data">The data to encode.</param>
        /// <returns>The encoded data.</returns>
        public static byte[] Encode(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            using (MemoryStream outStream = new MemoryStream())
            {
                _encoder.Encode(data, 0, data.Length, outStream);
                return outStream.ToArray();
            }
        }

        /// <summary>
        /// Encodes the specified data.
        /// </summary>
        /// <param name="data">The data to encode.</param>
        /// <returns>The encoded data.</returns>
        public static string EncodeToString(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            byte[] encodedData = Encode(data);
            return Encoding.ASCII.GetString(encodedData);
        }

        /// <summary>
        /// Decodes the specified data.
        /// </summary>
        /// <param name="data">The encoded data.</param>
        /// <returns>The decoded data.</returns>
        public static byte[] Decode(byte[] data)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            using (MemoryStream outStream = new MemoryStream())
            {
                _encoder.Decode(data, 0, data.Length, outStream);
                return outStream.ToArray();
            }
        }

        /// <summary>
        /// Decodes the specified data.
        /// </summary>
        /// <param name="data">The encoded data.</param>
        /// <returns>The decoded data.</returns>
        public static byte[] Decode(string data)
        {
            if (data == null)
                throw new ArgumentNullException("data");

            return Decode(Encoding.ASCII.GetBytes(data));
        }
    }
}
