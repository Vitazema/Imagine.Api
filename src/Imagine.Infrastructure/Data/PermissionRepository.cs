using System.Text.Json;
using Imagine.Core.Contracts;
using Imagine.Core.Entities;
using StackExchange.Redis;

namespace Imagine.Infrastructure.Data;

public class PermissionRepository : IPermissionRepository
{
    private readonly IDatabase _database;
    private readonly TimeSpan _defaultExpirationTime = TimeSpan.FromDays(10);

    public PermissionRepository(IConnectionMultiplexer redis)
    {
        _database = redis.GetDatabase();
    }

    public async Task<UserPermissions> GetPermissionsAsync(string id)
    {
        var data = await _database.StringGetAsync(id);
        if (data.IsNullOrEmpty)
        {
            var permissions = new UserPermissions(id);
            permissions.Permissions.Add(new Permission()
            {
                Id = 0,
                Action = "any",
                Resource = "any",
                QueryLimit = -1,
                Credentials = -1
            });
            return permissions;
        }

        return JsonSerializer.Deserialize<UserPermissions>(data);
    }

    public async Task<UserPermissions> UpsertPermissionsAsync(UserPermissions userPermission)
    {
        var created = await _database.StringSetAsync(userPermission.Id,
            JsonSerializer.Serialize(userPermission), _defaultExpirationTime);
        return !created ? null : await GetPermissionsAsync(userPermission.Id);
    }

    public async Task<bool> DeletePermissionsAsync(string id)
    {
        return await _database.KeyDeleteAsync(id);
    }
}
