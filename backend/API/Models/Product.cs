using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class Product : BaseEntity
    {

        [Required]
        public string Name { get; set; } = default!;

        public string? PictureUrl { get; set; }

        public string? Description { get; set; }

        public decimal? RegularPrice { get; set; }
        public decimal? SellingPrice { get; set; }

        public int? Rate { get; set; }
        
        public bool IsMandatory { get; set; } = false;
        public int? MandatoryCount { get; set; }

        public bool IsActive { get; set; } = false;


        // van porperties
        public Category? Category { get; set; }
        public Guid? CategoryId { get; set; }
    }
}