using System.ComponentModel.DataAnnotations;

namespace API.Models.OrderAggregate
{
    public class ProductItemOrdered
    {
        public ProductItemOrdered()
        {
        }
        public ProductItemOrdered(Guid productItemId, string Name, string pictureUrl)
        {
            ProductItemId = productItemId;
            this.Name = Name;
            PictureUrl = pictureUrl;
        }

        [Key]
        public Guid ProductItemId { get; set; }
        public string? Name { get; set; }
        public string? PictureUrl { get; set; }
    }
}
