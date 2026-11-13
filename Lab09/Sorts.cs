using System;

public static class Sorts
{
    public static void SelectionSort<T>(T[] a, Func<T, T, int> cmp, out long comparisons, out long moves)
    {
        comparisons = 0;
        moves = 0;
        int n = a.Length;

        for (int i = 0; i < n - 1; i++)
        {
            int minIdx = i;
            for (int j = i + 1; j < n; j++)
            {
                comparisons++;
                if (cmp(a[j], a[minIdx]) < 0)
                {
                    minIdx = j;
                }
            }

            if (minIdx != i)
            {
                T temp = a[i];
                a[i] = a[minIdx];
                a[minIdx] = temp;
                moves += 2; 
            }
        }
    }

    public static void QuickSort<T>(T[] a, Func<T, T, int> cmp, out long comparisons, out long moves)
    {
        comparisons = 0;
        moves = 0;
        if (a.Length <= 1) return;
        QuickSortRecursive(a, 0, a.Length - 1, cmp, ref comparisons, ref moves);
    }

    private static void QuickSortRecursive<T>(T[] a, int lo, int hi, Func<T, T, int> cmp, ref long comparisons, ref long moves)
    {
        if (lo >= hi) return;

        T pivot = a[lo + (hi - lo) / 2];
        int i = lo - 1;
        int j = hi + 1;

        while (true)
        {
            do
            {
                i++;
                comparisons++;
            } while (cmp(a[i], pivot) < 0);

            do
            {
                j--;
                comparisons++;
            } while (cmp(a[j], pivot) > 0);

            if (i >= j) break;

            T temp = a[i];
            a[i] = a[j];
            a[j] = temp;
            moves += 2;
        }

        QuickSortRecursive(a, lo, j, cmp, ref comparisons, ref moves);
        QuickSortRecursive(a, j + 1, hi, cmp, ref comparisons, ref moves);
    }

    public static void ShellSort<T>(T[] a, Func<T, T, int> cmp, out long comparisons, out long moves)
    {
        comparisons = 0;
        moves = 0;
        int n = a.Length;

        for (int gap = n / 2; gap > 0; gap /= 2)
        {
            for (int i = gap; i < n; i++)
            {
                T temp = a[i];
                int j = i;

                while (j >= gap)
                {
                    comparisons++;
                    if (cmp(a[j - gap], temp) > 0)
                    {
                        a[j] = a[j - gap];
                        moves++;
                        j -= gap;
                    }
                    else
                    {
                        break;
                    }
                }

                if (j != i)
                {
                    a[j] = temp;
                    moves++;
                }
            }
        }
    }

    public static void InsertionSort<T>(T[] a, Func<T, T, int> cmp, out long comparisons, out long moves)
    {
        comparisons = 0;
        moves = 0;
        int n = a.Length;

        for (int i = 1; i < n; i++)
        {
            T key = a[i];
            int j = i - 1;

            while (j >= 0)
            {
                comparisons++;
                if (cmp(a[j], key) > 0)
                {
                    a[j + 1] = a[j];
                    moves++;
                    j--;
                }
                else
                {
                    break;
                }
            }

            if (j + 1 != i)
            {
                a[j + 1] = key;
                moves++;
            }
        }
    }
}