namespace Imagine.Core.Entities;

public class UserPermission
{
    public string UserName { get; set; }
    public string Action { get; set; }
    public string Resource { get; set; }
    public int QueryLimit { get; set; }
    public int Credentials { get; set; }
    public int TimeToLive { get; set; }
}
