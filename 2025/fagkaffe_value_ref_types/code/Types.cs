using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace code;

public delegate void ConsoleLog(string s);

public class Person(string name, int age)
{
    public string Name { get; set; } = name;
    public int Age { get; set; } = age;

    public override string ToString()
    {
        return $"{{Name: {Name}, Age: {Age}}}";
    }
}

public interface ICoordinates
{
    double X { get; init; }
    double Y { get; init; }
}

public readonly struct Coordinates(double x, double y) : ICoordinates
{
    public double X { get; init; } = x;
    public double Y { get; init; } = y;
}

public readonly struct CoordinatesWithLocation : ICoordinates
{
    public double X { get; init; }
    public double Y { get; init; }
    public string Location { get; init; }
}

public ref struct CoordinatesWithCharBuffer : ICoordinates
{
    public double X { get; init; }
    public double Y { get; init; }
    public ReadOnlySpan<char> Location
    {
        get => location.AsReadonlySpan();
    }
    private CharBuffer location;

    public CoordinatesWithCharBuffer(double x, double y, string location)
    {
        X = x;
        Y = y;
        this.location = default;

        int len = Math.Min(location.Length, 31);
        for (int i = 0; i < len; i++)
        {
            this.location[i] = location[i];
        }

        if (len < 32)
            this.location[len] = '\0';
    }

    public override string ToString()
    {
        return $"(x: {X}, y: {Y}) - {Location}";
    }
}

[InlineArray(32)]
public struct CharBuffer
{
    private char _element;

    public ReadOnlySpan<char> AsReadonlySpan()
    {
        return MemoryMarshal.CreateReadOnlySpan(ref _element, 32);
    }
}
