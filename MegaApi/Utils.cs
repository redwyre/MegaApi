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
    }
}
