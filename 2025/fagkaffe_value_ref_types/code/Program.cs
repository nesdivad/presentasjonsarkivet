using code;
using Spectre.Console;

ConsoleLog log = AnsiConsole.MarkupLine;
Console.CancelKeyPress += (_, _) => Console.CursorVisible = true;
string[] baseChoices = [
    "1. Value types",
    "2. Reference types",
    "3. ???"
];

const string aMarkup = "[blue]a[/]";
const string bMarkup = "[green]b[/]";
const string refMarkup = "[lime]ref[/]";

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
                referenceExamples();
                break;
            case 2:
                break;
            default: break;
        }
    }
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

    AnsiConsole.Write(
        writePaddedText(
            """
            void increment([lime]ref[/] int [green]toIncrement[/])
            {
                [green]toIncrement[/]++;
            }
            
            int [blue]a[/] = 4;
            increment([lime]ref[/] [blue]a[/]);
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
    yield return $"{aMarkup} before increment: {a}";

    int b = a;
    a++;

    yield return $"{aMarkup} after increment: {a}";
    yield return $"{bMarkup} after increment: {b}";
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
    {aMarkup} before increment: {aCopy}
    {aMarkup} after increment: {a}
    {bMarkup} after increment: {b}
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
string valueThree()
{
    int a = 4;
    int aCopy = a;
    increment(ref a);

    void increment(ref int toIncrement)
    {
        toIncrement++;
    }

    return
    $"""
    {aMarkup} before increment: {aCopy}
    {aMarkup} after increment: {a}
    """;
}

#endregion

#region [ Reference types ]

void referenceExamples()
{
    AnsiConsole.Write(
        writePaddedText(
            """
            void [yellow]returnToTheTwenties[/](Person [teal]person[/])
            {
                [teal]person[/].Age--;
            }

            Person [teal]person[/] = new(
                name: "Kristoffer",
                age: 30
            );

            [yellow]returnToTheTwenties[/]([teal]person[/]);
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
    renderSeparator();

    AnsiConsole.Write(
        writePaddedText(
            """
            void [yellow]returnToTheTwenties[/]([lime]ref[/] Person [teal]person[/])
            {
                [teal]person[/].Age--;
            }

            Person [teal]person[/] = new(
                name: "Kristoffer",
                age: 30
            );

            [yellow]returnToTheTwenties[/]([lime]ref[/] [teal]person[/]);
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
    renderSeparator();

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

            int[[]] [teal]numbers2[/] = [[100, 200, 300]];
            [red]edit2[/]([teal]numbers2[/]);
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

    yield return $"[teal]person[/] before assignment: {person}";

    returnToTheTwenties(person);

    yield return $"[teal]person[/] after assignment: {person}";
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

    yield return $"list of numbers after [orangered1]edit[/]: {string.Join(", ", numbers)}";

    int[] numbers2 = [100, 200, 300];

    edit2(numbers2);

    yield return $"list of numbers after [red]edit2[/]: {string.Join(", ", numbers2)}";
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