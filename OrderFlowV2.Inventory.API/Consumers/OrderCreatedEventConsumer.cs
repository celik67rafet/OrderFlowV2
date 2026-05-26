using MassTransit;
using OrderFlowV2.Inventory.API.Repositories;
using OrderFlowV2.Shared.Events;

namespace OrderFlowV2.Inventory.API.Consumers
{
    public class OrderCreatedEventConsumer : IConsumer<OrderCreatedEvent>
    {

        private readonly IStockRepository _stockRepository;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ILogger<OrderCreatedEventConsumer> _logger;

        public OrderCreatedEventConsumer( IStockRepository stockRepository, IPublishEndpoint publishEndpoint, ILogger<OrderCreatedEventConsumer> logger )
        {
            _stockRepository = stockRepository;
            _publishEndpoint = publishEndpoint;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            _logger.LogInformation("Sipariş alındı, stok kontrolü yapılıyor. Sipariş ID: {OrderId}", context.Message.OrderId);

            var stockResults = new List<bool>();

            // 1.Siparişteki her bir ürün için stok kontrolü yap:
            foreach( var item in context.Message.OrderItems)
            {
                var stock = await _stockRepository.GetStockByProductIdAsync(item.ProductId);

                if( stock != null && stock.Count >= item.Count)
                {
                    stockResults.Add(true);
                }
                else
                {
                    stockResults.Add(false);
                }
            }

            //( 2. Eğer TÜM ürünlerin stoğu yeterliyse
            if (stockResults.All(x => x))
            {

                foreach (var item in context.Message.OrderItems)
                {

                    var stock = await _stockRepository.GetStockByProductIdAsync(item.ProductId);
                    stock.Count -= item.Count; // Stoktan düş
                    await _stockRepository.UpdateStockAsync(stock);

                }

                _logger.LogInformation("Stoklar başarıyla rezerve edildi. Sipariş ID: {OrderID}", context.Message.OrderId);

                // BİR SONRAKİ ADIMI TETİKLE ( Ödeme Servisi İçin )
                await _publishEndpoint.Publish(new StockReservedEvent
                {
                    OrderId = context.Message.OrderId,
                    BuyerId = context.Message.BuyerId,
                    TotalPrice = context.Message.TotalPrice,
                    OrderItems = context.Message.OrderItems
                });

            }
            else
            {
                var failedProducts = new List<FailedProductDetails>();

                foreach ( var item in context.Message.OrderItems)
                {
                    var stock = await _stockRepository.GetStockByProductIdAsync(item.ProductId);

                    if( stock == null || stock.Count < item.Count)
                    {
                        failedProducts.Add(
                        
                            new FailedProductDetails
                            {
                                ProductId = item.ProductId,
                                ProductName = item.ProductName
                            }

                        );
                    }
                }

                // 3. Stok yetersizse HATA EVENT'I fırlat:
                _logger.LogWarning("Stok yetersiz! Sipariş iptal edilecek. Sipariş ID: {OrderId}", context.Message.OrderId);

                await _publishEndpoint.Publish(new StockNotAvailableEvent
                {

                    OrderId = context.Message.OrderId,
                    Message = $"Şu ürünlerin stoğu yetersiz, tekrar sipariş verin: {string.Join(", ", failedProducts.Select( x => $"{x.ProductName} (Id: {x.ProductId})"))}",
                    FailedProducts = failedProducts

                });
            }
            

        }
    }
}
