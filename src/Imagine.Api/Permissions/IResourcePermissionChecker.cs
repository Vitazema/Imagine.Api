namespace Imagine.Api.Permissions;

public interface IResourcePermissionChecker
{
    Task<(bool, IEnumerable<string>)> GetResourcesIds(string action);

    bool HasPermission(string action, string resourceId);

    bool HasAllPermission(string action);

    bool HasAnyPermission(string action);

    Task<bool> HasOwnedPermission(
        string action,
        IReadOnlyCollection<string> resourceIds);
}