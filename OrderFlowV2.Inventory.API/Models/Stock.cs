namespace OrderFlowV2.Inventory.API.Models
{
    public class Stock
    {
        public int Id { get; set; }
        public int ProductId { get; set; } // Ürün ID
        public string ProductName { get; set; } // Ürün Adı
        public decimal Price { get; set; }
        public int Count { get; set; } // Stok Adedi
    }
}
