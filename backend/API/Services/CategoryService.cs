using API.Data;
using API.DTOs.CategoryDTOs;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using API.Models;
using API.Validators;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;


        public CategoryService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<PagedList<CategoryListDto>> GetCategoriesAsync(PaginParams categoryParams)
        {
            var query = _context.Categories.AsNoTracking();

            return await PagedList<CategoryListDto>.CreateAsync(
                query.ProjectTo<CategoryListDto>(_mapper.ConfigurationProvider),
                categoryParams.PageNumber,
                categoryParams.PageSize);
        }

        public async Task<CategoryDto> GetCategoryByIdAsync(Guid id)
        {
            var category = await _context.Categories
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
            {
                throw new KeyNotFoundException($"Category with id {id} not found.");
            }

            return _mapper.Map<CategoryDto>(category);
        }

        public async Task<CategoryDto> GetCategoryByNameAsync(string categoryName)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(c => c.CategoryName == categoryName);

            if (category == null) return null!;

            return _mapper.Map<CategoryDto>(category);
        }

        public async Task<CategoryDto> AddCategoryAsync(CategorySaveDto categorySaveDto)
        {
            var validator = new CategoryValidator(_context);
            var validationResult = validator.Validate(categorySaveDto);

            if (!validationResult.IsValid)
                throw new Exception("Category name must be unique.");

            var category = _mapper.Map<Category>(categorySaveDto);

            category.CreateDate = DateTime.UtcNow.ToCustomFormat();
            _context.Categories.Add(category);

            var result = await _context.SaveChangesAsync() > 0;

            if (!result)
            {
                throw new Exception("Failed to add the category.");
            }

            return _mapper.Map<CategoryDto>(category);
        }

        public async Task UpdateCategoryAsync(Guid id, CategorySaveDto categorySaveDto)
        {
            // حتى اتمكن من اضافة المعرف الى المتحقق اضطررت لاستخدام طريقة التحقق اليدوية
            var validator = new CategoryValidator(_context, id);
            var validationResult = validator.Validate(categorySaveDto);

            if (!validationResult.IsValid)
                throw new Exception("Category name must be unique.");

            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                throw new KeyNotFoundException($"Category with id {id} not found.");
            } 

            _mapper.Map(categorySaveDto, category);
            category.UpdateDate = DateTime.UtcNow.ToCustomFormat();
            
            var result = await _context.SaveChangesAsync() > 0;

            if (!result)
            {
                throw new Exception("Failed to update the category.");
            }
        }

        public async Task DeleteCategoryAsync(Guid id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                throw new KeyNotFoundException($"Category with id {id} not found.");
            }

            _context.Categories.Remove(category);

            var result = await _context.SaveChangesAsync() > 0;

            if (!result)
            {
                throw new Exception("Failed to delete the category.");
            }
        }

    }
}
