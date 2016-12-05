using IdSharp.Tagging.ID3v2;
using NUnit.Framework;

namespace IdSharp.Tagging.Tests
{
    [TestFixture]
    public class ID3v22_FrameTest : FrameTest
    {
        public ID3v22_FrameTest()
            : base(ID3v2TagVersion.ID3v22)
        {
        }

        [Ignore("Not supported")]
        public override void CommercialFrameWithLogo()
        {
            base.CommercialFrameWithLogo();
        }

        [Ignore("Not supported")]
        public override void CommercialFrameWithoutLogo()
        {
            base.CommercialFrameWithLogo();
        }

        [Ignore("Not supported")]
        public override void EncryptionMethodWithoutData()
        {
            base.EncryptionMethodWithoutData();
        }

        [Ignore("Not supported")]
        public override void EncryptionMethodWithData()
        {
            base.EncryptionMethodWithData();
        }
    }
}
