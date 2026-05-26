using MassTransit;
using OrderFlowV2.Inventory.API.Repositories;
using OrderFlowV2.Shared.Events;

namespace OrderFlowV2.Inventory.API.Consumers
{
    public class PaymentFailedEventConsumer : IConsumer<PaymentFailedEvent>
    {
        private readonly IStockRepository _stockRepository;
        private readonly ILogger<PaymentFailedEventConsumer> _logger;
        public PaymentFailedEventConsumer(IStockRepository stockRepository, ILogger<PaymentFailedEventConsumer> logger)
        {
            _stockRepository = stockRepository;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<PaymentFailedEvent> context)
        {
            _logger.LogWarning("Ödeme başarısız! Stoklar geri iade ediliyor. Sipariş ID: {OrderId}", context.Message.OrderId);

            foreach (var item in context.Message.OrderItems)
            {
                // Veritabanından mevcut stoğu bul:
                var stock = await _stockRepository.GetStockByProductIdAsync(item.ProductId);

                if (stock != null)
                {

                    // Stoğu geri artır (Müşterinin ayırdığı miktar kadar geri ekliyoruz)
                    stock.Count += item.Count;
                    await _stockRepository.UpdateStockAsync(stock);

                    _logger.LogInformation("Ürün ID: {ProductId} stoğu {Count} adet artırıldı.", item.ProductId, item.Count);

                }
            }

            _logger.LogInformation("Tüm stoklar başarıyla iade edildi. Sipariş ID: {OrderId}", context.Message.OrderId);
        }
    }
}
