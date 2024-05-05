using Microsoft.AspNetCore.Identity;

namespace WebApplication3.Models.Entities
{
    public class AppUser: IdentityUser
    {
        // navigation property
        public List<Wallet> Wallets { get; set; } = new List<Wallet>();
    }
}
