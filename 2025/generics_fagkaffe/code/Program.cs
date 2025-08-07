using Spectre.Console;
using Types;

string[] choices = [
    "Stuff",
    "Exit",
];

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
                .AddChoices(choices)
        );

        index = Array.FindIndex(
            array: choices,
            match: x => x.Equals(choice, StringComparison.OrdinalIgnoreCase)
        );

        switch (index)
        {
            case 0:
                genericsWithConstraints();
                break;
            default:
                index = 100;
                break;
        }
    }
    while (index < 10);
}

#region [ Generics ]

void genericsWithConstraints()
{
    Test testClass = new("testName", "testType");
    ClassWithConstraint<Test> classWithConstraint = new(testClass);

    AnsiConsole.Write(
        writePaddedText(
            string.Join('\n', classWithConstraint.PrintProperties())
        )
    );

    AnsiConsole.Write(
        writePaddedText(
            classWithConstraint.PrintSize()
        )
    );

    promptNext("");
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
