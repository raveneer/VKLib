using NUnit.Framework;
using VKLib.IO;

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
}