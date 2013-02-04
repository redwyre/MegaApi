using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MegaApi
{
    public class Utils
    {
        public static void EnsureSize<T>(List<T> list, int count)
        {
            while (list.Count <= count)
            {
                list.Add(default(T));
            }
        }


        public static T[] ArrayConcat<T>(T[] a1, T[] a2)
        {
            T[] r = new T[a1.Length + a2.Length];
            Array.Copy(a1, 0, r, 0, a1.Length);
            Array.Copy(a2, 0, r, a1.Length, a2.Length);
            return r;
        }
    }
}
