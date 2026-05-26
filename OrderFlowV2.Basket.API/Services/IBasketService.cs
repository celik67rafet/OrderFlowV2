using OrderFlowV2.Basket.API.Models;

namespace OrderFlowV2.Basket.API.Services
{
    public interface IBasketService
    {
        Task<CustomerBasket> GetBasketAsync(string buyerId);
        Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket);
        Task<bool> DeleteBasketAsync(string buyerId);
    }
}
