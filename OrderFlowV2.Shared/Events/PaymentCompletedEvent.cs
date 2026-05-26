using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderFlowV2.Shared.Events
{

    // Bu mesaj Payment.API tarafından ödeme başarılı olduğunda yayınlanacak ve hem Shipping.API ( Kargo ) hem de Order.API tarafından dinlenecek.
    public record PaymentCompletedEvent
    {
        public Guid OrderId { get; init; }
        public string BuyerId { get; init; }
    }
}
