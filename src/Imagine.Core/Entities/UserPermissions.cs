namespace Imagine.Core.Entities;

public class UserPermissions
{
    public string Id { get; set; }
    public List<Permission> Permissions { get; set; } = new List<Permission>();
    
    public UserPermissions()
    {
    }

    public UserPermissions(string id)
    {
        Id = id;
    }

}