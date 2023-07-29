using System.Text.Json;
using Imagine.Core.Entities;
using Imagine.Core.Entities.Identity;
using Imagine.Core.Interfaces;
using StackExchange.Redis;
using Role = Imagine.Core.Entities.Role;

namespace Imagine.Api.Permissions;

public class PermissionRepository : IPermissionRepository
{
    private readonly IDatabase _database;
    private readonly TimeSpan _defaultExpirationTime = TimeSpan.FromDays(1);

    public PermissionRepository(IConnectionMultiplexer redis)
    {
        _database = redis.GetDatabase();
    }

    public async Task<UserPermission> GetPermissionsAsync(User user)
    {
        var userName = user?.UserName;
        if (userName == null) return null;
        
        var data = await _database.StringGetAsync(userName);
        return data.IsNullOrEmpty ? RenewUserPermissions(userName, user.Role) : JsonSerializer.Deserialize<UserPermission>(data);
    }

    public async Task<UserPermission> UpsertPermissionsAsync(UserPermission userUserPermission)
    {
        var created = await _database.StringSetAsync(userUserPermission.UserName,
            JsonSerializer.Serialize(userUserPermission), _defaultExpirationTime);
        var newPermission = await _database.StringGetAsync(userUserPermission.UserName);
        return !created ? null : JsonSerializer.Deserialize<UserPermission>(newPermission);
    }

    public async Task<bool> DeletePermissionsAsync(string id)
    {
        return await _database.KeyDeleteAsync(id);
    }
    
    public async Task EditCredentialsAsync(string userName, int amount)
    {
        var data = await _database.StringGetAsync(userName);
        if (!data.IsNullOrEmpty)
        {
            var permissions = JsonSerializer.Deserialize<UserPermission>(data);
            permissions.Credentials += amount;
            await UpsertPermissionsAsync(permissions);
        }
    }
    
    private UserPermission RenewUserPermissions(string userName, Role role)
    {
        return role switch
        {
            Role.System => new UserPermission()
            {
                UserName = userName,
                Action = "*any*",
                Resource = "*any*",
                QueryLimit = -1,
                Credentials = -1
            },
            Role.Guest => new UserPermission()
            {
                UserName = userName,
                Action = "read",
                Resource = "art",
                QueryLimit = 1,
                Credentials = 0
            },
            Role.Free => new UserPermission()
            {
                UserName = userName,
                Action = "*any*",
                Resource = "*any*",
                QueryLimit = 1,
                Credentials = 100
            },
            Role.Trial => new UserPermission()
            {
                UserName = userName,
                Action = "*any*",
                Resource = "*any*",
                QueryLimit = 3,
                Credentials = 5000
            },
            Role.Paid => new UserPermission()
            {
                UserName = userName,
                Action = "*any*",
                Resource = "*any*",
                QueryLimit = 3,
                Credentials = 5000
            },
            _ => throw new ArgumentOutOfRangeException(nameof(role), role, null)
        };
    }
}
