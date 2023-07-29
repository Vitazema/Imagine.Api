using Imagine.Auth.Repository;
using Imagine.Core.Interfaces;

namespace Imagine.Api.Permissions;

public class PermissionChecker : IPermissionChecker
{
    private readonly IPermissionRepository _permissionRepository;
    private readonly IUserRepository _userRepository;

    private readonly IResourcePermissionChecker _allowAllChecker = new AllowAllPermissionsChecker();

    public PermissionChecker(IPermissionRepository permissionRepository, IUserRepository userRepository)
    {
        _permissionRepository = permissionRepository;
        _userRepository = userRepository;
    }
    
    public Task<(bool, IEnumerable<string>)> GetResourcesIdsForAction(string resourceCategory, string action)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> HasPermission(string resourceCategory, string action, IReadOnlyCollection<string> resourceIds, bool skipResourcesCheck)
    {
        // todo: get user id from context (request header)
        
        var currentUser = await _userRepository.GetUserAsync("System");
        
        var userPermissions = await _permissionRepository.GetPermissionsAsync(currentUser);
        var resourcePermissionChecker = await this.GetChecker(userPermissions.Resource);
        
        var hasOwnedPermission = await resourcePermissionChecker.HasOwnedPermission(action, resourceIds);
        
        return resourceIds
            .Select(x => skipResourcesCheck
                ? resourcePermissionChecker.HasAnyPermission(action)
                : resourcePermissionChecker.HasPermission(action, x))
            .All(x => x) || hasOwnedPermission;
    }

    public async Task<bool> HasAllPermission(string resourceCategory, string action)
    {
        var resourcePermissionChecker = await this.GetChecker(resourceCategory);
        return resourcePermissionChecker.HasAllPermission(action);
    }

    public async Task<bool> HasAnyPermission(string resourceCategory, string action)
    {
        var resourcePermissionChecker = await this.GetChecker(resourceCategory);
        return resourcePermissionChecker.HasAnyPermission(action);
    }
    
    private async Task<IResourcePermissionChecker> GetChecker(string resourceCategory)
    {
        return _allowAllChecker;
    }
}
