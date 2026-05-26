using MassTransit;
using OrderFlowV2.Order.API.Repositories;
using OrderFlowV2.Shared.Enums;
using OrderFlowV2.Shared.Events;

namespace OrderFlowV2.Order.API.Consumers
{
    public class PaymentCompletedEventConsumer : IConsumer<PaymentCompletedEvent>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<PaymentCompletedEventConsumer> _logger;

        public PaymentCompletedEventConsumer( IOrderRepository orderRepository, ILogger<PaymentCompletedEventConsumer> logger )
        {
            _orderRepository = orderRepository;
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<PaymentCompletedEvent> context)
        {
            _logger.LogInformation("Ödeme başarı haberi alındı. Sipariş durumu 'Paid' yapılıyor. ID: {OrderId}", context.Message.OrderId);

            // Veritabanı gidip statusu 3 (Paid) yapıyoruz...
            await _orderRepository.UpdateOrderStatusAsync(context.Message.OrderId, OrderStatus.Paid);

            _logger.LogInformation("Sipariş veritabanında başarıyla güncellendi.");
        }
    }
}
