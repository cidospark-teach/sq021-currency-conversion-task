using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using WebApplication3.Models.DTOs;
using WebApplication3.Models.Entities;
using WebApplication3.Services;

namespace WebApplication3.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class WalletController : ControllerBase
    {
        private readonly ILogger<WalletController> _logger;
        private readonly IConfiguration _config;
        private readonly IWalletService _walletService;
        private readonly UserManager<AppUser> _userManager;

        public WalletController(ILogger<WalletController> logger, IWalletService walletService,
             IConfiguration config, UserManager<AppUser> userManager)
        {
            _logger = logger;
            _config = config;
            _walletService = walletService;
            _userManager = userManager;
        }


        [HttpPost("fund-wallet")]
        public async Task<IActionResult> FundWallet([FromBody] WalletToFund model)
        {
            if(model.AmountToFund < 1)
                return BadRequest("Invalid entry for amount to fund.");

            var isSupported = _walletService.IsSupportedCurrency(model.CurrencyCode);
            if (isSupported["code"] == "400")
                return BadRequest(isSupported["message"]);

            var wallet = await _walletService.GetAsync(model.WalletId);
            if (wallet == null)
            {
                return BadRequest($"No record found for wallet with id {model.WalletId}");
            }

            var loggedInUser = await _userManager.GetUserAsync(User);
            var walletToReturn = new Wallet();
            var res = new Dictionary<string, string>();
            if(loggedInUser.Id == wallet.OwnerId)
            {
                var loggedInUserRole = (await _userManager.GetRolesAsync(loggedInUser)).First();
                switch (loggedInUserRole)
                {
                    case "noob":
                        var noobFundingResult = await _walletService.FundNoobAsync(wallet, model.CurrencyCode, model.AmountToFund);
                        if (noobFundingResult["code"] == "400")
                            return BadRequest(noobFundingResult["message"]);

                        res.Add("walletId", noobFundingResult["walletId"]);
                        res.Add("TransactionId", noobFundingResult["tnxId"]);
                        return Ok(res);

                    case "elite":
                        var eliteFundingResult = await _walletService.FundEliteAsync(wallet, model.CurrencyCode, model.AmountToFund);
                        if (eliteFundingResult["code"] == "400")
                            return BadRequest(eliteFundingResult["message"]);

                        res.Add("walletId", eliteFundingResult["walletId"]);
                        res.Add("TransactionId", eliteFundingResult["tnxId"]);
                        return Ok(res);

                    default:
                        return BadRequest($"{loggedInUserRole} role is not supported for funding.");
                }
            }
            return BadRequest("LoggedIn user does not match the wallet owner");
        }

        [HttpPost("withdraw-funds")]
        [Authorize(Roles ="noob,elite")]
        public async Task<IActionResult> Withdraw([FromBody] WalletWithdrawalDto model)
        {
            if (model.AmountToFund < 1)
                return BadRequest("Invalid entry for amount to fund.");

            var isSupported = _walletService.IsSupportedCurrency(model.CurrencyCode);
            if (isSupported["code"] == "400")
                return BadRequest(isSupported["message"]);

            var wallet = await _walletService.GetAsync(model.WalletId);
            if (wallet == null)
            {
                return BadRequest($"No record found for wallet with id {model.WalletId}");
            }

            var loggedInUser = await _userManager.GetUserAsync(User);
            var walletToReturn = new Wallet();
            var res = new Dictionary<string, string>();
            if (loggedInUser.Id == wallet.OwnerId)
            {
                var loggedInUserRole = (await _userManager.GetRolesAsync(loggedInUser)).First();
                switch (loggedInUserRole)
                {
                    case "noob":
                        var noobWithdrawalResult = await _walletService.WithdrawalFromNoobAsync(wallet, model.CurrencyCode, model.AmountToFund);
                        if (noobWithdrawalResult["code"] == "400")
                            return BadRequest(noobWithdrawalResult["message"]);

                        res.Add("walletId", noobWithdrawalResult["walletId"]);
                        res.Add("TransactionId", noobWithdrawalResult["tnxId"]);
                        return Ok(res);

                    case "elite":
                        var eliteWithdrawalResult = await _walletService.FundEliteAsync(wallet, model.CurrencyCode, model.AmountToFund);
                        if (eliteWithdrawalResult["code"] == "400")
                            return BadRequest(eliteWithdrawalResult["message"]);

                        res.Add("walletId", eliteWithdrawalResult["walletId"]);
                        res.Add("TransactionId", eliteWithdrawalResult["tnxId"]);
                        return Ok(res);

                    default:
                        return BadRequest($"{loggedInUserRole} role is not supported for withdrawal.");
                }
            }
            return BadRequest("LoggedIn user does not match the wallet owner");
        }

        [HttpGet("get-latest-convertions")]
        public async Task<IActionResult> GetLatest()
        {
            var result = _walletService._latestConversions;
            return Ok(result);
        }
        [HttpGet("get-single/{code}")]
        public async Task<IActionResult> GetLatest(string code)
        {
            var result = _walletService._latestConversions.FirstOrDefault(x => x.Key.ToLower() == code.ToLower());
            return Ok(result);
        }

        
    }
}
