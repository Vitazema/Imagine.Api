using Imagine.Core.Entities.Identity;

namespace Imagine.Core.Interfaces;

public interface ITokenService
{
    string CreateToken(User user);
}
