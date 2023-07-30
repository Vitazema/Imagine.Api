using Imagine.Core.Contracts;
using Imagine.Core.Entities;

namespace Imagine.Api.Services;

public interface IAiService
{
    Task GenerateAsync(CancellationToken token, Art art);
}