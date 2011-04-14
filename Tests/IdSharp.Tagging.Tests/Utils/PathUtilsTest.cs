using System;
using System.IO;
using System.Reflection;
using IdSharp.Tagging.Utils;
using NUnit.Framework;

namespace IdSharp.Tagging.Tests.Utils
{
    [TestFixture]
    public class PathUtilsTest
    {
        [TestFixtureSetUp]
        public void Setup()
        {
        }

        [Test]
        public void GetTempFileName_ArgumentExceptions()
        {
            Assert.Throws<ArgumentException>(() => PathUtils.GetTempFileName("."));
            Assert.Throws<ArgumentException>(() => PathUtils.GetTempFileName(".."));
            Assert.Throws<ArgumentException>(() => PathUtils.GetTempFileName("mp3."));
            Assert.Throws<ArgumentException>(() => PathUtils.GetTempFileName(".mp?3"));
            Assert.Throws<ArgumentException>(() => PathUtils.GetTempFileName(@".mp\3"));
            Assert.Throws<ArgumentException>(() => PathUtils.GetTempFileName(".mp:3"));
        }

        [Test]
        public void GetTempFileName_NormalCase()
        {
            string a = PathUtils.GetTempFileName(null);
            string b = PathUtils.GetTempFileName(string.Empty);
            string c = PathUtils.GetTempFileName(".txt");
            string d = PathUtils.GetTempFileName("txt");

            Assert.That(string.IsNullOrEmpty(Path.GetExtension(a)));
            Assert.That(string.IsNullOrEmpty(Path.GetExtension(b)));
            Assert.That(Path.GetExtension(c), Is.EqualTo(".txt"));
            Assert.That(Path.GetExtension(d), Is.EqualTo(".txt"));

            Assert.That(!File.Exists(a));
            Assert.That(!File.Exists(b));
            Assert.That(!File.Exists(c));
            Assert.That(!File.Exists(d));
        }

        [Test]
        public void GetTemporaryFileNameBasedOnFileName_ArgumentExceptions()
        {
            Assert.Throws<ArgumentNullException>(() => PathUtils.GetTemporaryFileNameBasedOnFileName(null));
            Assert.Throws<ArgumentNullException>(() => PathUtils.GetTemporaryFileNameBasedOnFileName(string.Empty));
        }

        [Test]
        public void GetTemporaryFileNameBasedOnFileName_NormalCase()
        {
            const string path = @"C:\WINDOWS\HelloWorld.txt";
            string a = PathUtils.GetTemporaryFileNameBasedOnFileName(path);

            Assert.That(a.StartsWith(path));
            Assert.That(!File.Exists(a));
        }

        [Test]
        public void GetUniqueFileName_ArgumentExceptions()
        {
            Assert.Throws<ArgumentNullException>(() => PathUtils.GetUniqueFileName(null));
            Assert.Throws<ArgumentNullException>(() => PathUtils.GetUniqueFileName(string.Empty));
        }

        [Test]
        public void GetUniqueFileName_NormalCase_FileExists()
        {
            string assemblyLocation = Assembly.GetExecutingAssembly().Location;

            Assert.That(File.Exists(assemblyLocation), "File.Exists(assemblyLocation) failed");

            string uniqueFileName = PathUtils.GetUniqueFileName(assemblyLocation);

            Assert.That(uniqueFileName, Is.Not.Null);
            Assert.That(uniqueFileName != assemblyLocation, "uniqueFileName != assemblyLocation failed");
            Assert.That(Path.GetDirectoryName(uniqueFileName) == Path.GetDirectoryName(assemblyLocation), "Path.GetDirectoryName(uniqueFileName) == Path.GetDirectoryName(assemblyLocation) failed");
        }

        [Test]
        public void GetUniqueFileName_NormalCase_FileDoesNotExist()
        {
            string assemblyLocation = Assembly.GetExecutingAssembly().Location;
            string assemblyDirectory = Path.GetDirectoryName(assemblyLocation);
            string newFileName = Path.Combine(assemblyDirectory, "doesNotExist.txt");

            Assert.That(!File.Exists(newFileName), "!File.Exists(newFileName) failed");

            string uniqueFileName = PathUtils.GetUniqueFileName(newFileName);

            Assert.That(uniqueFileName, Is.Not.Null);
            Assert.That(uniqueFileName == newFileName, "uniqueFileName == newFileName failed");
        }
    }
}
