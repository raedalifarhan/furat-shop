
namespace API.DTOs.ProductDTOs
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal? RegularPrice { get; set; }
        public decimal? SellingPrice { get; set; }
        public int? Rate { get; set; }
        public string? PictureUrl { get; set; }

        public string? CategoryName { get; set; }
    }
}
