using Microsoft.AspNetCore.Identity;

namespace IdentityService.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}