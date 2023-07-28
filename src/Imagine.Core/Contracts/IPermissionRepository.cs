using Imagine.Core.Entities;

namespace Imagine.Core.Contracts;

public interface IPermissionRepository
{
    Task<UserPermission> GetPermissionsAsync(User user);
    Task<UserPermission> UpsertPermissionsAsync(UserPermission userUserPermission);
    Task<bool> DeletePermissionsAsync(string id);
    Task EditCredentialsAsync(string userName, int amount);
}
