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
    }
}
