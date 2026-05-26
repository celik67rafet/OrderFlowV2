using MassTransit;
using OrderFlowV2.Order.API.Repositories;
using OrderFlowV2.Shared.Enums;
using OrderFlowV2.Shared.Events;

namespace OrderFlowV2.Order.API.Consumers
{
    public class StockNotAvailableEventConsumer : IConsumer<StockNotAvailableEvent>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<StockNotAvailableEvent> _logger;
        public StockNotAvailableEventConsumer( IOrderRepository orderRepository, ILogger<StockNotAvailableEvent> logger )
        {
            _orderRepository = orderRepository;
            _logger = logger;

        }

        public async Task Consume(ConsumeContext<StockNotAvailableEvent> context)
        {
            _logger.LogWarning("Stok yetersiz mesajı alındı. Sipariş iptal ediliyor. Sipariş ID: {OrderId}, Mesaj: {Message}",
                context.Message.OrderId, context.Message.Message
            );

            // Sipariş durumu 'Cancelled' (iptal) olarak güncelleniyor:
            await _orderRepository.UpdateOrderStatusAsync(context.Message.OrderId, OrderStatus.Cancelled);

            _logger.LogInformation("Sipariş stok yetersizliği nedeniyle iptal edildi. Sipariş ID: {OrderId}", context.Message.OrderId);
        }
    }
}
