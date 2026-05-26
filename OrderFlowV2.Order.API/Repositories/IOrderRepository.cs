using OrderFlowV2.Shared.Enums;

// Namespace üzerinden isim çakışması olduğu için using'i isimlendirdik ve class'a bu isim üzerinden ulaştık:
using OrderModels = OrderFlowV2.Order.API.Models;

namespace OrderFlowV2.Order.API.Repositories
{
    public interface IOrderRepository
    {
        Task<bool> CreateOrderAsync(OrderModels.Order order);

        Task<bool> UpdateOrderStatusAsync(Guid orderId, OrderStatus status);

        Task<IEnumerable<OrderModels.Order>> GetOrderByBuyerIdAsync( string buyerId );

        Task<OrderModels.Order> GetOrderDetailsAsync( Guid orderId );
    }
}

