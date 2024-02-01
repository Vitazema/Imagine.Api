namespace Imagine.Core.Contracts;

public record OrderCreationDto()
{
    public int? Credentials { get; set; }
    public int? SubscriptionMonths { get; set; }
}
