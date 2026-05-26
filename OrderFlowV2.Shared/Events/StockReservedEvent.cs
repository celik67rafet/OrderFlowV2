using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderFlowV2.Shared.Events
{

   // Bu mesaj Inventory.API tarafından stoklar başarıyla ayrıldığında yayınlanacak ve Payment.API tarafından dinlenecek.
   public record StockReservedEvent
    {
        public Guid OrderId { get; init; }
        public string BuyerId { get; init; }
        public decimal TotalPrice { get; init; }

        // OrderItemMessage'ı OrderCreatedEvent içinde tanımladığımız için ve Shared projesi içinde olduğu için oradan tanıdı.
        public List<OrderItemMessage> OrderItems { get; init; }
    }
}
