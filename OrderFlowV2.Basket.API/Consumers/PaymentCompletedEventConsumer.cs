using MassTransit;
using OrderFlowV2.Basket.API.Services;
using OrderFlowV2.Shared.Events;

namespace OrderFlowV2.Basket.API.Consumers
{
    public class PaymentCompletedEventConsumer : IConsumer<PaymentCompletedEvent>
    {
        private readonly IBasketService _basketService;
        private readonly ILogger<PaymentCompletedEventConsumer> _logger; 
        public PaymentCompletedEventConsumer( IBasketService basketService, ILogger<PaymentCompletedEventConsumer> logger )
        {
            _basketService = basketService;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<PaymentCompletedEvent> context)
        {
            _logger.LogInformation("Ödeme tamamlandı haberi alındı. Sepet temizleniyor. Kullanıcı ID: {BuyerId}", context.Message.BuyerId);

            // Kullanıcının sepeti Redis'ten tamamen siliyoruz:
            await _basketService.DeleteBasketAsync(context.Message.BuyerId);

            _logger.LogInformation("Sepet başarıyla temizlendi.");
        }
    }
}
