
using System.Collections.Generic;
using System.Linq;

namespace amulware.Graphics.utilities
{
    internal static class InternalExtensions
    {
        internal static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
        {
            return source == null || !source.Any();
        }
    }
}
