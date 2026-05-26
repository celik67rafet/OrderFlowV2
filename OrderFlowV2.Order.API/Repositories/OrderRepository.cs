using MySqlConnector;
using Dapper;
using OrderFlowV2.Shared.Enums;
using OrderModels = OrderFlowV2.Order.API.Models;

namespace OrderFlowV2.Order.API.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        public OrderRepository( 
            
                IConfiguration configuration 
                
            )
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<bool> CreateOrderAsync(OrderModels.Order order)
        {
            using var connection = new MySqlConnection( _connectionString );

            await connection.OpenAsync();

            using var transaction = await connection.BeginTransactionAsync();

            try
            {
                // 1. Ana sipariş tablosunu ekleme:
                var orderSql = "INSERT INTO orders (id, buyerid, totalprice, createddate, status) VALUES (@Id, @BuyerId, @TotalPrice, @CreatedDate, @Status)";

                await connection.ExecuteAsync( orderSql, new { order.Id, order.BuyerId, order.TotalPrice, order.CreatedDate, Status = (int)order.Status }, transaction );

                // 2. Sipariş kalemlerini ( items ) Ekleme:
                var itemSql = "INSERT INTO orderitems ( productid, count, price, orderid ) VALUES ( @ProductId, @Count, @Price, @OrderId )";

                foreach( var item in order.Items)
                {
                    await connection.ExecuteAsync( itemSql, new { item.ProductId, item.Count, item.Price, OrderId = order.Id }, transaction );
                }

                await transaction.CommitAsync();

                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                return false;
            }

        }

        public async Task<bool> UpdateOrderStatusAsync( Guid orderId, OrderStatus status ) 
        {
            using var connection = new MySqlConnection(_connectionString);

            var sql = "UPDATE orders SET status = @Status WHERE Id = @Id";

            var result = await connection.ExecuteAsync( sql, new { Status = (int)status, Id = orderId } );

            return result > 0;
        }

        public async Task<IEnumerable<OrderModels.Order>> GetOrderByBuyerIdAsync(string buyerId)
        {
            using var connection = new MySqlConnection( _connectionString );

            var sql = "SELECT * FROM orders WHERE buyerid = @BuyerId ORDER BY createddate DESC";

            return await connection.QueryAsync<OrderModels.Order>(sql, new { BuyerId = buyerId });
        }

        public async Task<OrderModels.Order> GetOrderDetailsAsync(Guid orderId)
        {
            using var connection = new MySqlConnection(_connectionString);

            // JOIN kullanarak hem siparişi hem de içindeki ürünleri tek seferde çekiyoruz:
            var sql = @"SELECT * FROM orders WHERE id = @OrderId;
                        SELECT * FROM orderitems WHERE orderid = @OrderId;
            ";

            using ( var multi = await connection.QueryMultipleAsync(sql, new { orderId }))
            {
                var order = await multi.ReadFirstOrDefaultAsync<OrderModels.Order>();

                if( order != null)
                {
                    order.Items = (await multi.ReadAsync<OrderModels.OrderItem>()).ToList();
                }

                return order;
            }

        }
    }
}
