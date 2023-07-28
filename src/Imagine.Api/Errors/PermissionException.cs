using System.Runtime.Serialization;

namespace Imagine.Api.Errors;

/// <inheritdoc />
/// <summary>Represents the permissions exception.</summary>
[Serializable]
public sealed class PermissionException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="T:IDP.Shared.Permissions.PermissionException" /> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public PermissionException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:IDP.Shared.Permissions.PermissionException" /> class.
    /// </summary>
    /// <param name="serializationInfo">The serialization information.</param>
    /// <param name="streamingContext">The streaming context.</param>
    private PermissionException(
        SerializationInfo serializationInfo,
        StreamingContext streamingContext)
        : base(serializationInfo, streamingContext)
    {
    }
}
