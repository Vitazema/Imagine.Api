using Imagine.Core.Contracts;
using Imagine.Core.Entities;

namespace Imagine.Api.Services;

public interface IAiApiService
{
    Task<SdResponse> RequestAsync(Art art, CancellationToken token);
}
