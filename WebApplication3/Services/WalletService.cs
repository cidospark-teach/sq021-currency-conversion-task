using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Abstractions;
using WebApplication3.Data;
using WebApplication3.Models.DTOs;
using WebApplication3.Models.Entities;

namespace WebApplication3.Services
{
    public class WalletService : IWalletService
    {
        private readonly IHttpService _httpService;
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
        private readonly string _baseUrl;
        private readonly string _apikey;
        private readonly string _baseCurrency;
        public Dictionary<string, double> _latestConversions { get; private set; }
        public WalletService(IHttpService httpService, IConfiguration config, AppDbContext context)
        {
            _httpService = httpService;
            _config = config;
            _context = context;
            _apikey = _config.GetSection("APISettings:ApiKey").Value;
            _baseUrl = _config.GetSection("APISettings:BaseUrl").Value;
            _baseCurrency = "NGN";
            _latestConversions = this.GetLatestConversions(_baseCurrency).Result;
        }

        private async Task<Dictionary<string, double>> GetLatestConversions(string defaultCurr)
        {
            Dictionary<string, string> q = new Dictionary<string, string>();
            q.Add("apikey", _apikey);
            q.Add("base_currency", _baseCurrency);
            var request = new ApiRequest
            {
                Url = _baseUrl,
                ApiType = "GET",
                Endpoint = "latest",
                QueryParams = q
            };
            var result = await _httpService.MakeRequestAsync<ConversionResponse>(request);
            var listOfCurrencies = new Dictionary<string, double>();
            if (result.IsSuccess)
            {
                var props = (result.Data.Data.GetType().GetProperties()).ToList();
                foreach (var kvp in props)
                {
                    var kvpValue = (Attr)kvp.GetValue(result.Data.Data);
                    listOfCurrencies.Add(kvp.Name, Convert.ToDouble(kvpValue.value.ToString()));
                }

            }

            return listOfCurrencies;
        }
        
        public async Task<Wallet> GetAsync(string id)
        {
            return await _context.Wallet.FirstOrDefaultAsync(x => x.WalletId == id);
        }

        public IQueryable<Wallet> GetByUserIdAsync(string userId)
        {
            return _context.Wallet.Where(x => x.OwnerId == userId);
        }

        public Dictionary<string, string> IsSupportedCurrency(string currency)
        {
            var res = new Dictionary<string, string>();
            SupportedCurrenciesTypes supportedCurrencyType;
            if (!SupportedCurrenciesTypes.TryParse(currency.ToString().ToUpper(), out supportedCurrencyType))
            {
                res.Add("code", "400");
                res.Add("message", $"{currency} is not a supported currency type: Currency type should either be 'NGN', 'USD' or 'EUR'");
            }
            else
            {
                res.Add("code", "200");
                res.Add("message", "found");
            }

            return res;
        }

        public async Task<Dictionary<string, string>> FundNoobAsync(Wallet wallet, string currencyCode, double amount)
        {
            var oldBal = 0.0;
            var res = new Dictionary<string, string>();
            var newlyFundedWalletId = wallet.WalletId;
            if (wallet.WalletCurrency.ToLower() == currencyCode.ToLower())
            {
                oldBal = wallet.Balance;
                wallet.Balance += amount;
            }
            else
            {
                var conversionEquivalent = _latestConversions.First(x => x.Key.ToLower() == currencyCode.ToLower());
                oldBal = wallet.Balance;
                wallet.Balance += (conversionEquivalent.Value * amount);
            }

            var transaction = new WalletTransaction
            {
                WalletId = wallet.WalletId,
                TransactionType = TransactionTypes.funding.ToString(),
                OldBalance = oldBal,
                NewBalance = wallet.Balance
            };

            using var transactionObject = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.Update(wallet);
                await _context.SaveChangesAsync();
                _context.Add(transaction);
                await _context.SaveChangesAsync();

                await transactionObject.CommitAsync();

                res.Add("code", "200");
                res.Add("message", "Funded successfully.");
                res.Add("walletId", newlyFundedWalletId);
                res.Add("tnxId", transaction.Id);
                return res;
            }
            catch (Exception ex)
            {
                await transactionObject.RollbackAsync();

                res.Add("code", "400");
                res.Add("message", "Funding failed.");
                return res;
            }
        }

        public async Task<Dictionary<string, string>> FundEliteAsync(Wallet wallet, string currencyCode, double amount)
        {
            var oldBal = 0.0;
            var res = new Dictionary<string, string>();
            var newlyFundedWalletId = wallet.WalletId;
            using var transactionObject = await _context.Database.BeginTransactionAsync();
            try
            {
                if (wallet.WalletCurrency.ToLower() == currencyCode.ToLower())
                {
                    oldBal = wallet.Balance;
                    wallet.Balance += amount;
                    _context.Update(wallet);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    var newWallet = new Wallet
                    {
                        WalletName = $"{currencyCode}_Wallet",
                        WalletCurrency = currencyCode,
                        Balance = amount,
                        IsMain = false,
                        OwnerId = wallet.OwnerId
                    };
                    _context.Add(newWallet);
                    await _context.SaveChangesAsync();
                    newlyFundedWalletId = newWallet.WalletId;
                }

                var transaction = new WalletTransaction
                {
                    WalletId = newlyFundedWalletId,
                    TransactionType = TransactionTypes.funding.ToString(),
                    OldBalance = oldBal,
                    NewBalance = wallet.Balance
                };

                _context.Add(transaction);
                await _context.SaveChangesAsync();

                await transactionObject.CommitAsync();

                res.Add("code", "200");
                res.Add("message", $"Funded successfully.");
                res.Add("walletId", newlyFundedWalletId);
                res.Add("tnxId", transaction.Id);
                return res;
            }
            catch (Exception ex)
            {
                await transactionObject.RollbackAsync();

                res.Add("code", "400");
                res.Add("message", "Funding failed.");
                return res;
            }
        }

        public async Task<Dictionary<string, string>> WithdrawalFromNoobAsync(Wallet wallet, string currencyCode, double amount)
        {
            var oldBal = 0.0;
            var res = new Dictionary<string, string>();
            if (wallet.WalletCurrency.ToLower() == currencyCode.ToLower())
            {
                if(wallet.Balance < amount)
                {
                    res.Add("code", "400");
                    res.Add("message", "Insufficient funds.");
                    return res;
                }
                oldBal = wallet.Balance;
                wallet.Balance -= amount;
            }
            else
            {
                var conversionEquivalent = _latestConversions.First(x => x.Key.ToLower() == currencyCode.ToLower());
                if (wallet.Balance < (conversionEquivalent.Value * amount))
                {
                    res.Add("code", "400");
                    res.Add("message", "Insufficient funds.");
                    return res;
                }
                oldBal = wallet.Balance;
                wallet.Balance -= (conversionEquivalent.Value * amount);
            }

            var transaction = new WalletTransaction
            {
                WalletId = wallet.WalletId,
                TransactionType = TransactionTypes.withdrawal.ToString(),
                OldBalance = oldBal,
                NewBalance = wallet.Balance
            };

            using var transactionObject = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.Update(wallet);
                await _context.SaveChangesAsync();
                _context.Add(transaction);
                await _context.SaveChangesAsync();

                await transactionObject.CommitAsync();

                res.Add("code", "200");
                res.Add("message", "Withdrawal is successful.");
                res.Add("tnxId", transaction.Id);
                return res;
            }
            catch (Exception ex)
            {
                await transactionObject.RollbackAsync();

                res.Add("code", "400");
                res.Add("message", "Withdrawal failed.");
                return res;
            }
        }

        public async Task<Dictionary<string, string>> WithdrawalFromEliteAsync(Wallet wallet, string currencyCode, double amount)
        {
            var oldBal = 0.0;
            var res = new Dictionary<string, string>();
            var totalBalance = this.GetTotalWalletBalance(wallet.OwnerId, amount);
            if (wallet.WalletCurrency.ToLower() == currencyCode.ToLower())
            {
                if (wallet.Balance < amount)
                {
                    if(totalBalance < amount)
                    {
                        res.Add("code", "400");
                        res.Add("message", "Insufficient funds.");
                        return res;
                    }
                    else
                    {
                        oldBal = wallet.Balance;
                        wallet.Balance = 0;
                    }
                }
                else
                {
                    oldBal = wallet.Balance;
                    wallet.Balance -= amount;
                }
            }
            else
            {
                var conversionEquivalent = _latestConversions.First(x => x.Key.ToLower() == currencyCode.ToLower()).Value;
                var convertedAmt = (amount * conversionEquivalent);
                if (wallet.Balance < convertedAmt)
                {
                    if (totalBalance < convertedAmt)
                    {
                        res.Add("code", "400");
                        res.Add("message", "Insufficient funds.");
                        return res;
                    }
                    else
                    {
                        oldBal = wallet.Balance;
                        wallet.Balance = 0;
                    }
                }
                else
                {
                    oldBal = wallet.Balance;
                    wallet.Balance -= amount;
                }
            }

            var transaction = new WalletTransaction
            {
                WalletId = wallet.WalletId,
                TransactionType = TransactionTypes.withdrawal.ToString(),
                OldBalance = oldBal,
                NewBalance = wallet.Balance
            };

            using var transactionObject = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.Update(wallet);
                await _context.SaveChangesAsync();
                _context.Add(transaction);
                await _context.SaveChangesAsync();

                await transactionObject.CommitAsync();

                res.Add("code", "200");
                res.Add("message", "Withdrawal is successful.");
                res.Add("tnxId", transaction.Id);
                return res;
            }
            catch (Exception ex)
            {
                await transactionObject.RollbackAsync();

                res.Add("code", "400");
                res.Add("message", "Withdrawal failed.");
                return res;
            }
        }

        public double GetMainWalletBalance(string ownerId)
        {
            var wallets = this.GetByUserIdAsync(ownerId);
            if(wallets != null && wallets.Any(x => x.IsMain))
                return wallets.First(x => x.IsMain).Balance;

            return 0;
        }

        public double GetTotalWalletBalance(string ownerId, double amount)
        {
            var wallets = this.GetByUserIdAsync(ownerId);
            if (wallets != null && wallets.Any())
            {
                var total = 0.0;
                wallets.ToList().ForEach(w => {
                    var conversionEquivalent = _latestConversions.FirstOrDefault(x => x.Key.ToLower() == w.WalletCurrency.ToLower()).Value;
                    total += (conversionEquivalent * w.Balance);
                });
                return total;
            }

            return 0;
        }

    }
}

