using API.DTOs.CategoryDTOs;
using API.Helpers;
using API.Interfaces;
using API.RequestHelpers;
using API.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<PagedList<CategoryListDto>>> GetAllCategories([FromQuery] PaginParams categoryParams)
        {
            return Ok(await _categoryService.GetCategoriesAsync(categoryParams));
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDto>> GetCategory(Guid id)
        {
            try
            {
                return Ok(await _categoryService.GetCategoryByIdAsync(id));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [AllowAnonymous]
        // [Authorize(Roles = $"{RolesNames.VENDOR}, {RolesNames.IT}")]
        [HttpPost]
        public async Task<ActionResult<CategoryDto>> CreateCategory(CategorySaveDto model)
        {
            try
            {
                var category = await _categoryService.AddCategoryAsync(model);
                return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred while saving the category: {ex.Message}");
            }
        }

        [AllowAnonymous]
        // [Authorize(Roles = $"{RolesNames.VENDOR}, {RolesNames.IT}")]
        [HttpPut]
        public async Task<ActionResult> UpdateCategory(Guid id, CategorySaveDto model)
        {
            try
            {
                await _categoryService.UpdateCategoryAsync(id, model);
                return Ok("Update completed successfully");
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred while updating the category: {ex.Message}");
            }
        }

       [AllowAnonymous]
        // [Authorize(Roles = $"{RolesNames.VENDOR}, {RolesNames.IT}")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCategory(Guid id)
        {
            try
            {
                await _categoryService.DeleteCategoryAsync(id);
                return Ok("Delete completed Successfully");
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred while deleting the category: {ex.Message}");
            }
        }
    }
}
