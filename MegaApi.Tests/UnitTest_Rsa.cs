using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace MegaApi.Tests
{
    [TestClass]
    public class UnitTest_Rsa
    {
        [TestMethod]
        public void Test_RSAdecrypt()
        {
            uint[] t = new uint[] { 1, 2, 3, 4 };
            uint[] p1 = new uint[] { 1, 2, 3, 4 };
            uint[] p2 = new uint[] { 1, 2, 3, 4 };
            uint[] p3 = new uint[] { 1, 2, 3, 4 };
            uint[] p4 = new uint[] { 1, 2, 3, 4 };

            uint[] expected = new uint[] { 0 };
            uint[] actual = Rsa.RSAdecrypt(t, p1, p2, p3, p4);

            Assert.IsTrue(Enumerable.SequenceEqual(actual, expected));
        }
    }
}
