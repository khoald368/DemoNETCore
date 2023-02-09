using API.Models;
using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ProductsController : BaseController
    {
        private readonly IProductService productService;

        public ProductsController(IProductService productService)
        {
            this.productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var products = await productService.GetAllAsync();

            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var product = await productService.GetByIdAsync(id);

            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] Product product)
        {
            var result = await productService.AddAsync(product);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await productService.DeleteAsync(id);

            return Ok();
        }

        [HttpGet("Filter")]
        public async Task<IActionResult> GetAllAsync([FromQuery] string name, [FromQuery] decimal price)
        {
            return Ok(await productService.GetProductsAsync(name, price));
        }
    }
}
