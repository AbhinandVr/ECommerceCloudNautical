using static ECommerceCloudNautical.Models.OrderModels;

namespace ECommerceCloudNautical.Service
{
    public interface IOrderService
    {
        Task<OrderResponse?> GetLatestOrderAsync(string email, string customerId);
    }
}
