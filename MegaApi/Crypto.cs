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

                    var aes = new Sjcl.Cipher.Aes(key);
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

        public static uint[] str_to_a32(byte[] b)
        {
            var a = new uint[(b.Length + 3) >> 2];

            for (var i = 0; i < b.Length; i++)
            {
                a[i >> 2] |= ((uint)b[i] << (24 - (i & 3) * 8));
            }

            return a;
        }

        // array of 32-bit words to string (big endian)
        public static byte[] a32_to_str(uint[] a)
        {
            List<byte> str = new List<byte>(a.Length / 4);

            for (var i = 0; i < a.Length * 4; i++)
            {
                str.Add((byte)((a[i >> 2] >> (24 - (i & 3) * 8)) & 255));
            }

            return str.ToArray();
        }

        public static string stringhash(string s, Sjcl.Cipher.Aes aes)
        {
            var s32 = str_to_a32(s);
            var h32 = new uint[] { 0, 0, 0, 0 };

            for (int i = 0; i < s32.Length; i++) { h32[i & 3] ^= s32[i]; }

            for (int i = 16384; (i--) != 0; ) { h32 = aes.Encrypt(h32); }

            return a32_to_base64(new uint[] { h32[0], h32[2] });
        }

        public static string a32_to_base64(uint[] a)
        {
            return base64urlencode(a32_to_str(a));
        }

        public static string base64urlencode(byte[] bytes)
        {
            string ss = Convert.ToBase64String(bytes);
            return ss.Replace('+', '-').Replace('/', '_').Replace("=", "");
        }

        public static byte[] base64urldecode(string data)
        {
            data += "==".Substring((2 - data.Length * 3) & 3);
            data = data.Replace('_', '/').Replace('-', '+').Replace(",", "");
            return Convert.FromBase64String(data);
        }


        public static uint[] base64_to_a32(string s)
        {
            return str_to_a32(base64urldecode(s));
        }

        public static uint[] decrypt_key(Sjcl.Cipher.Aes aes, uint[] keyData)
        {
            throw new NotImplementedException();
        }

        public static uint[] encrypt_key(Sjcl.Cipher.Aes aes, uint[] p)
        {
            throw new NotImplementedException();
        }
    }
}
