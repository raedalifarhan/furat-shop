using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class BasketItem : BaseEntity
    {
        
        [Required]
        public string? Name { get; set; }

        public decimal? Price { get; set; }

        public int Quantity { get; set; }

        public string? Category { get; set; }
 
        public string? FoodBrand { get; set; }

        public string? Type { get; set; }

        public string? PictureUrl { get; set; }
    }
}