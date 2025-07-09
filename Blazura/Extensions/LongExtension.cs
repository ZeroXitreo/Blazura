namespace Blazura.Extensions;

public static class LongExtension
{
    public static string ToByte(this long source)
    {
        return source switch
        {
            >= 1024L * 1024L * 1024L => $"{source / 1024L / 1024L / 1024L} GB",
            >= 1024L * 1024L => $"{source / 1024L / 1024L} MB",
            >= 1024L => $"{source / 1024L} KB",
            _ => $"{source} B",
        };
    }
}
