using Imagine.Core.Entities.Identity;
using Imagine.Infrastructure.Persistence;

namespace Imagine.Auth.Repository;

public class UserRepository : IUserRepository
{
    private readonly UserIdentityDbContext _userDbContext;

    public UserRepository(UserIdentityDbContext userDbContext)
    {
        _userDbContext = userDbContext;
    }
    public async Task<User?> GetUserAsync(string userName)
    {
        var user = _userDbContext.Users.FirstOrDefault(u => u.UserName == userName);
        return user;
    }

    public async Task<User?> GetUserByIdAsync(string userId)
    {
        var user = await _userDbContext.Users.FindAsync(userId);
        return user;
    }
}
