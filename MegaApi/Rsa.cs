using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


/* RSA public key encryption/decryption
 * The following functions are (c) 2000 by John M Hanna and are
 * released under the terms of the Gnu Public License.
 * You must freely redistribute them with their source -- see the
 * GPL for details.
 *  -- Latest version found at http://sourceforge.net/projects/shop-js
 *
 * Modifications and GnuPG multi precision integer (mpi) conversion added
 * 2004 by Herbert Hanewinkel, www.haneWIN.de
 */

// --- Arbitrary Precision Math ---
// badd(a,b), bsub(a,b), bsqr(a), bmul(a,b)
// bdiv(a,b), bmod(a,b), bexpmod(g,e,m), bmodexp(g,e,m)

namespace MegaApi
{
    public class Rsa
    {
        // bs is the shift, bm is the mask
        // set single precision bits to 28
        public static readonly int bs = 28;
        public static readonly int bx2 = 1 << bs;
        public static readonly int bm = bx2 - 1;
        public static readonly int bx = bx2 >> 1;
        public static readonly int bd = bs >> 1;
        public static readonly int bdm = (1 << bd) - 1;


        public static uint[] RSAdecrypt(uint[] t, uint[] p1, uint[] p2, uint[] p3, uint[] p4)
        {
            throw new NotImplementedException();
        }

        // -----------------------------------------------------------------
        // conversion functions: num array <-> multi precision integer (mpi)
        // mpi: 2 octets with length in bits + octets in big endian order

        public static uint[] mpi2b(byte[] s)
        {
            uint bn = 1;
            List<uint> r = new List<uint>();
            int rn = 0;
            uint sb = 256;
            uint c = 0;
            int sn = s.Length;
            if (sn < 2) return new uint[] { 0 };

            int len = (sn - 2) * 8;
            int bits = s[0] * 256 + s[1];
            if (bits > len || bits < len - 8) return new uint[] { 0 };

            for (var n = 0; n < len; n++)
            {
                if ((sb <<= 1) > 255)
                {
                    sb = 1; c = s[--sn];
                }

                if (bn > bm)
                {
                    bn = 1;
                    ++rn;
                    Utils.EnsureSize(r, rn);
                    r[rn] = 0;
                }

                if ((c & sb) != 0) { Utils.EnsureSize(r, rn); r[rn] |= bn; }

                bn <<= 1;
            }
            return r.ToArray();
        }
    }
}
