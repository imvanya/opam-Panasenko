using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;


class Student
{
    public string Name = "";
    public string Group = "";
    public int Score;
}

class Program
{
    const int N = 18;
    const int K = 12;
    const int MinFrequency = K % 3 + 2;   
    const int DuplicateMax = K % 10 + 6;  
    const int SetMax = 20 + K;            

    const string TestText =
        "Алгоритм описує послідовність дій. Алгоритм можна записати словами, " +
        "а можна записати кодом. Кодом алгоритм стає тоді, коли його " +
        "виконує машина, а не людина. Одна задача — багато алгоритмів.";

    static readonly string[] Bands = { "0–59", "60–74", "75–89", "90–100" };

    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

#if DEBUG
        Console.WriteLine("УВАГА: це Debug-збірка, для завдання 5 запускайте: dotnet run -c Release\n");
#endif

        RunTask1();
        RunTask2();
        RunTask3();
        RunTask4();
        RunEdgeCases();
        RunTask5();
    }

    static List<string> SplitWords(string text)
    {
        List<string> words = new List<string>();
        StringBuilder current = new StringBuilder();
        for (int i = 0; i < text.Length; i++)
        {
            char c = text[i];
            if (char.IsLetter(c))
            {
                current.Append(char.ToLowerInvariant(c));
            }
            else if (current.Length > 0)
            {
                words.Add(current.ToString());
                current.Clear();
            }
        }
        if (current.Length > 0)
            words.Add(current.ToString());
        return words;
    }

    static Dictionary<string, int> CountWordsDictionary(List<string> words)
    {
        Dictionary<string, int> freq = new Dictionary<string, int>();
        for (int i = 0; i < words.Count; i++)
        {
            string w = words[i];
            if (freq.TryGetValue(w, out int count))
                freq[w] = count + 1;
            else
                freq[w] = 1;
        }
        return freq;
    }

    static (List<string> keys, List<int> counts) CountWordsLists(List<string> words)
    {
        List<string> keys = new List<string>();
        List<int> counts = new List<int>();
        for (int i = 0; i < words.Count; i++)
        {
            int index = keys.IndexOf(words[i]);
            if (index < 0)
            {
                keys.Add(words[i]);
                counts.Add(1);
            }
            else
            {
                counts[index]++;
            }
        }
        return (keys, counts);
    }

    static bool SameFrequencies(Dictionary<string, int> dict, List<string> keys, List<int> counts)
    {
        if (dict.Count != keys.Count)
            return false;
        for (int i = 0; i < keys.Count; i++)
        {
            if (!dict.TryGetValue(keys[i], out int c) || c != counts[i])
                return false;
        }
        return true;
    }

    static void PrintFrequent(Dictionary<string, int> freq)
    {
        Console.WriteLine($"Частота не менша за {MinFrequency}:");
        foreach (KeyValuePair<string, int> pair in freq)
            if (pair.Value >= MinFrequency)
                Console.WriteLine($"  {pair.Key}: {pair.Value}");
        Console.WriteLine("Різних слів: " + freq.Count);
    }

    static void RunTask1()
    {
        Console.WriteLine("Завдання 1: частоти слів");
        List<string> words = SplitWords(TestText);
        Console.WriteLine("Слів у тексті: " + words.Count);

        Dictionary<string, int> freq = CountWordsDictionary(words);
        PrintFrequent(freq);

        (List<string> keys, List<int> counts) = CountWordsLists(words);
        Console.WriteLine("Списковий спосіб дає той самий результат: " + SameFrequencies(freq, keys, counts));
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

    static string ScoreBand(int score)
    {
        if (score < 60) return Bands[0];
        if (score < 75) return Bands[1];
        if (score < 90) return Bands[2];
        return Bands[3];
    }

    static Dictionary<string, List<Student>> GroupByBand(List<Student> students)
    {
        Dictionary<string, List<Student>> groups = new Dictionary<string, List<Student>>();
        for (int i = 0; i < students.Count; i++)
        {
            string key = ScoreBand(students[i].Score);
            if (!groups.TryGetValue(key, out List<Student>? list))
            {
                list = new List<Student>();
                groups[key] = list;
            }
            list.Add(students[i]);
        }
        return groups;
    }

    static double AverageScore(List<Student> students)
    {
        if (students.Count == 0)
            return 0;
        int sum = 0;
        for (int i = 0; i < students.Count; i++)
            sum += students[i].Score;
        return (double)sum / students.Count;
    }

    static (int[] counts, double[] averages) GroupByBandLists(List<Student> students)
    {
        int[] counts = new int[Bands.Length];
        double[] averages = new double[Bands.Length];
        for (int b = 0; b < Bands.Length; b++)
        {
            int sum = 0;
            for (int i = 0; i < students.Count; i++)
            {
                if (ScoreBand(students[i].Score) == Bands[b])
                {
                    counts[b]++;
                    sum += students[i].Score;
                }
            }
            averages[b] = counts[b] == 0 ? 0 : (double)sum / counts[b];
        }
        return (counts, averages);
    }

    static void PrintGroups(Dictionary<string, List<Student>> groups)
    {
        for (int b = 0; b < Bands.Length; b++)
        {
            if (groups.TryGetValue(Bands[b], out List<Student>? list))
                Console.WriteLine($"Діапазон {Bands[b]}: студентів {list.Count}, середній бал {AverageScore(list):F2}");
            else
                Console.WriteLine($"Діапазон {Bands[b]}: студентів 0, середній бал немає");
        }
    }

    static bool SameGroups(Dictionary<string, List<Student>> groups, int[] counts, double[] averages)
    {
        for (int b = 0; b < Bands.Length; b++)
        {
            int c = 0;
            double avg = 0;
            if (groups.TryGetValue(Bands[b], out List<Student>? list))
            {
                c = list.Count;
                avg = AverageScore(list);
            }
            if (c != counts[b] || Math.Abs(avg - averages[b]) > 1e-9)
                return false;
        }
        return true;
    }

    static void RunTask2()
    {
        Console.WriteLine("\nЗавдання 2: групування за діапазоном балу");
        List<Student> students = MakeStudents();
        Dictionary<string, List<Student>> groups = GroupByBand(students);
        PrintGroups(groups);

        (int[] counts, double[] averages) = GroupByBandLists(students);
        Console.WriteLine("Списковий спосіб дає той самий результат: " + SameGroups(groups, counts, averages));
    }


    static List<int> MakeNumbers(int count, int max, int seed)
    {
        Random rnd = new Random(seed);
        List<int> list = new List<int>(count);
        for (int i = 0; i < count; i++)
            list.Add(rnd.Next(1, max + 1)); // [1; max]
        return list;
    }

    static List<int> RemoveDuplicatesUnordered(List<int> src)
    {
        HashSet<int> set = new HashSet<int>(src);
        return new List<int>(set);
    }

    static List<int> RemoveDuplicatesOrdered(List<int> src)
    {
        HashSet<int> seen = new HashSet<int>();
        List<int> result = new List<int>();
        for (int i = 0; i < src.Count; i++)
            if (seen.Add(src[i]))
                result.Add(src[i]);
        return result;
    }

    static List<int> RemoveDuplicatesList(List<int> src)
    {
        List<int> result = new List<int>();
        for (int i = 0; i < src.Count; i++)
            if (!result.Contains(src[i]))
                result.Add(src[i]);
        return result;
    }

    static bool SameSequence(List<int> x, List<int> y)
    {
        if (x.Count != y.Count)
            return false;
        for (int i = 0; i < x.Count; i++)
            if (x[i] != y[i])
                return false;
        return true;
    }

    static string Format(List<int> list)
    {
        return "[" + string.Join(", ", list) + "]";
    }

    static void RunTask3()
    {
        Console.WriteLine($"\nЗавдання 3: дублікати, числа в [1; {DuplicateMax}]");
        List<int> source = MakeNumbers(25, DuplicateMax, N);
        Console.WriteLine($"Вихідний список ({source.Count}):   " + Format(source));

        List<int> unordered = RemoveDuplicatesUnordered(source);
        List<int> ordered = RemoveDuplicatesOrdered(source);
        List<int> viaList = RemoveDuplicatesList(source);

        Console.WriteLine($"HashSet, порядок не гарантований ({unordered.Count}): " + Format(unordered));
        Console.WriteLine($"Зі збереженням порядку ({ordered.Count}):            " + Format(ordered));
        Console.WriteLine("Порядок у першому способі збігся з порядком появи: " + SameSequence(unordered, ordered));
        Console.WriteLine("Списковий спосіб збігається зі способом зі збереженням порядку: " + SameSequence(ordered, viaList));
    }


    static (bool isSubset, List<int> extras) SubsetWithSets(List<int> a, List<int> b)
    {
        HashSet<int> setA = new HashSet<int>(a);
        HashSet<int> setB = new HashSet<int>(b);
        HashSet<int> extra = new HashSet<int>(setA); // копія
        extra.ExceptWith(setB);
        return (setA.IsSubsetOf(setB), new List<int>(extra));
    }

    static (bool isSubset, List<int> extras) SubsetManual(List<int> a, List<int> b)
    {
        List<int> extras = new List<int>();
        for (int i = 0; i < a.Count; i++)
        {
            bool found = false;
            for (int j = 0; j < b.Count; j++)
            {
                if (a[i] == b[j])
                {
                    found = true;
                    break;
                }
            }
            if (!found && !extras.Contains(a[i]))
                extras.Add(a[i]);
        }
        return (extras.Count == 0, extras);
    }

    static bool SameElements(List<int> x, List<int> y)
    {
        if (x.Count != y.Count)
            return false;
        for (int i = 0; i < x.Count; i++)
            if (!y.Contains(x[i]))
                return false;
        return true;
    }

    static string FormatAscending(List<int> values, int max)
    {
        List<int> ordered = new List<int>();
        for (int v = 1; v <= max; v++)
            if (values.Contains(v))
                ordered.Add(v);
        return Format(ordered);
    }

    static void PrintSubsetCase(string title, List<int> a, List<int> b)
    {
        Console.WriteLine(title);
        (bool subsetSet, List<int> extrasSet) = SubsetWithSets(a, b);
        (bool subsetManual, List<int> extrasManual) = SubsetManual(a, b);
        Console.WriteLine($"  перший набір підмножина другого: {subsetSet}");
        Console.WriteLine($"  зайвих елементів: {extrasSet.Count}, це {FormatAscending(extrasSet, SetMax)}");
        Console.WriteLine($"  вручну збігається з HashSet: {subsetSet == subsetManual && SameElements(extrasSet, extrasManual)}");
    }

    static void RunTask4()
    {
        Console.WriteLine($"\nЗавдання 4: підмножина, числа в [1; {SetMax}]");
        List<int> a = MakeNumbers(30, SetMax, N);
        List<int> b = MakeNumbers(30, SetMax, N + 100);
        Console.WriteLine("Перший набір: " + Format(a));
        Console.WriteLine("Другий набір: " + Format(b));
        PrintSubsetCase("Результат:", a, b);
        Console.WriteLine("Вихідні набори не змінено: " + (a.Count == 30 && b.Count == 30));
    }


    static void RunEdgeCases()
    {
        Console.WriteLine("\nКрайові випадки");

        Dictionary<string, int> empty = CountWordsDictionary(SplitWords(""));
        Console.WriteLine("Порожній текст, завдання 1: різних слів " + empty.Count);

        Dictionary<string, int> allDifferent = CountWordsDictionary(SplitWords("один два три чотири"));
        int frequent = 0;
        foreach (KeyValuePair<string, int> pair in allDifferent)
            if (pair.Value >= MinFrequency)
                frequent++;
        Console.WriteLine($"Усі слова різні, завдання 1: різних слів {allDifferent.Count}, з частотою не менше {MinFrequency}: {frequent}");

        List<Student> oneBand = new List<Student>();
        string[] names = { "Перший", "Другий", "Третій" };
        int[] scores = { 80, 85, 76 };
        for (int i = 0; i < names.Length; i++)
        {
            Student s = new Student();
            s.Name = names[i];
            s.Group = "КН-3/1";
            s.Score = scores[i];
            oneBand.Add(s);
        }
        Console.WriteLine("Усі студенти в одному діапазоні (80, 85, 76), завдання 2:");
        PrintGroups(GroupByBand(oneBand));

        List<int> noDuplicates = new List<int> { 1, 2, 3 };
        Console.WriteLine("Без дублікатів [1, 2, 3], завдання 3: "
            + Format(RemoveDuplicatesUnordered(noDuplicates)) + " і "
            + Format(RemoveDuplicatesOrdered(noDuplicates)));

        PrintSubsetCase("Набори не перетинаються, [1, 2, 3] і [4, 5, 6], завдання 4:",
            new List<int> { 1, 2, 3 }, new List<int> { 4, 5, 6 });
        PrintSubsetCase("Набори однакові, [5, 6, 7] і [5, 6, 7], завдання 4:",
            new List<int> { 5, 6, 7 }, new List<int> { 5, 6, 7 });
    }


    static int[] MakeQueries(int count, int max)
    {
        Random rnd = new Random(N);
        int[] q = new int[count];
        for (int i = 0; i < count; i++)
            q[i] = rnd.Next(0, max);
        return q;
    }

    static string[] MakeWords(int count, int vocabulary)
    {
        Random rnd = new Random(N);
        string[] words = new string[count];
        for (int i = 0; i < count; i++)
            words[i] = "w" + rnd.Next(vocabulary);
        return words;
    }

    static int[] MakeInts(int count, int max)
    {
        Random rnd = new Random(N);
        int[] a = new int[count];
        for (int i = 0; i < count; i++)
            a[i] = rnd.Next(0, max);
        return a;
    }

    static List<int> MakeDistinct(int count, int max, int seed)
    {
        Random rnd = new Random(seed);
        HashSet<int> seen = new HashSet<int>();
        List<int> list = new List<int>(count);
        while (list.Count < count)
        {
            int x = rnd.Next(0, max);
            if (seen.Add(x))
                list.Add(x);
        }
        return list;
    }

    static (double ms, int check) MeasureMembershipList(int setSize, int checks)
    {
        List<int> list = new List<int>(setSize);
        for (int i = 0; i < setSize; i++)
            list.Add(i);
        int[] queries = MakeQueries(checks, 2 * setSize);

        int found = 0;
        Stopwatch sw = Stopwatch.StartNew();
        for (int i = 0; i < queries.Length; i++)
            if (list.Contains(queries[i]))
                found++;
        sw.Stop();
        return (sw.Elapsed.TotalMilliseconds, found);
    }

    static (double ms, int check) MeasureMembershipSet(int setSize, int checks)
    {
        HashSet<int> set = new HashSet<int>();
        for (int i = 0; i < setSize; i++)
            set.Add(i);
        int[] queries = MakeQueries(checks, 2 * setSize);

        int found = 0;
        Stopwatch sw = Stopwatch.StartNew();
        for (int i = 0; i < queries.Length; i++)
            if (set.Contains(queries[i]))
                found++;
        sw.Stop();
        return (sw.Elapsed.TotalMilliseconds, found);
    }

    static (double ms, int check) MeasureFrequencyDictionary(string[] words)
    {
        Stopwatch sw = Stopwatch.StartNew();
        Dictionary<string, int> freq = new Dictionary<string, int>();
        for (int i = 0; i < words.Length; i++)
        {
            string w = words[i];
            if (freq.TryGetValue(w, out int c))
                freq[w] = c + 1;
            else
                freq[w] = 1;
        }
        sw.Stop();
        return (sw.Elapsed.TotalMilliseconds, freq.Count);
    }

    static (double ms, int check) MeasureFrequencyLists(string[] words)
    {
        Stopwatch sw = Stopwatch.StartNew();
        List<string> keys = new List<string>();
        List<int> counts = new List<int>();
        for (int i = 0; i < words.Length; i++)
        {
            int index = keys.IndexOf(words[i]);
            if (index < 0)
            {
                keys.Add(words[i]);
                counts.Add(1);
            }
            else
            {
                counts[index]++;
            }
        }
        sw.Stop();
        return (sw.Elapsed.TotalMilliseconds, keys.Count);
    }

    static (double ms, int check) MeasureDedupSet(int[] numbers)
    {
        Stopwatch sw = Stopwatch.StartNew();
        HashSet<int> set = new HashSet<int>(numbers);
        sw.Stop();
        return (sw.Elapsed.TotalMilliseconds, set.Count);
    }

    static (double ms, int check) MeasureDedupList(int[] numbers)
    {
        Stopwatch sw = Stopwatch.StartNew();
        List<int> result = new List<int>();
        for (int i = 0; i < numbers.Length; i++)
            if (!result.Contains(numbers[i]))
                result.Add(numbers[i]);
        sw.Stop();
        return (sw.Elapsed.TotalMilliseconds, result.Count);
    }

    static (double ms, int check) MeasureCommonSet(List<int> a, List<int> b)
    {
        Stopwatch sw = Stopwatch.StartNew();
        HashSet<int> set = new HashSet<int>(a);
        set.IntersectWith(b);
        sw.Stop();
        return (sw.Elapsed.TotalMilliseconds, set.Count);
    }

    static (double ms, int check) MeasureCommonLists(List<int> a, List<int> b)
    {
        Stopwatch sw = Stopwatch.StartNew();
        int common = 0;
        for (int i = 0; i < a.Count; i++)
        {
            for (int j = 0; j < b.Count; j++)
            {
                if (a[i] == b[j])
                {
                    common++;
                    break;
                }
            }
        }
        sw.Stop();
        return (sw.Elapsed.TotalMilliseconds, common);
    }

    static void PrintRow(string title, (double ms, int check) list, (double ms, int check) set)
    {
        string verdict = list.check == set.check ? "" : "  УВАГА: результати версій розійшлися!";
        Console.WriteLine($"{title}");
        Console.WriteLine($"   список {list.ms:F3} мс, словник/множина {set.ms:F3} мс, у {list.ms / set.ms:F1} разів{verdict}");
    }

    static void RunTask5()
    {
        const int membershipSize = 100000;
        const int membershipChecks = 10000;
        const int wordCount = 50000;
        const int vocabulary = 5000;
        const int dedupCount = 20000;
        const int dedupRange = 40000;
        const int commonSize = 5000;
        const int commonRange = 10000;

        Console.WriteLine("\nЗавдання 5: замір виграшу");

        MeasureMembershipList(1000, 1000);
        MeasureMembershipSet(1000, 1000);
        string[] warmWords = MakeWords(1000, 100);
        MeasureFrequencyDictionary(warmWords);
        MeasureFrequencyLists(warmWords);
        int[] warmInts = MakeInts(1000, 2000);
        MeasureDedupSet(warmInts);
        MeasureDedupList(warmInts);
        List<int> warmA = MakeDistinct(500, 1000, N);
        List<int> warmB = MakeDistinct(500, 1000, N + 100);
        MeasureCommonSet(warmA, warmB);
        MeasureCommonLists(warmA, warmB);

        PrintRow($"{membershipChecks} перевірок наявності в наборі зі {membershipSize} елементів",
            MeasureMembershipList(membershipSize, membershipChecks),
            MeasureMembershipSet(membershipSize, membershipChecks));

        string[] words = MakeWords(wordCount, vocabulary);
        PrintRow($"підрахунок частот {wordCount} слів (словник проти двох паралельних списків)",
            MeasureFrequencyLists(words),
            MeasureFrequencyDictionary(words));

        int[] numbers = MakeInts(dedupCount, dedupRange);
        PrintRow($"вилучення дублікатів із {dedupCount} чисел",
            MeasureDedupList(numbers),
            MeasureDedupSet(numbers));

        List<int> a = MakeDistinct(commonSize, commonRange, N);
        List<int> b = MakeDistinct(commonSize, commonRange, N + 100);
        PrintRow($"спільні елементи двох наборів по {commonSize} елементів",
            MeasureCommonLists(a, b),
            MeasureCommonSet(a, b));
    }
}