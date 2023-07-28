namespace Imagine.Api.Permissions;

public interface IPermissionChecker
{
    /// <summary>
    /// Gets the resources ids for action.
    /// </summary>
    /// <param name="resourceCategory">The resource category.</param>
    /// <param name="action">The action.</param>
    /// <returns>
    /// Flag if user can see all resources, Owned by user model, List of resource ids that user can view.
    /// </returns>
    Task<(bool, IEnumerable<string>)> GetResourcesIdsForAction(string resourceCategory, string action);

    /// <summary>
    /// Determines whether user has access to the specified action.
    /// </summary>
    Task<bool> HasPermission(string resourceCategory, string action, IReadOnlyCollection<string> resourceId,
        bool skipResourcesCheck);

    /// <summary>
    /// Determines whether user has all access to the specified action.
    /// </summary>
    Task<bool> HasAllPermission(string resourceCategory, string action);

    /// <summary>
    /// Determines whether user has any permission for the action. It can be either *all* value or id value.
    /// </summary>
    Task<bool> HasAnyPermission(string resourceCategory, string action);
}
