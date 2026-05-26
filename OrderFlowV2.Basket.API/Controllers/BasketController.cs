using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderFlowV2.Basket.API.Models;
using OrderFlowV2.Basket.API.Services;

namespace OrderFlowV2.Basket.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IBasketService _basketService;

        public BasketController( IBasketService basketService )
        {
            _basketService = basketService;
        }

        // Sepeti getir (Kullanıcı ID'sine göre)
        [HttpGet("{buyerId}")]
        public async Task<IActionResult> GetBasket(string buyerId)
        {
            var basket = await _basketService.GetBasketAsync(buyerId);
            return Ok( basket ?? new CustomerBasket { BuyerId = buyerId } );
        }

        // Sepeti güncelle veya yeni ürün ekle
        [HttpPost]
        public async Task<IActionResult> UpdateBasket( CustomerBasket basket)
        {
            var updateBasket = await _basketService.UpdateBasketAsync(basket);

            return Ok(updateBasket);
        }

        // Sepeti Sil: 
        [HttpDelete("{buyerId}")]
        public async Task<IActionResult> DeleteBasket(string buyerId)
        {
            await _basketService.DeleteBasketAsync(buyerId);
            return Ok();
        }
    }
}
