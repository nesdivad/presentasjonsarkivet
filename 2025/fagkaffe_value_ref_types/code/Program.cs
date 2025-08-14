using code;
using Spectre.Console;

ConsoleLog log = AnsiConsole.MarkupLine;
Console.CancelKeyPress += (_, _) => Console.CursorVisible = true;
string[] baseChoices = [
    "1. Value types",
    "2. Reference types",
    "3. Boxing and Unboxing",
    "4. Advanced examples",
    "5. Exit"
];

const string aMarkup = "[blue]a[/]";
const string bMarkup = "[green]b[/]";
const string refMarkup = "[red]ref[/]";

Start();

void Start()
{
    int index;
    do
    {
        AnsiConsole.Clear();
        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Velg neste steg:")
                .AddChoices(baseChoices)
        );

        index = Array.FindIndex(
            array: baseChoices,
            match: x => x.Equals(choice, StringComparison.OrdinalIgnoreCase)
        );

        switch (index)
        {
            case 0:
                valueExamples();
                break;
            case 1:
                referenceExamples();
                break;
            case 2:
                boxingUnboxingExamples();
                break;
            case 3:
                advancedExamples();
                break;
            default:
                index = 100;
                break;
        }
    }
    while (index < 10);
}

#region [ Value types ]

void valueExamples()
{
    AnsiConsole.Write(
        writePaddedText(
            $"""
            int {aMarkup} = 4;
            int {bMarkup} = {aMarkup};
            {aMarkup}++;

            // b = ?
            """
        )
    );

    promptNext("");
    renderWeakSeparator(Color.Green);

    AnsiConsole.Write(
        writePaddedText(
            string.Join('\n', valueOne())
        )
    );

    promptNext("");
    renderSeparator();

    AnsiConsole.Write(
        writePaddedText(
            $"""
            int {aMarkup} = 4;
            {refMarkup} int {bMarkup} = {refMarkup} {aMarkup};
            {aMarkup}++;

            // b = ?
            """
        )
    );

    promptNext("");
    renderWeakSeparator(Color.Green);

    AnsiConsole.Write(
        writePaddedText(
            valueOnePointFive()
        )
    );

    promptNext("");
    renderSeparator();

    AnsiConsole.Write(
        writePaddedText(
            $"""
            int {aMarkup} = 4;
            {refMarkup} int {bMarkup} = {refMarkup} {aMarkup};
            {aMarkup}++;
            {bMarkup}++;

            // a = ?
            // b = ?
            """
        )
    );

    promptNext("");
    renderWeakSeparator(Color.Green);

    AnsiConsole.Write(
        writePaddedText(
            valueOnePointFive(secondIncrement: true)
        )
    );

    promptNext("");
    AnsiConsole.Clear();

    AnsiConsole.Write(
        writePaddedText(
            """
            void increment([red]ref[/] int [green]toIncrement[/])
            {
                [green]toIncrement[/]++;
            }
            
            int [blue]a[/] = 4;
            increment([red]ref[/] [blue]a[/]);

            // a = ?
            """
        )
    );

    promptNext("");
    renderWeakSeparator(Color.Green);

    AnsiConsole.Write(
        writePaddedText(
            valueThree()
        )
    );

    promptNext("");
}


// example showing value types copying values on assignment
IEnumerable<string> valueOne()
{
    int a = 4;
    int b = a;
    a++;

    yield return $"{aMarkup} after increment: {a}";
    yield return $"{bMarkup} after increment: {b}";
}

string valueOnePointFive(bool secondIncrement = false)
{
    int a = 4;
    ref int b = ref a;
    a++;

    if (secondIncrement)
    {
        b++;
    }

    return
    $"""
    {aMarkup} after increment: {a}
    {bMarkup} after increment: {b}
    """;
}

// example showing pointer operators in unsafe context 
unsafe void valueTwo()
{
    int a = 4;
    int* aPtr = &a;
    *aPtr += 1;
    log($"a after increment: {a}");
    log($"address of a: {(long)aPtr:X}");
}

// example showing how to manipulate value type using 'ref' keyword
// the 'ref' keyword allows you to pass arguments by reference
string valueThree()
{
    int a = 4;
    increment(ref a);

    void increment(ref int toIncrement)
    {
        toIncrement++;
    }

    return $"{aMarkup} after increment: {a}";
}

#endregion

#region [ Reference types ]

void referenceExamples()
{
    AnsiConsole.Write(
        writePaddedText(
            """
            public class Person(string name, int age)
            {
                public string Name { get; set; } = name;
                public int Age { get; set; } = age;

                ...
            }

            void [yellow]returnToTheTwenties[/](Person [teal]person[/])
            {
                [teal]person[/].Age--;
            }

            Person [teal]person[/] = new(
                name: "Kristoffer",
                age: 30
            );

            [yellow]returnToTheTwenties[/]([teal]person[/]);

            // hvor gammel er [teal]person[/]?
            """
        )
    );

    promptNext("");
    renderWeakSeparator(Color.Green);

    AnsiConsole.Write(
        writePaddedText(
            string.Join('\n', referenceOne())
        )
    );

    promptNext("");
    AnsiConsole.Clear();

    AnsiConsole.Write(
        writePaddedText(
            """
            void [yellow]returnToTheTwenties[/]([red]ref[/] Person [teal]person[/])
            {
                [teal]person[/].Age--;
            }

            Person [teal]person[/] = new(
                name: "Kristoffer",
                age: 30
            );

            [yellow]returnToTheTwenties[/]([red]ref[/] [teal]person[/]);

            // hvor gammel er [teal]person[/]?
            """
        )
    );

    promptNext("");
    renderWeakSeparator(Color.Green);

    AnsiConsole.Write(
        writePaddedText(
            string.Join('\n', referenceTwo())
        )
    );

    promptNext("");
    AnsiConsole.Clear();

    AnsiConsole.Write(
        writePaddedText(
            """
            void [orangered1]edit[/](int[[]] [teal]numbers[/])
            {
                for (int i = 0; i < [teal]numbers[/].Length; i++)
                {
                    [teal]numbers[/][[i]]++;
                }
            }

            void [red]edit2[/](int[[]] [teal]numbers[/])
            {
                for (int i = 0; i < [teal]numbers[/].Length; i++)
                {
                    int target = [teal]numbers[/][[i]];
                    target++;
                }
            }

            int[[]] [teal]numbers[/] = [[100, 200, 300]];
            [orangered1]edit[/]([teal]numbers[/]);

            int[[]] [green]numbers2[/] = [[100, 200, 300]];
            [red]edit2[/]([green]numbers2[/]);

            // [teal]numbers[/] = ?
            // [green]numbers2[/] = ?
            """
        )
    );

    promptNext("");
    renderWeakSeparator(Color.Green);

    AnsiConsole.Write(
        writePaddedText(
            string.Join('\n', referenceThree())
        )
    );

    promptNext("");
    AnsiConsole.Clear();

    AnsiConsole.Write(
        writePaddedText(
            """
            void [orangered1]edit[/](Person[[]] [teal]list[/])
            {
                for (int i = 0; i < [teal]list[/].Length; i++)
                {
                    Person person = [teal]list[/][[i]];
                    person.Age--;
                }
            }
            void [red]edit2[/](Person[[]] [teal]list[/])
            {
                for (int i = 0; i < [teal]list[/].Length; i++)
                {
                    int age = [teal]list[/][[i]].Age;
                    age--;
                }
            }

            Person[[]] [green]people[/] = [[new("Kristoffer", 30), new("Per", 90)]];
            [orangered1]edit[/]([green]people[/]);

            Person[[]] [yellow]people2[/] = [[new("Kristoffer", 30), new("Per", 90)]];
            [red]edit2[/]([yellow]people2[/]);

            // [green]people[/] = ?
            // [yellow]people2[/] = ?
            """
        )
    );

    promptNext("");
    renderWeakSeparator(Color.Green);

    AnsiConsole.Write(
        writePaddedText(
            string.Join('\n', referenceFour())
        )
    );

    promptNext("");
    renderSeparator();
}

IEnumerable<string> referenceOne()
{
    void returnToTheTwenties(Person person)
    {
        person.Age--;
    }

    Person person = new(
        name: "Kristoffer",
        age: 30
    );

    yield return $"[teal]person[/] before change: {person}";

    returnToTheTwenties(person);

    yield return $"[teal]person[/] after change: {person}";
}

IEnumerable<string> referenceTwo()
{
    void returnToTheTwenties(ref Person person)
    {
        person.Age--;
    }

    Person person = new(
        name: "Kristoffer",
        age: 30
    );

    yield return $"[teal]person[/] before assignment: {person}";

    returnToTheTwenties(ref person);

    yield return $"[teal]person[/] after assignment: {person}";
}

IEnumerable<string> referenceThree()
{
    void edit(int[] list)
    {
        for (int i = 0; i < list.Length; i++)
        {
            list[i]++;
        }
    }

    void edit2(int[] list)
    {
        for (int i = 0; i < list.Length; i++)
        {
            int target = list[i];
            target++;
        }
    }

    int[] numbers = [100, 200, 300];

    edit(numbers);

    yield return $"[teal]numbers[/] after [orangered1]edit[/]: {string.Join(", ", numbers)}";

    int[] numbers2 = [100, 200, 300];

    edit2(numbers2);

    yield return $"[green]numbers2[/] after [red]edit2[/]: {string.Join(", ", numbers2)}";
}

IEnumerable<string> referenceFour()
{
    void edit(Person[] list)
    {
        for (int i = 0; i < list.Length; i++)
        {
            Person person = list[i];
            person.Age--;
        }
    }

    void edit2(Person[] list)
    {
        for (int i = 0; i < list.Length; i++)
        {
            int age = list[i].Age;
            age--;
        }
    }

    Person[] people = [new("Kristoffer", 30), new("Per", 90)];
    edit(people);

    Person[] people2 = [new("Kristoffer", 30), new("Per", 90)];
    edit2(people2);

    yield return $"[green]people[/] after change: {people.Print()}";
    yield return $"[yellow]people2[/] after change: {people2.Print()}";
}

#endregion

#region [ Boxing and unboxing ]

void boxingUnboxingExamples()
{
    AnsiConsole.Write(
        writePaddedText(
            """
            int [teal]a[/] = 4;
            object [green]b[/] = [teal]a[/];         // implicit boxing
            object [orangered1]c[/] = (object) [teal]a[/]; // explicit boxing

            // [green]b[/] = ?
            // [orangered1]c[/] = ?
            """
        )
    );

    promptNext("");
    renderWeakSeparator(Color.Green);

    AnsiConsole.Write(
        writePaddedText(
            string.Join('\n', boxingOne())
        )
    );

    promptNext("");
    renderSeparator();

    AnsiConsole.Write(
        writePaddedText(
            """
            object [teal]d[/] = 4.0;
            double [green]e[/] = (double) [teal]d[/]; // explicit

            // only explicit casts are available when unboxing
            """
        )
    );

    promptNext("");
    renderWeakSeparator(Color.Green);

    AnsiConsole.Write(
        writePaddedText(
            string.Join('\n', boxingTwo())
        )
    );

    promptNext("");
    AnsiConsole.Clear();

    AnsiConsole.Write(
        writePaddedText(
            """
            // string.Format(string format, object? arg0, object? arg1, object? arg2);
            string [teal]formatStr[/] = string.Format(
                format: "{0}, {1}, {2}",
                arg0: "Hello, world!",      // reference type
                arg1: true,                 // value type
                arg2: 42                    // value type
            );
            """
        )
    );

    promptNext("");
    renderWeakSeparator(Color.Green);

    AnsiConsole.Write(
        writePaddedText(
            string.Join('\n', boxingThree())
        )
    );

    promptNext("");
    AnsiConsole.Clear();

    AnsiConsole.Write(
        writePaddedText(
            """
            string [teal]patternMatching[/](object obj)
            {
                // if obj is pattern matched, perform unboxing to value type
                // implicit unboxing done by internal pattern matching logic
                return obj switch
                {
                    char c => $"obj is char: {c}, {c.GetType()}",
                    int d => $"obj is int: {d}, {d.GetType()}",
                    bool b => $"obj is bool: {b}, {b.GetType()}",
                    _ => $"type unknown"
                };
            }

            // performs boxing from value types to reference type 'object'
            [teal]patternMatching[/]('A');       // char
            [teal]patternMatching[/](true);      // bool
            [teal]patternMatching[/](42);        // int
            [teal]patternMatching[/](4.2);       // double
            """
        )
    );

    promptNext("");
    renderWeakSeparator(Color.Green);

    AnsiConsole.Write(
        writePaddedText(
            string.Join('\n', boxingFour())
        )
    );

    promptNext("");
    renderSeparator();
}

IEnumerable<string> boxingOne()
{
    int a = 4;
    object b = a;         // implicit, the value '4' is copied into 'b' and stored on the heap
    object c = (object)a; // explicit, the value '4' is copied into 'c' and stored on the heap

    yield return $"[green]b[/] after boxing: {b}";
    yield return $"type of [green]b[/] when calling [green]b[/].GetType(): {b.GetType()}";
    yield return string.Empty;
    yield return $"[red]c[/] after boxing: {c}";
    yield return $"type of [red]c[/] when calling [red]c[/].GetType(): {c.GetType()}";
}

IEnumerable<string> boxingTwo()
{
    object d = 4.0;
    double e = (double)d; // explicit

    yield return $"type of [teal]d[/] before unboxing when calling [teal]d[/].GetType(): {d.GetType()}";
    yield return $"[green]e[/] after unboxing: {e}";
}

IEnumerable<string> boxingThree()
{
    // string.Format(string format, object? arg0, object? arg1, object? arg2);
    string formatStr = string.Format(
        format: "{0}, {1}, {2}",
        arg0: "Hello, world!",      // reference type
        arg1: true,                 // value type
        arg2: 42                    // value type
    );

    yield return $"result of [teal]formatStr[/]: {formatStr}";
}

IEnumerable<string> boxingFour()
{
    string patternMatching(object obj)
    {
        return obj switch
        {
            char c => $"obj is char: {c}, {c.GetType()}",
            int d => $"obj is int: {d}, {d.GetType()}",
            bool b => $"obj is bool: {b}, {b.GetType()}",
            _ => $"type unknown"
        };
    }

    yield return $"result of pattern matching char c: {patternMatching('A')}";
    yield return $"result of pattern matching bool b: {patternMatching(true)}";
    yield return $"result of pattern matching int i: {patternMatching(42)}";
    yield return $"result of pattern matching double d: {patternMatching(4.2)}";
}


#endregion

#region [ Advanced examples ]

void advancedExamples()
{
    string[] advancedExampleChoices = [
        "1. Structs",
        "2. Structs with inline arrays",
        "Exit"
    ];

    int index;

    do
    {
        AnsiConsole.Clear();
        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Velg neste steg:")
                .AddChoices(advancedExampleChoices)
        );

        index = Array.FindIndex(
            array: advancedExampleChoices,
            match: x => x.Equals(choice, StringComparison.OrdinalIgnoreCase)
        );

        switch (index)
        {
            case 0:
                structExamples();
                break;
            case 1:
                structInlineArraysExamples();
                break;
            default:
                index = 100;
                break;
        }
    }
    while (index < 10);
}

void structExamples()
{
    AnsiConsole.Write(
        writePaddedText(
            """
            public readonly struct [green]Coordinates[/](double x, double y)
            {
                public double X { get; init; } = x;
                public double Y { get; init; } = y;
            }
            """
        )
    );

    promptNext("");

    AnsiConsole.Write(
        writePaddedText(
            """
            [green]Coordinates[/] coords = new(x: 5.33, y: 60.37);
            coords.X = 5.34; // [red]don't do this[/]
            """
        )
    );

    promptNext("");

    AnsiConsole.Write(
        writePaddedText(
            "[green]Coordinates[/] updatedCoords = coords with { X = 5.34 }; // [green]do this[/]"
        )
    );

    promptNext("");
    renderSeparator();

    AnsiConsole.Write(
        writePaddedText(
            """
            public readonly struct CoordinatesWithLocation
            {
                public double X { get; init; }          // value type
                public double Y { get; init; }          // value type
                public string Location { get; init; }   // reference type
            }
            """
        )
    );

    promptNext("");
    renderSeparator();
}

void structInlineArraysExamples()
{
    CoordinatesWithCharBuffer coordinates = new(x: 5.34, y: 60.37, location: "Solheimsgaten");

    AnsiConsole.Write(
        writePaddedText(
            """
            Example using a ref struct '[green]CoordinatesWithCharBuffer[/]', which prohibits the struct from escaping to the heap.
            
            The ref struct also contains a field of type '[green]CharBuffer[/]', which is decorated with an [yellow][[InlineArray]][/] attribute.
            A struct with an [yellow][[InlineArray]][/] attribute creates an array that is stored on the stack. The type of the array is decided by the private field inside the struct, in our case a [teal]char[/].
            """
        )
    );

    AnsiConsole.Write(
        writePaddedText(
            """
            public ref struct [green]CoordinatesWithCharBuffer[/] : ICoordinates
            {
                public double X { get; init; }
                public double Y { get; init; }            
                public ReadOnlySpan<char> Location { get => location.AsReadonlySpan(); }
                private CharBuffer location;

                public CoordinatesWithCharBuffer(double x, double y, string location)
                {
                    X = x; Y = y; this.location = default;

                    int len = Math.Min(location.Length, 31);
                    for (int i = 0; i < len; i++)
                    {
                        this.location[[i]] = location[[i]];
                    }

                    if (len < 32) this.location[[len]] = '\0';
                }
                ...
            }

            [[InlineArray(32)]]
            public struct [green]CharBuffer[/]
            {
                private char _element;
                
                ...
            }
            """
        )
    );

    promptNext("");
    renderWeakSeparator(Color.Green);

    AnsiConsole.Write(
        writePaddedText(
            $"""
            [green]CoordinatesWithCharBuffer[/] coordinates = new(x: 5.34, y: 60.37, location: "Solheimsgaten");
            
            ToString(): {coordinates.ToString()}
            """
        )
    );

    promptNext("");
    renderSeparator();
}

#endregion

#region [ Helpers ]

void promptNext(string prompt)
{
    ConsoleKeyInfo cki;
    AnsiConsole.Markup(prompt);

    do cki = Console.ReadKey(intercept: true);
    while (cki.Key is not ConsoleKey.Enter);
}

Padder writePaddedText(string text)
{
    var txt = new Markup(text);
    return new Padder(txt).PadLeft(4);
}

static void renderSeparator()
{
    AnsiConsole.Write(new Rule().HeavyBorder());
}

static void renderWeakSeparator(Color color)
{
    AnsiConsole.Write(new Rule().SquareBorder().RuleStyle(color));
}

#endregion
