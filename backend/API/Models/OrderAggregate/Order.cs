using API.Extensions;

namespace API.Models.OrderAggregate
{
    public class Order : BaseEntity
    {
        public Order()
        {
        }
        public Order(string customerPhoneNumber, Address shipToAddress, DeliveryMethod deliveryMethod, 
            IReadOnlyList<OrderItem> orderItems, decimal subtotal)
        {
            CustomerPhoneNumber = customerPhoneNumber;
            ShipToAddress = shipToAddress;
            DeliveryMethod = deliveryMethod;
            OrderItems = orderItems;
            Subtotal = subtotal;
        }
        
        
        public string? CustomerPhoneNumber { get; set; }
        public string OrderDate { get; set; } = DateTime.UtcNow.ToCustomFormat();
        public Address? ShipToAddress { get; set; }
        public DeliveryMethod? DeliveryMethod { get; set; }
        public decimal Subtotal { get; set; }
        public OrderStatus? Status { get; set; } = OrderStatus.Pending;
        
        // Stripe give us payment intent id as the user intends.
        public string? PaymentIntentId { get; set; }

        // nav properties.
        public IReadOnlyList<OrderItem>? OrderItems { get; set; }

        public string? CustomerId { get; set; }
        public AppUser? Customer { get; set; }


        public decimal GetTotal()
        {
            return Subtotal + DeliveryMethod!.Price;
        }
    }
}
