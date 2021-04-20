using System.Collections.Generic;

namespace Npoi.Report.Extends
{
    public static class EnumerableExtend
    {
        public static bool IsNullOrEmpty<TSource>(this IEnumerable<TSource> source)
        {
            if (!source.IsNull())
            {
                using (IEnumerator<TSource> enumerator = source.GetEnumerator())
                {
                    if (enumerator.MoveNext())
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}