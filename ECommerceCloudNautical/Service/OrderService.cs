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
            var result = await _orderRepository.GetLatestOrderAsync(email, customerId);
            var orderResponse = result.FirstOrDefault();
            if (orderResponse == null)
            {
                throw new InvalidUserException("The customer does not exist");
            }
            if (orderResponse?.Email != email)
            {
                throw new InvalidUserException("Mail Id provided does not match with the customerid");
            }

            var customerDetails = new Customer
            {
                FirstName = orderResponse.FirstName,
                LastName = orderResponse.LastName
            };

            // Check if there are any valid orders guessing the orderid cannot be 0(null)
            var orderGroups = result.Where(r => r.OrderId != 0).GroupBy(r => r.OrderId);

            if (!orderGroups.Any())
            {
                // Return empty order list if no valid orders exist
                return new OrderResponse { Customer = customerDetails, Order = new List<Order>() };
            }

            var orders = orderGroups.Select(orderGroup => new Order
            {
                OrderNumber = orderGroup.Key,
                OrderDate = DateOnly.FromDateTime(orderGroup.First().OrderDate),
                DeliveryExpected = DateOnly.FromDateTime(orderGroup.First().DeliveryExpected),
                DeliveryAddress = orderGroup.First().DeliveryAddress,
                OrderItems = orderGroup.Select(r => new OrderItem
                {
                    Product = r.ProductName ?? "",
                    Quantity = r.Quantity,
                    PriceEach = r.Price
                }).ToList()
            }).ToList();

            return new OrderResponse { Customer = customerDetails, Order = orders };
        }

    }


}
