using System;
using System.Collections.Generic;
using System.Diagnostics;


class Program
{
    const int N = 18;
    const int K = 12;
    const int Limit = K % 7;               // 5
    const int LenFirst = 10 + K % 5;       // 12
    const int LenSecond = 6 + K % 4;       // 6

    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

#if DEBUG
        Console.WriteLine("УВАГА: це Debug-збірка, для завдання 5 запускайте: dotnet run -c Release\n");
#endif

        List<int> numbers = MakeMainList();
        Console.WriteLine("Основний список: " + Format(numbers));

        RunTask1(numbers);
        RunTask2(numbers);
        RunTask3();
        RunTask4(numbers);
        RunEdgeCases();
        RunTask5();

        Console.WriteLine("\nОсновний список після усіх завдань: " + Format(numbers));
    }


    static List<int> MakeMainList()
    {
        Random rnd = new Random(N);
        List<int> list = new List<int>(20);
        for (int i = 0; i < 20; i++)
            list.Add(rnd.Next(-9, 10)); 
        return list;
    }

    static string Format(List<int> list)
    {
        return "[" + string.Join(", ", list) + "]";
    }

    static int CountEqual(List<int> list, int value)
    {
        int count = 0;
        for (int i = 0; i < list.Count; i++)
            if (list[i] == value)
                count++;
        return count;
    }

    static bool SameList(List<int> x, List<int> y)
    {
        if (x.Count != y.Count)
            return false;
        for (int i = 0; i < x.Count; i++)
            if (x[i] != y[i])
                return false;
        return true;
    }


    static List<int> FilterGreaterThan(List<int> src, int limit)
    {
        List<int> result = new List<int>();
        for (int i = 0; i < src.Count; i++)
            if (src[i] > limit)
                result.Add(src[i]);
        return result;
    }

    static void RunTask1(List<int> numbers)
    {
        Console.WriteLine($"\nЗавдання 1: елементи, більші за {Limit}");
        List<int> result = FilterGreaterThan(numbers, Limit);
        Console.WriteLine("Результат: " + Format(result));
        Console.WriteLine("Count: " + result.Count);
    }


    static List<int> RemoveForwardWrong(List<int> src)
    {
        List<int> list = new List<int>(src);
        if (list.Count == 0)
            return list;

        int target = list[0]; 
        for (int i = 0; i < list.Count; i++)
            if (list[i] == target)
                list.RemoveAt(i);
        return list;
    }

    static void RemoveEqualToFirstInPlace(List<int> list)
    {
        if (list.Count == 0)
            return;

        int target = list[0]; 
        for (int i = list.Count - 1; i >= 0; i--)
            if (list[i] == target)
                list.RemoveAt(i);
    }

    static List<int> BuildWithoutFirstValue(List<int> src)
    {
        List<int> result = new List<int>();
        if (src.Count == 0)
            return result;

        int target = src[0];
        for (int i = 0; i < src.Count; i++)
            if (src[i] != target)
                result.Add(src[i]);
        return result;
    }

    static List<int> RemoveWithRemoveAll(List<int> src)
    {
        List<int> copy = new List<int>(src);
        if (copy.Count == 0)
            return copy;

        int target = copy[0];
        copy.RemoveAll(x => x == target);
        return copy;
    }

    static void PrintTask2(List<int> src, string title)
    {
        Console.WriteLine($"{title}: {Format(src)}");

        List<int> wrong = RemoveForwardWrong(src);

        List<int> backward = new List<int>(src); 
        RemoveEqualToFirstInPlace(backward);

        List<int> built = BuildWithoutFirstValue(src);
        List<int> viaRemoveAll = RemoveWithRemoveAll(src);

        Console.WriteLine("Хибний (з початку):  " + Format(wrong));
        if (src.Count > 0)
            Console.WriteLine($"  пропущено елементів, що дорівнюють {src[0]}: {CountEqual(wrong, src[0])}");
        Console.WriteLine("Цикл з кінця:        " + Format(backward));
        Console.WriteLine("Новий список:        " + Format(built));
        Console.WriteLine("Варіанти 2 і 3 збігаються: " + SameList(backward, built));
        Console.WriteLine("RemoveAll (додатково) збігається: " + SameList(built, viaRemoveAll));
        Console.WriteLine("Оригінал не змінено: " + Format(src));
    }

    static void RunTask2(List<int> numbers)
    {
        Console.WriteLine("\nЗавдання 2: вилучення елементів, що дорівнюють першому");
        PrintTask2(numbers, "основний список");
        PrintTask2(new List<int> { 3, 3, 3, 1, 3 }, "демонстраційний список");
    }

    static List<int> BuildSortedList(int length, int seed)
    {
        Random rnd = new Random(seed);
        List<int> list = new List<int>(length);
        int current = 0;
        for (int i = 0; i < length; i++)
        {
            current += rnd.Next(1, 10);
            list.Add(current);
        }
        return list;
    }

    static List<int> MergeSorted(List<int> first, List<int> second)
    {
        List<int> result = new List<int>(first.Count + second.Count);
        int i = 0;
        int j = 0;

        while (i < first.Count && j < second.Count)
        {
            if (first[i] <= second[j])
            {
                result.Add(first[i]);
                i++;
            }
            else
            {
                result.Add(second[j]);
                j++;
            }
        }

        while (i < first.Count)
        {
            result.Add(first[i]);
            i++;
        }
        while (j < second.Count)
        {
            result.Add(second[j]);
            j++;
        }
        return result;
    }

    static bool IsSortedAscending(List<int> list)
    {
        for (int i = 1; i < list.Count; i++)
            if (list[i] < list[i - 1])
                return false;
        return true;
    }

    static void RunTask3()
    {
        Console.WriteLine("\nЗавдання 3: злиття впорядкованих списків");
        List<int> first = BuildSortedList(LenFirst, N);
        List<int> second = BuildSortedList(LenSecond, N + 100);
        Console.WriteLine($"Перший ({first.Count}):  " + Format(first));
        Console.WriteLine($"Другий ({second.Count}):  " + Format(second));

        List<int> merged = MergeSorted(first, second);
        Console.WriteLine($"Злитий ({merged.Count}): " + Format(merged));
        Console.WriteLine("Упорядковано: " + IsSortedAscending(merged));
        Console.WriteLine("Довжина збігається з сумою довжин: " + (merged.Count == first.Count + second.Count));
    }

    static (List<int> less, List<int> rest) SplitByThreshold(List<int> src, int limit)
    {
        List<int> less = new List<int>();
        List<int> rest = new List<int>();
        for (int i = 0; i < src.Count; i++)
        {
            if (src[i] < limit)
                less.Add(src[i]);
            else
                rest.Add(src[i]);
        }
        return (less, rest);
    }

    static void RunTask4(List<int> numbers)
    {
        Console.WriteLine($"\nЗавдання 4: менші за {Limit} і решта");
        (List<int> less, List<int> rest) = SplitByThreshold(numbers, Limit);
        Console.WriteLine("Менші:  " + Format(less));
        Console.WriteLine("Решта:  " + Format(rest));
        Console.WriteLine("Сума Count збігається з вихідним: " + (less.Count + rest.Count == numbers.Count));
    }


    static void EdgeCase(string title, List<int> list)
    {
        Console.WriteLine($"{title}: {Format(list)}");
        Console.WriteLine($"  Завд.1 (> {Limit}):        " + Format(FilterGreaterThan(list, Limit)));
        Console.WriteLine("  Завд.2 (хибний):       " + Format(RemoveForwardWrong(list)));

        List<int> backward = new List<int>(list);
        RemoveEqualToFirstInPlace(backward);
        Console.WriteLine("  Завд.2 (з кінця):      " + Format(backward));
        Console.WriteLine("  Завд.2 (новий список): " + Format(BuildWithoutFirstValue(list)));

        (List<int> less, List<int> rest) = SplitByThreshold(list, Limit);
        Console.WriteLine($"  Завд.4: менші {Format(less)}, решта {Format(rest)}, суми збігаються: {less.Count + rest.Count == list.Count}");
    }

    static void RunEdgeCases()
    {
        Console.WriteLine("\nКрайові випадки (завдання 1-4)");
        EdgeCase("порожній список", new List<int>());
        EdgeCase("один елемент", new List<int> { 7 });
        EdgeCase("жоден не підходить для завдання 1", new List<int> { 1, 2, 3 });
        EdgeCase("усі підходять для завдання 1", new List<int> { 6, 7, 8, 9 });
        EdgeCase("усі дорівнюють першому (завдання 2)", new List<int> { 4, 4, 4 });

        Console.WriteLine("злиття з порожнім списком");
        List<int> merged = MergeSorted(new List<int>(), new List<int> { 2, 5, 9 });
        Console.WriteLine("  [] + [2, 5, 9] = " + Format(merged) + ", упорядковано: " + IsSortedAscending(merged));
        merged = MergeSorted(new List<int> { 2, 5, 9 }, new List<int>());
        Console.WriteLine("  [2, 5, 9] + [] = " + Format(merged) + ", упорядковано: " + IsSortedAscending(merged));
    }


    static double MeasureAdd(int count)
    {
        Stopwatch sw = Stopwatch.StartNew();
        List<int> list = new List<int>();
        for (int i = 0; i < count; i++)
            list.Add(i);
        sw.Stop();
        GC.KeepAlive(list);
        return sw.Elapsed.TotalMilliseconds;
    }

    static double MeasureAddWithCapacity(int count)
    {
        Stopwatch sw = Stopwatch.StartNew();
        List<int> list = new List<int>(count);
        for (int i = 0; i < count; i++)
            list.Add(i);
        sw.Stop();
        GC.KeepAlive(list);
        return sw.Elapsed.TotalMilliseconds;
    }

    static double MeasureInsertFront(int count)
    {
        Stopwatch sw = Stopwatch.StartNew();
        List<int> list = new List<int>();
        for (int i = 0; i < count; i++)
            list.Insert(0, i);
        sw.Stop();
        GC.KeepAlive(list);
        return sw.Elapsed.TotalMilliseconds;
    }

    static double MeasureContains(int size, int repeats)
    {
        List<int> list = new List<int>(size);
        for (int i = 0; i < size; i++)
            list.Add(i);

        int found = 0;
        Stopwatch sw = Stopwatch.StartNew();
        for (int r = 0; r < repeats; r++)
            if (list.Contains(-1))
                found++;
        sw.Stop();

        if (found != 0)
            Console.WriteLine("Неочікувано: значення знайдено");
        return sw.Elapsed.TotalMilliseconds;
    }

    static void RunTask5()
    {
        const int count = 100000;
        const int repeats = 1000;

        Console.WriteLine("\nЗавдання 5: замір часу");

        MeasureAdd(1000);
        MeasureAddWithCapacity(1000);
        MeasureInsertFront(1000);
        MeasureContains(1000, 1000);

        double tAdd = MeasureAdd(count);
        double tAddCap = MeasureAddWithCapacity(count);
        double tInsert = MeasureInsertFront(count);
        double tContains = MeasureContains(count, repeats);

        Console.WriteLine($"{count} x Add(x), без місткості:        {tAdd:F3} мс");
        Console.WriteLine($"{count} x Add(x), з місткістю {count}:  {tAddCap:F3} мс");
        Console.WriteLine($"{count} x Insert(0, x):                 {tInsert:F3} мс");
        Console.WriteLine($"{repeats} x Contains у списку з {count}: {tContains:F3} мс");
        Console.WriteLine($"Insert(0, x) / Add(x) = {tInsert / tAdd:F1} разів");
        Console.WriteLine($"Add без місткості / Add з місткістю = {tAdd / tAddCap:F2} разів");
    }
}