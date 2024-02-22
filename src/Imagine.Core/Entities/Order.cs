using System.Text.Json.Serialization;
using Imagine.Core.Entities.Identity;

namespace Imagine.Core.Entities;

public class Order : BaseEntity
{
    public Order()
    {
        
    }
    
    public Order(User user, decimal? subtotal)
    {
        User = user;
        Subtotal = subtotal;
    }

    public Guid UserId { get; set; }
    public User User { get; set; }
    public int? Credentials { get; set; }
    public Guid SubscriptionId { get; set; }
    public Subscription Subscription { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public OrderStatus Status { get; set; }
    public decimal? Subtotal { get; set; }
    public string PaymentIntentId { get; set; }

}
