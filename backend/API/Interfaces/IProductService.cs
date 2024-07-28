using API.DTOs.ProductDTOs;
using API.Helpers;

namespace API.Interfaces
{
    public interface IProductService
    {
        Task<PagedList<ProductListDto>> GetProductsAsync(PaginParams productParams);
        Task<ProductDto> GetProductByIdAsync(Guid id);
        Task<ProductDto> AddProductAsync(ProductSaveDto obj);
        Task UpdateProductAsync(Guid id, ProductSaveDto obj);
        Task ToggleActivateProductAsync(Guid id);
    }
}
