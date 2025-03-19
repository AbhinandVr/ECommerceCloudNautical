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

        public async Task<IEnumerable<OrderBind>> GetLatestOrderAsync(string email, string customerId)
        {
            using var connection = new SqlConnection(_connectionString);

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

            return await connection.QueryAsync<OrderBind>(query, new { Email = email, CustomerId = customerId });
        }
    }
}
