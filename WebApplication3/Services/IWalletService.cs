using WebApplication3.Models.Entities;

namespace WebApplication3.Services
{
    public interface IWalletService
    {
        Dictionary<string, double> _latestConversions { get; }
        //Task<Dictionary<string, double>> GetLatestConversions();
        Task<Wallet> GetAsync(string id);
        IQueryable<Wallet> GetByUserIdAsync(string id);
        Dictionary<string, string> IsSupportedCurrency(string currency);
        Task<Dictionary<string, string>> FundNoobAsync(Wallet wallet, string currencyCode, double amount);
        Task<Dictionary<string, string>> FundEliteAsync(Wallet wallet, string currencyCode, double amount);
        Task<Dictionary<string, string>> WithdrawalFromNoobAsync(Wallet wallet, string currencyCode, double amount);
        double GetMainWalletBalance(string ownerId);
        double GetTotalWalletBalance(string ownerId, double amount);
    }
}
