using MassTransit;
using OrderFlowV2.Shared.Events;

namespace OrderFlowV2.Shipping.API.Consumers
{
    public class PaymentCompletedEventConsumer : IConsumer<PaymentCompletedEvent>
    {
        private readonly ILogger<PaymentCompletedEventConsumer> _logger;
        public PaymentCompletedEventConsumer( ILogger<PaymentCompletedEventConsumer> logger )
        {
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<PaymentCompletedEvent> context)
        {
            // Gerçek dünyada burada kargo firmasının API'sine istek atılır.
            // Biz sadece log basarak süreci tamamlıyoruz.

            _logger.LogInformation("Ödeme onayı alındı. Kargo hazırlık süreci başlatılıyor... Sipariş ID: {OrderID}", context.Message.OrderId);

            _logger.LogInformation("Kargo başarıyla oluşturuldu. Müşteri ID: {BuyerId}", context.Message.BuyerId);

            await context.Publish( new OrderShippedEvent { OrderId = context.Message.OrderId } );

            await Task.CompletedTask;
        }
    }
}
