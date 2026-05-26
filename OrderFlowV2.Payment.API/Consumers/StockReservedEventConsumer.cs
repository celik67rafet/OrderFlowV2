using MassTransit;
using OrderFlowV2.Shared.Events;

namespace OrderFlowV2.Payment.API.Consumers
{
    public class StockReservedEventConsumer : IConsumer<StockReservedEvent>
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<StockReservedEventConsumer> _logger;

        public StockReservedEventConsumer( IPublishEndpoint publishEndpoint, ILogger<StockReservedEventConsumer> logger )
        {
            _publishEndpoint = publishEndpoint;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<StockReservedEvent> context)
        {
            _logger.LogInformation("Ödeme işlemi başlatılıyor... Tutar: {TotalPrice}, Sipariş ID: {OrderId}", context.Message.TotalPrice, context.Message.OrderId);

            // --- ÖDEME SİMÜLASYONU ---
            // Burada basit bir kural koyalım: Eğer tutar 1000 TL'den büyükse ödeme bşaarısız olsun.
            // ( Böylece hem başarılı hem başarısız senaryoyu test edebiliriz )

            bool isPaymentSuccessful = context.Message.TotalPrice <= 1000;

            if( isPaymentSuccessful)
            {
                _logger.LogInformation("Ödeme başarılı. Sipariş ID: {OrderId}", context.Message.OrderId);

                // BAŞARILI EVENT'İ FIRLAT:

                await _publishEndpoint.Publish( new PaymentCompletedEvent
                {
                    OrderId = context.Message.OrderId,
                    BuyerId = context.Message.BuyerId

                } );
            }
            else
            {
                _logger.LogWarning("Ödeme başarısız! Bakiye yetersiz. Sipariş ID: {OrderId}", context.Message.OrderId);

                // BAŞARISIZ EVENT'I fırlat
                await _publishEndpoint.Publish( new PaymentFailedEvent
                {

                    OrderId = context.Message.OrderId,
                    Message = "Kredi kartı limiti yetersiz.",
                    OrderItems = context.Message.OrderItems // Stokların geri iadesi için bir liste şart!

                } );
            }
        }
    }
}
