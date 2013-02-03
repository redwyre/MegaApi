using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MegaApi
{
    public class Hex
    {
        public static byte[] b2s(uint[] b)
        {
            List<byte> rr = new List<byte>();
            uint bn = 1, bc = 0;
            uint rb = 1;
            uint rn = 0;
            var bits = b.Length * Rsa.bs;
            byte[] r = new byte[bits]; // dunno how big this needs to be

            for (int n = 0; n < bits; n++)
            {
                if ((b[bc] & bn) != 0) r[rn] |= (byte)rb;
                if ((rb <<= 1) > 255)
                {
                    rb = 1; r[++rn] = 0;
                }
                if ((bn <<= 1) > Rsa.bm)
                {
                    bn = 1; bc++;
                }
            }

            while (rn >= 0 && r[rn] == 0) rn--;
            for (int n = 0; n <= rn; n++)
            {
                if (r[n] == 0)
                    rr.Clear();
                else
                    rr.Insert(0, r[n]);
            }
            return rr.ToArray();
        }
    }
}
