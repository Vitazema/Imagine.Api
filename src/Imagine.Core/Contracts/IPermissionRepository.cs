using Imagine.Core.Entities;

namespace Imagine.Core.Contracts;

public interface IPermissionRepository
{
    Task<UserPermissions> GetPermissionsAsync(string id);
    Task<UserPermissions> UpsertPermissionsAsync(UserPermissions userPermission);
    Task<bool> DeletePermissionsAsync(string id);
}
