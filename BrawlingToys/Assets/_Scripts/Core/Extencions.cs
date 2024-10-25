using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BrawlingToys.Core
{
    public static class Extencions 
    {
        public static T RandomItem<T>(this List<T> list) => list[Random.Range(0, list.Count -1)];

        public static T RandomItem<T>(this T[] array) => array[Random.Range(0, array.Length)];

        public static IEnumerable<T> Randomize<T>(this IEnumerable<T> collection)
        {
            T[] items = collection.ToArray();
            System.Random random = new();

            int n = items.Length;

            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                T value = items[k];
                items[k] = items[n];
                items[n] = value;
            }

            foreach (T item in items)
            {
                yield return item;
            }
        }

        public static Transform[] GetAllChields(this Transform tr)
        {
            var chields = new Transform[tr.childCount]; 

            for (int i = 0; i < chields.Length; i++)
            {
                chields[i] = tr.GetChild(i); 
            }

            return chields; 
        } 
    }
}
