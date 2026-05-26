using MassTransit;
using OrderFlowV2.Order.API.Repositories;
using OrderFlowV2.Shared.Enums;
using OrderFlowV2.Shared.Events;

namespace OrderFlowV2.Order.API.Consumers
{
    public class OrderShippedEventConsumer : IConsumer<OrderShippedEvent>
    {
        private readonly IOrderRepository _orderRepository;

        private readonly ILogger<OrderShippedEventConsumer> _logger;
        public OrderShippedEventConsumer( IOrderRepository orderRepository, ILogger<OrderShippedEventConsumer> logger )
        {
            _orderRepository = orderRepository;
            _logger = logger;
        }
        public async Task Consume(ConsumeContext<OrderShippedEvent> context)
        {
            _logger.LogInformation("Kargo haberi alindi. Siparis durumu 'Shipped' yapiliyor. ID: {OrderId}", context.Message.OrderId);

            // Veritabanında durumu 6 yapıyoruz yani 'Shipped'
            await _orderRepository.UpdateOrderStatusAsync(context.Message.OrderId, OrderStatus.Shipped);
        }
    }
}
