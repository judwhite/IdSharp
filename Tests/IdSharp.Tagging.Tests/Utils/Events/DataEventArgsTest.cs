using IdSharp.Tagging.Utils.Events;
using NUnit.Framework;

namespace IdSharp.Tagging.Tests.Utils.Events
{
    [TestFixture]
    public class DataEventArgsTest
    {
        [Test]
        public void TestConstructorAndDataProperty_Null()
        {
            DataEventArgs<string> dea = new DataEventArgs<string>(null);
            Assert.IsNull(dea.Data);
        }

        [Test]
        public void TestConstructorAndDataProperty_NotNull()
        {
            DataEventArgs<int> dea = new DataEventArgs<int>(42);
            Assert.AreEqual(42, dea.Data);
        }
    }
}
