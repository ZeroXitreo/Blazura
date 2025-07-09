namespace Blazura.Extensions;

public static class IntExtension
{
    public static string ToSimple(this int source)
    {
        return source switch
        {
            >= 1000000 => (float)(source / 100000) / 10 + "m",
            > 1000 => (float)(source / 100) / 10 + "k",
            _ => source.ToString(),
        };
    }
}
