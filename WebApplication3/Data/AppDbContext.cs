using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Models.Entities;

namespace WebApplication3.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<AppUser>(options)
    {
        public DbSet<Wallet> Wallet { get; set; }
        public DbSet<WalletTransaction> WalletTransactions { get; set; }
    }
}