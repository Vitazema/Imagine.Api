using System.Text.RegularExpressions;

namespace Imagine.Core.Utilities;

public static class Extensions
{
    public static bool IsValidEmailAddress(this string s)
    {
        var regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
        return regex.IsMatch(s);
    }
}
