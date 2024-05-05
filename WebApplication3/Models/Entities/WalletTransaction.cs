namespace WebApplication3.Models.Entities
{
    public class WalletTransaction
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string WalletId { get; set; }
        public string TransactionType { get; set; }
        public double OldBalance { get; set; }
        public double NewBalance { get; set; }
        public string CreatedOn { get; set; } = DateTime.UtcNow.ToString();
        public string UpdatedOn { get; set; } = DateTime.UtcNow.ToString();

        // navigation props
        public Wallet? Wallet { get; set; }
    }
}
