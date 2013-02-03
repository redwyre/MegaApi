using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MegaApi.Tests
{
    class Utils
    {
        public static bool CompareTables<T>(T[] arr1, T[] arr2)
        {
            if (arr1.Length != arr2.Length) return false;

            for (int i = 0; i < arr1.Length; ++i)
            {
                if (!Comparer<T>.Equals(arr1[i], arr2[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool CompareTables<T>(T[][] arr1, T[][] arr2)
        {
            if (arr1.Length != arr2.Length) return false;

            for (int i = 0; i < arr1.Length; ++i)
            {
                if (!CompareTables(arr1[i], arr2[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool CompareTables<T>(T[][][] arr1, T[][][] arr2)
        {
            if (arr1.Length != arr2.Length) return false;

            for (int i = 0; i < arr1.Length; ++i)
            {
                if (!CompareTables(arr1[i], arr2[i]))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
