using IdSharp.Tagging.ID3v2;
using NUnit.Framework;

namespace IdSharp.Tagging.Tests
{
    [TestFixture]
    public class ID3v24_FrameTest : FrameTest
    {
        public ID3v24_FrameTest()
            : base(ID3v2TagVersion.ID3v24)
        {
        }
    }
}
