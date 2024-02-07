using System.Security.Claims;
using Imagine.Api.Services;
using Imagine.Auth.Repository;
using Imagine.Core.Utilities;
using Microsoft.AspNetCore.Authorization;

namespace Imagine.Api.Controllers;

[Authorize]
public class OrderController(IOrderService orderService, IUserRepository userRepository) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Order>>> GetOrdersForUser()
    {
        var user = await userRepository.GetUserAsync(User.GetUserNameFromPrincipal());
        var orders = await orderService.GetOrdersAsync(user);
        return Ok(orders);
    }
    
    [HttpGet("{id:Guid}")]
    public async Task<ActionResult<IReadOnlyList<Order>>> GetOrderById(Guid id)
    {
        var order = await orderService.GetOrderByIdAsync(id);
        return Ok(order);
    }
    
    [HttpPost]
    public async Task<ActionResult<bool>> CreateOrderAsync([FromBody] OrderCreationDto orderCreation)
    {
        var user = await userRepository.GetCurrentUserAsync(User);
        var order = await orderService.CreateOrderAsync(user, orderCreation.Credentials,
            orderCreation.SubscriptionMonths);

        if (order == null) return BadRequest(new ApiResponse(400, "Problem creating order"));
        return Ok(true);
    }
}
