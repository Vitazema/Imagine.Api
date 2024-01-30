using Imagine.Api.Permissions;
using Imagine.Core.Contracts.Errors;
using Imagine.Core.Models;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Imagine.Api.Mvc;

public class PermissionsCheckServiceFilter : IAsyncActionFilter
{
    private readonly Lazy<IPermissionChecker> _permissionChecker;

    public PermissionsCheckServiceFilter(
        Lazy<IPermissionChecker> permissionChecker)
    {
        _permissionChecker = permissionChecker;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var actionPermissions = context.GetActionPermissions();

        var requireUserContext = actionPermissions.Any(x => x.RequireUserContext);
        // if (this.contextProvider.Value.IsSystemContext() && requireUserContext)
        // {
        //     throw new PermissionException(
        //         "This action require Application Name and User Name passed in HTTP headers or token.");
        // }

        if (actionPermissions.Count == 0)
        {
            await next();
        }

        var hasPermission = await this.HasAnyPermission(actionPermissions, context);
        if (hasPermission)
        {
            await next();
        }
        else
        {
            throw new PermissionException("User doesn't have access to this resource.");
        }
        
    }

    private async Task<bool> HasAnyPermission(
        IEnumerable<Permission> permissions,
        ActionExecutingContext context)
    {
        var tasks = permissions.Select(x => this.HasPermission(x, context)).ToList();
        await Task.WhenAll(tasks);

        return tasks.All(x => x.Result);
    }

    private async Task<bool> HasPermission(Permission permission, ActionExecutingContext context)
    {
        if (permission.SkipResourcesCheck)
            return true;

        if (permission.ResourceIdParameterName == null)
            return await this._permissionChecker.Value.HasAnyPermission(permission.Resource, permission.Action);

        var resourceSystemNames = permission.ResourceIdParameterName;

        var hasAll = await this._permissionChecker.Value.HasAllPermission(permission.Resource, permission.Action);

        var hasResourcePermission = await this._permissionChecker.Value.HasPermission(permission.Resource,
            permission.Action, new List<string>() { resourceSystemNames }, permission.SkipResourcesCheck);

        return hasAll || hasResourcePermission;
    }
}

public static class PermissionsCollector
{
    public static List<Permission> GetActionPermissions(this ActionExecutingContext context)
    {
        var actionPermissions = context.ActionDescriptor.EndpointMetadata
            .OfType<Permission>()
            .ToList();

        return actionPermissions;
    }
}
