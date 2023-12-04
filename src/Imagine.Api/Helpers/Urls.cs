namespace Imagine.Api.Helpers;

public static class Urls
{
    private static readonly Lazy<string> GetBaseUrl = new(GetCurrentBaseUrl);
    public static string BaseUrl => GetBaseUrl.Value;
    private static string GetCurrentBaseUrl()
    {
        var environmentUrl = Environment.GetEnvironmentVariable("ASPNETCORE_URLS")
            ?.Split(";")
            .FirstOrDefault()
            ?.Replace("+", "localhost");
        
        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production")
            environmentUrl = Environment.GetEnvironmentVariable("API_WAN_URL");
        
        if (string.IsNullOrEmpty(environmentUrl)) throw new Exception("Can't find application base URL");

        return environmentUrl;
    }
}
