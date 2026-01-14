using ProductService.Domain.Dto;

namespace ProductService.Domain.Service
{
    public interface IProductCommandService
    {
        Task<Guid> CreateAsync(CreateProductRequest request);
        Task UpdateAsync(Guid id, UpdateProductRequest request);
    }
}