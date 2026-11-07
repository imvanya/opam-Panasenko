using System;
using System.Diagnostics;

class Program
{
    static void Main()
    {
        int N = 18;
        int K = 12;
        int a = 0;
        int b = 2;
        int c = 3;

        Console.WriteLine($"N = {N}, K = {K}, a = {a}, b = {b}, c = {c}");
        Console.WriteLine("========================================\n");

        int size = 100000;
        int maxVal = 20 * K; // 240
        int[] originalArray = new int[size];
        Random rand = new Random(N);

        for (int i = 0; i < size; i++)
        {
            originalArray[i] = rand.Next(0, maxVal + 1);
        }

        Func<int, int, int> intCmp = (x, y) => x.CompareTo(y);

        int[] sortedArray = (int[])originalArray.Clone();
        Array.Sort(sortedArray, (x, y) => intCmp(x, y));

        int X = 10 * K; // 120
        int absentVal = 20 * K + 1; // 241

        Console.WriteLine("Завдання 1: Лiнiйний пошук");
        int idx1 = Search.LinearSearch(sortedArray, X, intCmp, out int comp1);
        int idx2 = Search.LinearSearch(sortedArray, sortedArray[0], intCmp, out int comp2);
        int idx3 = Search.LinearSearch(sortedArray, absentVal, intCmp, out int comp3);

        Console.WriteLine($"1. Значення X ({X}): iндекс = {idx1}, порiвнянь = {comp1}");
        Console.WriteLine($"2. Перший елемент: iндекс = {idx2}, порiвнянь = {comp2}");
        Console.WriteLine($"3. Вiдсутнє значення ({absentVal}): iндекс = {idx3}, порiвнянь = {comp3}\n");

        Console.WriteLine("Завдання 2: Бiнарний пошук та поломки");
        Search.BinarySearch(sortedArray, X, intCmp, out int bComp1);
        Console.WriteLine($"Правильний бiнарний пошук для X ({X}): порiвнянь = {bComp1}");

        int[] unsortedArray = (int[])originalArray.Clone();
        int failedCount = 0;
        for (int i = 0; i < 100; i++)
        {
            int targetVal = sortedArray[rand.Next(0, sortedArray.Length)];
            BrokenSearch.BinarySearchBroken4(unsortedArray, targetVal, intCmp, out _);
            if (BrokenSearch.BinarySearchBroken4(unsortedArray, targetVal, intCmp, out _) == -1)
            {
                failedCount++;
            }
        }
        Console.WriteLine($"Поломка 4 (невiдсортований масив): не знайдено значень у {failedCount} з 100 випадкiв.\n");

        Console.WriteLine("Завдання 3: Пошук межi (b = 2)");
        int boundaryIdx = Search.FindBoundary(sortedArray, val => val >= X, out int boundComps);
        Console.WriteLine($"Iндекс першого елемента >= {X}: {boundaryIdx}, порiвнянь = {boundComps}\n");

        Console.WriteLine("Завдання 4: Експоненцiйний пошук (c = 3)");
        int expIdx = Search.ExponentialSearch(sortedArray, X, intCmp, out int expComps);
        Console.WriteLine($"Експоненцiйний пошук для X ({X}): iндекс = {expIdx}, порiвнянь = {expComps}\n");

        Console.WriteLine("Завдання 5: Вимiрювання точки окупностi");
        int[] qValues = { 1, 5, 10, 20, 30, 50, 100 };
        Console.WriteLine("q\tЛiнiйно (мс)\tСортування + бiнарно (мс)\tХто швидший");

        foreach (int q in qValues)
        {
            MeasureLinear(sortedArray, q, intCmp);
            MeasureSortAndBinary(originalArray, q, intCmp);

            long minLinearTime = long.MaxValue;
            for (int run = 0; run < 3; run++)
            {
                Stopwatch sw = Stopwatch.StartNew();
                MeasureLinear(sortedArray, q, intCmp);
                sw.Stop();
                if (sw.ElapsedMilliseconds < minLinearTime)
                    minLinearTime = sw.ElapsedMilliseconds;
            }

            long minSortBinaryTime = long.MaxValue;
            for (int run = 0; run < 3; run++)
            {
                Stopwatch sw = Stopwatch.StartNew();
                MeasureSortAndBinary(originalArray, q, intCmp);
                sw.Stop();
                if (sw.ElapsedMilliseconds < minSortBinaryTime)
                    minSortBinaryTime = sw.ElapsedMilliseconds;
            }

            string faster = minLinearTime < minSortBinaryTime ? "Лiнiйний" : "Сортування + бiнарний";
            Console.WriteLine($"{q}\t{minLinearTime}\t\t{minSortBinaryTime}\t\t\t{faster}");
        }
    }

    static void MeasureLinear(int[] arr, int q, Func<int, int, int> cmp)
    {
        Random r = new Random(42);
        for (int i = 0; i < q; i++)
        {
            int target = arr[r.Next(arr.Length)];
            Search.LinearSearch(arr, target, cmp, out _);
        }
    }

    static void MeasureSortAndBinary(int[] original, int q, Func<int, int, int> cmp)
    {
        int[] workArr = (int[])original.Clone();
        Array.Sort(workArr, (x, y) => cmp(x, y));
        Random r = new Random(42);
        for (int i = 0; i < q; i++)
        {
            int target = workArr[r.Next(workArr.Length)];
            Search.BinarySearch(workArr, target, cmp, out _);
        }
    }
}