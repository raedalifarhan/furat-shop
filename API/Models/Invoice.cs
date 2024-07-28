using API.Models.OrderAggregate;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class Invoice : BaseEntity
    {
        [Required]
        public string InvoiceNumber { get; set; } = string.Empty;

        [Required]
        public decimal TotalAmount { get; set; }

        [Required]
        public DateTime InvoiceDate { get; set; }

        public Order? Order { get; set; }
        public Guid? OrderId { get; set; }

        public string? CustomerId { get; set; }
        public AppUser? Customer { get; set; }

        public DateTime? DeliveryDate { get; set; }

        // رسوم الشحن
        public decimal? ShippingFee { get; set; }

        // ضرائب الفاتورة
        public decimal? Taxes { get; set; }

        // معلومات الدفع
        public string? PaymentMethod { get; set; }

        public DateTime? ExpectedDeliveryTime { get; set; }
    }
}
