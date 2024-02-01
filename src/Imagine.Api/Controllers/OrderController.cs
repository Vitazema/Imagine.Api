using Imagine.Api.Services;
using Imagine.Auth.Repository;
using Imagine.Core.Contracts;
using Imagine.Core.Contracts.Errors;
using Imagine.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Imagine.Api.Controllers;

[Authorize]
public class OrderController (IOrderService orderService, IUserRepository userRepository): BaseApiController
{
     [HttpPost]
     public async Task<ActionResult<bool>> CreateOrderAsync([FromBody] OrderCreationDto orderCreation)
     {
         var user = await userRepository.GetCurrentUserAsync(User);
         var order = await orderService.CreateOrderAsync(user, orderCreation.Credentials, orderCreation.SubscriptionMonths);
         
         if (order == null) return BadRequest(new ApiResponse(400, "Problem creating order"));
         return Ok(true);
     }
}
