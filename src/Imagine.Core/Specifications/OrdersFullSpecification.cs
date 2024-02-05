using Imagine.Core.Entities;
using Imagine.Core.Entities.Identity;

namespace Imagine.Core.Specifications;

public class OrdersFullSpecification : BaseSpecification<Order>
{
    public OrdersFullSpecification(User user) : base(o => o.User == user)
    {
        AddInclude(o => o.User);
        AddInclude(o => o.Subscription);
        AddOrderByDescending(o => o.CreatedAt);
    }

    public OrdersFullSpecification(Guid id) : base(o => o.Id == id)
    {
        AddInclude(o => o.Subscription);
    }

    public OrdersFullSpecification(Guid id, User user) : base(o => o.Id == id && o.User == user)
    {
        AddInclude(o => o.User);
        AddInclude(o => o.Subscription);
    }
}
