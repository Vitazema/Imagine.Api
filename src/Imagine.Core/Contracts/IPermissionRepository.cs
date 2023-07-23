using Imagine.Core.Entities;

namespace Imagine.Core.Contracts;

public interface IPermissionRepository
{
    Task<Permission> GetPermissionsAsync(string userName);
    Task<Permission> UpsertPermissionsAsync(Permission userPermission);
    Task<bool> DeletePermissionsAsync(string id);
}
