using System.Collections.Generic;
using NUnit.Framework;
using VKLib.NativeExtension;

namespace UnitTestProject
{
    [TestFixture]
    public class TestArrayExtension
    {
        [Test]
        public void Test_IsEmpty()
        {
            Assert.AreEqual(true, new int[0].IsEmpty());
            Assert.AreEqual(false, new int[1].IsEmpty());
        }

        [Test]
        public void Test_IsEvenElemCount()
        {
            Assert.AreEqual(true, new int[0].IsEvenElemCount());
            Assert.AreEqual(false, new int[1].IsEvenElemCount());
            Assert.AreEqual(true, new int[2].IsEvenElemCount());
        }
    }

    [TestFixture]
    public class TestEnumerableExtension
    {
        [Test]
        public void Test_IsEmpty()
        {
            Assert.AreEqual(true, new List<int>().IsEmpty());
            Assert.AreEqual(true, new List<int>(0).IsEmpty());
            Assert.AreEqual(true, new List<int>(1).IsEmpty());
            Assert.AreEqual(false, new List<int>(){1}.IsEmpty());
        }
    }
}