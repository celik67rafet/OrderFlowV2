using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderFlowV2.Shared.Events
{
    
    // Bu mesaj kullanıcı sipariş verdiğinde Order.API tarafından yayınlanacak ve Inventory.API ( Stok ) tarafından dinlenecek.

    // Record kullanıyoruz çünkü taşınacak veriler immutable ( yani değiştirilemez ) olacak.
    public record OrderCreatedEvent
    {
        public Guid OrderId { get; init; } // Siparişin benzersiz id'si.
        public string BuyerId { get; init; } // Siparişi veren kullanıcı.
        public List<OrderItemMessage> OrderItems { get; init; } // Hangi üründen kaç tane alındığı ( Stok servisi buna bakarak düşüm yapacak ).
        public decimal TotalPrice { get; init; } // Toplam tutar ( Ödeme servisine lazım olan ).
    }

    public record OrderItemMessage
    {
        public int ProductId { get; init; }
        public int Count { get; init; }
        public string ProductName { get; init; }
    }


}
