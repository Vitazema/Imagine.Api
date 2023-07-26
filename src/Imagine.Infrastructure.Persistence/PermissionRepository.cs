using System.Text.Json;
using Imagine.Core.Contracts;
using Imagine.Core.Entities;
using StackExchange.Redis;

namespace Imagine.Infrastructure.Persistence;

public class PermissionRepository : IPermissionRepository
{
    private readonly IDatabase _database;
    private readonly TimeSpan _defaultExpirationTime = TimeSpan.FromDays(10);

    public PermissionRepository(IConnectionMultiplexer redis)
    {
        _database = redis.GetDatabase();
    }

    public async Task<Permission> GetPermissionsAsync(string userName)
    {
        if (userName == null) return null;
        
        var data = await _database.StringGetAsync(userName);
        if (data.IsNullOrEmpty)
        {
            var permissions = new Permission()
            {
                UserName = userName,
                Action = "any",
                Resource = "any",
                QueryLimit = -1,
                Credentials = -1
            };
            return permissions;
        }
        

        return JsonSerializer.Deserialize<Permission>(data);
    }

    public async Task<Permission> UpsertPermissionsAsync(Permission userPermission)
    {
        var created = await _database.StringSetAsync(userPermission.UserName,
            JsonSerializer.Serialize(userPermission), _defaultExpirationTime);
        return !created ? null : await GetPermissionsAsync(userPermission.UserName);
    }

    public async Task<bool> DeletePermissionsAsync(string id)
    {
        return await _database.KeyDeleteAsync(id);
    }
}
