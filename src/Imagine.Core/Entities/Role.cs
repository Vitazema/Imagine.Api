using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;

namespace Imagine.Core.Entities;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Role
{
    System,
    Guest,
    Free,
    Trial,
    Paid
}

public class ApplicationRole : IdentityRole<Guid>
{
}
