using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace MegaApi.Tests
{
    [TestClass]
    public class UnitTest_Hex
    {
        [TestMethod]
        public void Test_b2s()
        {
            uint[] input = new uint[] { 1, 2, 3, 4 };

            byte[] expected = new byte[] { 64, 0, 0, 3, 0, 0, 0, 32, 0, 0, 1 };
            byte[] actual = Hex.b2s(input);

            Assert.IsTrue(Enumerable.SequenceEqual(actual, expected));
        }
    }
}
