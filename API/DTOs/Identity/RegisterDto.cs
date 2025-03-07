using System.ComponentModel.DataAnnotations;

namespace API.Models.Identity
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = default!;

        [Required]
        public string Password { get; set; } = default!;

        [Required]
        public string Username { get; set; } = default!;

        [Required]
        public string DisplayName { get; set; } = default!;

        [Required]
        public string Role { get; set; } = default!;

        //[Required]
        //public Guid BranchId { get; set; } = default!;
    }
}