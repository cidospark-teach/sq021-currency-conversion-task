using System.ComponentModel.DataAnnotations;

namespace WebApplication3.Models.DTOs
{
    public class WalletToFund
    {
        [Required]
        public string WalletId { get; set; }
        [Required]
        public double AmountToFund { get; set; }
        [Required]
        public string CurrencyCode { get; set; }
    }
}
