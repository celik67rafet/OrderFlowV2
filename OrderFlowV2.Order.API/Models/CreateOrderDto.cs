namespace OrderFlowV2.Order.API.Models
{
    public record CreateOrderDto
    {
        public string BuyerId { get; init; }

        public List<CreateOrderItemDto> OrderItems { get; init; }
    }

    public record CreateOrderItemDto
    {
        public int ProductId { get; init; }
        public int Count { get; init; }
        public decimal Price { get; init; }
    }
}
