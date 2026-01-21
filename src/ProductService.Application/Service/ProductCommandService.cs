using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Dto;
using ProductService.Domain.Entities;
using ProductService.Domain.Event;
using ProductService.Domain.Repository;
using ProductService.Domain.Service;
using ProductService.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Application.Service
{
    public class ProductCommandService : IProductCommandService
    {
        private readonly IProductRepository _repository;
        private readonly ProductDbContext _context;

        public ProductCommandService(IProductRepository repository, ProductDbContext productDbContext)
        {
            _repository = repository;
            _context = productDbContext;
        }

        public async Task<Guid> CreateAsync(CreateProductRequest request)
        {
            var product = new Product(request.Name, request.Price);
            await _repository.AddAsync(product);
            return product.Id;
        }

        public async Task UpdateAsync(Guid id, UpdateProductRequest request)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null)
                throw new KeyNotFoundException("Product not found");

            product.UpdatePrice(request.Price);
            product.UpdateName(request.Name);
            await _repository.UpdateAsync(product);
        }

        public async Task CreateOrderAsync(OrderCreatedEvent order)
        {
            if (_context.Orders.Any(x => x.OrderId == order.OrderId))
                return;

            var orderDb = new Order(order.OrderId, order.ProductName, order.Amount, order.CreatedAt);

            await _repository.CreateOrderAsync(orderDb);
            await _context.SaveChangesAsync();
        }
    }
}
