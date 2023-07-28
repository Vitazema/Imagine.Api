using Imagine.Core.Entities;
using Imagine.Infrastructure.Persistence;

namespace Imagine.Auth.Repository;

public class UserRepository : IUserRepository
{
    private readonly ArtDbContext _dbContext;

    public UserRepository(ArtDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<User?> GetUserAsync(string userId)
    {
        var isParsed = int.TryParse(userId, out var intId);
        if (!isParsed) throw new ArgumentException("Id must be an integer", nameof(userId));
        var user = await _dbContext.Users.FindAsync(intId);
        return user;
    }
}
