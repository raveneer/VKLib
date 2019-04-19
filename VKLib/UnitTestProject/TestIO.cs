using System;
using NUnit.Framework;
using VKLib.IO;
using VKLib.NativeExtension;

namespace UnitTestProject
{
    [TestFixture]
    public class TestIO
    {
        [Test]
        public void Test_RemoveNotUseableCharsToFolderName()
        {
            var folderName2 = "aa:aa";
            Assert.AreEqual("aa aa", FolderNameFixer.Fix(folderName2));
        }
    }

    [TestFixture]
    public class TestStringExtension
    {
        [Test]
        public void Test_GetBetweenChar()
        {
            Assert.AreEqual("11", "[11]22".GetBetween('[',']'));
            Assert.AreEqual("11", "[11]2]2".GetBetween('[', ']'));
            Assert.AreEqual("[11", "[[11]2]2".GetBetween('[', ']'));
        }

        [Test]
        public void Test_GetBetweenString()
        {
            Assert.AreEqual("11", "[11]22".GetBetween("[", "]"));
            Assert.AreEqual("22", "112233".GetBetween("11", "33"));
        }
    }
}