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