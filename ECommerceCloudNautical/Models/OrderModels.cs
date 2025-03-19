namespace ECommerceCloudNautical.Models
{
    public class OrderModels
    {
        public class OrderRequest
        {
            public string User { get; set; }
            public string CustomerId { get; set; }
        }

        public class OrderResponse
        {
            public Customer Customer { get; set; }
            public List<Order> Order { get; set; }
        }

        public class Customer
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }

        public class Order
        {
            public int? OrderNumber { get; set; }
            public DateTime? OrderDate { get; set; }
            public string DeliveryAddress { get; set; }
            public List<OrderItem> OrderItems { get; set; } = new();
            public DateTime? DeliveryExpected { get; set; }
        }

        public class OrderItem
        {
            public string Product { get; set; }
            public int? Quantity { get; set; }
            public decimal? PriceEach { get; set; }
        }

        public class OrderBind
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public int OrderId { get; set; }
            public DateTime OrderDate { get; set; }
            public DateTime DeliveryExpected { get; set; }
            public bool ContainsGift { get; set; }
            public string DeliveryAddress { get; set; }
            public string ProductName { get; set; }
            public int Quantity { get; set; }
            public decimal Price { get; set; }

        }

    }
}
