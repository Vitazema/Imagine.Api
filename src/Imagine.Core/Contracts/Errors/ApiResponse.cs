namespace Imagine.Core.Contracts.Errors;

public class ApiResponse
{
    public ApiResponse(int statusCode, string message = null)
    {
        StatusCode = statusCode;
        Message = message ?? GetDefaultMessageForStatusCode(statusCode);
    }

    public int StatusCode { get; set; }
    public string Message { get; set; }
    
    private string GetDefaultMessageForStatusCode(int statusCode)
    {
        return statusCode switch
        {
            400 => "Bad request",
            401 => "Not authorized",
            404 => "Not found",
            500 => "Server error occured",
            _ => null
        };
    }
}
