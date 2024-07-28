namespace API.DTOs.CategoryDTOs
{
    public class CategoryListDto
    {
        public Guid Id { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string ParentCategoryName { get; set; } = string.Empty;
        public string? CreateDate { get; set; }
        public string? UpdateDate { get; set; }
    }
}
