using Imagine.Core.Entities;
using Imagine.Core.Entities.Identity;

namespace Imagine.Api.Services;

public interface IOrderService
{
    Task<Order> CreateOrderAsync(User user, int? credentials, int? subscriptionMonths);
    Task<Order> GetOrderByIdAsync(Guid id);
    Task<IReadOnlyList<Order>> GetOrdersAsync(User user);
    Task DeleteOrderAsync(Guid id);
}
