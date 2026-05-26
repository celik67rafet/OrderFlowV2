using Dapper;
using MySqlConnector;
using OrderFlowV2.Inventory.API.Models;

namespace OrderFlowV2.Inventory.API.Repositories
{
    public class StockRepository : IStockRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public StockRepository( IConfiguration configuration )
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Stock>> GetAllStockAsync()
        {
            using var connection = new MySqlConnection(_connectionString);

            var sql = "SELECT * FROM stocks";

            return await connection.QueryAsync<Stock>(sql);
        }

        public async Task<Stock> GetStockByProductIdAsync(int productId)
        {
            using var connection = new MySqlConnection(_connectionString);

            var sql = "SELECT * FROM stocks WHERE ProductId = @ProductId";

            return await connection.QueryFirstOrDefaultAsync<Stock>(sql, new { ProductId = productId });
        }

        public async Task<bool> UpdateStockAsync(Stock stock)
        {
            using var connection = new MySqlConnection(_connectionString);

            var sql = "UPDATE stocks SET Count = @Count WHERE ProductId = @ProductId";
            var result = await connection.ExecuteAsync(sql, new { stock.Count, stock.ProductId });

            return result > 0;
        }
    }
}
