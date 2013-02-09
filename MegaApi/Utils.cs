using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace MegaApi
{
    public static class Utils
    {
        public static void EnsureIndex<T>(List<T> list, int count)
        {
            while (list.Count <= count)
            {
                list.Add(default(T));
            }
        }

        public static T[] Slice<T>(this T[] array, int begin, int end)
        {
            if (begin > end) throw new Exception();

            if (begin < 0)
            {
                begin = array.Length + begin;
            }

            if (end < 0)
            {
                end = array.Length + end;
            }

            int elements = end - begin;
            T[] ret = new T[elements];
            Array.Copy(array, begin, ret, 0, elements);
            return ret;
        }

        public static T[] Slice<T>(this T[] array, int begin)
        {
            return Slice<T>(array, begin, array.Length);
        }

        public static T[] Concat<T>(this T[] array, T[] other)
        {
            T[] ret = new T[array.Length + other.Length];
            Array.Copy(array, 0, ret, 0, array.Length);
            Array.Copy(other, 0, ret, array.Length, other.Length);
            return ret; 
        }
    }
}
