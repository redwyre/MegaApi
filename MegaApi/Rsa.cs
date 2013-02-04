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

        public static readonly double log2 = Math.Log(2);

        public static uint[] zclip(uint[] r)
        {
            var n = r.Length;
            if (r[n - 1] != 0) return r;
            while (n > 1 && r[n - 1] == 0) n--;
            return r.Take(n).ToArray();
        }

        // returns bit length of integer x
        public static int nbits(uint x)
        {
            int n = 1;
            uint t;
            if ((t = x >> 16) != 0) { x = t; n += 16; }
            if ((t = x >> 8) != 0) { x = t; n += 8; }
            if ((t = x >> 4) != 0) { x = t; n += 4; }
            if ((t = x >> 2) != 0) { x = t; n += 2; }
            if ((t = x >> 1) != 0) { x = t; n += 1; }
            return n;
        }

        public static uint[] badd(uint[] a, uint[] b)
        {
            //var al=a.length;
            //var bl=b.length;

            //if(al < bl) return badd(b,a);

            //var r=new Array(al);
            //var c=0, n=0;

            //for(; n<bl; n++)
            //{
            // c+=a[n]+b[n];
            // r[n]=c & bm;
            // c>>>=bs;
            //}
            //for(; n<al; n++)
            //{
            // c+=a[n];
            // r[n]=c & bm;
            // c>>>=bs;
            //}
            //if(c) r[n]=c;
            //return r;

            return new uint[] { };
        }

        public static uint[] bsub(uint[] a, uint[] b)
        {
            // var al=a.length;
            // var bl=b.length;

            // if(bl > al) return [];
            // if(bl == al)
            // {
            //  if(b[bl-1] > a[bl-1]) return [];
            //  if(bl==1) return [a[0]-b[0]];
            // }

            // var r=new Array(al);
            // var c=0;

            // for(var n=0; n<bl; n++)
            // {
            //  c+=a[n]-b[n];
            //  r[n]=c & bm;
            //  c>>=bs;
            // }
            // for(;n<al; n++)
            // {
            //  c+=a[n];
            //  r[n]=c & bm;
            //  c>>=bs;
            // }
            // if(c) return [];

            // return zclip(r);            

            return new uint[] { };
        }

        public static uint ip(uint[] w, uint n, uint x, uint y, uint c)
        {
            var xl = x & bdm;
            var xh = x >> bd;

            var yl = y & bdm;
            var yh = y >> bd;

            var m = xh * yl + yh * xl;
            var l = xl * yl + ((m & bdm) << bd) + w[n] + c;
            w[n] = (uint)(l & bm);
            c = (uint)(xh * yh + (m >> bd) + (l >> bs));
            return c;
        }

        // Multiple-precision squaring, HAC Algorithm 14.16

        public static uint[] bsqr(uint[] x)
        {
            uint t = (uint)x.Length;
            uint n = 2 * t;
            var r = new uint[n];
            uint c = 0;
            uint i, j;

            for (i = 0; i < t; i++)
            {
                c = ip(r, 2 * i, x[i], x[i], 0);
                for (j = i + 1; j < t; j++)
                {
                    c = ip(r, i + j, 2 * x[j], x[i], c);
                }
                r[i + t] = c;
            }

            return zclip(r);
        }

        // Multiple-precision multiplication, HAC Algorithm 14.12

        public static uint[] bmul(uint[] x, uint[] y)
        {
            var n = x.Length;
            var t = y.Length;
            var r = new uint[n + t - 1];
            uint c, i, j;

            for (i = 0; i < t; i++)
            {
                c = 0;
                for (j = 0; j < n; j++)
                {
                    c = ip(r, i + j, x[j], y[i], c);
                }
                r[i + n] = c;
            }

            return zclip(r);
        }

        public static uint toppart(uint[] x, int start, int len)
        {
            uint n = 0;
            while (start >= 0 && len-- > 0) n = (uint)(n * bx2 + x[start--]);
            return n;
        }


        public struct QAndMod
        {
            public uint[] q;
            public uint[] mod;
        }

        // Multiple-precision division, HAC Algorithm 14.20
        public static QAndMod bdiv(uint[] a, uint[] b)
        {
            var n = a.Length - 1;
            var t = b.Length - 1;
            var nmt = n - t;

            // trivial cases; a < b
            if (n < t || n == t && (a[n] < b[n] || n > 0 && a[n] == b[n] && a[n - 1] < b[n - 1]))
            {
                return new QAndMod { q = new uint[] { 0 }, mod = a };
            }

            // trivial cases; q < 4
            if (n == t && toppart(a, t, 2) / toppart(b, t, 2) < 4)
            {
                var tx = a.ToArray();
                uint qq = 0;
                uint[] xx;
                for (; ; )
                {
                    xx = bsub(tx, b);
                    if (xx.Length == 0) break;
                    tx = xx; qq++;
                }
                return new QAndMod { q = new uint[] { qq }, mod = tx };
            }

            // normalize
            int shift2 = (int)Math.Floor(Math.Log(b[t]) / log2) + 1;
            int shift = bs - shift2;

            var x = a.ToArray();
            var y = b.ToArray();

            if (shift != 0)
            {
                for (int i = t; i > 0; i--) y[i] = (uint)(((y[i] << shift) & bm) | (y[i - 1] >> shift2));
                y[0] = (uint)((y[0] << shift) & bm);
                if ((x[n] & ((bm << shift2) & bm)) != 0)
                {
                    x[++n] = 0; nmt++;
                }
                for (int i = n; i > 0; i--) x[i] = (uint)(((x[i] << shift) & bm) | (x[i - 1] >> shift2));
                x[0] = (uint)((x[0] << shift) & bm);
            }

            int j;
            uint[] x2;
            var q = new uint[nmt + 1];
            var y2 = Utils.ArrayConcat(new uint[nmt], y);
            for (; ; )
            {
                x2 = bsub(x, y2);
                if (x2.Length == 0) break;
                q[nmt]++;
                x = x2;
            }

            var yt = y[t];
            var top = toppart(y, t, 2);

            for (int i = n; i > t; i--)
            {
                var m = i - t - 1;
                if (i >= x.Length) q[m] = 1;
                else if (x[i] == yt) q[m] = (uint)bm;
                else q[m] = (uint)Math.Floor(toppart(x, i, 2) / (double)yt);

                var topx = toppart(x, i, 3);
                while (q[m] * top > topx) q[m]--;

                //x-=q[m]*y*b^m
                y2 = y2.Skip(1).ToArray();
                x2 = bsub(x, bmul(new uint[] { q[m] }, y2));
                if (x2.Length == 0)
                {
                    q[m]--;
                    x2 = bsub(x, bmul(new uint[] { q[m] }, y2));
                }
                x = x2;
            }

            // de-normalize
            if (shift != 0)
            {
                for (int i = 0; i < x.Length - 1; i++) x[i] = (uint)((x[i] >> shift) | ((x[i + 1] << shift2) & bm));
                x[x.Length - 1] >>= shift;
            }

            return new QAndMod { q = zclip(q), mod = zclip(x) };
        }

        public static uint simplemod(uint[] i, uint m) // returns the mod where m < 2^bd
        {
            uint c = 0, v;
            for (var n = i.Length - 1; n >= 0; n--)
            {
                v = i[n];
                c = ((v >> bd) + (c << bd)) % m;
                c = (uint)(((v & bdm) + (c << bd)) % m);
            }
            return c;
        }

        public static uint[] bmod(uint[] p, uint[] m)
        {
            if (m.Length == 1)
            {
                if (p.Length == 1) return new uint[] { p[0] % m[0] };
                if (m[0] < bdm) return new uint[] { simplemod(p, m[0]) };
            }

            var r = bdiv(p, m);
            return r.mod;
        }

        // Barrett's modular reduction, HAC Algorithm 14.42

        public static uint[] bmod2(uint[] x, uint[] m, uint[] mu)
        {
            //var xl=x.Length - (m.Length << 1);
            //if(xl > 0) return bmod2(x.slice(0,xl).concat(bmod2(x.slice(xl),m,mu)),m,mu);

            //var ml1=m.Length+1, ml2=m.Length-1,rr;
            ////var q1=x.slice(ml2)
            ////var q2=bmul(q1,mu)
            //var q3=bmul(x.slice(ml2),mu).slice(ml1);
            //var r1=x.slice(0,ml1);
            //var r2=bmul(q3,m).slice(0,ml1);
            //var r=bsub(r1,r2);

            //if(r.Length==0)
            //{
            // r1[ml1]=1;
            // r=bsub(r1,r2);
            //}
            //for(var n=0;;n++)
            //{
            // rr=bsub(r,m);
            // if(rr.Length==0) break;
            // r=rr;
            // if(n>=3) return bmod2(r,m,mu);
            //}
            //return r;

            return new uint[] { };
        }

        // Modular exponentiation, HAC Algorithm 14.79

        public static uint[] bexpmod(uint[] g, uint[] e, uint[] m)
        {
            uint[] a = g.ToArray();
            int l = e.Length - 1;
            int n = nbits(e[l]) - 2;

            for (; l >= 0; l--)
            {
                for (; n >= 0; n -= 1)
                {
                    a = bmod(bsqr(a), m);
                    if ((e[l] & (1 << n)) != 0) a = bmod(bmul(a, g), m);
                }
                n = bs - 1;
            }
            return a;
        }

        // Modular exponentiation using Barrett reduction

        public static uint[] bmodexp(uint[] g, uint[] e, uint[] m)
        {
            var a = g.ToArray();
            var l = e.Length - 1;
            var n = m.Length * 2;
            var mu = new uint[n + 1];
            mu[n] = 1;
            mu = bdiv(mu, m).q;

            n = nbits(e[l]) - 2;

            for (; l >= 0; l--)
            {
                for (; n >= 0; n -= 1)
                {
                    a = bmod2(bsqr(a), m, mu);
                    if ((e[l] & (1 << n)) != 0) a = bmod2(bmul(a, g), m, mu);
                }
                n = bs - 1;
            }
            return a;
        }


        // -----------------------------------------------------
        // Compute s**e mod m for RSA public key operation

        public static uint[] RSAencrypt(uint[] s, uint[] e, uint[] m)
        {
            return bexpmod(s, e, m);
        }


        // Compute m**d mod p*q for RSA private key operations.
        public static uint[] RSAdecrypt(uint[] m, uint[] d, uint[] p, uint[] q, uint[] u)
        {
            var xp = bmodexp(bmod(m, p), bmod(d, bsub(p, new uint[] { 1 })), p);
            var xq = bmodexp(bmod(m, q), bmod(d, bsub(q, new uint[] { 1 })), q);

            var t = bsub(xq, xp);
            if (t.Length == 0)
            {
                t = bsub(xp, xq);
                t = bmod(bmul(t, u), q);
                t = bsub(q, t);
            }
            else
            {
                t = bmod(bmul(t, u), q);
            }
            return badd(bmul(t, p), xp);
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
