using OrderFlowV2.Shared.Enums;

namespace OrderFlowV2.Order.API.Models
{
    public class Order
    {
        public Guid Id { get; set; } // Sipariş No
        public string BuyerId { get; set; } // Müşteri No
        public decimal TotalPrice { get; set; } // Toplam Tutar
        public DateTime CreatedDate { get; set; } // Oluşturulma Tarihi
        public OrderStatus Status { get; set; } // Sipariş Durumu ( Shared'dan Geliyor )

        // Bir siparişin birden fazla kalemi olabilir:
        public List<OrderItem> Items { get; set; } = new();
    }

    public class OrderItem
    {
        public int Id { get; set; }
        public int ProductId { get; set; } // Ürün No
        public int Count { get; set; } // Adet
        public decimal Price { get; set; } // Birim Fiyat
        public Guid OrderId { get; set; } // Hangi siparişe ait olduğu
    }

}
