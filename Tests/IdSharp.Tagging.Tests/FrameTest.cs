using IdSharp.Tagging.ID3v2;
using NUnit.Framework;

namespace IdSharp.Tagging.Tests
{
    public abstract class FrameTest
    {
        private readonly ID3v2TagVersion _version;

        protected FrameTest(ID3v2TagVersion tagVersion)
        {
            _version = tagVersion;
        }

        [Test]
        public virtual void AudioEncryption()
        {
            Frames.AudioEncryption(_version);
        }

        [Test]
        public virtual void CommercialFrameWithoutLogo()
        {
            Frames.Commercial(_version, false);
        }

        [Test]
        public virtual void CommercialFrameWithLogo()
        {
            Frames.Commercial(_version, true);
        }

        [Test]
        public virtual void EncryptionMethodWithoutData()
        {
            Frames.EncryptionMethod(_version, false);
        }

        [Test]
        public virtual void EncryptionMethodWithData()
        {
            Frames.EncryptionMethod(_version, true);
        }
    }
}
