using API.DTOs.CategoryDTOs;
using API.Helpers;
using API.Models;

namespace API.Interfaces
{
    public interface ICategoryService
    {
        Task<PagedList<CategoryListDto>> GetCategoriesAsync(PaginParams categoryParams);
        Task<CategoryDto> GetCategoryByIdAsync(Guid id);
        Task<CategoryDto> GetCategoryByNameAsync(string categoryName);
        Task<CategoryDto> AddCategoryAsync(CategorySaveDto category);
        Task UpdateCategoryAsync(Guid id, CategorySaveDto category);
        Task DeleteCategoryAsync(Guid id);
    }
}
