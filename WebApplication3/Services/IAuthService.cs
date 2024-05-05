using System.Security.Claims;
using WebApplication3.Models.DTOs;
using WebApplication3.Models.Entities;

namespace WebApplication3.Services
{
    public interface IAuthService
    {
        string GenerateJWT(AppUser user, List<string> roles, List<Claim> claims);

        Task<LoginResult> Login(string email, string password);

        Task<Dictionary<string, string>> ValidateLoggedInUser(ClaimsPrincipal user, string userId);
    }
}
