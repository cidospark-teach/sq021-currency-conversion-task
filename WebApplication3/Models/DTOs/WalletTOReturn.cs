namespace WebApplication3.Models.DTOs
{
    public class WalletTOReturn
    {
        public string WalletId { get; set; } = Guid.NewGuid().ToString();
        public string WalletName { get; set; }
        public string WalletCurrency { get; set; }
        public bool IsMain { get; set; }
        public double Balance { get; set; }
        public string CreatedOn { get; set; } = DateTime.UtcNow.ToString();
        public string UpdatedOn { get; set; } = DateTime.UtcNow.ToString();
        public string OwnerId { get; set; }
    }
}
