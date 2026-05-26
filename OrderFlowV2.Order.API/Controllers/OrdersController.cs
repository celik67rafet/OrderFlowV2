using MassTransit; // RabbitMQ ile mesaj/event publish etmek için kullandığımız kütüphane. IPublishEndpoint buradan gelir.
using Microsoft.AspNetCore.Mvc; // Controller, ApiController, Route, HttpPost, IActionResult gibi web API özellikleri buradan gelir.
using OrderFlowV2.Order.API.Repositories; // Interface ve Repository lerini kullanmak için.
using OrderFlowV2.Shared.Enums; // Ortak Enumları kullanmak için.
using OrderFlowV2.Shared.Events; // Servisler arası gönderilen event/message modellerini kullanmak için.
using OrderModels = OrderFlowV2.Order.API.Models; // Api için gerekli modellerimiz, dto'lar

namespace OrderFlowV2.Order.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IPublishEndpoint _publishEndpoint; // Masstransit için...

        public OrdersController( IOrderRepository orderRepository, IPublishEndpoint publishEndpoint )
        {
            _orderRepository = orderRepository;
            _publishEndpoint = publishEndpoint;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(OrderModels.CreateOrderDto createOrderDto)
        {
            // Veritabanı için yeni bir order nesnesi yaratıyoruz:
            var newOrder = new OrderModels.Order
            {
                Id = Guid.NewGuid(), // ID biz oluşturuyoruz.
                BuyerId = createOrderDto.BuyerId,
                Status = OrderStatus.Pending, // İlk durum: beklemede...
                TotalPrice = createOrderDto.OrderItems.Sum(x => x.Price * x.Count),
                CreatedDate = DateTime.Now,
                Items = createOrderDto.OrderItems.Select(x => new OrderModels.OrderItem
                {
                    ProductId = x.ProductId,
                    Count = x.Count,
                    Price = x.Price
                }).ToList()
            };

            // 2. Veritabanına kaydediyoruz:
            var result = await _orderRepository.CreateOrderAsync(newOrder);

            if( result)
            {
                // 3. RABBITMQ'YA MESAJ GÖNDERİYORUZ ( Saga burada başlıyor )
                var orderCreatedEvent = new OrderCreatedEvent
                {
                    OrderId = newOrder.Id,
                    BuyerId = newOrder.BuyerId,
                    TotalPrice = newOrder.TotalPrice,
                    OrderItems = newOrder.Items.Select( x => new OrderItemMessage
                    {
                        ProductId = x.ProductId,
                        Count = x.Count
                    } ).ToList()
                };

                await _publishEndpoint.Publish(orderCreatedEvent);

                return Ok( new { OrderId = newOrder.Id, Message = "Sipariş alındı, işlemler başlatıldı." } );
            }

            return BadRequest( "Sipariş kaydedilirken bir hata oluştu." );
        }

        [HttpGet("user/{buyerId}")]
        public async Task<IActionResult> GetOrderByUser(string buyerId)
        {
            var orders = await _orderRepository.GetOrderByBuyerIdAsync(buyerId);

            return Ok(orders.Select(o => new
            {
                o.Id,
                o.TotalPrice,
                o.CreatedDate,
                Status = o.Status.ToString() // İnsanların anlayacağı yazı hali
            }));
        }

        // 2. Bir siparişin tüm detaylarını getir:
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderDetails( Guid id)
        {
            var order = await ((OrderRepository)_orderRepository).GetOrderDetailsAsync(id);

            if( order == null ) return NotFound();

            return Ok(order);
        }
        
    }
}
