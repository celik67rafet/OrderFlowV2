using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderFlowV2.Shared.Events
{
    // Bu bir hata 
    public record StockNotAvailableEvent
    {
        public Guid OrderId { get; init; }
        public string Message { get; init; }

        // Hangi ürün eksikse onların bilgisini de ekliyoruz:
        public List<FailedProductDetails> FailedProducts { get; init; }
    }

    public record FailedProductDetails
    {
        public int ProductId { get; init; }
        public string ProductName { get; init; }
    }
}
