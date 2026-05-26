using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderFlowV2.Inventory.API.Repositories;

namespace OrderFlowV2.Inventory.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StocksController : ControllerBase
    {
        private readonly IStockRepository _stockRepository;

        public StocksController( IStockRepository stockRepository )
        {
            _stockRepository = stockRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var stocks = await _stockRepository.GetAllStockAsync();

            return Ok(stocks);
        }
    }
}
