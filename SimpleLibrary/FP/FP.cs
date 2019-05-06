using System;
using System.Collections.Generic;

namespace SimpleLibrary.FP
{
    /// <summary>
    /// Extension Method
    /// 因為 IEnumerable 沒有 ForEach()，只有 List 才有 ForEach()，因此我們必須先 ToList()
    /// 所以就來替 IEnumerable 打造一個 ForEach()
    /// </summary>
    public static class Extensions
    {
        internal static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T element in source)
                action(element);
        }
    }
}
