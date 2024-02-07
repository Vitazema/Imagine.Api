using Imagine.Core.Specifications;

namespace Imagine.Api.Services;

public class OrderService(IUnitOfWork unitOfWork, IMapper mapper) : IOrderService
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

    public async Task<OrderDto> GetOrderByIdAsync(Guid id)
    {
        var spec = new OrdersFullSpecification(id);
        var order = await unitOfWork.Repository<Order>().GetEntityWithSpec(spec);
        return mapper.Map<Order, OrderDto>(order);
    }

    public async Task<IReadOnlyList<OrderDto>> GetOrdersAsync(User user)
    {
        var spec = new OrdersFullSpecification(user);
        var orders = await unitOfWork.Repository<Order>().ListAsync(spec);

        return orders.Select(mapper.Map<Order, OrderDto>).ToList();
    }

    public async Task<Guid?> DeleteOrderAsync(Guid id)
    {
        var order = await unitOfWork.Repository<Order>().GetByIdAsync(id);
        unitOfWork.Repository<Order>().Delete(order);
        var result = await unitOfWork.Complete();
        return result <= 0 ? null : id;
    }
}
