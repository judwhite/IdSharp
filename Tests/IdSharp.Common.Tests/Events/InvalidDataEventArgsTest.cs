using IdSharp.Common.Events;
using NUnit.Framework;

namespace IdSharp.Common.Tests.Events
{
    [TestFixture]
    public class InvalidDataEventArgsTest
    {
        [Test]
        public void Test_Ctors()
        {
            var x = new InvalidDataEventArgs("propertyName", "message");
            Assert.AreEqual("propertyName", x.Property);
            Assert.AreEqual("message", x.Message);
            Assert.AreEqual(ErrorType.Warning, x.ErrorType);

            x = new InvalidDataEventArgs("propertyName2", "message", ErrorType.Error);
            Assert.AreEqual("propertyName2", x.Property);
            Assert.AreEqual("message", x.Message);
            Assert.AreEqual(ErrorType.Error, x.ErrorType);

            x = new InvalidDataEventArgs("propertyName3", ErrorType.None);
            Assert.AreEqual("propertyName3", x.Property);
            Assert.IsTrue(string.IsNullOrEmpty(x.Message));
            Assert.AreEqual(ErrorType.None, x.ErrorType);
        }
    }
}
