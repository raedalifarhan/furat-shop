namespace API.Models
{
    public class CustomerBasket : BaseEntity
    {
        public CustomerBasket()
        {
        }

        public CustomerBasket(Guid id)
        {
            this.Id = id;
        }


        public int? DeliveryMethod { get; set; }
        public string? ClientSecret { get; set; }
        public string? PaymentIntendId { get; set; }

        public string? CustomerId { get; set; }
        public AppUser? Customer { get; set; }
        
        // nav props
        public List<BasketItem>? Items { get; set; } = new();
    }
}
