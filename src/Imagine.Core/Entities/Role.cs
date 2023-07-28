using System.Text.Json.Serialization;

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
