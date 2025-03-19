using Microsoft.AspNetCore.Mvc;
using ECommerceCloudNautical.Service;
using ECommerceCloudNautical.Models;
using static ECommerceCloudNautical.Models.OrderModels;

namespace ECommerceCloudNautical.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<ActionResult<OrderResponse>> GetLatestOrder([FromBody] OrderRequest request)
        {
            var result = await _orderService.GetLatestOrderAsync(request.User, request.CustomerId);

            if (result == null)
                return BadRequest(new { message = "Invalid customer ID or email." });

            return Ok(result);
        }
    }
}
