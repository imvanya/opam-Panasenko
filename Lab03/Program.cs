using System;
using System.Collections.Generic;
using System.IO;


class Program
{
    const int N = 18;
    const int K = 12;

    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine("Завдання 1: найбільший файл");
        Console.Write("Введіть шлях до папки: ");
        string? root = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(root) || !Directory.Exists(root))
        {
            Console.WriteLine("Папку не знайдено.");
        }
        else
        {
            FileResult best = FindLargest(root);
            if (best.Size < 0)
                Console.WriteLine("Файлів не знайдено.");
            else
                Console.WriteLine($"Ім'я: {best.Name}\nРозмір: {best.Size} байт\nШлях: {best.Path}");
        }

        Console.WriteLine("\nЗавдання 2: максимум через дві половини");
        int[] arr = MakeArray(16);
        Console.WriteLine("Масив: " + string.Join(", ", arr));
        int max = Solve(arr, 0, arr.Length - 1);
        Console.WriteLine($"Максимум (рекурсія): {max}");

        (int _, int deepest) = SolveWithDepth(arr, 0, arr.Length - 1, 0);
        Console.WriteLine($"Максимальна глибина рекурсії: {deepest} (log2(16) = {Math.Log2(16)})");

        Console.WriteLine("\nЗавдання 3: шляхи в матриці");
        int m = K % 3 + 3; // 3
        int total = Paths(0, 0, m, new List<string>());
        Console.WriteLine($"Усього шляхів: {total}");

        Console.WriteLine("\nЗавдання 4: трасування (4 елементи)");
        int[] small = MakeArray(4);
        Console.WriteLine("Масив: " + string.Join(", ", small));
        Trace(small, 0, small.Length - 1, 0);

        Console.WriteLine("\nЗавдання 5: ітеративна версія");
        int maxIter = MaxIterative(arr);
        Console.WriteLine($"Максимум (цикл): {maxIter}");
        Console.WriteLine(max == maxIter ? "Результати збігаються." : "РЕЗУЛЬТАТИ РІЗНІ!");
    }


    struct FileResult
    {
        public string Name;
        public long Size;
        public string Path;
    }

    static FileResult FindLargest(string dir)
    {
        FileResult best = new FileResult { Name = "", Size = -1, Path = "" };

        string[] files;
        try
        {
            files = Directory.GetFiles(dir); 
        }
        catch (UnauthorizedAccessException)
        {
            return best; 
        }
        catch (IOException)
        {
            return best;
        }

        foreach (string f in files)
        {
            try
            {
                long size = new FileInfo(f).Length;
                if (size > best.Size)
                {
                    best.Name = System.IO.Path.GetFileName(f);
                    best.Size = size;
                    best.Path = System.IO.Path.GetFullPath(f);
                }
            }
            catch (UnauthorizedAccessException) { }
            catch (IOException) { }
        }

        string[] subs;
        try
        {
            subs = Directory.GetDirectories(dir);
        }
        catch (UnauthorizedAccessException)
        {
            return best;
        }
        catch (IOException)
        {
            return best;
        }

        foreach (string sub in subs)
        {
            FileResult r = FindLargest(sub);
            if (r.Size > best.Size)
                best = r;
        }
        return best;
    }


    static int[] MakeArray(int n)
    {
        Random rnd = new Random(N);
        int[] a = new int[n];
        for (int i = 0; i < n; i++)
            a[i] = rnd.Next(-20, 21); 
        return a;
    }

    static int Solve(int[] a, int lo, int hi)
    {
        if (lo == hi)
            return a[lo];

        int mid = (lo + hi) / 2;
        int left = Solve(a, lo, mid);
        int right = Solve(a, mid + 1, hi);
        return left > right ? left : right;
    }

    static (int max, int depth) SolveWithDepth(int[] a, int lo, int hi, int depth)
    {
        if (lo == hi)
            return (a[lo], depth);

        int mid = (lo + hi) / 2;
        (int lm, int ld) = SolveWithDepth(a, lo, mid, depth + 1);
        (int rm, int rd) = SolveWithDepth(a, mid + 1, hi, depth + 1);
        return (lm > rm ? lm : rm, ld > rd ? ld : rd);
    }


    static int Paths(int r, int c, int m, List<string> path)
    {
        path.Add($"({r},{c})");
        int count = 0;

        if (r == m - 1 && c == m - 1)
        {
            Console.WriteLine(string.Join(" -> ", path)); 
            count = 1;
        }
        else
        {
            if (c + 1 < m) count += Paths(r, c + 1, m, path); 
            if (r + 1 < m) count += Paths(r + 1, c, m, path); 
        }

        path.RemoveAt(path.Count - 1); 
        return count;
    }


    static int Trace(int[] a, int lo, int hi, int depth)
    {
        string indent = new string(' ', depth * 2);

        if (lo == hi)
        {
            Console.WriteLine($"{indent}enter Max({lo}..{hi}) -> {a[lo]}");
            return a[lo];
        }

        Console.WriteLine($"{indent}enter Max({lo}..{hi})");
        int mid = (lo + hi) / 2;
        int left = Trace(a, lo, mid, depth + 1);
        int right = Trace(a, mid + 1, hi, depth + 1);
        int res = left > right ? left : right;
        Console.WriteLine($"{indent}exit  Max({lo}..{hi}) -> {res}");
        return res;
    }


    static int MaxIterative(int[] a)
    {
        int max = a[0];
        for (int i = 1; i < a.Length; i++)
            if (a[i] > max)
                max = a[i];
        return max;
    }
}