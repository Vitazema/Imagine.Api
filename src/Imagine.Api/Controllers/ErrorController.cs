using Imagine.Core.Contracts.Errors;
using Microsoft.AspNetCore.Mvc;

namespace Imagine.Api.Controllers;

[Route("errors/{code}")]
[ApiExplorerSettings(IgnoreApi = true)]
public class ErrorController : BaseApiController
{
    public IActionResult Error(int code)
    {
        return new ObjectResult(new ApiResponse(code));
    }
}