namespace WebApplication3.Models.Entities
{
    public class Wallet
    {
        public string WalletId { get; set; } = Guid.NewGuid().ToString();
        public string WalletName { get; set; }
        public string WalletCurrency { get; set; }
        public bool IsMain { get; set; }
        public double Balance { get; set; }
        public string CreatedOn { get; set; } = DateTime.UtcNow.ToString();
        public string UpdatedOn { get; set; } = DateTime.UtcNow.ToString();
        public string OwnerId { get; set; }

        // navigation property
        public AppUser Owner { get; set; }
        public ICollection<WalletTransaction> WalletTransactions { get; set; } = new HashSet<WalletTransaction>();
    }
}
