using System.ComponentModel.DataAnnotations;

namespace API.DTOs.CategoryDTOs
{
    public class CategorySaveDto
    {
        [Required]
        public string CategoryName { get; set; } = string.Empty;

        public Guid? ParentId { get; set; }
    }
}
