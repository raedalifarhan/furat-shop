using API.Data;
using API.DTOs.ProductDTOs;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using API.Lib;
using API.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Services
{
    public class ProductService : IProductService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env; // To get the root path

        public ProductService(DataContext context, IMapper mapper, IWebHostEnvironment env)
        {
            _context = context;
            _mapper = mapper;
            _env = env;
        }

        public async Task<PagedList<ProductListDto>> GetProductsAsync(PaginParams productParams)
        {
            var query = _context.Products.AsNoTracking();

            return await PagedList<ProductListDto>.CreateAsync(
                query.ProjectTo<ProductListDto>(_mapper.ConfigurationProvider),
                productParams.PageNumber,
                productParams.PageSize);
        }

        public async Task<ProductDto> GetProductByIdAsync(Guid id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                throw new KeyNotFoundException($"Product with id {id} not found.");
            }
            return _mapper.Map<ProductDto>(product);
        }

        public async Task<ProductDto> AddProductAsync(ProductSaveDto obj)
        {
            try
            {
                var product = _mapper.Map<Product>(obj);

                if (obj.Picture != null && obj.Picture.Length > 0)
                {
                    product.PictureUrl = await FileUtils.SaveImageAsync(_env, obj.Picture);
                }

                if (obj.Picture == null || obj.Picture.Length <= 0) throw new Exception("Please choose a file!");

                product.CreateDate = DateTime.UtcNow.ToCustomFormat();

                if (obj.CategoryId == Guid.Empty)
                    product.CategoryId = null;
                else
                    product.CategoryId = obj.CategoryId;

                _context.Products.Add(product);

                var result = await _context.SaveChangesAsync() > 0;

                if (!result)
                {
                    throw new Exception("Failed to add the product.");
                }

                return _mapper.Map<ProductDto>(product);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while adding the product: {ex.Message}");
            }
        }

        public async Task UpdateProductAsync(Guid id, ProductSaveDto obj)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);

                if (product == null)
                {
                    throw new KeyNotFoundException($"Product with id {id} not found.");
                }

                // Delete old image if a new image is provided
                if (obj.Picture != null && obj.Picture.Length > 0)
                {
                    if (!string.IsNullOrEmpty(product.PictureUrl))
                    {
                        FileUtils.DeleteImage(_env, product.PictureUrl);
                    }

                    product.PictureUrl = await FileUtils.SaveImageAsync(_env, obj.Picture);
                }

                _mapper.Map(obj, product);
                product.UpdateDate = DateTime.UtcNow.ToCustomFormat();

                if (obj.CategoryId == Guid.Empty)
                    product.CategoryId = null;
                else
                    product.CategoryId = obj.CategoryId;

                var result = await _context.SaveChangesAsync() > 0;

                if (!result)
                {
                    throw new Exception("Failed to update the product.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while updating the product: {ex.Message}");
            }
        }

        public async Task ToggleActivateProductAsync(Guid id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                {
                    throw new KeyNotFoundException($"Product with id {id} not found.");
                }

                product.IsActive = !product.IsActive;

                var result = await _context.SaveChangesAsync() > 0;

                if (!result)
                {
                    throw new Exception("Failed to delete the product.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting the product: {ex.Message}");
            }
        }

    }
}
