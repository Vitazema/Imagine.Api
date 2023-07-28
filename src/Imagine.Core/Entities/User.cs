namespace Imagine.Core.Entities;

public class User : BaseEntity
{
    public string FullName { get; set; }
    public Role Role { get; set; }
}