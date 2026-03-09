using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AnhPD
{
    public class APDEnumUtilities : MonoBehaviour
    {
        private static readonly System.Random Rng = new System.Random();
        public static T GetRandomEnumValue<T>(List<T> excludeList = null) where T : Enum
        {
            // Lấy tất cả value trong enum
            var values = Enum.GetValues(typeof(T)).Cast<T>().ToList();

            // Loại bỏ những giá trị trong excludeList
            if (excludeList != null && excludeList.Count > 0)
            {
                values = values.Except(excludeList).ToList();
            }

            if (values.Count == 0)
            {
                throw new InvalidOperationException(
                    $"Không còn giá trị nào hợp lệ trong enum {typeof(T).Name} sau khi loại bỏ!"
                );
            }

            // Chọn random
            int index = Rng.Next(values.Count);
            return values[index];
        }
        
        public static List<T> GetEnumValuesExcluding<T>(List<T> excludeList = null) where T : Enum
        {
            var values = Enum.GetValues(typeof(T)).Cast<T>().ToList();

            if (excludeList != null && excludeList.Count > 0)
            {
                values = values.Except(excludeList).ToList();
            }

            return values;
        }
    }
}
