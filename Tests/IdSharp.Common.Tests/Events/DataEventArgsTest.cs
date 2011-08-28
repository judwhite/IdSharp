using IdSharp.Common.Events;
using NUnit.Framework;

namespace IdSharp.Common.Tests.Events
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

        [Test]
        public void TestSetData_Null()
        {
            var dea = new DataEventArgs<string>("Hello");
            Assert.AreEqual("Hello", dea.Data);
            dea.Data = null;
            Assert.IsNull(dea.Data);
        }

        [Test]
        public void TestSetData_NotNull()
        {
            var dea = new DataEventArgs<string>("Hello");
            Assert.AreEqual("Hello", dea.Data);
            dea.Data = "World";
            Assert.AreEqual("World", dea.Data);
        }
    }
}
