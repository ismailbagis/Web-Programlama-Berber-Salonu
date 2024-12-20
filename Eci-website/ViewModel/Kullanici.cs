using Microsoft.AspNetCore.Identity;

namespace Eci_website.ViewModel
{
    public class Kullanici : IdentityUser
    {
        public string? FullName { get; set; }
    }
}
