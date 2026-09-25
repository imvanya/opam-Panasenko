using System;


class Program
{
    static int[] GenerateArray(int size, int seed, int minInclusive, int maxInclusive)
    {
        Random rnd = new Random(seed);
        int[] a = new int[size];
        for (int i = 0; i < a.Length; i++)
        {
            a[i] = rnd.Next(minInclusive, maxInclusive + 1);
        }
        return a;
    }

    static int[] CopyArray(int[] a)
    {
        int[] copy = new int[a.Length];
        for (int i = 0; i < a.Length; i++)
        {
            copy[i] = a[i];
        }
        return copy;
    }

    static void PrintArray(int[] a, string title)
    {
        Console.Write(title + " [" + a.Length + "]: ");
        if (a.Length == 0)
        {
            Console.WriteLine("(порожнiй)");
            return;
        }
        for (int i = 0; i < a.Length; i++)
        {
            Console.Write(a[i]);
            if (i < a.Length - 1) Console.Write(", ");
        }
        Console.WriteLine();
    }

    struct Stats
    {
        public bool IsDefined; 
        public long Sum;
        public double Avg;
        public int Min;
        public int MinIndex;
        public int Max;
        public int MaxIndex;    
        public int ZerosCount;
    }

    static Stats ComputeStats(int[] a)
    {
        Stats s = new Stats();

        if (a.Length == 0)
        {
            s.IsDefined = false;
            return s;
        }

        s.IsDefined = true;
        s.Sum = 0;
        s.ZerosCount = 0;
        s.Min = a[0];
        s.MinIndex = 0;
        s.Max = a[0];
        s.MaxIndex = 0;

        for (int i = 0; i < a.Length; i++)
        {
            s.Sum += a[i];

            if (a[i] == 0)
            {
                s.ZerosCount++;
            }

            if (a[i] < s.Min)
            {
                s.Min = a[i];
                s.MinIndex = i;
            }

            if (a[i] > s.Max)
            {
                s.Max = a[i];
                s.MaxIndex = i;
            }
        }

        s.Avg = (double)s.Sum / a.Length;
        return s;
    }

    static void PrintStats(Stats s)
    {
        if (!s.IsDefined)
        {
            Console.WriteLine("  Масив порожнiй — результат не визначений.");
            return;
        }

        Console.WriteLine("  Сума       = " + s.Sum);
        Console.WriteLine("  Середнє    = " + s.Avg.ToString("F2"));
        Console.WriteLine("  Мiнiмум    = " + s.Min + " (iндекс " + s.MinIndex + ")");
        Console.WriteLine("  Максимум   = " + s.Max + " (iндекс " + s.MaxIndex + ", перше входження)");
        Console.WriteLine("  Кiлькiсть нулiв = " + s.ZerosCount);
    }

    static bool MatchesCriterion(int value)
    {
        return value > 2;
    }

    static int[] FilterByCriterion(int[] a)
    {
        int count = 0;
        for (int i = 0; i < a.Length; i++)
        {
            if (MatchesCriterion(a[i]))
            {
                count++;
            }
        }

        int[] result = new int[count];
        int idx = 0;
        for (int i = 0; i < a.Length; i++)
        {
            if (MatchesCriterion(a[i]))
            {
                result[idx] = a[i];
                idx++;
            }
        }
        return result;
    }

    struct RunResult
    {
        public bool IsDefined; 
        public int Value;
        public int Length;
        public int StartIndex;
    }

    static RunResult LongestRun(int[] a)
    {
        RunResult best = new RunResult();

        if (a.Length == 0)
        {
            best.IsDefined = false;
            return best;
        }

        best.IsDefined = true;
        best.Value = a[0];
        best.Length = 1;
        best.StartIndex = 0;

        int curStart = 0;
        int curLength = 1;

        for (int i = 1; i < a.Length; i++)
        {
            if (a[i] == a[i - 1])
            {
                curLength++;
            }
            else
            {
                curStart = i;
                curLength = 1;
            }

            if (curLength > best.Length)
            {
                best.Length = curLength;
                best.Value = a[i];
                best.StartIndex = curStart;
            }
        }

        return best;
    }

    static void PrintRun(RunResult r)
    {
        if (!r.IsDefined)
        {
            Console.WriteLine("  Масив порожній — результат не визначений.");
            return;
        }
        Console.WriteLine("  Значення = " + r.Value + ", довжина серії = " + r.Length + ", початок з індексу " + r.StartIndex);
    }

    static void MoveNonPositiveToFront(int[] a)
    {
        int writePos = 0; 

        for (int i = 0; i < a.Length; i++)
        {
            if (a[i] <= 0)
            {
                int temp = a[i];
                a[i] = a[writePos];
                a[writePos] = temp;
                writePos++;
            }
        }
    }

    static int[,] CreateMatrix(int rows, int cols, int seed)
    {
        int[,] m = new int[rows, cols];
        Random rnd = new Random(seed);

        for (int i = 0; i < m.GetLength(0); i++)
        {
            for (int j = 0; j < m.GetLength(1); j++)
            {
                m[i, j] = rnd.Next(1, 5); // [1; 4]
            }
        }
        return m;
    }

    static int[] RowSums(int[,] m)
    {
        int[] sums = new int[m.GetLength(0)];
        for (int i = 0; i < m.GetLength(0); i++)
        {
            int sum = 0;
            for (int j = 0; j < m.GetLength(1); j++)
            {
                sum += m[i, j];
            }
            sums[i] = sum;
        }
        return sums;
    }

    static int[] ColumnMaxes(int[,] m)
    {
        int[] maxes = new int[m.GetLength(1)];
        for (int j = 0; j < m.GetLength(1); j++)
        {
            int max = m[0, j];
            for (int i = 1; i < m.GetLength(0); i++)
            {
                if (m[i, j] > max)
                {
                    max = m[i, j];
                }
            }
            maxes[j] = max;
        }
        return maxes;
    }

    static int RowIndexWithMaxSum(int[] sums)
    {
        int bestIndex = 0;
        for (int i = 1; i < sums.Length; i++)
        {
            if (sums[i] > sums[bestIndex])
            {
                bestIndex = i;
            }
        }
        return bestIndex;
    }

    static void PrintMatrix(int[,] m)
    {
        for (int i = 0; i < m.GetLength(0); i++)
        {
            for (int j = 0; j < m.GetLength(1); j++)
            {
                Console.Write(m[i, j].ToString().PadLeft(3));
            }
            Console.WriteLine();
        }
    }

    static void RunEdgeCaseTests()
    {
        int[] empty = new int[0];

        int[] single = new int[] { 0 };

        int[] same = new int[] { 2, 2, 2, 2, 2 };

        Console.WriteLine();
        Console.WriteLine("Завдання 6: крайовi випадки");
        Console.WriteLine();
        Console.WriteLine("| Вхiд | Завдання | Очiкувано | Отримано |");
        Console.WriteLine("|---|---|---|---|");

        // ---- порожній масив ----
        Stats sEmpty = ComputeStats(empty);
        Console.WriteLine("| порожнiй масив | 1 | результат не визначений | " +
            (sEmpty.IsDefined ? "визначений (ПОМИЛКА)" : "\"результат не визначений\"") + " |");

        int[] filteredEmpty = FilterByCriterion(empty);
        Console.WriteLine("| порожнiй масив | 2 | новий масив розмiру 0 | масив довжиною " + filteredEmpty.Length + " |");

        RunResult rEmpty = LongestRun(empty);
        Console.WriteLine("| порожнiй масив | 3 | результат не визначений | " +
            (rEmpty.IsDefined ? "визначений (ПОМИЛКА)" : "\"результат не визначений\"") + " |");

        int[] moveEmpty = CopyArray(empty);
        MoveNonPositiveToFront(moveEmpty);
        Console.WriteLine("| порожнiй масив | 4 | масив без змiн (порожнiй) | довжина " + moveEmpty.Length + ", без виняткiв |");

        // ---- один елемент ----
        Stats sSingle = ComputeStats(single);
        Console.WriteLine("| один елемент {0} | 1 | сума=0, середнє=0, min=max=0 (індекс 0), нулiв=1 | сума=" +
            sSingle.Sum + ", середнє=" + sSingle.Avg.ToString("F2") + ", min=" + sSingle.Min + " (idx " + sSingle.MinIndex +
            "), max=" + sSingle.Max + " (idx " + sSingle.MaxIndex + "), нулiв=" + sSingle.ZerosCount + " |");

        int[] filteredSingle = FilterByCriterion(single);
        Console.WriteLine("| один елемент {0} | 2 | масив розмiру 0 (0 не бiльше 2) | масив довжиною " + filteredSingle.Length + " |");

        RunResult rSingle = LongestRun(single);
        Console.WriteLine("| один елемент {0} | 3 | значення=0, довжина=1, iндекс=0 | значення=" +
            rSingle.Value + ", довжина=" + rSingle.Length + ", iндекс=" + rSingle.StartIndex + " |");

        int[] moveSingle = CopyArray(single);
        MoveNonPositiveToFront(moveSingle);
        Console.WriteLine("| один елемент {0} | 4 | [0] (без змiн, вже на початку) | [" + moveSingle[0] + "] |");

        // ---- усі елементи однакові ----
        Stats sSame = ComputeStats(same);
        Console.WriteLine("| усi однакові (2,2,2,2,2) | 1 | сума=10, середнє=2.00, min=max=2 (idx 0), нулiв=0 | сума=" +
            sSame.Sum + ", середнє=" + sSame.Avg.ToString("F2") + ", min=" + sSame.Min + " (idx " + sSame.MinIndex +
            "), max=" + sSame.Max + " (idx " + sSame.MaxIndex + "), нулiв=" + sSame.ZerosCount + " |");

        RunResult rSame = LongestRun(same);
        Console.WriteLine("| усi однакові (2,2,2,2,2) | 3 | значення=2, довжина=5, iндекс=0 | значення=" +
            rSame.Value + ", довжина=" + rSame.Length + ", iндекс=" + rSame.StartIndex + " |");

        Console.WriteLine();
        Console.WriteLine("Примiтка: жодного винятку пiд час прогону не сталося.");
    }

    static void Main()
    {
        const int N = 18;
        const int K = 12;
        const int a = 0;
        const int b = 2;
        const int c = 3;

        Console.WriteLine("N = " + N + ", K = " + K + ", a = " + a + ", b = " + b + ", c = " + c);
        Console.WriteLine();

        // ---- основний масив ----
        int[] arr = GenerateArray(24, N, 1, 4);
        PrintArray(arr, "Масив");
        Console.WriteLine();

        // ---- завдання 1 ----
        Console.WriteLine("Завдання 1: характеристики масиву");
        Stats stats = ComputeStats(arr);
        PrintStats(stats);
        Console.WriteLine();

        // ---- завдання 2 ----
        Console.WriteLine("Завдання 2: вiдбiр за критерiєм (елементи > 2)");
        int[] filtered = FilterByCriterion(arr);
        PrintArray(filtered, "Вiдiбранi");
        Console.WriteLine();

        // ---- завдання 3 ----
        Console.WriteLine("Завдання 3: найдовша серiя однакових елементiв");
        RunResult run = LongestRun(arr);
        PrintRun(run);
        Console.WriteLine();

        // ---- завдання 4 ----
        Console.WriteLine("Завдання 4: перестановка на мiсцi (вiд'ємнi/нульовi -> початок)");
        PrintArray(arr, "До");
        MoveNonPositiveToFront(arr);
        PrintArray(arr, "Пiсля");
        Console.WriteLine();

        // ---- завдання 5 ----
        Console.WriteLine("Завдання 5: матриця 3 x 6");
        int[,] matrix = CreateMatrix(3, 6, N);
        PrintMatrix(matrix);

        int[] rowSums = RowSums(matrix);
        int[] colMaxes = ColumnMaxes(matrix);
        int bestRow = RowIndexWithMaxSum(rowSums);

        Console.Write("Суми рядкiв: ");
        for (int i = 0; i < rowSums.Length; i++)
        {
            Console.Write(rowSums[i] + " ");
        }
        Console.WriteLine();

        Console.Write("Максимуми стовпцiв: ");
        for (int j = 0; j < colMaxes.Length; j++)
        {
            Console.Write(colMaxes[j] + " ");
        }
        Console.WriteLine();

        Console.WriteLine("Рядок з найбiльшою сумою: " + bestRow + " (сума = " + rowSums[bestRow] + ")");

        // ---- завдання 6 ----
        RunEdgeCaseTests();
    }
}
