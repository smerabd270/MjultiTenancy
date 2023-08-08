using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MjultiTenancy.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet]
        public async Task<IActionResult> GetlAllProductAsync()
        {
            var products = await _productService.GetAllAsync();
            return Ok(products);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productService.GetById(id);
            return Ok(product);
        }
        [HttpPost]
        public async Task<IActionResult> CreateProductAsync( CreateProductDto productDto)
        {
            var product = new Product()
            {
                //test
                Description = productDto.Description,
                Name = productDto.Name,
                Rate = productDto.Rate
            };
          var createProduct= await  _productService.CreateAsync(product);
            return Ok(createProduct);

        }
    }
}
