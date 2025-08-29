namespace code;

public static class Extensions
{
    public static string Print(this Person[] list)
    {
        return string.Join(separator: ", ", values: list.Select(x => x.ToString()));
    }
}
