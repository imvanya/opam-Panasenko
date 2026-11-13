using System;
using System.Diagnostics;
using System.Text;

class Program
{
    static readonly Func<int, int, int> IntCmp = (x, y) => x.CompareTo(y);

    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        int N = 18, K = 12;

        Console.WriteLine($"N = {N}, K = {K}, a = 0, b = 2, c = 3\n");

        int[] sample8 = GenerateRandomArray(8, N, 20 * K);
        Console.WriteLine("Перші 8 елементів (для трасування): [" + string.Join(", ", sample8) + "]\n");

        int[] sameArr = new int[1000];
        Array.Fill(sameArr, 42);
        Sorts.QuickSort(sameArr, IntCmp, out long sameCmp, out long sameMoves);
        Console.WriteLine($"QuickSort на 1000 однакових елементах: порівнянь = {sameCmp}, переставлянь = {sameMoves}\n");

        Console.WriteLine("--- Завдання 3: ShellSort vs InsertionSort (n = 1000) ---");
        var inputs1000 = CreateFourInputs(1000, N, K);
        string[] inputNames = { "Випадковий", "Відсортований", "Навпаки", "Майже" };
        for (int t = 0; t < 4; t++)
        {
            int[] a1 = (int[])inputs1000[t].Clone();
            int[] a2 = (int[])inputs1000[t].Clone();
            Sorts.ShellSort(a1, IntCmp, out long shC, out long shM);
            Sorts.InsertionSort(a2, IntCmp, out long inC, out long inM);
            Console.WriteLine($"{inputNames[t],-15} | Shell: {shC} / {shM} | Insertion: {inC} / {inM}");
        }
        Console.WriteLine();

        Console.WriteLine("--- Завдання 4: Порівняння / Переставляння ---");
        int[] sizes4 = { 100, 1000, 10000 };
        foreach (int n in sizes4)
        {
            Console.WriteLine($"\nРозмір n = {n}:");
            var inputs = CreateFourInputs(n, N, K);
            for (int t = 0; t < 4; t++)
            {
                int[] orig = inputs[t];

                int[] cSel = (int[])orig.Clone();
                Sorts.SelectionSort(cSel, IntCmp, out long selC, out long selM);
                Verify(cSel, orig, "Selection");

                int[] cQck = (int[])orig.Clone();
                Sorts.QuickSort(cQck, IntCmp, out long qckC, out long qckM);
                Verify(cQck, orig, "Quick");

                int[] cShl = (int[])orig.Clone();
                Sorts.ShellSort(cShl, IntCmp, out long shlC, out long shlM);
                Verify(cShl, orig, "Shell");

                Console.WriteLine($"  {inputNames[t],-14} | Selection: {selC}/{selM} | Quick: {qckC}/{qckM} | Shell: {shlC}/{shlM}");
            }
        }
        Console.WriteLine();

        Console.WriteLine("--- Завдання 5: Час виконання (мс, мін. з 3 запусків) ---");
        int[] timeSizes = { 1000, 10000, 100000, 1000000 };
        foreach (int n in timeSizes)
        {
            int[] baseArr = GenerateRandomArray(n, N, 20 * K);

            string tSel = n <= 100000 ? MeasureTime(baseArr, arr => Sorts.SelectionSort(arr, IntCmp, out _, out _)).ToString("F3") : "—";
            double tQck = MeasureTime(baseArr, arr => Sorts.QuickSort(arr, IntCmp, out _, out _));
            double tShl = MeasureTime(baseArr, arr => Sorts.ShellSort(arr, IntCmp, out _, out _));
            double tArr = MeasureTime(baseArr, arr => Array.Sort(arr, (x, y) => IntCmp(x, y)));

            Console.WriteLine($"n = {n,-8} | Selection: {tSel,-10} | Quick: {tQck,-10:F3} | Shell: {tShl,-10:F3} | Array.Sort: {tArr:F3}");
        }
        Console.WriteLine();

        Console.WriteLine("--- Пошук межі прийнятності (> 1 сек) для SelectionSort ---");
        for (int n = 15000; n <= 60000; n += 5000)
        {
            int[] arr = GenerateRandomArray(n, N, 20 * K);
            double ms = MeasureTime(arr, a => Sorts.SelectionSort(a, IntCmp, out _, out _));
            Console.WriteLine($"n = {n}: {ms:F1} мс");
            if (ms > 1000.0)
            {
                Console.WriteLine($"-> Межа прийнятності досягнута при n = {n}\n");
                break;
            }
        }

        Console.WriteLine("--- Перевірка стабільності на Student[] ---");
        TestStability();

        Console.WriteLine("\n--- Перевірка крайових випадків ---");
        TestEdgeCases();
    }

    static int[] GenerateRandomArray(int n, int seed, int maxVal)
    {
        Random rnd = new Random(seed);
        int[] arr = new int[n];
        for (int i = 0; i < n; i++) arr[i] = rnd.Next(0, maxVal + 1);
        return arr;
    }

    static int[][] CreateFourInputs(int n, int seed, int K)
    {
        int[] random = GenerateRandomArray(n, seed, 20 * K);

        int[] sorted = (int[])random.Clone();
        Array.Sort(sorted);

        int[] reversed = (int[])sorted.Clone();
        Array.Reverse(reversed);

        int[] nearly = (int[])sorted.Clone();
        Random swapRnd = new Random(seed + 100);
        for (int i = 0; i < K; i++)
        {
            int idx1 = swapRnd.Next(0, n);
            int idx2 = swapRnd.Next(0, n);
            (nearly[idx1], nearly[idx2]) = (nearly[idx2], nearly[idx1]);
        }

        return new[] { random, sorted, reversed, nearly };
    }

    static void Verify(int[] sorted, int[] original, string name)
    {
        if (!Validation.IsSorted(sorted, IntCmp) || !Validation.IsPermutationOf(sorted, original))
            throw new Exception($"Помилка перевірки в алгоритмі {name}!");
    }

    static double MeasureTime(int[] original, Action<int[]> sortAction)
    {
        int[] warm = (int[])original.Clone();
        sortAction(warm);

        double minMs = double.MaxValue;
        for (int r = 0; r < 3; r++)
        {
            int[] copy = (int[])original.Clone();
            Stopwatch sw = Stopwatch.StartNew();
            sortAction(copy);
            sw.Stop();
            if (sw.Elapsed.TotalMilliseconds < minMs)
                minMs = sw.Elapsed.TotalMilliseconds;
        }
        return minMs;
    }

    static void TestStability()
    {
        Student[] baseStudents =
        {
            new("Бондаренко", "КН-21", 85),
            new("Коваленко", "КН-21", 90),
            new("Мельник", "КН-22", 85),
            new("Панасенко", "КН-21", 90),
            new("Ткаченко", "КН-22", 85)
        };

        Func<Student, Student, int> byScore = (a, b) => a.Score.CompareTo(b.Score);

        CheckStudentSort("SelectionSort", baseStudents, arr => Sorts.SelectionSort(arr, byScore, out _, out _));
        CheckStudentSort("QuickSort", baseStudents, arr => Sorts.QuickSort(arr, byScore, out _, out _));
        CheckStudentSort("ShellSort", baseStudents, arr => Sorts.ShellSort(arr, byScore, out _, out _));
        CheckStudentSort("Array.Sort", baseStudents, arr => Array.Sort(arr, (a, b) => byScore(a, b)));
    }

    static void CheckStudentSort(string name, Student[] original, Action<Student[]> sortAction)
    {
        Student[] copy = (Student[])original.Clone();
        sortAction(copy);
        bool isStable = true;
        string counterExample = "—";

        for (int i = 1; i < copy.Length; i++)
        {
            if (copy[i - 1].Score == copy[i].Score && string.Compare(copy[i - 1].Name, copy[i].Name, StringComparison.Ordinal) > 0)
            {
                isStable = false;
                counterExample = $"{copy[i - 1]} перед {copy[i]}";
                break;
            }
        }
        Console.WriteLine($"{name,-15} | Стабільний: {(isStable ? "Так" : "Ні"),-4} | Контрприклад: {counterExample}");
    }

    static void TestEdgeCases()
    {
        int[][] cases =
        {
            Array.Empty<int>(),
            new[] { 5 },
            new[] { 1, 2 },
            new[] { 2, 1 },
            new[] { 7, 7, 7, 7, 7 },
            new[] { 10, 20, 10, 20, 10, 20 }
        };
        string[] names = { "Порожній", "Один елемент", "Два в порядку", "Два навпаки", "Усі однакові", "Два значення по пів масиву" };

        for (int i = 0; i < cases.Length; i++)
        {
            int[] orig = cases[i];

            int[] a1 = (int[])orig.Clone(); Sorts.SelectionSort(a1, IntCmp, out _, out _); Verify(a1, orig, "Selection");
            int[] a2 = (int[])orig.Clone(); Sorts.QuickSort(a2, IntCmp, out _, out _); Verify(a2, orig, "Quick");
            int[] a3 = (int[])orig.Clone(); Sorts.ShellSort(a3, IntCmp, out _, out _); Verify(a3, orig, "Shell");

            Console.WriteLine($"{names[i],-28}: OK (IsSorted = True, IsPermutation = True)");
        }
    }
}