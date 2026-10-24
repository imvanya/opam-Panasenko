using System;
using System.Collections.Generic;
using System.Globalization;


class Program
{
    const int N = 18;
    const int K = 12;
    const int RequestCount = 8 + K % 5;     
    const int InitialCapacity = K % 4 + 2; 

    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        RunTask1();
        RunTask2();
        RunTask3();
        RunTask4();
        RunEdgeCases();
    }


    static bool IsOpening(char c)
    {
        return c == '(' || c == '[' || c == '{';
    }

    static bool IsClosing(char c)
    {
        return c == ')' || c == ']' || c == '}';
    }

    static bool Matches(char open, char close)
    {
        return (open == '(' && close == ')')
            || (open == '[' && close == ']')
            || (open == '{' && close == '}');
    }

    static (bool balanced, int errorIndex) CheckBrackets(string s)
    {
        Stack<int> open = new Stack<int>();
        for (int i = 0; i < s.Length; i++)
        {
            char c = s[i];
            if (IsOpening(c))
            {
                open.Push(i);
            }
            else if (IsClosing(c))
            {
                if (open.Count == 0)           
                    return (false, i);

                int top = open.Pop();
                if (!Matches(s[top], c))       
                    return (false, i);
            }
        }

        if (open.Count > 0)                  
        {
            int first = -1;
            while (open.Count > 0)
                first = open.Pop();            
            return (false, first);
        }
        return (true, -1);
    }

    static (bool balanced, int errorIndex) CheckBracketsOwnStack(string s)
    {
        IntStack open = new IntStack(InitialCapacity);
        for (int i = 0; i < s.Length; i++)
        {
            char c = s[i];
            if (IsOpening(c))
            {
                open.Push(i);
            }
            else if (IsClosing(c))
            {
                if (open.Count == 0)
                    return (false, i);

                int top = open.Pop();
                if (!Matches(s[top], c))
                    return (false, i);
            }
        }

        if (open.Count > 0)
        {
            int first = -1;
            while (open.Count > 0)
                first = open.Pop();
            return (false, first);
        }
        return (true, -1);
    }

    static (string text, bool balanced)[] GetBracketTests()
    {
        return new (string text, bool balanced)[]
        {
            ("(a[b]{c})", true),
            ("([)]", false),
            (")(", false),
            ("((", false),
            ("", true),
            (new string('(', K) + new string(')', K - 1), false)
        };
    }

    static void RunTask1()
    {
        Console.WriteLine("Завдання 1: збалансованість дужок");
        (string text, bool balanced)[] tests = GetBracketTests();
        for (int i = 0; i < tests.Length; i++)
        {
            (bool ok, int index) = CheckBrackets(tests[i].text);
            string verdict = ok ? "збалансований" : "незбалансований";
            Console.WriteLine($"\"{tests[i].text}\" -> {verdict}, індекс помилки: {index}, очікувано збалансований: {tests[i].balanced}, збігається: {ok == tests[i].balanced}");
        }
    }

    static bool IsOperator(string t)
    {
        return t == "+" || t == "-" || t == "*" || t == "/" || t == "^";
    }

    static double Apply(string op, double a, double b)
    {
        switch (op)
        {
            case "+": return a + b;
            case "-": return a - b;   
            case "*": return a * b;
            case "/": return a / b;
            default: return Math.Pow(a, b);
        }
    }

    static (bool ok, double value, string error) EvaluateRpn(string expr)
    {
        string[] tokens = expr.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (tokens.Length == 0)
            return (false, 0, "порожній вираз");

        Stack<double> stack = new Stack<double>();
        for (int i = 0; i < tokens.Length; i++)
        {
            string t = tokens[i];
            if (IsOperator(t))
            {
                if (stack.Count < 2)
                    return (false, 0, $"не вистачає чисел для операції '{t}' (токен {i + 1})");

                double b = stack.Pop();
                double a = stack.Pop();
                if (t == "/" && b == 0)
                    return (false, 0, "ділення на нуль");

                double r = Apply(t, a, b);
                if (double.IsNaN(r) || double.IsInfinity(r))
                    return (false, 0, $"результат операції '{t}' не є скінченним числом");
                stack.Push(r);
            }
            else if (double.TryParse(t, NumberStyles.Float, CultureInfo.InvariantCulture, out double v))
            {
                stack.Push(v);
            }
            else
            {
                return (false, 0, $"невідомий токен '{t}'");
            }
        }

        if (stack.Count != 1)
            return (false, 0, $"забагато чисел: у стеку лишилось {stack.Count}");

        return (true, stack.Pop(), "");
    }

    static void PrintRpn(string expr, string expected)
    {
        (bool ok, double value, string error) = EvaluateRpn(expr);
        string result = ok ? value.ToString(CultureInfo.InvariantCulture) : "помилка: " + error;
        Console.WriteLine($"\"{expr}\" -> {result}   (очікувано: {expected})");
    }

    static void RunTask2()
    {
        Console.WriteLine("\nЗавдання 2: зворотний польський запис");
        PrintRpn("3 4 + 2 *", "14");
        PrintRpn("5 3 -", "2");
        PrintRpn("8 2 /", "4");
        PrintRpn("2 3 4 * +", "14");
        PrintRpn("5 0 /", "повідомлення про помилку");

        Console.WriteLine("--- піднесення до степеня і некоректний вхід");
        PrintRpn("2 3 ^", "8");
        PrintRpn("1 +", "помилка: не вистачає чисел");
        PrintRpn("1 2", "помилка: забагато чисел");
        PrintRpn("1 x +", "помилка: невідомий токен");

        Console.WriteLine("--- власний вираз, результат якого дорівнює K");
        string own = "10 2 ^ 4 - 8 /"; 
        PrintRpn(own, K.ToString());
        (bool ok, double value, string error) = EvaluateRpn(own);
        Console.WriteLine("Результат дорівнює K: " + (ok && value == K));
    }


    struct Job
    {
        public int Id;
        public int Weight;
        public int Server;
        public int Start;
        public int End;
    }

    static int[] MakeWeights(int count)
    {
        Random rnd = new Random(N);
        int[] weights = new int[count];
        for (int i = 0; i < count; i++)
            weights[i] = rnd.Next(1, 10); 
        return weights;
    }

    static (List<Job> jobs, int maxQueue) SimulateTwoServers(int[] weights)
    {
        Queue<(int Id, int Weight)> queue = new Queue<(int Id, int Weight)>(weights.Length);
        for (int i = 0; i < weights.Length; i++)
            queue.Enqueue((i + 1, weights[i]));

        List<Job> jobs = new List<Job>(weights.Length);
        int[] freeAt = new int[2]; 
        int maxQueue = 0;

        while (queue.Count > 0) 
        {
            if (queue.Count > maxQueue)
                maxQueue = queue.Count;

            int server = freeAt[0] <= freeAt[1] ? 0 : 1; 
            (int id, int weight) = queue.Dequeue();

            Job job = new Job();
            job.Id = id;
            job.Weight = weight;
            job.Server = server + 1;
            job.Start = freeAt[server];
            job.End = job.Start + weight;
            freeAt[server] = job.End;
            jobs.Add(job);
        }
        return (jobs, maxQueue);
    }

    static void PrintSimulation(int[] weights)
    {
        (List<Job> jobs, int maxQueue) = SimulateTwoServers(weights);
        int finish = 0;
        for (int i = 0; i < jobs.Count; i++)
        {
            Job j = jobs[i];
            Console.WriteLine($"Заявка {j.Id,2}: вага {j.Weight}, обслуговувач {j.Server}, початок {j.Start,2}, завершення {j.End,2}");
            if (j.End > finish)
                finish = j.End;
        }
        Console.WriteLine("Максимальна довжина черги: " + maxQueue);
        Console.WriteLine("Усі заявки завершено на хвилині: " + finish);
    }

    static void RunTask3()
    {
        Console.WriteLine($"\nЗавдання 3: {RequestCount} заявок, два обслуговувачі, спільна черга");
        int[] weights = MakeWeights(RequestCount);
        Console.WriteLine("Ваги: " + string.Join(", ", weights));
        PrintSimulation(weights);
    }


    static void RunTask4()
    {
        Console.WriteLine($"\nЗавдання 4: IntStack, початкова місткість {InitialCapacity}");
        IntStack stack = new IntStack(InitialCapacity);
        Console.WriteLine($"Початок: Count = {stack.Count}, Capacity = {stack.Capacity}");

        int pushes = K % 4 + 3; 
        for (int i = 1; i <= pushes + 2; i++)
        {
            int before = stack.Capacity;
            stack.Push(i * 10);
            string note = stack.Capacity != before ? "  <- подвоєння" : "";
            Console.WriteLine($"Push({i * 10}): Count = {stack.Count}, Capacity = {stack.Capacity}{note}");
        }
        Console.WriteLine($"Peek() = {stack.Peek()}, Count після Peek = {stack.Count}");

        Console.WriteLine("завдання 1 на власному стеку проти стандартного");
        (string text, bool balanced)[] tests = GetBracketTests();
        for (int i = 0; i < tests.Length; i++)
        {
            (bool ok, int index) = CheckBrackets(tests[i].text);
            (bool okOwn, int indexOwn) = CheckBracketsOwnStack(tests[i].text);
            Console.WriteLine($"\"{tests[i].text}\": стандартний ({ok}, {index}), власний ({okOwn}, {indexOwn}), збігається: {ok == okOwn && index == indexOwn}");
        }
    }


    static void RunEdgeCases()
    {
        Console.WriteLine("\nКрайові випадки");

        (bool ok, int index) = CheckBrackets("");
        Console.WriteLine($"Порожній рядок, завдання 1: збалансований = {ok}, індекс = {index}");
        PrintRpn("", "повідомлення про помилку");

        (ok, index) = CheckBrackets("abc 123");
        Console.WriteLine($"Рядок без дужок, завдання 1: збалансований = {ok}, індекс = {index}");

        PrintRpn("7", "7");
        PrintRpn("5 0 /", "повідомлення про помилку");

        Console.WriteLine("Жодної заявки, завдання 3:");
        PrintSimulation(new int[0]);

        Console.WriteLine("Pop на порожньому стеку, завдання 4:");
        IntStack empty = new IntStack(InitialCapacity);
        try
        {
            empty.Pop();
            Console.WriteLine("  винятку не було (помилка!)");
        }
        catch (InvalidOperationException e)
        {
            Console.WriteLine("  перехоплено InvalidOperationException: " + e.Message);
        }
    }
}

class IntStack
{
    private int[] items;
    private int count;

    public IntStack(int initialCapacity)
    {
        if (initialCapacity < 1)
            initialCapacity = 1;
        items = new int[initialCapacity];
        count = 0;
    }

    public int Count
    {
        get { return count; }
    }

    public int Capacity
    {
        get { return items.Length; }
    }

    public void Push(int x)
    {
        if (count == items.Length) 
        {
            int[] bigger = new int[items.Length * 2];
            for (int i = 0; i < count; i++)
                bigger[i] = items[i];
            items = bigger;
        }
        items[count] = x;
        count++;
    }

    public int Pop()
    {
        if (count == 0)
            throw new InvalidOperationException("Стек порожній");
        count--;
        return items[count];
    }

    public int Peek()
    {
        if (count == 0)
            throw new InvalidOperationException("Стек порожній");
        return items[count - 1];
    }
}