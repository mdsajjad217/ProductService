using ProductService.Domain.Dto;
using ProductService.Domain.Repository;
using ProductService.Domain.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Service
{
    public class ProductQueryService : IProductQueryService
    {
        private readonly IProductRepository _repository;

        public ProductQueryService(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            var products = await _repository.GetAllAsync();
            return products.Select(p => new ProductDto(p.Id, p.Name, p.Price));
        }

        public async Task<ProductDto> GetByIdAsync(Guid id)
        {
            var product = await _repository.GetByIdAsync(id);

            if (product == null)
                return null;

            return new ProductDto(product.Id, product.Name, product.Price);
        }
    }
}
