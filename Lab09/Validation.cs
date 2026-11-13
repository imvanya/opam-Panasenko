using System;
using System.Collections.Generic;

public static class Validation
{
    public static bool IsSorted<T>(T[] a, Func<T, T, int> cmp)
    {
        for (int i = 1; i < a.Length; i++)
        {
            if (cmp(a[i - 1], a[i]) > 0)
                return false;
        }
        return true;
    }

    public static bool IsPermutationOf<T>(T[] sorted, T[] original) where T : notnull
    {
        if (sorted.Length != original.Length) return false;

        var counts = new Dictionary<T, int>();
        foreach (var item in original)
        {
            counts.TryGetValue(item, out int c);
            counts[item] = c + 1;
        }

        foreach (var item in sorted)
        {
            if (!counts.TryGetValue(item, out int c) || c == 0)
                return false;
            counts[item] = c - 1;
        }

        return true;
    }
}