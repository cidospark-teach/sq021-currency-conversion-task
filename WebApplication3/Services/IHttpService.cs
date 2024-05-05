using WebApplication3.Models.DTOs;

namespace WebApplication3.Services
{
    public interface IHttpService
    {
        Task<ApiResponse<T>> MakeRequestAsync<T>(ApiRequest request);
    }
}
