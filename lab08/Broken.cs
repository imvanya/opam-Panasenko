using System;

public static class BrokenSearch
{
    public static int BinarySearchBroken1<T>(T[] a, T target, Func<T, T, int> cmp, out int comparisons)
    {
        comparisons = 0;
        int lo = 0;
        int hi = a.Length - 1;

        while (lo < hi) 
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

    public static int BinarySearchBroken2<T>(T[] a, T target, Func<T, T, int> cmp, out int comparisons)
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
            else if (res < 0) lo = mid; 
            else hi = mid - 1;
        }
        return -1;
    }

    public static int BinarySearchBroken3<T>(T[] a, T target, Func<T, T, int> cmp, out int comparisons)
    {
        comparisons = 0;
        int lo = 0;
        int hi = a.Length - 1;

        while (lo <= hi)
        {
            int mid = (lo + hi) / 2; 
            comparisons++;
            int res = cmp(a[mid], target);

            if (res == 0) return mid;
            else if (res < 0) lo = mid + 1;
            else hi = mid - 1;
        }
        return -1;
    }

    public static int BinarySearchBroken4<T>(T[] a, T target, Func<T, T, int> cmp, out int comparisons)
    {
        return Search.BinarySearch(a, target, cmp, out comparisons);
    }
}