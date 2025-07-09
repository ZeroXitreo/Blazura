using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;

namespace Blazura;
public static class DisplayNameHelper
{
    public static string GetDisplayName<T>(Expression<Func<T>> For, Func<string, string>? format = null)
    {
        MemberExpression? expression = (MemberExpression)For.Body;
        DisplayAttribute? value = expression.Member.GetCustomAttribute<DisplayAttribute>();
        return format is not null ? format(value?.Name ?? expression.Member.Name ?? "") : value?.Name ?? expression.Member.Name ?? "";
    }
}
