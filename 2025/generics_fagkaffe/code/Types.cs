using System.Reflection;
using System.Runtime.InteropServices;

namespace Types;

public class Test(string name, string type)
{
    public string Name { get; set; } = name;
    public string Type { get; set; } = type; 
}

public class ClassWithConstraint<T> where T : class
{
    public T Value { get; set; }

    public ClassWithConstraint(T value)
    {
        Value = value;
    }

    public unsafe string PrintSize()
    {
       return $"Size of {typeof(T)} is: {sizeof(T)} bytes (using [orangered1]sizeof(T)[/])";
    }

    public IEnumerable<string> PrintProperties()
    {
        yield return $"Properties and values for type: {typeof(T)}\n";
        PropertyInfo[] props = typeof(T).GetProperties();
        foreach (PropertyInfo property in props)
        {
            if (property.PropertyType == typeof(string))
            {
                yield return $"{{Key: {property.Name}, Value: {property.GetValue(Value)}}}";
            }

            // TODO: add more property type checks
        }
    }
}
