using code;
using Spectre.Console;

ConsoleLog log = AnsiConsole.MarkupLine;
Console.CancelKeyPress += (_, _) => Console.CursorVisible = true;
string[] baseChoices = [
    "1. Value types",
    "2. Reference types",
    "3. Exit"
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
            default:
                break;
        }
    }
    while (index != 2);
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
    renderSeparator();

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
    renderSeparator();

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
    renderSeparator();

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