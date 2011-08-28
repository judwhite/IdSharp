using System;
using System.Text;
using IdSharp.Common.Utils;
using NUnit.Framework;

namespace IdSharp.Common.Tests.Utils
{
    [TestFixture]
    public class Base64EncoderTest
    {
        [Test]
        public void Encode_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => Base64Encoder.Encode(null));
        }

        [Test]
        public void Encode_Argument0Length()
        {
            byte[] result = Base64Encoder.Encode(new byte[0]);
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Length);
        }

        [Test]
        public void Encode_KnownArgumentUTF8()
        {
            byte[] stringBytes = Encoding.UTF8.GetBytes("檔案");
            byte[] expectedResult = new byte[] { 53, 113, 113, 85, 53, 113, 71, 73 };
            byte[] result = Base64Encoder.Encode(stringBytes);
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void Encode_KnownArgumentASCII()
        {
            byte[] stringBytes = Encoding.ASCII.GetBytes("Hello world");
            byte[] expectedResult = new byte[] { 83, 71, 86, 115, 98, 71, 56, 103, 100, 50, 57, 121, 98, 71, 81, 61 };
            byte[] result = Base64Encoder.Encode(stringBytes);
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void Encode_KnownArgumentBytes()
        {
            byte[] bytes = new byte[] { 0x00, 0x01, 0x03, 0x03, 0x07, 0x00 };
            byte[] expectedResult = new byte[] { 65, 65, 69, 68, 65, 119, 99, 65 };
            byte[] result = Base64Encoder.Encode(bytes);
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void EncodeToString_KnownArgumentUTF8()
        {
            byte[] stringBytes = Encoding.UTF8.GetBytes("檔案");
            string expectedResult = Encoding.ASCII.GetString(new byte[] { 53, 113, 113, 85, 53, 113, 71, 73 });
            string result = Base64Encoder.EncodeToString(stringBytes);
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void EncodeToString_KnownArgumentASCII()
        {
            byte[] stringBytes = Encoding.ASCII.GetBytes("Hello world");
            string expectedResult = Encoding.ASCII.GetString(new byte[] { 83, 71, 86, 115, 98, 71, 56, 103, 100, 50, 57, 121, 98, 71, 81, 61 });
            string result = Base64Encoder.EncodeToString(stringBytes);
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void EncodeToString_KnownArgumentBytes()
        {
            byte[] bytes = new byte[] { 0x00, 0x01, 0x03, 0x03, 0x07, 0x00 };
            string expectedResult = Encoding.ASCII.GetString(new byte[] { 65, 65, 69, 68, 65, 119, 99, 65 });
            string result = Base64Encoder.EncodeToString(bytes);
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void Decode_Bytes_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => Base64Encoder.Decode((byte[])null));
        }

        [Test]
        public void Decode_String_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => Base64Encoder.Decode((string)null));
        }

        [Test]
        public void Decode_Bytes_Argument0Length()
        {
            Assert.Throws<IndexOutOfRangeException>(() => Base64Encoder.Decode(new byte[0]));
        }

        [Test]
        public void Decode_String_Argument0Length()
        {
            Assert.Throws<IndexOutOfRangeException>(() => Base64Encoder.Decode(string.Empty));
        }

        [Test]
        public void Decode_Bytes_UTF8()
        {
            const string expectedResult = "檔案";
            byte[] data = new byte[] { 53, 113, 113, 85, 53, 113, 71, 73 };
            string result = Encoding.UTF8.GetString(Base64Encoder.Decode(data));
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void Decode_Bytes_ASCII()
        {
            const string expectedResult = "Hello world";
            byte[] data = new byte[] { 83, 71, 86, 115, 98, 71, 56, 103, 100, 50, 57, 121, 98, 71, 81, 61 };
            string result = Encoding.ASCII.GetString(Base64Encoder.Decode(data));
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void Decode_Bytes_Bytes()
        {
            byte[] expectedResult = new byte[] { 0x00, 0x01, 0x03, 0x03, 0x07, 0x00 };
            byte[] data = new byte[] { 65, 65, 69, 68, 65, 119, 99, 65 };
            byte[] result = Base64Encoder.Decode(data);
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void Decode_String_UTF8()
        {
            const string expectedResult = "檔案";
            string data = Encoding.ASCII.GetString(new byte[] { 53, 113, 113, 85, 53, 113, 71, 73 });
            string result = Encoding.UTF8.GetString(Base64Encoder.Decode(data));
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void Decode_String_ASCII()
        {
            const string expectedResult = "Hello world";
            string data = Encoding.ASCII.GetString(new byte[] { 83, 71, 86, 115, 98, 71, 56, 103, 100, 50, 57, 121, 98, 71, 81, 61 });
            string result = Encoding.ASCII.GetString(Base64Encoder.Decode(data));
            Assert.AreEqual(expectedResult, result);
        }

        [Test]
        public void Decode_String_Bytes()
        {
            byte[] expectedResult = new byte[] { 0x00, 0x01, 0x03, 0x03, 0x07, 0x00 };
            string data = Encoding.ASCII.GetString(new byte[] { 65, 65, 69, 68, 65, 119, 99, 65 });
            byte[] result = Base64Encoder.Decode(data);
            Assert.AreEqual(expectedResult, result);
        }
    }
}
