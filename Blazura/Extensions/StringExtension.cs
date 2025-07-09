namespace Blazura.Extensions;

public static class StringExtension
{
    public static string ToAllCapital(this string str, string[]? ignoredWords = null)
    {
        char splitter = ' ';
        string[] words = str.Split(splitter);
        for (int i = 0; i < words.Length; i++)
        {
            if (ignoredWords is null || !ignoredWords.Contains(words[i]))
            {
                words[i] = words[i].ToCapital();
            }
        }
        return words.Join(splitter);
    }

    public static string ToCapital(this string str) => string.IsNullOrWhiteSpace(str) ? str : char.ToUpper(str[0]) + str[1..];

    public static string Join(this IEnumerable<string> str, char separator) => string.Join(separator, str);
}
