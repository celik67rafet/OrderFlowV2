using MassTransit;
using OrderFlowV2.Order.API.Repositories;
using OrderFlowV2.Shared.Enums;
using OrderFlowV2.Shared.Events;

namespace OrderFlowV2.Order.API.Consumers
{
    public class PaymentFailedEventConsumer : IConsumer<PaymentFailedEvent>
    {

        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<PaymentFailedEventConsumer> _logger;

        public PaymentFailedEventConsumer(IOrderRepository orderRepository, ILogger<PaymentFailedEventConsumer> logger )
        {
            _orderRepository = orderRepository;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<PaymentFailedEvent> context)
        {
            _logger.LogWarning("Ödeme başarısız haberi alındı. Sipariş iptal ediliyor. Sipariş ID: {orderId}, Sebep: {Message}", context.Message.OrderId, context.Message.Message );

            // Veritabanındaki sipariş durumunu 'Cancelled' olarak güncelliyoruz
            await _orderRepository.UpdateOrderStatusAsync(context.Message.OrderId, OrderStatus.Cancelled);

            _logger.LogInformation("Sipariş başarıyla iptal durumuna getirildi. Sipariş ID: {OrderId}", context.Message.OrderId);
        }
    }
}
