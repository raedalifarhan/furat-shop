namespace API.DTOs.ProductDTOs
{
    public class ProductListDto
    {
        public string Name { get; set; } = string.Empty;
        public int RegularPrice { get; set; }
        public int SellingPrice { get; set; }
        public string PictureUrl { get; set; } = string.Empty;
    }
}
