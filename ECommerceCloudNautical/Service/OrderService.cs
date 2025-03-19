using ECommerceCloudNautical.Repositories;
using static ECommerceCloudNautical.Models.OrderModels;

namespace ECommerceCloudNautical.Service
{

        public class OrderService : IOrderService
        {
            private readonly IOrderRepository _orderRepository;

            public OrderService(IOrderRepository orderRepository)
            {
                _orderRepository = orderRepository;
            }

            public async Task<OrderResponse?> GetLatestOrderAsync(string email, string customerId)
            {
                return await _orderRepository.GetLatestOrderAsync(email, customerId);
            }
        }

    
}
