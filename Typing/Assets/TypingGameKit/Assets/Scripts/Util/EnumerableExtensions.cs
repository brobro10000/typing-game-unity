using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TypingGameKit.Util
{
    public static class EnumerableExtensions
    {
        public static T PickRandom<T>(this IEnumerable<T> sequence)
        {
            return sequence.ElementAtOrDefault(Random.Range(0, sequence.Count()));
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> sequence)
        {
            return sequence.OrderBy(_ => Random.Range(0f, 1f));
        }
    }
}