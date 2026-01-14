using Microsoft.AspNetCore.Mvc;
using ProductService.Application.Service;
using ProductService.Domain.Dto;
using ProductService.Domain.Service;

namespace ProductService.API.Controller
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductCommandService _productCommandService;
        private readonly IProductQueryService _productQueryService;

        public ProductsController(IProductCommandService productCommandService, IProductQueryService productQueryService)
        {
            _productCommandService = productCommandService;
            _productQueryService = productQueryService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProductRequest request)
        {
            var id = await _productCommandService.CreateAsync(request);

            return CreatedAtAction(nameof(GetById), new { id }, null);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var products = await _productQueryService.GetAllAsync();

            return Ok(products);
        }

        [HttpGet("{id}", Name = "GetById")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var products = await _productQueryService.GetByIdAsync(id);

            return Ok(products);
        }
    }
}
