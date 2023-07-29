using Imagine.Core.Entities;
using Imagine.Core.Entities.Identity;

namespace Imagine.Core.Interfaces;

public interface IPermissionRepository
{
    Task<UserPermission> GetPermissionsAsync(User user);
    Task<UserPermission> UpsertPermissionsAsync(UserPermission userUserPermission);
    Task<bool> DeletePermissionsAsync(string id);
    Task EditCredentialsAsync(string userName, int amount);
}
