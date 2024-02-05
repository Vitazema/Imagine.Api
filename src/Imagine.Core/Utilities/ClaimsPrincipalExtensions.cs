using System.Security.Claims;

namespace Imagine.Core.Utilities;

public static class ClaimsPrincipalExtensions
{
    public static string GetUserNameFromPrincipal(this ClaimsPrincipal user)
    {
        return user.FindFirstValue(ClaimTypes.Name);
    }
}
