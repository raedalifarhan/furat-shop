using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace API.DTOs.ProductDTOs
{
    public class ProductSaveDto
    {
        public Guid CategoryId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal? RegularPrice { get; set; }
        public decimal? SellingPrice { get; set; }

        public IFormFile? Picture { get; set; }
    }
}
