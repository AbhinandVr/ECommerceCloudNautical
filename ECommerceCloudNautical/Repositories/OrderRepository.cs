using Dapper;
using Microsoft.Data.SqlClient;
using ECommerceCloudNautical.Models;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using ECommerceCloudNautical.Repositories;
using static ECommerceCloudNautical.Models.OrderModels;

namespace CloudnaOrderApi.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly string _connectionString;

        public OrderRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<OrderResponse?> GetLatestOrderAsync(string email, string customerId)
        {
            using var connection = new SqlConnection(_connectionString);

            // Single query to get customer, latest order, and order items in one go
            var query = @"
                SELECT 
                    C.FIRSTNAME, 
                    C.LASTNAME, 
                    C.EMAIL,
                    O.ORDERID, 
                    O.ORDERDATE, 
                    O.DELIVERYEXPECTED, 
                    O.CONTAINSGIFT,
                    C.HOUSENO + ' ' + C.STREET + ', ' + C.TOWN + ', ' + C.POSTCODE AS DELIVERYADDRESS,
                    CASE 
                        WHEN O.CONTAINSGIFT = 1 THEN 'Gift' 
                        ELSE P.PRODUCTNAME 
                    END AS PRODUCTNAME,
                    OI.QUANTITY, 
                    OI.PRICE
                FROM 
                    CUSTOMERS C
                LEFT JOIN 
                    ORDERS O ON C.CUSTOMERID = O.CUSTOMERID
                LEFT JOIN 
                    ORDERITEMS OI ON O.ORDERID = OI.ORDERID
                LEFT JOIN 
                    PRODUCTS P ON OI.PRODUCTID = P.PRODUCTID
                WHERE 
                    C.CUSTOMERID = @CustomerId
                ORDER BY 
                    O.ORDERDATE DESC;";

            var result = await connection.QueryAsync<dynamic>(query, new { Email = email, CustomerId = customerId });

            var orderResponse = result.FirstOrDefault();
            if (orderResponse?.EMAIL != email)
            {
                throw new InvalidUserException("Invalid email or customer ID.");
            }

            if (orderResponse == null)
                return null;  // Invalid customer or no orders found

            var customer = new Customer
            {
                FirstName = orderResponse.FIRSTNAME,
                LastName = orderResponse.LASTNAME
            };

            var order = new Order
            {
                OrderNumber = orderResponse.ORDERID,
                OrderDate = orderResponse.ORDERDATE,
                DeliveryExpected = orderResponse.DELIVERYEXPECTED,
                DeliveryAddress = orderResponse.DELIVERYADDRESS,
                OrderItems = new List<OrderItem>()
            };

            // Use foreach to process all order items
            foreach (var row in result)
            {
                var orderItem = new OrderItem
                {
                    Product = row.PRODUCTNAME,
                    Quantity = row.QUANTITY,
                    PriceEach = row.PRICE
                };

                // Add order item to the order
                order.OrderItems.Add(orderItem);
            }

            return new OrderResponse { Customer = customer, Order = order };
        }
    }
}
