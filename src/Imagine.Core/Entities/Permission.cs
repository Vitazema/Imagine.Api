namespace Imagine.Core.Entities;

public class Permission
{
    public int Id { get; set; }
    public string Action { get; set; }
    public string Resource { get; set; }
    public int QueryLimit { get; set; }
    public int Credentials { get; set; }
}
