using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace MegaApi.Tests
{
    [TestClass]
    public class UnitTest_Rsa
    {
        [TestMethod]
        public void Test_zclip()
        {
            uint[] input = new uint[]{0};

            uint[] expected = new uint[] { 0 };
            uint[] actual = Rsa.zclip(input);

            Assert.IsTrue(Utils.CompareTables(actual, expected));
        }

        [TestMethod]
        public void Test_nbits()
        {
            uint input = 0;

            int expected = 0;
            int actual = Rsa.nbits(input);

            Assert.Equals(actual, expected);
        }

        [TestMethod]
        public void Test_badd()
        {
            uint[] a = new uint[] { 0 };
            uint[] b = new uint[] { 0 };

            uint[] expected = new uint[] { 0 };
            uint[] actual = Rsa.badd(a, b);

            Assert.IsTrue(Utils.CompareTables(actual, expected));
        }

        [TestMethod]
        public void Test_bsub()
        {
            uint[] a = new uint[] { 0 };
            uint[] b = new uint[] { 0 };

            uint[] expected = new uint[] { 0 };
            uint[] actual = Rsa.bsub(a, b);

            Assert.IsTrue(Utils.CompareTables(actual, expected));
        }

        [TestMethod]
        public void Test_ip()
        {
            uint[] w = new uint[] { 0 };
            uint n = 0;
            uint x = 0;
            uint y = 0;
            uint c = 0;

            uint expected = 0;
            uint actual = Rsa.ip(w, n, x, y, c);
            
            Assert.Equals(expected, actual);
        }

        [TestMethod]
        public void Test_bsqr()
        {
            uint[] x = new uint[] { 0 };

            uint[] expected = new uint[] { 0 };
            uint[] actual = Rsa.bsqr(x);

            Assert.IsTrue(Utils.CompareTables(actual, expected));
        }

        [TestMethod]
        public void Test_bmul()
        {
            uint[] x = new uint[] { 0 };
            uint[] y= new uint[] { 0 };

            uint[] expected = new uint[] { 0 };
            uint[] actual = Rsa.bmul(x, y);

            Assert.IsTrue(Utils.CompareTables(actual, expected));
        }

        [TestMethod]
        public void Test_toppart()
        {
            uint[] x = new uint[] { 0 };
            int start = 0;
            int end = 0;

            uint expected = 0;
            uint actual = Rsa.toppart(x, start, end);

            Assert.Equals(expected, actual);
        }

        [TestMethod]
        public void Test_bdiv()
        {
            uint[] a = new uint[] { 0 };
            uint[] b = new uint[] { 0 };

            Rsa.QAndMod expected = new Rsa.QAndMod { q = new uint[] { 0 }, mod = new uint[] { 0 } };
            Rsa.QAndMod actual = Rsa.bdiv(a, b);

            Assert.Equals(expected, actual);
        }

        [TestMethod]
        public void Test_simplemod()
        {
            uint[] i = new uint[] { 0 };
            uint m = 0;

            uint expected = 0;
            uint actual = Rsa.simplemod(i, m);

            Assert.Equals(expected, actual);
        }

        [TestMethod]
        public void Test_bmod()
        {
            uint[] p = new uint[] { 0 };
            uint[] m = new uint[] { 0 };

            uint[] expected = new uint[] { 0 };
            uint[] actual = Rsa.bmod(p, m);

            Assert.IsTrue(Utils.CompareTables(actual, expected));
        }

        [TestMethod]
        public void Test_bmod2()
        {
            uint[] x = new uint[] { 0 };
            uint[] m = new uint[] { 0 };
            uint[] mu = new uint[] { 0 };

            uint[] expected = new uint[] { 0 };
            uint[] actual = Rsa.bmod2(x, m, mu);

            Assert.IsTrue(Utils.CompareTables(actual, expected));
        }

        [TestMethod]
        public void Test_bexpmod()
        {
            uint[] g = new uint[] { 0 };
            uint[] e = new uint[] { 0 };
            uint[] m = new uint[] { 0 };

            uint[] expected = new uint[] { 0 };
            uint[] actual = Rsa.bexpmod(g, e, m);

            Assert.IsTrue(Utils.CompareTables(actual, expected));
        }

        [TestMethod]
        public void Test_bmodexp()
        {
            uint[] g = new uint[] { 0 };
            uint[] e = new uint[] { 0 };
            uint[] m = new uint[] { 0 };

            uint[] expected = new uint[] { 0 };
            uint[] actual = Rsa.bmodexp(g, e, m);

            Assert.IsTrue(Utils.CompareTables(actual, expected));
        }

        [TestMethod]
        public void Test_RSAencrypt()
        {
            uint[] s = new uint[] { 0 };
            uint[] e = new uint[] { 0 };
            uint[] m = new uint[] { 0 };

            uint[] expected = new uint[] { 0 };
            uint[] actual = Rsa.RSAencrypt(s, e, m);

            Assert.IsTrue(Utils.CompareTables(actual, expected));
        }

        [TestMethod]
        public void Test_RSAdecrypt()
        {
            uint[] m = new uint[] { 1, 2, 3, 4 };
            uint[] d = new uint[] { 1, 2, 3, 4 };
            uint[] p = new uint[] { 1, 2, 3, 4 };
            uint[] q = new uint[] { 1, 2, 3, 4 };
            uint[] u = new uint[] { 1, 2, 3, 4 };

            uint[] expected = new uint[] { 0 };
            uint[] actual = Rsa.RSAdecrypt(m, d, p, q, u);

            Assert.IsTrue(Utils.CompareTables(actual, expected));
        }

        [TestMethod]
        public void Test_mpi2b()
        {
            // "CAClMKYMcWw4Wn4cNIXdbn7A18__s8IwGvIKQLAZspb-phMiniUscMyhOio7yuADKUckIGytjyF9R7ikh8H_u6ljrsm7ptn_jiquMAIcEC_5wRLmNh6cC2_7O4qtTRYXyBfOFHlB71sOrLCgsPvDXJQMTAGabc3EGDgAdX0wqLBpAniX01e_oehZM7B-ua-9HN2omuvBOerOJ9AsZaUafWaWdEofNqmZYSKc4f2fBmNcEZypzOVx0Up9vKuyMNRkKQYtTxDUjzn_64ESXADxE2J-0gmvIb_bzb2dpjTQ6o9PIHNEpsW-qoI_0reE0ugfTDQPk_P25SxagjAcc8gvB_w6"
            byte[] input = new byte[] { 8, 0, 165, 48, 166, 12, 113, 108, 56, 90, 126, 28, 52, 133, 221, 110, 126, 192, 215, 207, 255, 179, 194, 48, 26, 242, 10, 64, 176, 25, 178, 150, 254, 166, 19, 34, 158, 37, 44, 112, 204, 161, 58, 42, 59, 202, 224, 3, 41, 71, 36, 32, 108, 173, 143, 33, 125, 71, 184, 164, 135, 193, 255, 187, 169, 99, 174, 201, 187, 166, 217, 255, 142, 42, 174, 48, 2, 28, 16, 47, 249, 193, 18, 230, 54, 30, 156, 11, 111, 251, 59, 138, 173, 77, 22, 23, 200, 23, 206, 20, 121, 65, 239, 91, 14, 172, 176, 160, 176, 251, 195, 92, 148, 12, 76, 1, 154, 109, 205, 196, 24, 56, 0, 117, 125, 48, 168, 176, 105, 2, 120, 151, 211, 87, 191, 161, 232, 89, 51, 176, 126, 185, 175, 189, 28, 221, 168, 154, 235, 193, 57, 234, 206, 39, 208, 44, 101, 165, 26, 125, 102, 150, 116, 74, 31, 54, 169, 153, 97, 34, 156, 225, 253, 159, 6, 99, 92, 17, 156, 169, 204, 229, 113, 209, 74, 125, 188, 171, 178, 48, 212, 100, 41, 6, 45, 79, 16, 212, 143, 57, 255, 235, 129, 18, 92, 0, 241, 19, 98, 126, 210, 9, 175, 33, 191, 219, 205, 189, 157, 166, 52, 208, 234, 143, 79, 32, 115, 68, 166, 197, 190, 170, 130, 63, 210, 183, 132, 210, 232, 31, 76, 52, 15, 147, 243, 246, 229, 44, 90, 130, 48, 28, 115, 200, 47, 7, 252, 58 };

            uint[] expected = new uint[] { 252181562, 29834370, 207258160, 255815250, 204738451, 221151732, 265467780, 199927843, 54830789, 150270471, 104124650, 215734746, 253870043, 132980890, 15799138, 135341504, 255459307, 82906440, 69797421, 186846534, 176012459, 240590100, 27044300, 6698433, 216137119, 160829993, 169817769, 107571012, 94706301, 41747142, 20572878, 176795324, 264051933, 185068442, 32004403, 221608954, 151156887, 51022598, 134247805, 215761283, 201431661, 97075396, 11598787, 15387402, 155316059, 24961351, 219551688, 62434004, 202076155, 241394153, 268026130, 2212098, 237678128, 174956536, 61786555, 268155542, 145000385, 35116155, 7122319, 43283010, 197844995, 169058979, 86798540, 20064738, 43450022, 67830171, 1765898, 268123171, 247519183, 140367591, 176036916, 118932357, 87074316, 10 };
            uint[] actual = Rsa.mpi2b(input);

            Assert.IsTrue(Utils.CompareTables(actual, expected));
        }

        //[TestMethod]
        //public void Test_template()
        //{
        //    string input = "";

        //    string expected = "";
        //    string actual = "x";

        //    Assert.AreEqual(actual, expected);
        //}

        //[TestMethod]
        //public void Test_zclip()
        //{
        //    uint[] input = new uint[] { 0 };

        //    uint[] expected = new uint[] { 0 };
        //    uint[] actual = Rsa.zclip(input);

        //    Assert.IsTrue(Utils.CompareTables(actual, expected));
        //}
    }
}
