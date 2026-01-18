using ProductService.Domain.Dto;
using ProductService.Domain.Event;

namespace ProductService.Domain.Service
{
    public interface IProductCommandService
    {
        Task<Guid> CreateAsync(CreateProductRequest request);
        Task UpdateAsync(Guid id, UpdateProductRequest request);
        Task CreateOrderAsync(OrderCreatedEvent order);
    }
}