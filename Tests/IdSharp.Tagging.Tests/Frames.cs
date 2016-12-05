using System;
using System.IO;
using System.Text;
using IdSharp.Tagging.ID3v2;
using IdSharp.Tagging.ID3v2.Frames;
using NUnit.Framework;

namespace IdSharp.Tagging.Tests
{
    internal static class Frames
    {
        public static void EncryptionMethod(ID3v2TagVersion tagVersion, bool useData)
        {
            if (tagVersion == ID3v2TagVersion.ID3v22)
                throw new NotSupportedException();

            IID3v2Tag id3 = new ID3v2Tag();
            IEncryptionMethod aud = id3.EncryptionMethodList.AddNew();

            using (MemoryStream ms = new MemoryStream())
            {
                Write(ms, Encoding.ASCII.GetBytes("http://owneridentifier.org"));
                ms.WriteByte(0); // terminate
                ms.WriteByte(0x95); // method symbol
                if (useData)
                {
                    Write(ms, new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04 });
                }

                TestFrame(aud, tagVersion, ms.ToArray());
            }
        }

        public static void Commercial(ID3v2TagVersion tagVersion, bool useLogo)
        {
            if (tagVersion == ID3v2TagVersion.ID3v22)
                throw new NotSupportedException();

            IID3v2Tag id3 = new ID3v2Tag();
            ICommercial aud = id3.CommercialInfoList.AddNew();

            using (MemoryStream ms = new MemoryStream())
            {
                ms.WriteByte(0); // text encoding
                Write(ms, Encoding.ASCII.GetBytes("usd10.00/cad15.00"));
                ms.WriteByte(0); // terminate
                Write(ms, Encoding.ASCII.GetBytes("20070610"));
                Write(ms, Encoding.ASCII.GetBytes("www.google.com"));
                ms.WriteByte(0); // terminate
                ms.WriteByte((byte)ReceivedAs.FileOverTheInternet);
                Write(ms, Encoding.ASCII.GetBytes("name of seller"));
                ms.WriteByte(0); // terminate
                Write(ms, Encoding.ASCII.GetBytes("description"));
                ms.WriteByte(0); // terminate
                if (useLogo)
                {
                    Write(ms, Encoding.ASCII.GetBytes("image/jpeg"));
                    ms.WriteByte(0); // terminate
                    Write(ms, new byte[] { 0x00, 0x01, 0x02, 0x03, 0x04 });
                }

                TestFrame(aud, tagVersion, ms.ToArray());
            }
        }

        public static void AudioEncryption(ID3v2TagVersion tagVersion)
        {
            IID3v2Tag id3 = new ID3v2Tag();
            IAudioEncryption aud = id3.AudioEncryptionList.AddNew();
            byte[] data = {
              0x61, 0x62, 0x63, 0x00, // "abc"
              0x00, 0x10, // PreviewStart
              0x10, 0x00, // PreviewLength
              0x01, 0x02, 0x03, 0x04, 0x05 // Encryption data 
            };

            TestFrame(aud, tagVersion, data);
        }

        private static void TestFrame(IFrame frame, ID3v2TagVersion tagVersion, byte[] data)
        {
            TagReadingInfo tagReadingInfo = new TagReadingInfo(tagVersion);
            Stream stream = GetFrame(tagVersion, data);

            frame.Read(tagReadingInfo, stream);

            byte[] data2 = frame.GetBytes(tagVersion);
            int offset = (tagVersion == ID3v2TagVersion.ID3v22 ? 6 : 10);

            Assert.AreEqual(data.Length, data2.Length - offset, "Frame sizes are different");

            for (int i = 0; i < data.Length; i++)
            {
                Assert.AreEqual(data[i], data2[i + offset], string.Format("Byte offset {0}", i + offset));
            }
        }

        private static Stream GetFrame(ID3v2TagVersion tagVersion, byte[] data)
        {
            MemoryStream ms = new MemoryStream();

            int length = data.Length;

            if (tagVersion == ID3v2TagVersion.ID3v22)
            {
                ms.WriteByte((byte)((length >> 16) & 0xFF));
                ms.WriteByte((byte)((length >> 8) & 0xFF));
                ms.WriteByte((byte)(length & 0xFF));
            }
            else if (tagVersion == ID3v2TagVersion.ID3v23)
            {
                ms.WriteByte((byte)((length >> 24) & 0xFF));
                ms.WriteByte((byte)((length >> 16) & 0xFF));
                ms.WriteByte((byte)((length >> 8) & 0xFF));
                ms.WriteByte((byte)(length & 0xFF));
            }
            else if (tagVersion == ID3v2TagVersion.ID3v24)
            {
                ms.WriteByte((byte)((length >> 21) & 0x7F));
                ms.WriteByte((byte)((length >> 14) & 0x7F));
                ms.WriteByte((byte)((length >> 7) & 0x7F));
                ms.WriteByte((byte)(length & 0x7F));
            }

            // Flags
            if (tagVersion != ID3v2TagVersion.ID3v22)
            {
                ms.WriteByte(0);
                ms.WriteByte(0);
            }

            // Frame data
            ms.Write(data, 0, data.Length);

            // Reset position
            ms.Position = 0;

            return ms;
        }

        private static void Write(MemoryStream ms, byte[] data)
        {
            ms.Write(data, 0, data.Length);
        }
    }
}
