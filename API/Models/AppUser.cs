using API.Models.OrderAggregate;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class AppUser : IdentityUser
    {
        [Required, MaxLength(50)]
        public string DisplayName { get; set; } = default!;
        public string? VerificationCode { get; set; }
        public bool IsAvailableDriver { get; set; }


        // Other relevant customer properties
        public ICollection<Order>? CustomerOrders { get; set; }

        public ICollection<CustomerBasket>? CustomerBaskets { get; set; }
        public ICollection<Invoice>? Invoices { get; set; }
    }
}
