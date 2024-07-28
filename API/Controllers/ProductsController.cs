using API.DTOs.ProductDTOs;
using API.Helpers;
using API.Interfaces;
using API.RequestHelpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productServices;

        public ProductsController(IProductService productServices)
        {
            _productServices = productServices;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<PagedList<ProductListDto>>> GetAllProducts([FromQuery] PaginParams productParams)
        {
            return Ok(await _productServices.GetProductsAsync(productParams));
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(Guid id)
        {
            try
            {
                return Ok(await _productServices.GetProductByIdAsync(id));
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<ProductDto>> CreateProduct([FromForm] ProductSaveDto model)
        {
            try
            {
                var product = await _productServices.AddProductAsync(model);
                return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred while saving the product: {ex.Message}");
            }
        }

        // [Authorize(Roles = $"{RolesNames.VENDOR}, {RolesNames.IT}")]
        [AllowAnonymous]
        [HttpPut]
        public async Task<ActionResult> UpdateProduct(Guid id, ProductSaveDto model)
        {
            try
            {
                await _productServices.UpdateProductAsync(id, model);
                return Ok("Update completed successfully");
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred while updating the product: {ex.Message}");
            }
        }

        [Authorize(Roles = $"{RolesNames.VENDOR}, {RolesNames.IT}")]
        [HttpDelete("{id}")]
        public async Task<ActionResult> ToggleActivationProductStatus(Guid id)
        {
            try
            {
                await _productServices.ToggleActivateProductAsync(id);
                return Ok("Reverse activation status Successfully!");
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred while reverse activation status of the product: {ex.Message}");
            }
        }
    }
}
