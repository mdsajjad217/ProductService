using ProductService.Domain.Dto;

namespace ProductService.Domain.Service
{
    public interface IProductQueryService
    {
        Task<IEnumerable<ProductDto>> GetAllAsync();
        Task<ProductDto> GetByIdAsync(Guid id);
    }
}