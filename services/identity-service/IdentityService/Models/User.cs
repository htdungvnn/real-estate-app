using Microsoft.AspNetCore.Identity;

namespace IdentityService.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty; // Provide default value
        public string LastName { get; set; } = string.Empty;  // Provide default value
    }
}