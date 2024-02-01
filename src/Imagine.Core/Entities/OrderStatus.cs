using System.Runtime.Serialization;

namespace Imagine.Core.Entities;

public enum OrderStatus
{
    Pending,
    PaymentReceived,
    PaymentFailed
}
