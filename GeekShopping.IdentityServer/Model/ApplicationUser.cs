using Microsoft.AspNetCore.Identity;

namespace GeekShopping.IdentityServer.Model
{
    public class ApplicationUser : IdentityUser
    {
        private string FirstaName { get; set; }
        private string LastaName { get; set; }

    }
}
