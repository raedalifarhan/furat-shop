using System.ComponentModel.DataAnnotations;

namespace API.Models.Identity
{
    public class PhoneLoginDto
    {
        [Required]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        public string VerificationCode { get; set; } = string.Empty;
    }
}
