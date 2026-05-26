using MassTransit;
using OrderFlowV2.Shared.Events;

namespace OrderFlowV2.Notification.API.Consumers
{
    public class GlobalEventConsumer : IConsumer<OrderCreatedEvent>, IConsumer<PaymentCompletedEvent>, IConsumer<PaymentFailedEvent>, IConsumer<StockNotAvailableEvent>
    {

        private readonly ILogger<GlobalEventConsumer> _logger;
        public GlobalEventConsumer( ILogger<GlobalEventConsumer> logger )
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            _logger.LogInformation("--- BİLDİRİM: Sayın Müşteri, {OrderId} nolu siparişiniz alındı. ---", context.Message.OrderId);
        }

        public async Task Consume(ConsumeContext<PaymentFailedEvent> context)
        {
            _logger.LogError("--- BİLDİRİM: Ödemeniz başarısız oldu. Sipariş iptal edildi. Sebep: {Reason} ---", context.Message.Message);
        }

        public async Task Consume(ConsumeContext<PaymentCompletedEvent> context)
        {
            _logger.LogInformation("--- BİLDİRİM: Ödemeniz onaylandı, kargonuz hazırlanıyor! ---");
        }

        public async Task Consume(ConsumeContext<StockNotAvailableEvent> context)
        {
            _logger.LogWarning("--- BİLDİRİM: Üzgünüz, stok yetersizliği nedeniyle siparişiniz iptal edildi. ---");
        }
    }
}
