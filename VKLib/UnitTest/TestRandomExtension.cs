using System;
using NUnit.Framework;
using VKLib.NativeExtension;

namespace UnitTest
{
    public class TestRandomExtension
    {
        [TestCase(-3.0, -1.0)]
        [TestCase(1.0, 3.0)]
        [TestCase(-1.0, 3.0)]
        [Test]
        public void Test_Range_Double(double min, double max)
        {
            var rnd = new Random();
            for (var i = 0; i < 10000; i++)
            {
                var rndDouble = rnd.Range(min, max);
                Assert.IsTrue(rndDouble > min);
                Assert.IsTrue(rndDouble < max);
            }
        }

        [TestCase(1f, -3f)]
        [TestCase(1f, 1f)]
        [Test]
        public void IfMinMaxFlipOrSameThrow(double min, double max)
        {
            var rnd = new Random();
            Assert.Throws<Exception>(() => rnd.Range(min, max));
        }
    }
}