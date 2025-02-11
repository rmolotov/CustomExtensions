using System;
using System.Collections.Generic;

namespace CustomExtensions.Collections
{
    public static class DictionaryExtensions
    {
        public static void AddRange<TKey, TValue>(
            this IDictionary<TKey, TValue> target,
            IEnumerable<TValue> source,
            Func<TValue, TKey> key, Func<TValue, TValue> selector,
            bool set = true)
        {
            source.ForEach(i =>
            {
                var dKey = key(i);
                var dValue = selector(i);
                if (set)
                    target[dKey] = dValue;
                else
                    target.Add(key(i), selector(i));
            });
        }

        private static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T obj in source)
                action(obj);
            return source;
        }
    }
}

