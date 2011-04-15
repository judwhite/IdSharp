using System;
using System.Text;
using IdSharp.Common.Utils;
using NUnit.Framework;

namespace IdSharp.Common.Tests.Utils
{
    [TestFixture]
    public class ByteUtilsTest
    {
        [Test]
        public void Clone_Null()
        {
            Assert.That(() => ByteUtils.Clone(null), Is.Null);
        }

        [Test]
        public void Clone_0Bytes()
        {
            byte[] original = new byte[0];
            TestClone(original);
        }

        [Test]
        public void Clone_1Bytes()
        {
            byte[] original = new[] { (byte)0xFF };
            TestClone(original);
        }

        [Test]
        public void Clone_6Bytes()
        {
            byte[] original = new[] { (byte)0x00, (byte)0x01, (byte)0x02, (byte)0x04, (byte)0x09, (byte)0x00 };
            TestClone(original);
        }

        [Test]
        public void Clone_1000000Bytes()
        {
            byte[] original = new byte[1000000];
            for (int i = 0; i < original.Length; i++)
            {
                original[i] = (byte)(i % 256);
            }

            TestClone(original);
        }

        [Test]
        public void Clone_UnicodeBytes()
        {
            const string unicode = "ひらがな平仮名";
            byte[] original = Encoding.UTF8.GetBytes(unicode);
            byte[] clone = TestClone(original);
            string unicodeClone = Encoding.UTF8.GetString(clone);
            Assert.That(unicode, Is.EqualTo(unicodeClone));
        }

        private static byte[] TestClone(byte[] original)
        {
            Assert.That(original, Is.Not.Null);

            byte[] clone = ByteUtils.Clone(original);

            Assert.That(clone != original, "clone != original failed");
            Assert.That(clone.Length, Is.EqualTo(original.Length), "clone.Length != original.Length");

            for (int i = 0; i < original.Length; i++)
            {
                Assert.That(clone[i], Is.EqualTo(original[i]), string.Format("clone[{0}] ({1}) != original[{0}] ({2})", i, clone[i], original[i]));
            }

            return clone;
        }

        [Test]
        public void Compare_TwoParameters_Nulls()
        {
            // first parameter null
            bool result = ByteUtils.Compare(null, new byte[0]);
            Assert.That(result, Is.False, "first parameter null (1)");
            result = ByteUtils.Compare(null, new byte[] { 0xFF });
            Assert.That(result, Is.False, "first parameter null (2)");

            // second parameter null
            result = ByteUtils.Compare(new byte[0], null);
            Assert.That(result, Is.False, "second parameter null (1)");
            result = ByteUtils.Compare(new byte[] { 0xFF }, null);
            Assert.That(result, Is.False, "second parameter null (2)");

            // both parameters null
            result = ByteUtils.Compare(null, null);
            Assert.That(result, Is.True, "both parameters null");
        }

        [Test]
        public void Compare_TwoParameters()
        {
            // equal
            bool result = ByteUtils.Compare(new byte[0], new byte[0]);
            Assert.That(result, Is.True);

            result = ByteUtils.Compare(new[] { (byte)0xFF }, new[] { (byte)0xFF });
            Assert.That(result, Is.True);

            result = ByteUtils.Compare(new[] { (byte)0xFF, (byte)0xFE }, new[] { (byte)0xFF, (byte)0xFE });
            Assert.That(result, Is.True);

            result = ByteUtils.Compare(new[] { (byte)0xFF, (byte)0xFE, (byte)0x00 }, new[] { (byte)0xFF, (byte)0xFE, (byte)0x00 });
            Assert.That(result, Is.True);

            result = ByteUtils.Compare(new[] { (byte)0x00, (byte)0xFF, (byte)0xFE, (byte)0x00 }, new[] { (byte)0x00, (byte)0xFF, (byte)0xFE, (byte)0x00 });
            Assert.That(result, Is.True);

            byte[] sameReference = new[] { (byte)0x00, (byte)0x01 };
            result = ByteUtils.Compare(sameReference, sameReference);
            Assert.That(result, Is.True, "same reference");

            // extra value in x
            result = ByteUtils.Compare(new[] { (byte)0x00, (byte)0xFF, (byte)0xFE, (byte)0x00, (byte)0x01 }, new[] { (byte)0x00, (byte)0xFF, (byte)0xFE, (byte)0x00 });
            Assert.That(result, Is.False, "extra value in x");

            // extra value in y
            result = ByteUtils.Compare(new[] { (byte)0x00, (byte)0xFF, (byte)0xFE, (byte)0x00 }, new[] { (byte)0x00, (byte)0xFF, (byte)0xFE, (byte)0x00, (byte)0x01 });
            Assert.That(result, Is.False, "extra value in y");

            // same length, different values
            result = ByteUtils.Compare(new[] { (byte)0x00, (byte)0xFF, (byte)0xFE, (byte)0x00 }, new[] { (byte)0x00, (byte)0xFF, (byte)0xFE, (byte)0x02 });
            Assert.That(result, Is.False, "same length, different values (1)");

            // same length, different values
            result = ByteUtils.Compare(new[] { (byte)0x02, (byte)0xFF, (byte)0xFE, (byte)0x00 }, new[] { (byte)0x00, (byte)0xFF, (byte)0xFE, (byte)0x00 });
            Assert.That(result, Is.False, "same length, different values (2)");
        }

        [Test]
        public void Compare_ThreeParameters_MaxLengthArgumentExceptions()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => ByteUtils.Compare(null, null, -1));
        }

        [Test]
        public void Compare_ThreeParameters_Nulls()
        {
            // first parameter null
            bool result = ByteUtils.Compare(null, new byte[0], 0);
            Assert.That(result, Is.False, "first parameter null, second parameter byte[0]");
            result = ByteUtils.Compare(null, new byte[] { 0xFF }, 0);
            Assert.That(result, Is.False, "first parameter null, second paramter byte[1]");

            // second parameter null
            result = ByteUtils.Compare(new byte[0], null, 0);
            Assert.That(result, Is.False, "second parameter null, first parameter byte[0]");
            result = ByteUtils.Compare(new byte[] { 0xFF }, null, 0);
            Assert.That(result, Is.False, "second parameter null, first parameter byte[1]");

            // both parameters null
            result = ByteUtils.Compare(null, null, 10);
            Assert.That(result, Is.True, "both parameters null");
        }

        [Test]
        public void Compare_ThreeParameters()
        {
            bool result;

            // extra value in x, maxLength = 4
            result = ByteUtils.Compare(new[] { (byte)0x00, (byte)0xFF, (byte)0xFE, (byte)0x00, (byte)0x01 }, new[] { (byte)0x00, (byte)0xFF, (byte)0xFE, (byte)0x00 }, 4);
            Assert.That(result, Is.True, "extra value in x, maxLength = 4");

            // extra value in y, maxLength = 4
            result = ByteUtils.Compare(new[] { (byte)0x00, (byte)0xFF, (byte)0xFE, (byte)0x00 }, new[] { (byte)0x00, (byte)0xFF, (byte)0xFE, (byte)0x00, (byte)0x01 }, 4);
            Assert.That(result, Is.True, "extra value in y, maxLength = 4");

            // same length, different values, maxLength = 2
            result = ByteUtils.Compare(new[] { (byte)0x00, (byte)0xFF, (byte)0xFE, (byte)0x00 }, new[] { (byte)0x00, (byte)0xFF, (byte)0xFE, (byte)0x02 }, 3);
            Assert.That(result, Is.True, "same length, different values, maxLength = 2");

            // same length, different values, maxLength = 2
            result = ByteUtils.Compare(new[] { (byte)0x02, (byte)0xFF, (byte)0xFE, (byte)0x00 }, new[] { (byte)0x00, (byte)0xFF, (byte)0xFE, (byte)0x00 }, 2);
            Assert.That(result, Is.False, "same length, different values, maxLength = 2");

            // same length, different values, maxLength = 0
            result = ByteUtils.Compare(new[] { (byte)0x02, (byte)0xFF, (byte)0xFE, (byte)0x00 }, new[] { (byte)0x00, (byte)0xFF, (byte)0xFE, (byte)0x00 }, 0);
            Assert.That(result, Is.True, "same length, different values, maxLength = 0");

            // extra value in x, maxLength = 10
            result = ByteUtils.Compare(new[] { (byte)0x00, (byte)0xFF, (byte)0xFE, (byte)0x00, (byte)0x01 }, new[] { (byte)0x00, (byte)0xFF, (byte)0xFE, (byte)0x00 }, 10);
            Assert.That(result, Is.True, "extra value in x, maxLength = 10");

            // extra value in y, maxLength = 10
            result = ByteUtils.Compare(new[] { (byte)0x00, (byte)0xFF, (byte)0xFE, (byte)0x00 }, new[] { (byte)0x00, (byte)0xFF, (byte)0xFE, (byte)0x00, (byte)0x01 }, 10);
            Assert.That(result, Is.True, "extra value in y, maxLength = 10");

            // same length, different values, maxLength = 10
            result = ByteUtils.Compare(new[] { (byte)0x02, (byte)0xFF, (byte)0xFE, (byte)0x00 }, new[] { (byte)0x00, (byte)0xFF, (byte)0xFE, (byte)0x00 }, 10);
            Assert.That(result, Is.False, "same length, different values, maxLength = 10");

            // same reference
            byte[] sameReference = new[] { (byte)0x00, (byte)0x01 };
            result = ByteUtils.Compare(sameReference, sameReference, 100);
            Assert.That(result, Is.True, "same reference");

        }
    }
}
