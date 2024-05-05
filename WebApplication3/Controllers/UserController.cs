using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Helpers;
using WebApplication3.Models.DTOs;
using WebApplication3.Models.Entities;
using WebApplication3.Services;

namespace WebApplication3.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IWalletService _walletService;
        public UserController(UserManager<AppUser> userManager, IWalletService walletService)
        {
            _userManager = userManager;
            _walletService = walletService;
        }

        [HttpPost("create-user")]
        public async Task<IActionResult> CreateUser([FromBody] UserToCreate model)
        {
            AllowedUserTypes allowedUserType;
            if (!AllowedUserTypes.TryParse(model.UserType.ToString().ToLower(), out allowedUserType))
            {
                return BadRequest($"{model.UserType} is an invalid User type: User type should either be 'noob' or 'elite'");
            }

            if (!model.Currencies.Any(x => x.isMain == true))
                return BadRequest("Main currency was not specified!");

            var count = 0;
            foreach(var item in model.Currencies)
            {
                var isSupported = _walletService.IsSupportedCurrency(item.Type);
                if (isSupported["code"] == "400")
                    return BadRequest(isSupported["message"]);

                if(item.isMain == true)
                {
                    count++;
                    if(count > 1)
                        return BadRequest("Only one currency can be made 'main'.");
                }
            }

            try
            {
                var user = new AppUser
                {
                    Email = model.Email,
                    UserName = model.Email,
                };

                var walletsToReturn = new List<WalletTOReturn>();
                if (model.UserType.ToLower().Equals(AllowedUserTypes.noob.ToString().ToLower()))
                {
                    var wallet = new Wallet();
                    if (model.Currencies.Count > 1)
                    {
                        return BadRequest("Noob users can only have one wallet with the main currency!");
                    }
                    wallet.IsMain = true;
                    wallet.WalletCurrency = model.Currencies.First().Type;
                    wallet.OwnerId = user.Id;
                    wallet.WalletName = $"{wallet.WalletCurrency}_Wallet";
                    user.Wallets.Add(wallet);
                    walletsToReturn.Add(new WalletTOReturn
                    {
                        WalletId = wallet.WalletId,
                        WalletCurrency = wallet.WalletCurrency,
                        WalletName = wallet.WalletName,
                        Balance = wallet.Balance,
                        IsMain = wallet.IsMain,
                        OwnerId = wallet.OwnerId,
                    });
                }
                else
                {
                    foreach (var currency in model.Currencies)
                    {
                        var wallet = new Wallet();
                        wallet.IsMain = currency.isMain;
                        wallet.WalletCurrency = currency.Type;
                        wallet.OwnerId = user.Id;
                        wallet.WalletName = $"{wallet.WalletCurrency}_Wallet";
                        user.Wallets.Add(wallet);
                        walletsToReturn.Add(new WalletTOReturn
                        {
                            WalletId = wallet.WalletId,
                            WalletCurrency = wallet.WalletCurrency,
                            WalletName = wallet.WalletName,
                            Balance = wallet.Balance,
                            IsMain = wallet.IsMain,
                            OwnerId = wallet.OwnerId,
                        });
                    }
                }

                var addUserResult = await _userManager.CreateAsync(user, model.Password);
                if (addUserResult.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, model.UserType.ToLower());
                    var userToReturn = new UserToReturn
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        Email = user.Email,
                        Wallets = walletsToReturn
                    };

                    return Ok(userToReturn);
                }
                var errMsgs = "";
                foreach(var err in addUserResult.Errors)
                {
                    errMsgs += err.Description + "\n";
                }
                return BadRequest(errMsgs);


            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-single")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    return NotFound($"No record found for user with id: {id}");
                }

                var userToReturn = new UserToReturn
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    UserRole = _userManager.GetRolesAsync(user).Result.First()
                };
                var wallets = _walletService.GetByUserIdAsync(user.Id);
                foreach(var wallet in wallets)
                {
                    userToReturn.Wallets.Add(new WalletTOReturn
                    {
                        WalletId = wallet.WalletId,
                        WalletCurrency = wallet.WalletCurrency,
                        WalletName = wallet.WalletName,
                        Balance = wallet.Balance,
                        IsMain = wallet.IsMain,
                        OwnerId = wallet.OwnerId,
                    });
                }
                return Ok(userToReturn);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-all")]
        public IActionResult GetAll(int page, int perPage)
        {
            try
            {
                var users = _userManager.Users;

                var usersToReturnList = new List<UserToReturn>();
                foreach (var user in users)
                {
                    var userToReturn = new UserToReturn
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        Email = user.Email,
                        UserRole = _userManager.GetRolesAsync(user).Result.First()
                    };
                    usersToReturnList.Add(userToReturn);
                }

                var paginated = UtitlityMethods.Paginate<UserToReturn>(usersToReturnList, page, perPage);

                return Ok(paginated);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPatch("change-user-role")]
        [Authorize(Roles ="admin")]
        public async Task<IActionResult> ChangeUserRole(string userId, string role)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    return BadRequest($"No record found for user with Id: {userId}");

                var userRoles = await _userManager.GetRolesAsync(user);
                if (userRoles != null)
                    return BadRequest($"User does not have a role");

                if (userRoles.Any(x => x.ToLower() == role.ToLower()))
                    return BadRequest($"User already has this role: {role}");

                await _userManager.RemoveFromRoleAsync(user, userRoles.First());
                await _userManager.AddToRoleAsync(user, role);
                return Ok("Role changed successfully.");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

    }
}
