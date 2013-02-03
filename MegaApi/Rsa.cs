using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MegaApi
{
    public class Rsa
    {
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
    }
}
