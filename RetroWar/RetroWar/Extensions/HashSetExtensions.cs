using System.Collections.Generic;

namespace RetroWar.Extensions
{
    public static class HashSetExtensions
    {
        public static void AddRange<T>(this HashSet<T> hashSet, HashSet<T> dataSet)
        {
            foreach (var item in dataSet)
            {
                hashSet.Add(item);
            }
        }
    }
}
