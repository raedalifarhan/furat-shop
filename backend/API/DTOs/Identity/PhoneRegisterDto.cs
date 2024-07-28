using System.ComponentModel.DataAnnotations;

namespace API.Models.Identity
{
    public class PhoneRegisterDto
    {
        [Required]
        public string PhoneNumber { get; set; } = default!;

        [Required]
        public string DisplayName { get; set; } = default!;

        [Required]
        public string Role { get; set; } = string.Empty;
    }

}
