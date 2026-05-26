using OrderFlowV2.Inventory.API.Models;

namespace OrderFlowV2.Inventory.API.Repositories
{
    public interface IStockRepository
    {
        // Bir ürünün stok bilgisini getir ( Ürün ID'ye göre )
        Task<Stock> GetStockByProductIdAsync(int productId);

        // Tüm stokları getirmek için:
        Task<IEnumerable<Stock>> GetAllStockAsync(); 

        // Stok miktarını güncelle ( Düşürme veya arttırma için )
        Task<bool> UpdateStockAsync(Stock stock);
    }
}
