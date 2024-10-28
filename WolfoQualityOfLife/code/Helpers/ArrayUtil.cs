﻿using System.Linq;

namespace WolfoQualityOfLife
{
    public static class ArrayUtil
    {
        public static T[] Add<T>(this T[] array, params T[] items)
        {
            return (array ?? Enumerable.Empty<T>()).Concat(items).ToArray();
        }

        public static T[] Remove<T>(this T[] array, params T[] items)
        {
            return (array ?? Enumerable.Empty<T>()).Except(items).ToArray();
        }
    }
}
