namespace Imagine.Core.Entities;

public class User : BaseEntity
{
    public string FullName { get; set; }
    public List<Art> Arts { get; set; }
}