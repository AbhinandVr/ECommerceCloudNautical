using static ECommerceCloudNautical.Models.OrderModels;

namespace ECommerceCloudNautical.Repositories
{
    public interface IOrderRepository
    {
        Task<OrderResponse?> GetLatestOrderAsync(string email, string customerId);
    }

}
