namespace Imagine.Api.Permissions;

public class AllowAllPermissionsChecker : IResourcePermissionChecker
{
    public Task<(bool, IEnumerable<string>)> GetResourcesIds(string action)
        => Task.FromResult((true, Enumerable.Empty<string>()));

    public bool HasPermission(string action, string resourceId) => true;

    public bool HasAllPermission(string action) => true;

    public bool HasAnyPermission(string action) => true;

    public Task<bool> HasOwnedPermission(string action, IReadOnlyCollection<string> resourceIds)
        => Task.FromResult(true);
}
