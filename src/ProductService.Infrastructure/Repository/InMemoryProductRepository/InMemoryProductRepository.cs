using ProductService.Domain.Entities;
using ProductService.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Infrastructure.Repository.InMemoryProductRepository
{
    public class InMemoryProductRepository : IProductRepository
    {
        private static readonly List<Product> _products =
        [
            new("Laptop", 1200),
            new("Mouse", 25)
        ];

        public Task AddAsync(Product product)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> GetAllAsync()
            => Task.FromResult(_products.AsEnumerable());

        public Task<Product?> GetByIdAsync(Guid id)
            => Task.FromResult(_products.FirstOrDefault(p => p.Id == id));

        public Task UpdateAsync(Product product)
        {
            throw new NotImplementedException();
        }
    }
}
