using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MegaApi
{
    public static class Crypto
    {
        public class Aes
        {
            uint[] _key;

            public static byte[] BigEUintToByte(uint[] bigdata)
            {
                // stupid but trying to use the same data as the website
                // convert 4 big endian uints to little endian byte array

                byte[] data = new byte[16];

                byte[] le0 = BitConverter.GetBytes(bigdata[0]);
                byte[] le1 = BitConverter.GetBytes(bigdata[1]);
                byte[] le2 = BitConverter.GetBytes(bigdata[2]);
                byte[] le3 = BitConverter.GetBytes(bigdata[3]);
                //Array.Reverse(le0);
                //Array.Reverse(le1);
                //Array.Reverse(le2);
                //Array.Reverse(le3);

                Array.Copy(le0, 0, data, 0, 4);
                Array.Copy(le1, 0, data, 4, 4);
                Array.Copy(le2, 0, data, 8, 4);
                Array.Copy(le3, 0, data, 12, 4);

                return data;
            }

            public static uint[] ByteToBigEUint(byte[] littledata)
            {
                // stupid but trying to use the same data as the website
                // convert 4 big endian uints to little endian byte array

                uint[] data = new uint[4];

                byte[] le0 = new byte[4];
                byte[] le1 = new byte[4];
                byte[] le2 = new byte[4];
                byte[] le3 = new byte[4];

                Array.Copy(littledata, 0, le0, 0, 4);
                Array.Copy(littledata, 4, le1, 0, 4);
                Array.Copy(littledata, 8, le2, 0, 4);
                Array.Copy(littledata, 12, le3, 0, 4);
                //Array.Reverse(le0);
                //Array.Reverse(le1);
                //Array.Reverse(le2);
                //Array.Reverse(le3);

                data[0] = BitConverter.ToUInt32(le0, 0);
                data[1] = BitConverter.ToUInt32(le1, 0);
                data[2] = BitConverter.ToUInt32(le2, 0);
                data[3] = BitConverter.ToUInt32(le3, 0);

                return data;
            }

            public Aes(uint[] key)
            {
                _key = key;
            }

            public uint[] Encrypt(uint[] bigdata)
            {
                byte[] data = BigEUintToByte(bigdata);
                byte[] encrypted;

                using (var aesAlg = new AesManaged())
                {
                    aesAlg.Key = BigEUintToByte(_key);
                    //aesAlg.IV = IV;

                    // Create a decrytor to perform the stream transform.
                    ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                    // Create the streams used for encryption.
                    using (MemoryStream msEncrypt = new MemoryStream())
                    {
                        using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        {
                            using (BinaryWriter swEncrypt = new BinaryWriter(csEncrypt))
                            {

                                //Write all data to the stream.
                                swEncrypt.Write(data);
                            }
                            encrypted = msEncrypt.ToArray();
                        }
                    }
                }


                // Return the encrypted bytes from the memory stream.
                return ByteToBigEUint(encrypted);
            }
        }

        public static uint[] prepare_key_pw(string password)
        {
            return prepare_key(str_to_a32(password));
        }

        // convert user-supplied password array
        public static uint[] prepare_key(uint[] a)
        {
            int i, j, r;
            var pkey = new uint[] { 0x93C467E3, 0x7DB0C7A4, 0xD1BE3F81, 0x0152CB56 };

            for (r = 65536; r-- > 0; )
            {
                for (j = 0; j < a.Length; j += 4)
                {
                    var key = new uint[] { 0, 0, 0, 0 };

                    for (i = 0; i < 4; i++)
                    {
                        if (i + j < a.Length)
                        {
                            key[i] = a[i + j];
                        }
                    }

                    //var aes = new sjcl.cipher.aes(key);
                    //pkey = aes.encrypt(pkey);

                    var aes = new Aes(key);
                    pkey = aes.Encrypt(pkey);
		        }
	        }

	        return pkey;
        }

        // string to array of 32-bit words (big endian)
        public static uint[] str_to_a32(string b)
        {
            var a = new uint[(b.Length + 3) >> 2];

            for (var i = 0; i < b.Length; i++)
            {
                a[i >> 2] |= ((uint)b[i] << (24 - (i & 3) * 8));
            }

            return a;
        }
    }
}
