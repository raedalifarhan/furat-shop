using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class Category : BaseEntity
    {
        [Required]
        public string CategoryName { get; set; } = default!;
        
        public bool IsActive { get; set; } = true;

        
        // nav properties
        public Guid? ParentId { get; set; }
        public Category? Parent { get; set; }

        public List<Product>? Products { get; set; }
        public List<Category>? Categries { get; set; }
    }
}