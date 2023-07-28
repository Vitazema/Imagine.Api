namespace Imagine.Core.Models;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public class Permission : Attribute
{
    /// <summary>Gets or sets the resource.</summary>
    /// <value>The resource.</value>
    public string Resource { get; set; }

    /// <summary>Gets or sets the actions.</summary>
    /// <value>The actions.</value>
    public string Action { get; set; }

    /// <summary>
    /// Gets or sets the name of the name of the resource identifier parameter.
    /// </summary>
    /// <value>The name of the resource identifier parameter.</value>
    public string ResourceIdParameterName { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether user context required for the operation.
    /// </summary>
    /// <value>
    ///   <c>true</c> if user context required; otherwise, <c>false</c>.
    /// </value>
    /// <remarks>
    /// The application API could be called from a system, not from user.
    /// Such calls doesn't contain information required for sending notifications to Auth service.
    /// So the system cannot call endpoints with <see cref="T:PermissionAttribute" /> with this flag set to <c>true</c>.
    /// </remarks>
    public bool RequireUserContext { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether user allowed to has access without permission checking.
    /// </summary>
    /// <remarks>
    /// Flag introduced to skip permission checking for an action, when the controller
    /// of the action already has <see cref="T:PermissionAttribute" />.
    /// </remarks>
    /// <value>
    ///   <c>true</c> if need to skip permission checking; otherwise, <c>false</c>.
    /// </value>
    public bool SkipPermissionCheck { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether user allowed to has access without resources checking.
    /// </summary>
    /// <remarks>
    /// Flag introduced to skip resources checking for an action, when the controller
    /// of the action already has <see cref="T:PermissionAttribute" />.
    /// </remarks>
    /// <value>
    ///   <c>true</c> if need to skip resources checking; otherwise, <c>false</c>.
    /// </value>
    public bool SkipResourcesCheck { get; set; }

    public Permission()
    {
    }

    public Permission(string action, string resourceIdParameterName)
        : this(null, action, resourceIdParameterName)
    {
    }

    public Permission(string action, string resourceIdParameterName, bool skipResourcesCheck)
        : this(null, action, resourceIdParameterName, skipResourcesCheck)
    {
    }

    public Permission(string resource, string action = null, string resourceIdParameterName = null, bool skipResourcesCheck = false)
    {
        this.Resource = resource;
        this.Action = action;
        this.ResourceIdParameterName = resourceIdParameterName;
        this.SkipResourcesCheck = skipResourcesCheck;
    }
}
