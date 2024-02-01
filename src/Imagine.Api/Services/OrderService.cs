using Imagine.Core.Entities;
using Imagine.Core.Entities.Identity;
using Imagine.Core.Interfaces;

namespace Imagine.Api.Services;

public class OrderService(IUnitOfWork unitOfWork, IRepository<Order> orderRepository) : IOrderService
{
    public async Task<Order> CreateOrderAsync(User user, int? credentials, int? subscriptionMonths)
    {
        decimal? subTotal = 0;

        subTotal += subscriptionMonths == null ? 0 : subscriptionMonths * 5;
        subTotal += credentials == null ? 0 : credentials * 0.01M;
        if (subTotal == 0) return null;
        var order = new Order(user, subTotal)
        {
            Credentials = credentials
        };

        if (subscriptionMonths != null)
        {
            order.Subscription = new Subscription()
            {
                User = user,
                ExpiresAt = DateTime.Now.AddMonths(subscriptionMonths.Value)
            };
        }

        unitOfWork.Repository<Order>().Add(order);
        var result = await unitOfWork.Complete();
        return result <= 0 ? null : order;
    }

    public Task<Order> GetOrderByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IReadOnlyList<Order>> GetOrdersAsync(User user)
    {
        throw new NotImplementedException();
    }

    public Task DeleteOrderAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}
