using System;

public static class Search
{
    public static int LinearSearch<T>(T[] a, T target, Func<T, T, int> cmp, out int comparisons)
    {
        comparisons = 0;
        for (int i = 0; i < a.Length; i++)
        {
            comparisons++;
            if (cmp(a[i], target) == 0)
            {
                return i;
            }
        }
        return -1;
    }

    public static int BinarySearch<T>(T[] a, T target, Func<T, T, int> cmp, out int comparisons)
    {
        comparisons = 0;
        int lo = 0;
        int hi = a.Length - 1;

        while (lo <= hi)
        {
            int mid = lo + (hi - lo) / 2;
            comparisons++;
            int res = cmp(a[mid], target);

            if (res == 0) return mid;
            else if (res < 0) lo = mid + 1;
            else hi = mid - 1;
        }
        return -1;
    }

    public static int FindBoundary<T>(T[] a, Func<T, bool> predicate, out int comparisons)
    {
        comparisons = 0;
        int lo = 0;
        int hi = a.Length;

        while (lo < hi)
        {
            int mid = lo + (hi - lo) / 2;
            comparisons++;
            if (predicate(a[mid]))
            {
                hi = mid;
            }
            else
            {
                lo = mid + 1;
            }
        }
        return lo;
    }

    public static int ExponentialSearch<T>(T[] a, T target, Func<T, T, int> cmp, out int comparisons)
    {
        comparisons = 0;
        if (a.Length == 0) return -1;

        comparisons++;
        if (cmp(a[0], target) == 0) return 0;

        int bound = 1;
        while (bound < a.Length)
        {
            comparisons++;
            if (cmp(a[bound], target) < 0)
            {
                bound *= 2;
            }
            else
            {
                break;
            }
        }

        int start = bound / 2;
        int end = Math.Min(bound, a.Length - 1);

        int lo = start;
        int hi = end;
        while (lo <= hi)
        {
            int mid = lo + (hi - lo) / 2;
            comparisons++;
            int res = cmp(a[mid], target);
            if (res == 0) return mid;
            else if (res < 0) lo = mid + 1;
            else hi = mid - 1;
        }
        return -1;
    }
}