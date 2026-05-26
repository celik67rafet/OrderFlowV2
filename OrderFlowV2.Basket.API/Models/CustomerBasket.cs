namespace OrderFlowV2.Basket.API.Models
{
    public class CustomerBasket
    {
        public string BuyerId { get; set; } // Sepetin sahibi olan kullanıcının ID'si
        public List<BasketItem> Items { get; set; } // Sepetteki ürün listesi
    }

    public class BasketItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
    }
}
