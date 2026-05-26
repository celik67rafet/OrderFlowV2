using Microsoft.Extensions.Caching.Distributed;
using OrderFlowV2.Basket.API.Models;
using System.Text.Json;

namespace OrderFlowV2.Basket.API.Services
{
    public class BasketService : IBasketService
    {
        private readonly IDistributedCache _redis;
        public BasketService( IDistributedCache redis )
        {
            _redis = redis;
        }

        public async Task<bool> DeleteBasketAsync(string buyerId)
        {
            await _redis.RemoveAsync(buyerId);

            return true;
        }

        public async Task<CustomerBasket> GetBasketAsync(string buyerId)
        {
            var basket = await _redis.GetStringAsync(buyerId);
            if( string.IsNullOrEmpty(basket) ) return null;

            return JsonSerializer.Deserialize<CustomerBasket>(basket);
        }

        public async Task<CustomerBasket> UpdateBasketAsync(CustomerBasket basket)
        {
            // Nesneyi JSON string'e çevirip Redis'e kaydediyoruz:
            await _redis.SetStringAsync(basket.BuyerId, JsonSerializer.Serialize(basket));

            return await GetBasketAsync(basket.BuyerId);
        }
    }
}
