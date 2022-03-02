using Microsoft.AspNetCore.Identity;

namespace Identity.Management.API.Model
{
    public class UserModel : IdentityUser
    {
        public string Name { get; set; } = string.Empty;
        public string? Age { get; set; }
    }
}
