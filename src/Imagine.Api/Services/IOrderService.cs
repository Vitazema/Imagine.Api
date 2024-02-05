using Imagine.Core.Contracts;
using Imagine.Core.Entities;
using Imagine.Core.Entities.Identity;

namespace Imagine.Api.Services;

public interface IOrderService
{
    Task<Order> CreateOrderAsync(User user, int? credentials, int? subscriptionMonths);
    Task<OrderDto> GetOrderByIdAsync(Guid id);
    Task<IReadOnlyList<OrderDto>> GetOrdersAsync(User user);
    Task<Guid?> DeleteOrderAsync(Guid id);
}
