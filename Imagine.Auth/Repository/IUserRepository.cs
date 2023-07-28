using Imagine.Core.Entities.Identity;

namespace Imagine.Auth.Repository;

public interface IUserRepository
{
    Task<User?> GetUserAsync(string userName);
    Task<User?> GetUserByIdAsync(string userId);
}
