using System;
using System.Collections.Generic;
using System.Numerics;


class Program
{
    const int N = 18;
    const int K = 12;

    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        List<int> numbers = MakeNumbers();
        List<string> words = MakeWords();
        List<Student> students = MakeStudents();

        Console.WriteLine("Числа:   " + Format(numbers));
        Console.WriteLine("Рядки:   " + Format(words));
        Console.WriteLine("Студентів: " + students.Count);

        RunTask1(numbers, words, students);
        RunTask2(numbers, words, students);
        RunTask3(numbers);
        RunTask4(words, students);
        RunTask5();
        RunEdgeCases(students);
    }


    static List<int> MakeNumbers()
    {
        Random rnd = new Random(N);
        List<int> list = new List<int>(20);
        for (int i = 0; i < 20; i++)
            list.Add(rnd.Next(-K, K + 1));
        return list;
    }

    static List<string> MakeWords()
    {
        return new List<string>
        {
            "кіт", "мова", "алгоритм", "делегат", "веб-сторінка", "код", "узагальнення",
            "цикл", "будь-який", "функція", "стек", "по-новому", "масив", "я"
        };
    }

    static List<Student> MakeStudents()
    {
        string[,] raw =
        {
            { "Коваленко", "КН-3/1", "88" }, { "Мельник", "КН-3/2", "64" },
            { "Іщенко", "КН-3/1", "93" },    { "Бондар", "КН-3/1", "71" },
            { "Савченко", "КН-3/2", "55" },  { "Ткачук", "КН-3/2", "90" },
            { "Кравець", "КН-3/1", "76" },   { "Іванів", "КН-3/1", "48" },
            { "Мороз", "КН-3/2", "82" },     { "Шевчук", "КН-3/1", "67" },
            { "Бойко", "КН-3/2", "95" },     { "Кушнір", "КН-3/2", "73" }
        };

        List<Student> list = new List<Student>(raw.GetLength(0));
        for (int i = 0; i < raw.GetLength(0); i++)
        {
            Student s = new Student();
            s.Name = raw[i, 0];
            s.Group = raw[i, 1];
            s.Score = int.Parse(raw[i, 2]);
            list.Add(s);
        }
        return list;
    }


    static string Format<T>(List<T> list)
    {
        return "[" + string.Join(", ", list) + "]";
    }

    static string Show(int? value)
    {
        return value.HasValue ? value.Value.ToString() : "немає (null)";
    }

    static string ScoreBand(int score)
    {
        if (score < 60) return "0–59";
        if (score < 75) return "60–74";
        if (score < 90) return "75–89";
        return "90–100";
    }

    static void PrintGroups<TKey, T>(string title, Dictionary<TKey, List<T>> groups) where TKey : notnull
    {
        Console.WriteLine(title);
        foreach (KeyValuePair<TKey, List<T>> pair in groups)
            Console.WriteLine($"  {pair.Key}: {pair.Value.Count}");
    }


    static void RunTask1(List<int> numbers, List<string> words, List<Student> students)
    {
        Console.WriteLine("\nЗавдання 1: Filter<T>");

        List<int> bigNumbers = Algorithms.Filter(numbers, x => x > K);
        List<string> longWords = Algorithms.Filter(words, w => w.Length > 5);
        List<Student> goodStudents = Algorithms.Filter(students, s => s.Score >= 75);

        Console.WriteLine($"Числа, більші за {K}: {Format(bigNumbers)}, розмір {bigNumbers.Count}");
        Console.WriteLine($"Рядки довші за 5 символів: {Format(longWords)}, розмір {longWords.Count}");
        List<string> goodNames = Algorithms.Map(goodStudents, s => $"{s.Name} ({s.Score})");
        Console.WriteLine($"Студенти з балом не менше 75: {Format(goodNames)}, розмір {goodStudents.Count}");

        List<int> aboveHalf = Algorithms.Filter(numbers, x => x > K / 2);
        Console.WriteLine($"Додатково, числа більші за {K / 2}: {Format(aboveHalf)}, розмір {aboveHalf.Count}");
    }


    static void RunTask2(List<int> numbers, List<string> words, List<Student> students)
    {
        Console.WriteLine("\nЗавдання 2: Map<TIn, TOut>");

        List<int> signs = Algorithms.Map(numbers, x => Math.Sign(x));   
        List<char> letters = Algorithms.Map(words, w => w[0]);          
        List<int> scores = Algorithms.Map(students, s => s.Score);      

        Console.WriteLine("Числа -> знак:           " + Format(signs));
        Console.WriteLine("Рядки -> перша літера:   " + Format(letters));
        Console.WriteLine("Студенти -> бал:         " + Format(scores));

        List<int> passedScores = Algorithms.Map(
            Algorithms.Filter(students, s => s.Score >= 75),
            s => s.Score);
        Console.WriteLine("Filter -> Map (бали студентів з 75+): " + Format(passedScores));
    }


    static void PrintAggregates(List<int> list)
    {
        int sum = Algorithms.Aggregate(list, 0, (acc, x) => acc + x);

        BigInteger product = Algorithms.Aggregate(list, BigInteger.One, (acc, x) => acc * x);

        int? max = Algorithms.Aggregate<int, int?>(list, null, (acc, x) => acc == null || x > acc ? x : acc);

        int? min = Algorithms.Aggregate<int, int?>(list, null, (acc, x) => acc == null || x < acc ? x : acc);

        Console.WriteLine($"  сума = {sum}, добуток = {product}, максимум = {Show(max)}, мінімум = {Show(min)}");
    }

    static void RunTask3(List<int> numbers)
    {
        Console.WriteLine("\nЗавдання 3: Aggregate<T, TAcc>");
        PrintAggregates(numbers);
    }


    static void RunTask4(List<string> words, List<Student> students)
    {
        Console.WriteLine("\nЗавдання 4: GroupBy<T, TKey>");

        Dictionary<string, List<Student>> byGroup = Algorithms.GroupBy(students, s => s.Group);
        Dictionary<char, List<Student>> byLetter = Algorithms.GroupBy(students, s => s.Name[0]);
        Dictionary<string, List<Student>> byBand = Algorithms.GroupBy(students, s => ScoreBand(s.Score));
        Dictionary<int, List<string>> byLength = Algorithms.GroupBy(words, w => w.Length);

        PrintGroups("Студенти за групою:", byGroup);
        PrintGroups("Студенти за першою літерою прізвища:", byLetter);
        PrintGroups("Студенти за діапазоном балу:", byBand);
        PrintGroups("Рядки за довжиною:", byLength);
    }


    static string CaptureAfterLambda()
    {
        int threshold = K;
        Func<int, bool> isAbove = x => x > threshold;
        bool before = isAbove(500);
        threshold = 1000;               
        bool after = isAbove(500);
        return $"isAbove(500) до зміни порога ({K}): {before}; після threshold = 1000: {after}";
    }

    static string CaptureFor()
    {
        List<Func<int>> funcs = new List<Func<int>>();
        for (int i = 0; i < 3; i++)
            funcs.Add(() => i);       
        List<int> results = new List<int>();
        foreach (Func<int> f in funcs)
            results.Add(f());
        return Format(results);
    }

    static string CaptureForeach()
    {
        List<Func<int>> funcs = new List<Func<int>>();
        int[] array = { 0, 1, 2 };
        foreach (int x in array)
            funcs.Add(() => x);         
        List<int> results = new List<int>();
        foreach (Func<int> f in funcs)
            results.Add(f());
        return Format(results);
    }

    static string CaptureForFixed()
    {
        List<Func<int>> funcs = new List<Func<int>>();
        for (int i = 0; i < 3; i++)
        {
            int copy = i;               
            funcs.Add(() => copy);
        }
        List<int> results = new List<int>();
        foreach (Func<int> f in funcs)
            results.Add(f());
        return Format(results);
    }

    static void RunTask5()
    {
        Console.WriteLine("\nЗавдання 5: захоплення змінних");
        Console.WriteLine("1. Змінна після лямбди: " + CaptureAfterLambda());
        Console.WriteLine("2. Лічильник for:       " + CaptureFor());
        Console.WriteLine("3. foreach:             " + CaptureForeach());
        Console.WriteLine("Виправлений пункт 2:    " + CaptureForFixed());
    }


    static void RunEdgeCases(List<Student> students)
    {
        Console.WriteLine("\nКрайові випадки");

        List<int> empty = new List<int>();
        List<int> filtered = Algorithms.Filter(empty, x => x > 0);
        List<string> mapped = Algorithms.Map(empty, x => x.ToString());
        Dictionary<int, List<int>> grouped = Algorithms.GroupBy(empty, x => x % 2);
        Console.WriteLine($"Порожній список, завдання 1, 2, 4: Filter {Format(filtered)}, Map {Format(mapped)}, груп {grouped.Count}");

        Console.Write("Порожній список, завдання 3:");
        PrintAggregates(empty);

        List<string> words = MakeWords();
        List<string> none = Algorithms.Filter(words, w => w.Length > 100);
        Console.WriteLine($"Жоден елемент не підходить, завдання 1: {Format(none)}, розмір {none.Count}");

        List<Student> oneGroup = Algorithms.Filter(students, s => s.Group == "КН-3/1");
        Dictionary<string, List<Student>> byGroup = Algorithms.GroupBy(oneGroup, s => s.Group);
        int inGroup = byGroup.TryGetValue("КН-3/1", out List<Student>? onlyGroup) ? onlyGroup.Count : 0;
        Console.WriteLine($"Усі елементи в одній групі, завдання 4: груп {byGroup.Count}, студентів у групі {inGroup}");

        Console.Write("Список з одного елемента [7], завдання 3:");
        PrintAggregates(new List<int> { 7 });
    }
}