using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderFlowV2.Shared.Events
{
    // Bu mesaj, Payment.API tarafından ödeme herhangi bir sebeple ( limit yetersizliği vb. ) başarısız olduğunda yayınlanacak.
    public record PaymentFailedEvent
    {
        public Guid OrderId { get; init; }
        public string Message { get; init; }
        public List<OrderItemMessage> OrderItems { get; init; }

    }
}
