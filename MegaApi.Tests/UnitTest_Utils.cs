using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace MegaApi.Tests
{
    [TestClass]
    public class UnitTest_Utils
    {
        [TestMethod]
        public void Test_EnsureSize()
        {
            List<uint> input = new List<uint> { 1, 2 };

            uint[] expected = new uint[] { 1, 2, 0, 0, 0, 0 };
            MegaApi.Utils.EnsureIndex(input, 5);
            uint[] actual = input.ToArray();

            Assert.IsTrue(Utils.CompareTables(actual, expected));
        }

        [TestMethod]
        public void Test_ArraySlice1()
        {
            uint[] input = new uint[] { 1, 2, 3, 4, 5 };

            uint[] expected = new uint[] { 3, 4, 5 };
            uint[] actual = input.Slice(2);

            Assert.IsTrue(Utils.CompareTables(actual, expected));
        }

        [TestMethod]
        public void Test_ArraySlice2()
        {
            uint[] input = new uint[] { 1, 2, 3, 4, 5 };

            uint[] expected = new uint[] { 2, 3, 4 };
            uint[] actual = input.Slice(1, 4);

            Assert.IsTrue(Utils.CompareTables(actual, expected));
        }

        [TestMethod]
        public void Test_ArrayConcat()
        {
            uint[] a = new uint[] { 1, 2, 3 };
            uint[] b = new uint[] { 4, 5, 6 };

            uint[] expected = new uint[] { 1, 2, 3, 4, 5, 6 };
            uint[] actual = a.Concat(b);

            Assert.IsTrue(Utils.CompareTables(actual, expected));
        }
    }
}
