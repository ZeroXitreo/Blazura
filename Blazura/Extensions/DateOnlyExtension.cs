namespace Blazura.Extensions;

public static class DateOnlyExtension
{
    public static string ToSimple(this DateOnly source)
    {
        int totalDaysAgo = DateOnly.FromDateTime(DateTime.UtcNow).DayNumber - source.DayNumber;
        bool isInFuture = totalDaysAgo < 0;
        int days = Math.Abs(totalDaysAgo);

        int years = (int)(days / 365.25);
        if (years > 0) return WrapToRelativeTimeString(years, "year", isInFuture);

        int months = (int)(days / 30.4375);
        if (months > 0) return WrapToRelativeTimeString(months, "month", isInFuture);

        int weeks = days / 7;
        if (weeks > 0) return WrapToRelativeTimeString(weeks, "week", isInFuture);

        if (days > 0) return WrapToRelativeTimeString(days, "day", isInFuture);

        return "today";
    }

    private static string WrapToRelativeTimeString(int timeInterval, string noun, bool isInFuture)
    {
        string timeIntervalString = $"{noun}{(timeInterval == 1 ? null : "s")}";
        if (isInFuture)
        {
            return $"in {timeInterval} {timeIntervalString}";
        }

        return $"{timeInterval} {timeIntervalString} ago";
    }
}
