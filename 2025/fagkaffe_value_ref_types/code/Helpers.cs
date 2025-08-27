using Spectre.Console;

namespace code;

public static class ConsoleHelper
{
    public static Table RenderStack(Stack<int> stack, int? capacity = null)
    {
        var items = stack.ToArray();
        var table = new Table()
            .LeftAligned()
            .Border(TableBorder.Heavy);

        table.AddColumn(new TableColumn($"[orangered1]Stack (cap: {stack.Capacity}[/])").Centered());

        if (capacity.HasValue && capacity.Value > items.Length)
        {
            for (int i = 0; i < (capacity.Value - items.Length); i++)
            {
                table.AddRow(new Markup($"[grey37](tom)[/]").Centered());
            }
        }

        foreach (int item in items)
        {
            table.AddRow(new Markup($"[blue]{item}[/]").Centered());
        }


        return table;
    }

    public static Table RenderHeap(List<string> list)
    {
        var table = new Table()
            .RightAligned()
            .Border(TableBorder.Heavy)
            .Title($"[mediumpurple2]Heap (size: {list.Count}[/])");

        int levels = (int) Math.Floor(Math.Log2(list.Count)) + 1;
        int maxCols = (int) Math.Pow(2, levels - 1);
        int index = 0;

        for (int a = 0; a < maxCols; a++)
        {
            table.AddColumn(new TableColumn("").Centered());
        }

        for (int i = 0; i < levels; i++)
        {
            List<string> row = new();
            int itemsOnLevel = (int) Math.Pow(2, i);
            int padding = (maxCols - itemsOnLevel) / 2;

            for (int j = 0; j < padding; j++)
            {
                row.Add("");
            }

            for (int k = 0; k < itemsOnLevel; k++)
            {
                if (index < list.Count)
                {
                    row.Add($"[blue] {list[index]} [/]");
                    index++;
                }
                else
                {
                    row.Add("");
                }
            }

            while (row.Count < maxCols)
            {
                row.Add("");
            }
            
            table.AddRow(row.ToArray());
        }


        return table;
    }
}
