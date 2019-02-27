using System.Collections.Generic;

namespace MyTools
{
    public static class DictionayExtend
    {
        public static TValue GetOrCreateValue<TKey, TValue>(this Dictionary<TKey, TValue> dic, TKey key)
            where TValue : new()
        {
            TValue value;
            if (!dic.TryGetValue(key, out value))
            {
                value = new TValue();
                dic.Add(key, value);
            }

            return value;
        }
    }
}