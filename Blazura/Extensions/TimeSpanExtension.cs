namespace Blazura.Extensions;

public static class TimeSpanExtension
{
    public static string ToSimple(this TimeSpan source)
    {
        if (source.TotalDays >= 1) return $"{(int)source.TotalDays} day{((int)source.TotalDays == 1 ? null : "s")} ago";
        if (source.TotalHours >= 1) return $"{(int)source.TotalHours} hr{((int)source.TotalHours == 1 ? null : "s")} ago";
        if (source.TotalMinutes >= 1) return $"{(int)source.TotalMinutes} min ago";
        return $"{(int)source.TotalSeconds} sec ago";
    }
}
