using API.DTOs.ProductDTOs;

namespace API.DTOs.CategoryDTOs
{
    public class CategoryDto
    {
        public Guid Id { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public ICollection<ProductDto>? Products { get; set; }
    }
}
