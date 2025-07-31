using System.Runtime.CompilerServices;

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
