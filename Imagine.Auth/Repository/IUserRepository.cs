using Imagine.Core.Entities;

namespace Imagine.Auth.Repository;

public interface IUserRepository
{
    Task<User?> GetUserAsync(string userId);
}
