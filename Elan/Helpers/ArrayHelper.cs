using System;
using System.Collections;

namespace Elan.Helpers
{
    internal static class ArrayHelper
    {
        public static Array Append(Array array1, Array array2)
        {
            var array1Type = array1.GetType().GetElementType();
            var array2Type = array1.GetType().GetElementType();

            if (array1Type != array2Type)
            {
                throw new Exception("Типы массивов отличаются");
            }

            var list = new ArrayList(array1.Length + array2.Length - 1);
            list.AddRange(array1);
            list.AddRange(array2);
            return list.ToArray(array1Type);
        }
        public static Array Shrink(Array array, object removeValue)
        {
            var list = new ArrayList(array.Length - 1);
            foreach (var value in array)
            {
                if (value != removeValue)
                {
                    list.Add(value);
                }
            }
            list.TrimToSize();
            return list.ToArray(array.GetType().GetElementType());
        }
    }
}