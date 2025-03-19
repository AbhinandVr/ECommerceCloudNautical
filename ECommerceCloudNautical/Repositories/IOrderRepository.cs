using static ECommerceCloudNautical.Models.OrderModels;

namespace ECommerceCloudNautical.Repositories
{
    public interface IOrderRepository
    {
        Task<IEnumerable<OrderBind>> GetLatestOrderAsync(string email, string customerId);        
    }

}
