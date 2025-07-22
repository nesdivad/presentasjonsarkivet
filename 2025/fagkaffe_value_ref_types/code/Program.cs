using code;
using Spectre.Console;

ConsoleLog log = AnsiConsole.MarkupLine;
Console.CancelKeyPress += (_, _) => Console.CursorVisible = true;
string[] baseChoices = [
    "1. Value types",
    "2. Reference types",
    "3. ???"
];

Start();

void Start()
{
    while (true)
    {
        AnsiConsole.Clear();
        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Velg neste steg:")
                .AddChoices(baseChoices)
        );

        var index = Array.FindIndex(
            array: baseChoices,
            match: x => x.Equals(choice, StringComparison.OrdinalIgnoreCase)
        );

        switch (index)
        {
            case 0:
                valueExamples();
                break;
            case 1:
                break;
            case 2:
                break;
            default: break;
        }
    }
}

void valueExamples()
{
    AnsiConsole.Write(
        writePaddedText(
            """
            int [blue]a[/] = 4;
            int [green]b[/] = [blue]a[/];
            [blue]a[/]++;
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
            """
            int [blue]a[/] = 4;
            int ref [green]b[/] = ref [blue]a[/];
            [blue]a[/]++;
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
            """
            int [blue]a[/] = 4;
            int ref [green]b[/] = ref [blue]a[/];
            [blue]a[/]++;
            [green]b[/]++;
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
    renderSeparator();
}


// example showing value types copying values on assignment
IEnumerable<string> valueOne()
{
    int a = 4;
    yield return $"[blue]a[/] before increment: {a}";

    int b = a;
    a++;

    yield return $"[blue]a[/] after increment: {a}";
    yield return $"[green]b[/] after increment: {b}";
}

string valueOnePointFive(bool secondIncrement = false)
{
    int a = 4;
    int aCopy = a;
    ref int b = ref a;
    a++;

    if (secondIncrement)
    {
        b++;
    }

    return
    $"""
    a before increment: {aCopy}
    a after increment: {a}
    b after increment: {b}
    """;
}

// example showing pointer operators in unsafe context 
unsafe void valueTwo()
{
    int a = 4;
    log($"a before increment: {a}");

    int* aPtr = &a;
    *aPtr += 1;
    log($"a after increment: {a}");

    log($"address of a: {(long)aPtr:X}");
}

// example showing how to manipulate value type using 'ref' keyword
// the 'ref' keyword allows you to pass arguments by reference
void valueThree()
{
    int a = 4;
    log($"a before increment: {a}");
    increment(ref a);
    log($"a after increment: {a}");

    void increment(ref int toIncrement)
    {
        toIncrement++;
    }
}

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