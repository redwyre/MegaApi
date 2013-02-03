using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MegaApi.Tests
{
    [TestClass]
    public class UnitTest_Crypto
    {
        [TestMethod]
        public void Test_str_to_a32()
        {
            string pass = "test";
            uint[] expected = new uint[] { 0x74657374 };

            uint[] actual = Crypto.str_to_a32(pass);

            Assert.IsTrue(Utils.CompareTables(expected, actual));
        }

        [TestMethod]
        public void Test_a32_to_str()
        {
            uint[] input = new uint[] { 0x74657374 };
            byte[] expected = new byte[] { 0x74, 0x65, 0x73, 0x74 };

            byte[] actual = Crypto.a32_to_str(input);

            Assert.IsTrue(Utils.CompareTables(expected, actual));
        }

        [TestMethod]
        public void Test_base64urlencode()
        {
            byte[] input = new byte[] { 0x74, 0x65, 0x73, 0x74, 0x74, 0x65, 0x73, 0x74, 0x74, 0x65, 0x73, 0x74 }; // "testtesttest"
            string expected = "dGVzdHRlc3R0ZXN0";

            string actual = Crypto.base64urlencode(input);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Test_base64urldecode()
        {
            string input = "0123456789";

            byte[] expected = new byte[] { 211, 93, 183, 227, 158, 187, 243 };
            byte[] actual = Crypto.base64urldecode(input);

            Assert.IsTrue(Utils.CompareTables(actual, expected));
        }

        [TestMethod]
        public void Test_a32_to_base64()
        {
            uint[] a32 = new uint[] { 0x74657374 };
            string expected = "dGVzdA";

            string actual = Crypto.a32_to_base64(a32);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Test_prepare_key()
        {
            uint[] key = new uint[] { 1, 2, 3, 4 };
            uint[] expected = new uint[] { 0xF223D8D4, 0xE9F12172, 0x4B3CB51B, 0xB81D515C };

            uint[] actual = Crypto.prepare_key(key);

            Assert.IsTrue(Utils.CompareTables(expected, actual));
        }

        [TestMethod]
        public void Test_prepare_key_pw()
        {
            string pass = "test";
            uint[] expected = new uint[] { 0xBDB14516, 0xFEC014CD, 0xB97ADCE4, 0x1572ECF4 };

            uint[] actual = Crypto.prepare_key_pw(pass);

            Assert.IsTrue(Utils.CompareTables(expected, actual));
        }

        [TestMethod]
        public void Test_stringhash()
        {
            var aes = new Sjcl.Cipher.Aes(Crypto.prepare_key_pw(Config.TestUserPass));
            string input = Config.TestUserName.ToLower();

            string expected = Config.TestUserHash;
            string actual = Crypto.stringhash(input, aes);

            Assert.AreEqual(actual, expected);
        }

        [TestMethod]
        public void Test_base64_to_a32()
        {
            string input = "test";

            uint[] expected = new uint[] { 0xB5EB2D00 };
            uint[] actual = Crypto.base64_to_a32(input);

            Assert.IsTrue(Utils.CompareTables(actual, expected));
        }

        [TestMethod]
        public void Test_decrypt_key()
        {
            var aes = new Sjcl.Cipher.Aes(Crypto.prepare_key_pw(Config.TestUserPass));
            uint[] input = new uint[] {0};

            uint[] expected = new uint[] { 0 };
            uint[] actual = Crypto.decrypt_key(aes, input);

            Assert.IsTrue(Utils.CompareTables(actual, expected));
        }

        [TestMethod]
        public void Test_encrypt_key()
        {
            var aes = new Sjcl.Cipher.Aes(Crypto.prepare_key_pw(Config.TestUserPass));
            uint[] input = new uint[] { 0 };

            uint[] expected = new uint[] { 0 };
            uint[] actual = Crypto.encrypt_key(aes, input);

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
    }
}
