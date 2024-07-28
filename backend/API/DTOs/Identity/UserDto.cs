namespace API.Models.Identity
{
    // Contain properties return back after loging or register.
    public class UserDto
    {
        public required string Id { get; set; }
        public string? DisplayName { get; set; }
        public string? Username { get; set; }
        public string? Token { get; set; }
        public string? Role { get; set; }
        public string? VerificationCode { get; set; }
    }
}